import { useState } from 'react';
import { useNavigate } from 'react-router';
import { useErrorBoundary } from 'react-error-boundary';
import Modal from 'react-bootstrap/Modal';
import Stack from 'react-bootstrap/Stack';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';
import HumanVerification from './HumanVerification';

function DeleteConfirmation({ show, title, deletionQuestion, verificationInfo, cancelText, deleteText, deleteContext, onClose, onDelete }) {
    const navigate = useNavigate();
    const { showBoundary } = useErrorBoundary();
    const [verificationKey, setVerificationKey] = useState();
    const [verificationCode, setVerificationCode] = useState();
    const [deletionEnabled, setDeletionEnabled] = useState(false);

    return (
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>{title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Stack gap='3'>
                    <p>{deletionQuestion}</p>
                    <HumanVerification 
                        verificationInfo={verificationInfo}
                        onVerificationGenerated={handleVerificationGenerated} 
                        onVerificationExpires={handleVerificationExpires} 
                        onVerificationVerified={handleVerificationVerified} 
                        onVerificationFailed={handleVerificationFailed} />
                </Stack>
            </Modal.Body>
            <Modal.Footer>
                <ButtonToolbar>
                    <ButtonGroup className='me-2'>
                        <Button type='button' variant='danger' disabled={deletionEnabled !== true} onClick={() => handleDelete(deleteContext, verificationKey, verificationCode, onDelete, navigate)}>{deleteText}</Button>
                    </ButtonGroup>
                    <ButtonGroup className='me-2'>
                        <Button type='button' variant='secondary' onClick={onClose}>{cancelText}</Button>
                    </ButtonGroup>
                </ButtonToolbar>
            </Modal.Footer>
        </Modal>
    )

    function handleVerificationGenerated() {
        setVerificationKey(undefined);
        setVerificationCode(undefined);
        setDeletionEnabled(false);
    }

    function handleVerificationExpires() {
        setVerificationKey(undefined);
        setVerificationCode(undefined);
        setDeletionEnabled(false);
    }

    function handleVerificationVerified(verificationKey, verificationCode) {
        setVerificationKey(verificationKey);
        setVerificationCode(verificationCode);
        setDeletionEnabled(true);
    }

    function handleVerificationFailed() {
        setVerificationKey(undefined);
        setVerificationCode(undefined);
        setDeletionEnabled(false);
    }

    async function handleDelete(deleteContext, verificationKey, verificationCode, onDelete, navigate) {
        if (deletionEnabled !== true) {
            return;
        }

        try {
            setDeletionEnabled(false);

            const returnUrl = await onDelete(deleteContext, verificationKey, verificationCode);

            navigate(returnUrl, { replace: true, preventScrollReset: true });
        }
        catch (error) {
            showBoundary(error);
        }
    }
}

export default DeleteConfirmation;