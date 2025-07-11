import { useContext, useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBug } from '@fortawesome/free-solid-svg-icons';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Alert from 'react-bootstrap/Alert';
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
        <Alert variant='danger' className='mt-5 ms-5 me-5'>
            <Alert.Heading>
                <span>
                    <FontAwesomeIcon icon={faBug} />&nbsp;<strong>{staticTextHelper.getSomethingWentWrongText(errorContent.staticTexts)}</strong>
                </span>
            </Alert.Heading>
            <hr />
            <p>{errorContent.errorMessage}</p>
        </Alert>
    );

    async function populateErrorContent(errorMessage) {
        const json = await homeService.getErrorContent(errorMessage);
        setErrorContent(json);
    }
}

export default Fallback;