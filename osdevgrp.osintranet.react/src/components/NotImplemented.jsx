import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleInfo } from '@fortawesome/free-solid-svg-icons';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Alert from 'react-bootstrap/Alert';
import Loading from './Loading';

function NotImplemented() {
    const { showBoundary } = useErrorBoundary();
    const homeService = useContext(ServiceContext).homeService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [content, setContent] = useState();

    useEffect(() => {
        populateContent()
            .catch(error => showBoundary(error));
    }, []);

    if (content === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <Alert variant='info'>
            <Alert.Heading>
                <span>
                    <FontAwesomeIcon icon={faCircleInfo} />&nbsp;<strong>{staticTextHelper.getFunctionalityNotImplmentedText(content.staticTexts)}</strong>
                </span>
            </Alert.Heading>
            <hr />
            <p>{staticTextHelper.getFunctionalityNotImplmentedDetailsText(content.staticTexts)}</p>
        </Alert>
    );

    async function populateContent() {
        const json = await homeService.getNotImplementedContent();
        setContent(json);
    }
}

export default NotImplemented;