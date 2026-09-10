import { useContext, useState, useEffect, useCallback } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Alert from 'react-bootstrap/Alert';
import Loading from './Loading';

function AccessDenied() {
    const { showBoundary } = useErrorBoundary();
    const securityService = useContext(ServiceContext).securityService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [accessDeniedContent, setAccessDeniedContent] = useState();
    const populateAccessDeniedContent = useCallback(async () => {
        const json = await securityService.getAccessDeniedContent();
        setAccessDeniedContent(json);
    }, [securityService]);

    useEffect(() => {
        async function fetchAccessDeniedContent() {
            populateAccessDeniedContent()
                .catch(error => showBoundary(error));
        }
        fetchAccessDeniedContent();
    }, [populateAccessDeniedContent, showBoundary]);

    if (accessDeniedContent === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <Alert variant='danger'>
            <Alert.Heading>
                <span>
                    <FontAwesomeIcon icon={faCircleXmark} />&nbsp;<strong>{staticTextHelper.getAccessDeniedText(accessDeniedContent.staticTexts)}</strong>
                </span>
            </Alert.Heading>
            <hr />
            <p>{staticTextHelper.getMissingPermissionToPageText(accessDeniedContent.staticTexts)}</p>
            <p>{staticTextHelper.getCheckYourCredentialsText(accessDeniedContent.staticTexts)}</p>
        </Alert>
    );
}

export default AccessDenied;