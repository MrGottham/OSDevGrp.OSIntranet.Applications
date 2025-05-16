import { useContext, useState, useEffect } from 'react';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Loading from './Loading';

function AccessDenied() {
    const securityService = useContext(ServiceContext).securityService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [accessDeniedContent, setAccessDeniedContent] = useState();

    useEffect(() => {
        populateAccessDeniedContent();
    }, []);

    if (accessDeniedContent === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <div class="alert alert-danger">
            <span>
                <i class="fa-regular fa-circle-xmark"></i>&nbsp;<strong>{staticTextHelper.getStaticTextByKey(accessDeniedContent.staticTexts, 'AccessDenied')}</strong>
            </span>
            <hr />
            <p>{staticTextHelper.getStaticTextByKey(accessDeniedContent.staticTexts, 'MissingPermissionToPage')}</p>
            <p>{staticTextHelper.getStaticTextByKey(accessDeniedContent.staticTexts, 'CheckYourCredentials')}</p>
        </div>
    )

    async function populateAccessDeniedContent() {
        const json = await securityService.getAccessDeniedContent();
        setAccessDeniedContent(json);
    }
}

export default AccessDenied;