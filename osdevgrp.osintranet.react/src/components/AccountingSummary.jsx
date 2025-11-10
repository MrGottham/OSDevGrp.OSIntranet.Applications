import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClock } from '@fortawesome/free-solid-svg-icons';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Stack from 'react-bootstrap/Stack';
import Table from 'react-bootstrap/Table'
import Loading from './Loading';

function AccountingSummary({ accountingNumber }) {
    const { showBoundary } = useErrorBoundary();
    const accountingService = useContext(ServiceContext).accountingService;
    const [content, setContent] = useState();

    useEffect(() => {
        populateContent(accountingNumber)
            .catch(error => showBoundary(error));
    }, []);

    if (content === undefined) {
        return (
            <Loading />
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <Stack gap={0}>
                        <h2>{content.name}</h2>
                        <Stack className='align-items-center mb-2' direction='horizontal' gap={2}>
                            <FontAwesomeIcon icon={faClock} />
                            <p className='mb-0'>{content.statusDate.value}</p>
                        </Stack>
                    </Stack>
                </Col>
            </Row>
            <Row>
                <Col xs={12} sm={12} md={12} lg={6} xl={4} xxl={4}>
                    <Stack gap={3}>
                        <p>Posting lines!</p>
                        {getBudgetStatementContent(content.budgetStatementForMonthOfStatusDate, 'd-block d-sm-block d-md-block d-lg-none d-xl-none d-xxl-none')}
                        {getBalanceSheetContent(content.balanceSheetAtStatusDate, 'd-block d-sm-block d-md-block d-lg-none d-xl-none d-xxl-none')}
                        {getObligeePartiesContent(content.obligeePartiesAtStatusDate, 'd-block d-sm-block d-md-block d-lg-none d-xl-none d-xxl-none')}
                        <hr />
                    </Stack>
                </Col>
                <Col className='d-none d-lg-block d-xl-block d-xxl-block' lg={6} xl={4} xxl={4}>
                    <Stack gap={3}>
                        {getBudgetStatementContent(content.budgetStatementForMonthOfStatusDate)}
                        {getBalanceSheetContent(content.balanceSheetAtStatusDate)}
                        {getObligeePartiesContent(content.obligeePartiesAtStatusDate, 'd-block d-sm-block d-md-block d-lg-block d-xl-none d-xxl-none')}
                        <hr />
                    </Stack>
                </Col>
                <Col className='d-none d-xl-block d-xxl-block' xl={4} xxl={4}>
                    <Stack gap={3}>
                        {getObligeePartiesContent(content.obligeePartiesAtStatusDate)}
                        <hr />
                    </Stack>
                </Col>
            </Row>
        </>
    );

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
        const json = await accountingService.getAccountingSummary(accountingNumber);
        setContent(json);
    }
}

export default AccountingSummary;