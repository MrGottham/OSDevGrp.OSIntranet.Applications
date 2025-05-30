import { useContext, useState, useEffect } from 'react';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Loading from './Loading';

function Fallback({ error, resetErrorBoundary }) {
    const homeService = useContext(ServiceContext).homeService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [errorContent, setErrorContent] = useState();

    useEffect(() => {
        populateErrorContent(error.message)
            .catch(error => console.error('Error while populating error content:', error));
    }, []);

    if (errorContent === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <div className='alert alert-danger mt-5 ms-5 me-5'>
            <span>
                <i className='fa-solid fa-bug'></i>&nbsp;<strong>{staticTextHelper.getSomethingWentWrongText(errorContent.staticTexts)}</strong>
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