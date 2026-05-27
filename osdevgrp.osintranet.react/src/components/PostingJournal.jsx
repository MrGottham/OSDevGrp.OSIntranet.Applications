import { useState, useContext } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { Formik } from 'formik';
import { object } from 'yup';
import { HelperContext } from '../contexts/HelperContext';
import Table from 'react-bootstrap/Table'
import Stack from 'react-bootstrap/Stack';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import SubmitToolbar from './SubmitToolbar';
import DeleteConfirmation from './DeleteConfirmation';

function PostingJournal({ postingJournal, staticTexts }) {
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [showEditModal, setShowEditModal] = useState(false);
    const [editModalTitle, setEditModalTitle] = useState(undefined);
    const [editModalOkText, setEditModalOkText] = useState(undefined);
    const [editModalOkCallback, setEditModalOkCallback] = useState(undefined);
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [deletionQuestion, setDeletionQuestion] = useState();
    const [deleteContext, setDeleteContext] = useState();

    const validationSchema = object().shape({
    });

    if (postingJournal === undefined || postingJournal === null || postingJournal.modifiable !== true) {
        return (
            <>
            </>
        );
    }

    return (
        <Stack gap={3}>
            <p className='mb-1 fw-bold'>{postingJournal.postingJournalHeader}</p>
            <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
                <Formik validationSchema={validationSchema} initialValues={{}} onSubmit={editModalOkCallback}>
                    {({ handleSubmit, handleReset }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>{editModalTitle}</Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                            </Modal.Body>
                            <Modal.Footer>
                                <SubmitToolbar submitText={editModalOkText} submitVariant='primary' staticTexts={staticTexts} onReset={handleReset} onCancel={() => setShowEditModal(false)} />
                            </Modal.Footer>
                        </Form>
                    )}
                </Formik>
            </Modal>
            <DeleteConfirmation
                show={showDeleteConfirmation} 
                title={staticTextHelper.getConfirmDeletionText(staticTexts)} 
                deletionQuestion={deletionQuestion} 
                verificationInfo={staticTextHelper.getDeleteVerificationInfoText(staticTexts)}
                cancelText={staticTextHelper.getCancelText(staticTexts)}
                deleteText={staticTextHelper.getDeleteText(staticTexts)}
                deleteContext={deleteContext}
                onClose={() => setShowDeleteConfirmation(false)}
                onDelete={handleDelete} />
            <Table className='p-0' responsive={true}>
                <thead>
                    <tr>
                        <th className='text-nowrap'>{postingJournal.postingDateHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournal.postingReferenceHeader}</th>
                        <th className='d-none d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournal.accountHeader}</th>
                        <th>{postingJournal.postingTextHeader}</th>
                        <th className='d-none d-sm-none d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournal.budgetAccountHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournal.debitHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournal.creditHeader}</th>
                        <th className='d-table-cell d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-none d-xxl-none text-end text-nowrap'>{postingJournal.postingValueHeader}</th>
                        <th className='text-nowrap text-end'>
                            <button type='button' className='btn btn-link p-0 border-0 align-baseline text-decoration-none' onClick={() => handleAddPostingJournalLine(staticTextHelper.getAddPostingJournalLineText(staticTexts), staticTextHelper.getCreateText(staticTexts))}>
                                <FontAwesomeIcon icon={faPlus} />
                            </button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {postingJournal.postingJournalLines.map(getPostingJournalLineContent)}
                </tbody>
            </Table>
        </Stack>
    );

    function getPostingJournalLineContent(postingJournalLineDisplayer) {
        return (
            <tr key={postingJournalLineDisplayer.identifier ?? postingJournalLineDisplayer.identifierAsText}>
                <td className='text-nowrap'>{postingJournalLineDisplayer.postingDateAsText}</td>
                <td className='d-none d-sm-none d-md-none d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournalLineDisplayer.postingReference}</td>
                <td className='d-none d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournalLineDisplayer.account}</td>
                <td>{postingJournalLineDisplayer.postingText}</td>
                <td className='d-none d-sm-none d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournalLineDisplayer.budgetAccount}</td>
                <td className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournalLineDisplayer.debitAsText}</td>
                <td className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournalLineDisplayer.creditAsText}</td>
                <td className='d-table-cell d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-none d-xxl-none text-end text-nowrap'>{postingJournalLineDisplayer.postingValueAsText}</td>
                <td className='text-nowrap text-end'>
                    <span className='fa-stack'>
                        <button type='button' className='btn btn-link p-0 border-0 align-baseline text-decoration-none me-2' onClick={() => handleModifyPostingJournalLine(staticTextHelper.getUpdatePostingJournalLineText(staticTexts), staticTextHelper.getUpdateText(staticTexts), postingJournalLineDisplayer)}>
                            <FontAwesomeIcon icon={faPen} />
                        </button>
                        <button type='button' className='btn btn-link p-0 border-0 align-baseline text-decoration-none' onClick={() => confirmDeletion(staticTextHelper.getPostingJournalLineDeletionQuestionText(staticTexts, postingJournalLineDisplayer.postingText), postingJournalLineDisplayer.identifier ?? postingJournalLineDisplayer.identifierAsText)}>
                            <FontAwesomeIcon icon={faTrash} />
                        </button>
                    </span>
                </td>
            </tr>
        );
    }

    function handleAddPostingJournalLine(editModalTitle, editModalOkText) {
        setEditModalTitle(editModalTitle);
        setEditModalOkText(editModalOkText);
        setEditModalOkCallback(() => handleCreatePostingJournalLine);
        setShowEditModal(true);
    }

    function handleCreatePostingJournalLine() {
        console.debug('handleCreatePostingJournalLine');

        return undefined;
    }

    function handleModifyPostingJournalLine(editModalTitle, editModalOkText, postingJournalLineDisplayer) {
        void postingJournalLineDisplayer;
        setEditModalTitle(editModalTitle);
        setEditModalOkText(editModalOkText);
        setEditModalOkCallback(() => handleUpdatePostingJournalLine);
        setShowEditModal(true);
    }

    function handleUpdatePostingJournalLine() {
        console.debug('handleUpdatePostingJournalLine');

        return undefined;
    }

    function confirmDeletion(deletionQuestion, deleteContext) {
        setDeletionQuestion(deletionQuestion);
        setDeleteContext(deleteContext);
        setShowDeleteConfirmation(true);
    }

    async function handleDelete(deleteContext, verificationKey, verificationCode) {
        console.debug(`deleteContext=${deleteContext}`);
        console.debug(`verificationKey=${verificationKey}`);
        console.debug(`verificationCode=${verificationCode}`);

        setShowDeleteConfirmation(false);

        return undefined;
    }
}

export default PostingJournal;