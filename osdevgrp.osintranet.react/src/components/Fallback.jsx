import { useContext, useState, useEffect } from 'react';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Loading from './Loading';

function Fallback({ error, resetErrorBoundary }) {
    const homeService = useContext(ServiceContext).homeService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [errorContent, setErrorContent] = useState();

    useEffect(() => {
        populateErrorContent(error.message);
    }, []);

    if (errorContent === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <div className='alert alert-danger'>
            <span>
                <i className='fa-solid fa-bug'></i>&nbsp;<strong>{staticTextHelper.getStaticTextByKey(errorContent.staticTexts, 'SomethingWentWrong')}</strong>
            </span>
            <hr />
            <p>{errorContent.errorMessage}</p>
        </div>
    );

    async function populateErrorContent(errorMessage) {
        const json = await homeService.getErrorContent(errorMessage);
        setErrorContent(json);
    }
}

export default Fallback;