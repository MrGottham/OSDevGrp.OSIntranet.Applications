import { useContext, useState, useEffect, useCallback } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import Stack from 'react-bootstrap/Stack';
import Image from 'react-bootstrap/Image';
import * as VerificationInputModule from 'react-verification-input';
import Loading from './Loading';

function HumanVerification({ verificationInfo, onVerificationGenerated, onVerificationExpires, onVerificationVerified, onVerificationFailed }) {
    const { showBoundary } = useErrorBoundary();
    const securityService = useContext(ServiceContext).securityService;
    const [verificationKey, setVerificationKey] = useState();
    const [verificationImage, setVerificationImage] = useState();
    const [expiresAt, setExpiresAt] = useState();
    const populateVerification = useCallback(async () => {
        const json = await securityService.generateVerification();
        setVerificationKey(json.verificationKey);
        setVerificationImage(json.verificationImage);
        setExpiresAt(Date.parse(json.expires));
        onVerificationGenerated(json.verificationKey);
    }, [onVerificationGenerated, securityService]);

    useEffect(() => {
        if (verificationKey !== undefined && verificationKey !== null && verificationImage !== undefined && verificationImage !== null) {
            return;
        }

        async function fetchVerification() {
            populateVerification()
            .catch(error => showBoundary(error));
        }
        fetchVerification();
    }, [verificationKey, verificationImage, populateVerification, showBoundary]);

    useEffect(() => {
        if (verificationKey === undefined || verificationKey === null || expiresAt === undefined || expiresAt === null) {
            return;
        }

        const intervalId = setInterval(() => {
            if (expiresAt !== undefined && expiresAt !== null && expiresAt <= Date.now()) {
                onVerificationExpires(verificationKey);
                setVerificationKey(undefined);
                setVerificationImage(undefined);
                setExpiresAt(undefined);
            }
        }, 250);

        return () => clearInterval(intervalId);
    }, [verificationKey, expiresAt, onVerificationExpires]);

    if (verificationKey === undefined || verificationImage === undefined || expiresAt === undefined) {
        return (
            <Loading />
        );
    }

    const VerificationInput = VerificationInputModule.default.default ??  VerificationInputModule.default;

    return (
        <Stack>
            <p>{verificationInfo}</p>
            <Image src={`data:image/png;base64,${verificationImage}`} fluid />
            <VerificationInput 
                length={6}
                validChars='A-Za-z0-9'
                autoFocus={true}
                classNames={{
                    container: 'container-fluid mt-3',
                    character: 'text-center',
                    characterInactive: 'text-center', 
                    characterSelected: 'text-center',
                    characterFilled: 'text-center'
                }}
                onComplete={value => handleComplete(value, onVerificationVerified, onVerificationFailed)}/>
        </Stack>
    );

    async function handleComplete(value, onVerificationVerified, onVerificationFailed) {
        if (value === undefined || value === null) {
            return;
        }

        try {
            const json = await securityService.verifyVerificationCode(verificationKey, value);
            if (json.verified !== undefined && json.verified !== null && json.verified === true) {
                onVerificationVerified(verificationKey, value);
                return;
            }

            onVerificationFailed(verificationKey);
        }
        catch (error) {
            showBoundary(error);
        }
    }
}

export default HumanVerification;