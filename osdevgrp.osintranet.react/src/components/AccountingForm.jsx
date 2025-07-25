import { useContext } from 'react';
import { useNavigate } from 'react-router';
import { HelperContext } from '../contexts/HelperContext';
import { Formik } from 'formik';
import { object } from 'yup';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import SubmitToolbar from './SubmitToolbar';

function AccountingForm({accountingNumber, readonlyAccountingNumber, accountingName, letterHeadNumber, letterHeads, balanceBelowZero, backDating, staticTexts, validationRuleSet, submitText, onSubmit, cancelUrl}) {
    const navigate = useNavigate();
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const validationSchemaHelper = useContext(HelperContext).validationSchemaHelper;

    const validationSchema = object().shape({
        accountingNumber: validationSchemaHelper.forInteger(validationRuleSet, 'AccountingNumber', { withRequiredValueRule: true, withMinValueRule: true, withMaxValueRule: true }),
        accountingName: validationSchemaHelper.forString(validationRuleSet, 'AccountingName', { withRequiredValueRule: true, withMinLengthRule: true, withMaxLengthRule: true }),
        letterHeadNumber: validationSchemaHelper.forInteger(validationRuleSet, 'LetterHeadNumber', { withRequiredValueRule: true, withMinValueRule: true, withMaxValueRule: true }),
        balanceBelowZero: validationSchemaHelper.forInteger(validationRuleSet, 'BalanceBelowZero', { withRequiredValueRule: true, withOneOfRule: true }),
        backDating: validationSchemaHelper.forInteger(validationRuleSet, 'BackDating', { withRequiredValueRule: true, withMinValueRule: true, withMaxValueRule: true })
    });

    return (
        <Formik validationSchema={validationSchema} initialValues={{ accountingNumber: accountingNumber, accountingName: accountingName, letterHeadNumber: letterHeadNumber, balanceBelowZero: balanceBelowZero, backDating: backDating }} onSubmit={onSubmit} >
            {({ handleSubmit, handleReset, handleChange, setFieldValue, values, touched, errors }) => (
                <Form noValidate onSubmit={handleSubmit}>
                    <Row className='mb-3'>
                        {getAccountingNumberFormGroup(readonlyAccountingNumber, staticTexts, staticTextHelper, handleChange, values, touched, errors)}
                    </Row>
                    <Row className='mb-3'>
                        {getAccountingNameFormGroup(staticTexts, staticTextHelper, handleChange, values, touched, errors)}
                    </Row>
                    <Row className='mb-3'>
                        {getLetterHeadFormGroup(letterHeads, staticTexts, staticTextHelper, setFieldValue, values, touched, errors)}
                    </Row>
                    <Row className='mb-3'>
                        {getBalanceBelowZeroFormGroup(staticTexts, staticTextHelper, setFieldValue, values, touched, errors)}
                    </Row>
                    <Row className='mb-3'>
                        {getBackDatingFormGroup(staticTexts, staticTextHelper, handleChange, values, touched, errors)}
                    </Row>
                    <Row>
                        <Col xs={12} sm={12} md={12} lg={12} xl={12} xxl={12}>
                            <SubmitToolbar submitText={submitText} submitVariant='primary' staticTexts={staticTexts} onReset={handleReset} onCancel={_ => navigate(cancelUrl, { replace: true, preventScrollReset: true })} />
                        </Col>
                    </Row>
                </Form>
            )}
        </Formik>
    );

    function getAccountingNumberFormGroup(readonlyAccountingNumber, staticTexts, staticTextHelper, handleChange, values, touched, errors) {
        if (readonlyAccountingNumber !== undefined && readonlyAccountingNumber !== null && readonlyAccountingNumber === true)
        {
            return (
                <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikAccountingNumber'>
                    <Form.Label>{staticTextHelper.getAccountingNumberText(staticTexts)}</Form.Label>
                    <Form.Control type='number' name='accountingNumber' value={values.accountingNumber} readOnly={false} disabled={true} isValid={touched.accountingNumber && !errors.accountingNumber} isInvalid={!!errors.accountingNumber} />
                    <Form.Control.Feedback type='invalid'>{errors.accountingNumber}</Form.Control.Feedback>
                </Form.Group>
            );
        }

        return (
            <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikAccountingNumber'>
                <Form.Label>{staticTextHelper.getAccountingNumberText(staticTexts)}</Form.Label>
                <Form.Control type='text' name='accountingNumber' value={values.accountingNumber} onChange={handleChange} isValid={touched.accountingNumber && !errors.accountingNumber} isInvalid={!!errors.accountingNumber} />
                <Form.Control.Feedback type='invalid'>{errors.accountingNumber}</Form.Control.Feedback>
            </Form.Group>
        );
    }

    function getAccountingNameFormGroup(staticTexts, staticTextHelper, handleChange, values, touched, errors) {
        return (
            <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikAccountingName'>
                <Form.Label>{staticTextHelper.getAccountingNameText(staticTexts)}</Form.Label>
                <Form.Control type='text' name='accountingName' value={values.accountingName} onChange={handleChange} isValid={touched.accountingName && !errors.accountingName} isInvalid={!!errors.accountingName} />
                <Form.Control.Feedback type='invalid'>{errors.accountingName}</Form.Control.Feedback>
            </Form.Group>
        );
    }

    function getLetterHeadFormGroup(letterHeads, staticTexts, staticTextHelper, setFieldValue, values, touched, errors) {
        return (
            <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikLetterHeadNumber'>
                <Form.Label>{staticTextHelper.getLetterHeadText(staticTexts)}</Form.Label>
                <Form.Select name='letterHeadNumber' value={values.letterHeadNumber} onChange={e => setFieldValue(e.target.name, parseInt(e.target.value))} isValid={touched.letterHeadNumber && !errors.letterHeadNumber} isInvalid={!!errors.letterHeadNumber}>
                    {letterHeads.map(letterHead => (<option key={letterHead.number} value={letterHead.number}>{letterHead.name}</option>))}
                </Form.Select>
                <Form.Control.Feedback type='invalid'>{errors.letterHeadNumber}</Form.Control.Feedback>
            </Form.Group>
        );
    }

    function getBalanceBelowZeroFormGroup(staticTexts, staticTextHelper, setFieldValue, values, touched, errors) {
        return (
            <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikBalanceBelowZero'>
                <Form.Label>{staticTextHelper.getBalanceBelowZeroText(staticTexts)}</Form.Label>
                <Form.Select name='balanceBelowZero' value={values.balanceBelowZero} onChange={e => setFieldValue(e.target.name, parseInt(e.target.value))} isValid={touched.balanceBelowZero && !errors.balanceBelowZero} isInvalid={!!errors.balanceBelowZero}>
                    <option key={0} value={0}>{staticTextHelper.getDebtorsText(staticTexts)}</option>
                    <option key={1} value={1}>{staticTextHelper.getCreditorsText(staticTexts)}</option>
                </Form.Select>
                <Form.Control.Feedback type='invalid'>{errors.balanceBelowZero}</Form.Control.Feedback>
            </Form.Group>
        );
    }

    function getBackDatingFormGroup(staticTexts, staticTextHelper, handleChange, values, touched, errors) {
        return (
            <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikBackDating'>
                <Form.Label>{staticTextHelper.getBackDatingText(staticTexts)}</Form.Label>
                <Form.Control type='number' name='backDating' value={values.backDating} onChange={handleChange} isValid={touched.backDating && !errors.backDating} isInvalid={!!errors.backDating} />
                <Form.Control.Feedback type='invalid'>{errors.backDating}</Form.Control.Feedback>
            </Form.Group>
        );
    }
}

export default AccountingForm;