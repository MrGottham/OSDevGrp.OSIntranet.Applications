import { useContext, useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router';
import { useErrorBoundary } from 'react-error-boundary';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Loading from './Loading';
import AccessDenid from './AccessDenied';
import AccountingForm from './AccountingForm';

function EditAccounting() {
    const { showBoundary } = useErrorBoundary();
    const accountingService = useContext(ServiceContext).accountingService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const urlHelper = useContext(HelperContext).urlHelper;
    const [content, setContent] = useState();
    const [submitting, setSubmitting] = useState(false);
    const navigate = useNavigate();
    const accountingNumber = useParams().accountingNumber;

    useEffect(() => {
        if (submitting !== undefined && submitting !== null && submitting) 
        {
            return;
        }

        if (content !== undefined) {
            setContent(undefined);
        }

        populateContent(accountingNumber)
            .catch(error => showBoundary(error));
    }, [accountingNumber, submitting]);

    if (content === undefined || (submitting !== undefined && submitting !== null && submitting)) {
        return (
            <Loading />
        );
    }

    if (content.modifiable === undefined || content.modifiable === null || content.modifiable !== true) {
        return (
            <AccessDenid />
        );
    }

    return (
        <>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <h1>{staticTextHelper.getUpdateAccountingText(content.staticTexts)}</h1>
                </Col>
            </Row>
            <Row>
                <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                    <AccountingForm 
                        accountingNumber={content.number} 
                        readonlyAccountingNumber={true} 
                        accountingName={content.name} 
                        letterHeadNumber={content.letterHead.number} 
                        letterHeads={content.letterHeads} 
                        balanceBelowZero={content.balanceBelowZero} 
                        backDating={content.backDating}
                        staticTexts={content.staticTexts} 
                        validationRuleSet={content.validationRuleSet} 
                        submitText={staticTextHelper.getUpdateText(content.staticTexts)}
                        onSubmit={submit}
                        cancelUrl={urlHelper.getAccountingUrl(content.number)} />
                </Col>
            </Row>
        </>
    );

    async function populateContent(accountingNumber) {
        const json = await accountingService.getAccounting(accountingNumber);
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

export default EditAccounting;