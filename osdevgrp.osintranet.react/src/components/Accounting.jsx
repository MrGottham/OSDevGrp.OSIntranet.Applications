import { useContext, useState, useEffect } from 'react';
import { useParams } from 'react-router';
import { useErrorBoundary } from 'react-error-boundary';
import { Link } from 'react-router';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/esm/Row';
import Col from 'react-bootstrap/esm/Col';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';
import Accordion from 'react-bootstrap/Accordion';
import Stack from 'react-bootstrap/Stack';
import Loading from './Loading';

function Accounting() {
    const { showBoundary } = useErrorBoundary();
    const accountingService = useContext(ServiceContext).accountingService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [content, setContent] = useState();
    const accountingNumber = useParams().accountingNumber;

    useEffect(() => {
        if (content !== undefined) {
            setContent(undefined);
        }
        populateContent(accountingNumber)
            .catch(error => showBoundary(error));
    }, [accountingNumber]);

    if (content === undefined) {
        return (
            <>
                <Loading />
            </>
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <h1>{content.name}</h1>
                </Col>
            </Row>
            {getOperationsContent(content.number, content.modifiable, content.deletable, content.staticTexts, staticTextHelper)}
            {getMasterDataContent(content, content.dynamicTexts, content.staticTexts, staticTextHelper)}
        </>
    );

    function getOperationsContent(accountingNumber, modifiable, deletable, staticTexts, staticTextHelper) {
        if (modifiable === undefined || modifiable === null || deletable === undefined || deletable === null || (modifiable === false && deletable === false)) {
            return (
                <>
                </>
            );
        }

        return (
            <Row className='mb-3'>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <ButtonToolbar>
                        <ButtonGroup className='me-2'>
                            {getUpdateAccountingContent(accountingNumber, modifiable, staticTexts, staticTextHelper)}
                            {getDeleteAccountingContent(accountingNumber, deletable, staticTexts, staticTextHelper)}
                        </ButtonGroup>
                    </ButtonToolbar>
                </Col>
            </Row>
        );
    }

    function getUpdateAccountingContent(accountingNumber, modifiable, staticTexts, staticTextHelper) {
        if (modifiable === undefined || modifiable === null || modifiable === false) {
            return (
                <>
                </>
            );
        }

        return (
            <Button as={Link} to={`/accountings/${accountingNumber}/edit`}><FontAwesomeIcon icon={faPen} />&nbsp;{staticTextHelper.getUpdateAccountingText(staticTexts)}</Button>
        );
    }

    function getDeleteAccountingContent(accountingNumber, deletable, staticTexts, staticTextHelper) {
        if (deletable === undefined || deletable === null || deletable === false) {
            return (
                <>
                </>
            );
        }

        return (
            <Button as={Link} to={`/accountings/${accountingNumber}/remove`}><FontAwesomeIcon icon={faTrash} />&nbsp;{staticTextHelper.getDeleteAccountingText(staticTexts)}</Button>
        );
    }

    function getMasterDataContent(content, dynamicTexts, staticTexts, staticTextHelper) {
        return (
            <Row className='mb-3'>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <Accordion defaultActiveKey={['0']} alwaysOpen>
                        <Accordion.Item eventKey='0'>
                            <Accordion.Header><h2>{staticTextHelper.getMasterDataText(staticTexts)}</h2></Accordion.Header>
                            <Accordion.Body>
                                <Stack gap={3}>
                                    <div>
                                        <p className='small'>{staticTextHelper.getLetterHeadText(staticTexts)}</p>
                                        <p>{content.letterHead.name}</p>
                                    </div>
                                    <div>
                                        <p className='small'>{dynamicTexts.balanceBelowZero.label}</p>
                                        <p>{dynamicTexts.balanceBelowZero.value}</p>
                                    </div>
                                    <div>
                                        <p className='small'>{dynamicTexts.backDating.label}</p>
                                        <p>{dynamicTexts.backDating.value}</p>
                                    </div>
                                </Stack>
                            </Accordion.Body>
                        </Accordion.Item>
                    </Accordion>
                </Col>
            </Row>
        );
    }

    async function populateContent(accountingNumber) {
        const json = await accountingService.getAccounting(accountingNumber);
        setContent(json);
    }
}

export default Accounting;