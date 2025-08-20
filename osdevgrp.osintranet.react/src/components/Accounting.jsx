import { useContext, useState, useEffect } from 'react';
import { useParams } from 'react-router';
import { useErrorBoundary } from 'react-error-boundary';
import { Link } from 'react-router';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';
import Accordion from 'react-bootstrap/Accordion';
import Stack from 'react-bootstrap/Stack';
import Table from 'react-bootstrap/Table'
import Loading from './Loading';
import DeleteConfirmation from './DeleteConfirmation';

function Accounting() {
    const { showBoundary } = useErrorBoundary();
    const accountingService = useContext(ServiceContext).accountingService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const urlHelper = useContext(HelperContext).urlHelper;
    const [content, setContent] = useState();
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [deletionQuestion, setDeletionQuestion] = useState();
    const [deleteContext, setDeleteContext] = useState();
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
            <Loading />
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <h1>{content.name}</h1>
                </Col>
            </Row>
            <DeleteConfirmation
                show={showDeleteConfirmation} 
                title={staticTextHelper.getConfirmDeletionText(content.staticTexts)} 
                deletionQuestion={deletionQuestion} 
                verificationInfo={staticTextHelper.getDeleteVerificationInfoText(content.staticTexts)}
                cancelText={staticTextHelper.getCancelText(content.staticTexts)}
                deleteText={staticTextHelper.getDeleteText(content.staticTexts)}
                deleteContext={deleteContext}
                onClose={() => setShowDeleteConfirmation(false)}
                onDelete={handleDelete} />
            {getOperationsContent(content.number, content.modifiable, content.deletable, content.staticTexts, staticTextHelper)}
            <Row className='mb-3'>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <Accordion defaultActiveKey={['0', '1']} alwaysOpen>
                        {getMasterDataContent('0', content, content.dynamicTexts, content.staticTexts, staticTextHelper)}
                        {getCurrentStatusContent('1', content.dynamicTexts, content.staticTexts, staticTextHelper)}
                    </Accordion>
                </Col>
            </Row>
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

        const deleteContext = {
            type: 'accounting',
            accountingNumber: accountingNumber
        };

        return (
            <>
                <Button onClick={() => confirmDeletion(staticTextHelper.getAccountingDeletionQuestionText(staticTexts), deleteContext)}><FontAwesomeIcon icon={faTrash} />&nbsp;{staticTextHelper.getDeleteAccountingText(staticTexts)}</Button>
            </>
        );
    }

    function getMasterDataContent(eventKey, content, dynamicTexts, staticTexts, staticTextHelper) {
        return (
            <Accordion.Item eventKey={eventKey}>
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
        );
    }

    function getCurrentStatusContent(eventKey, dynamicTexts, staticTexts, staticTextHelper) {
        return (
            <Accordion.Item eventKey={eventKey}>
                <Accordion.Header><h2>{staticTextHelper.getCurrentStatusText(staticTexts)}</h2></Accordion.Header>
                    <Accordion.Body>
                        <Row>
                            <Col xs={12} sm={12} md={6} lg={4} xl={4} xxl={4}>
                                <Stack gap={3}>
                                    {getBudgetStatementContent(dynamicTexts.budgetStatementForMonthOfStatusDate)}
                                    {getBudgetStatementContent(dynamicTexts.budgetStatementForLastMonthOfStatusDate)}
                                    {getBudgetStatementContent(dynamicTexts.budgetStatementForYearToDateOfStatusDate)}
                                    {getBudgetStatementContent(dynamicTexts.budgetStatementForLastYearOfStatusDate)}
                                    {getBalanceSheetContent(dynamicTexts.balanceSheetAtStatusDate, 'd-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none')}
                                    {getBalanceSheetContent(dynamicTexts.balanceSheetAtEndOfLastMonthFromStatusDate, 'd-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none')}
                                    {getBalanceSheetContent(dynamicTexts.balanceSheetAtEndOfLastYearFromStatusDate, 'd-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none')}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtStatusDate, 'd-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none')}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtEndOfLastMonthFromStatusDate, 'd-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none')}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtEndOfLastYearFromStatusDate, 'd-block d-sm-block d-md-none d-lg-none d-xl-none d-xxl-none')}
                                </Stack>
                            </Col>
                            <Col className='d-none d-md-block d-lg-block d-xl-block d-xxl-block' md={6} lg={4} xl={4} xxl={4}>
                                <Stack gap={3}>
                                    {getBalanceSheetContent(dynamicTexts.balanceSheetAtStatusDate)}
                                    {getBalanceSheetContent(dynamicTexts.balanceSheetAtEndOfLastMonthFromStatusDate)}
                                    {getBalanceSheetContent(dynamicTexts.balanceSheetAtEndOfLastYearFromStatusDate)}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtStatusDate, 'd-block d-sm-block d-md-block d-lg-none d-xl-none d-xxl-none')}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtEndOfLastMonthFromStatusDate, 'd-block d-sm-block d-md-block d-lg-none d-xl-none d-xxl-none')}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtEndOfLastYearFromStatusDate, 'd-block d-sm-block d-md-block d-lg-none d-xl-none d-xxl-none')}
                                </Stack>
                            </Col>
                            <Col className='d-none d-lg-block d-xl-block d-xxl-block' lg={4} xl={4} xxl={4}>
                                <Stack gap={3}>
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtStatusDate)}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtEndOfLastMonthFromStatusDate)}
                                    {getObligeePartiesContent(dynamicTexts.obligeePartiesAtEndOfLastYearFromStatusDate)}
                                </Stack>
                            </Col>
                        </Row>
                    </Accordion.Body>
                </Accordion.Item>
        );
    }

    function getBalanceSheetContent(balanceSheetDisplayer, className) {
        if (balanceSheetDisplayer === undefined || balanceSheetDisplayer === null) {
            return (
                <>
                </>
            )
        }

        return (
            <div className={className === undefined || className === null ? '' : className}>
                <p className='mb-1 fw-bold'>{balanceSheetDisplayer.header}</p>
                <Table className='p-0' borderless={true} size='sm' responsive={true}>
                    <tbody>
                        <tr>
                            <td className='p-0'>{balanceSheetDisplayer.assets.label}</td>
                            <td className='p-0 text-end text-nowrap'>{balanceSheetDisplayer.assets.value}</td>
                        </tr>
                        <tr>
                            <td className='p-0'>{balanceSheetDisplayer.liabilities.label}</td>
                            <td className='p-0 text-end text-nowrap'>{balanceSheetDisplayer.liabilities.value}</td>
                        </tr>
                    </tbody>
                </Table>
            </div>
        );
    }

    function getBudgetStatementContent(budgetStatementDisplayer, className) {
        if (budgetStatementDisplayer === undefined || budgetStatementDisplayer === null) {
            return (
                <>
                </>
            )
        }

        return (
            <div className={className === undefined || className === null ? '' : className}>
                <p className='mb-1 fw-bold'>{budgetStatementDisplayer.header}</p>
                <Table className='p-0' borderless={true} size='sm' responsive={true}>
                    <tbody>
                        <tr>
                            <td className='p-0'>{budgetStatementDisplayer.budget.label}</td>
                            <td className='p-0 text-end text-nowrap'>{budgetStatementDisplayer.budget.value}</td>
                        </tr>
                        <tr>
                            <td className='p-0'>{budgetStatementDisplayer.posted.label}</td>
                            <td className='p-0 text-end text-nowrap'>{budgetStatementDisplayer.posted.value}</td>
                        </tr>
                    </tbody>
                </Table>
            </div>
        );
    }

    function getObligeePartiesContent(obligeePartiesDisplayer, className) {
        if (obligeePartiesDisplayer === undefined || obligeePartiesDisplayer === null) {
            return (
                <>
                </>
            )
        }

        return (
            <div className={className === undefined || className === null ? '' : className}>
                <p className='mb-1 fw-bold'>{obligeePartiesDisplayer.header}</p>
                <Table className='p-0' borderless={true} size='sm' responsive={true}>
                    <tbody>
                        <tr>
                            <td className='p-0'>{obligeePartiesDisplayer.debtors.label}</td>
                            <td className='p-0 text-end text-nowrap'>{obligeePartiesDisplayer.debtors.value}</td>
                        </tr>
                        <tr>
                            <td className='p-0'>{obligeePartiesDisplayer.creditors.label}</td>
                            <td className='p-0 text-end text-nowrap'>{obligeePartiesDisplayer.creditors.value}</td>
                        </tr>
                    </tbody>
                </Table>
            </div>
        );
    }

    async function populateContent(accountingNumber) {
        const json = await accountingService.getAccounting(accountingNumber);
        setContent(json);
    }

    function confirmDeletion(deletionQuestion, deleteContext) {
        setDeletionQuestion(deletionQuestion);
        setDeleteContext(deleteContext);
        setShowDeleteConfirmation(true);
    }

    async function handleDelete(deleteContext, verificationKey, verificationCode) {
        console.debug(`deleteContext=${JSON.stringify(deleteContext)}`);
        console.debug(`verificationKey=${verificationKey}`);
        console.debug(`verificationCode=${verificationCode}`);

        return urlHelper.getNotImplementedUrl();
    }
}

export default Accounting;