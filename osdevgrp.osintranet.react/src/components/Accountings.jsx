import { useContext, useState, useEffect } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import { Link } from 'react-router';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/esm/Row';
import Col from 'react-bootstrap/esm/Col';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';
import Loading from './Loading';
import AccountingCard from './AccountingCard';

function Accountings() {
    const { showBoundary } = useErrorBoundary();
    const accountingService = useContext(ServiceContext).accountingService;
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
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <h1>{staticTextHelper.getAccountingsText(content.staticTexts)}</h1>
                </Col>
            </Row>
            {getOperationsContent(content.creationAllowed, content.staticTexts, staticTextHelper)}
            <Row xs={1} sm={1} md={2} lg={3} xl={4} xxl={4} className='g-3'>
                {content.accountings.map((accounting, index) => getAccountingContent(accounting, index))}
            </Row>
        </>
    )

    function getOperationsContent(creationAllowed, staticTexts, staticTextHelper) {
        if (creationAllowed === undefined || creationAllowed === null || creationAllowed === false) {
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
                            <Button as={Link} to='/accountings/add'>{staticTextHelper.getCreateNewAccountingText(staticTexts)}</Button>
                        </ButtonGroup>
                    </ButtonToolbar>
                </Col>
            </Row>
        );
    }

    function getAccountingContent(accounting, index) {
        return (
            <Col key={index}>
                <AccountingCard accounting={accounting} />
            </Col>
        );
    }

    async function populateContent() {
        const json = await accountingService.getAccountings();
        setContent(json);
    }
}

export default Accountings;