import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Loading from './Loading';

function AccessDenied() {
    const { showBoundary } = useErrorBoundary();
    const securityService = useContext(ServiceContext).securityService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [accessDeniedContent, setAccessDeniedContent] = useState();

    useEffect(() => {
        populateAccessDeniedContent()
            .catch(error => showBoundary(error));
    }, []);

    if (accessDeniedContent === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <div className='alert alert-danger'>
            <span>
                <i className='fa-solid fa-circle-xmark'></i>&nbsp;<strong>{staticTextHelper.getAccessDeniedText(accessDeniedContent.staticTexts)}</strong>
            </span>
            <hr />
            <p>{staticTextHelper.getMissingPermissionToPageText(accessDeniedContent.staticTexts)}</p>
            <p>{staticTextHelper.getCheckYourCredentialsText(accessDeniedContent.staticTexts)}</p>
        </div>
    )

    async function populateAccessDeniedContent() {
        const json = await securityService.getAccessDeniedContent();
        setAccessDeniedContent(json);
    }
}

export default AccessDenied;