import { useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Loading from './Loading';
import AccountingForm from './AccountingForm';

function AddAccounting() {
    const { showBoundary } = useErrorBoundary();
    const accountingService = useContext(ServiceContext).accountingService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const urlHelper = useContext(HelperContext).urlHelper;
    const [content, setContent] = useState();
    const [submitting, setSubmitting] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        if (submitting !== undefined && submitting !== null && submitting) 
        {
            return;
        }

        populateContent()
            .catch(error => showBoundary(error));
    }, [submitting]);

    if (content === undefined || (submitting !== undefined && submitting !== null && submitting)) {
        return (
            <Loading />
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <h1>{staticTextHelper.getCreateNewAccountingText(content.staticTexts)}</h1>
                </Col>
            </Row>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <AccountingForm 
                        readonlyAccountingNumber={false} 
                        letterHeadNumber={content.letterHeads[0].number ?? 0}
                        letterHeads={content.letterHeads} 
                        balanceBelowZero={0} 
                        staticTexts={content.staticTexts} 
                        validationRuleSet={content.validationRuleSet} 
                        submitText={staticTextHelper.getCreateText(content.staticTexts)}
                        onSubmit={submit}
                        cancelUrl={urlHelper.getAccountingsUrl()} />
                </Col>
            </Row>
        </>
    );

    async function populateContent(accountingNumber) {
        const json = await accountingService.getAccountingPreCreation();
        setContent(json);
    }

    function submit(values, actions) {
        try {
            setSubmitting(true);

            console.debug(values);
            console.debug(actions);

            navigate(urlHelper.getNotImplementedUrl(), { replace: true, preventScrollReset: true });
        }
        catch (error) {
            showBoundary(error);
        }
    }
}

export default AddAccounting;