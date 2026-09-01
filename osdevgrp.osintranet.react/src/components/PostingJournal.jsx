import { useContext, useState, useEffect, useRef, useCallback, useTransition, useMemo } from 'react';
import { flushSync } from 'react-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { Formik } from 'formik';
import { object } from 'yup';
import { v7 as newGuid, parse as parseGuid } from 'uuid';
import { HelperContext } from '../contexts/HelperContext';
import { ServiceContext } from '../contexts/ServiceContext';
import Table from 'react-bootstrap/Table'
import Stack from 'react-bootstrap/Stack';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Toast from 'react-bootstrap/Toast';
import ToastContainer from 'react-bootstrap/ToastContainer';
import DatePicker from "react-datepicker";
import SubmitToolbar from './SubmitToolbar';
import DeleteConfirmation from './DeleteConfirmation';

function PostingJournal({ postingJournal: initialPostingJournal, staticTexts, validationRuleSet }) {
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const dateHelper = useContext(HelperContext).dateHelper;
    const formHelper = useContext(HelperContext).formHelper;
    const validationSchemaHelper = useContext(HelperContext).validationSchemaHelper;
    const validationRuleSetHelper = useContext(HelperContext).validationRuleSetHelper;
    const accountingService = useContext(ServiceContext).accountingService;
    const [postingJournal] = useState(initialPostingJournal);
    const [accountingNumber, setAccountingNumber] = useState(initialPostingJournal.accountingNumber);
    const [formData, setFormData] = useState({
        postingJournalLineIdentifier: undefined,
        postingDate: dateHelper.getCurrentDate(),
        postingReference: undefined,
        accountNumber: undefined,
        postingText: undefined,
        budgetAccountNumber: undefined,
        debit: undefined,
        credit: undefined,
        contactAccountNumber: undefined,
    });
    const [computedData, setComputedData] = useState({
        account: { name: '', credit: '', available: '' },
        budgetAccount: { name: '', posted: '', available: '' },
        contactAccount: { name: '', balance: '' },
    });
    const [modalState, setModalState] = useState({
        showEditModal: false,
        title: undefined,
        okText: undefined,
        okCallback: undefined,
    });
    const [deleteState, setDeleteState] = useState({
        show: false,
        question: undefined,
        context: undefined,
    });
    const [toasts, setToasts] = useState([]);
    const changeTimeout = 750;
    const accountNumberChangeTimer = useRef(null);
    const budgetAccountNumberChangeTimer = useRef(null);
    const contactAccountNumberChangeTimer = useRef(null);
    const [isAccountPending, startAccountTransition] = useTransition();
    const [isBudgetAccountPending, startBudgetAccountTransition] = useTransition();
    const [isContactAccountPending, startContactAccountTransition] = useTransition();
    const addToast = useCallback((body) => {
        const toastId = newGuid();
        setToasts(prev => [...prev, { id: toastId, body }]);
        setTimeout(() => {
            setToasts(prev => prev.filter(t => t.id !== toastId));
        }, 5000);
    }, []);
    const populateAccountDetails = useCallback(async () => {
        flushSync(() => {
            setComputedData(prev => ({...prev, account: { name: '', credit: '', available: '' }}));
        });

        if (accountingNumber === undefined || accountingNumber === null)  {
            return;
        }

        if (formData.accountNumber === undefined || formData.accountNumber === null || formData.accountNumber.trim() === '') {
            return;
        }

        if (formData.postingDate === undefined || formData.postingDate === null) {
            return;
        }

        try {
            const isoDateString = dateHelper.convertToIsoString(formData.postingDate);
            const response = await accountingService.getAccountSummary(accountingNumber, formData.accountNumber, isoDateString);
            setComputedData(prev => ({
                ...prev,
                account: {
                    name: response.accountName,
                    credit: response.valuesAtStatusDate?.credit?.value,
                    available: response.valuesAtStatusDate?.available?.value,
                }
            }));
        } catch (error) {
            addToast(error?.message || 'Unknown error');
        }
    }, [accountingNumber, formData.accountNumber, formData.postingDate, dateHelper, accountingService, addToast]);
    const populateBudgetAccountDetails = useCallback(async () => {
        flushSync(() => {
            setComputedData(prev => ({ ...prev, budgetAccount: { name: '', posted: '', available: '' }}));
        });

        if (accountingNumber === undefined || accountingNumber === null)  {
            return;
        }

        if (formData.budgetAccountNumber === undefined || formData.budgetAccountNumber === null || formData.budgetAccountNumber.trim() === '') {
            return;
        }

        if (formData.postingDate === undefined || formData.postingDate === null) {
            return;
        }

        try {
            const isoDateString = dateHelper.convertToIsoString(formData.postingDate);
            const response = await accountingService.getBudgetAccountSummary(accountingNumber, formData.budgetAccountNumber, isoDateString);
            setComputedData(prev => ({
                ...prev,
                budgetAccount: {
                    name: response.accountName,
                    posted: response.valuesForMonthOfStatusDate?.posted?.value,
                    available: response.valuesForMonthOfStatusDate?.available?.value,
                }
            }));
        } catch (error) {
            addToast(error.message);
        }
    }, [accountingNumber, formData.budgetAccountNumber, formData.postingDate, dateHelper, accountingService, addToast]);
    const populateContactAccountDetails = useCallback(async () => {
        flushSync(() => {
            setComputedData(prev => ({ ...prev, contactAccount: { name: '', balance: '' }}));
        });

        if (accountingNumber === undefined || accountingNumber === null)  {
            return;
        }

        if (formData.contactAccountNumber === undefined || formData.contactAccountNumber === null || formData.contactAccountNumber.trim() === '') {
            return;
        }

        if (formData.postingDate === undefined || formData.postingDate === null) {
            return;
        }

        try {
            const isoDateString = dateHelper.convertToIsoString(formData.postingDate);
            const response = await accountingService.getContactAccountSummary(accountingNumber, formData.contactAccountNumber, isoDateString);
            setComputedData(prev => ({
                ...prev,
                contactAccount: {
                    name: response.accountName,
                    balance: response.valuesAtStatusDate?.balance?.value,
                }
            }));
        } catch (error) {
            addToast(error.message);
        }
    }, [accountingNumber, formData.contactAccountNumber, formData.postingDate, dateHelper, accountingService, addToast]);

    useEffect(() => {
        return () => setFormData(prev => ({ ...prev, accountNumber: undefined, budgetAccountNumber: undefined, contactAccountNumber: undefined }));
    }, [accountingNumber]);

    useEffect(() => {
        startAccountTransition(async () => {
            await populateAccountDetails();
        });
    }, [accountingNumber, formData.accountNumber, formData.postingDate, startAccountTransition, populateAccountDetails]);

    useEffect(() => {
        startBudgetAccountTransition(async () => {
            await populateBudgetAccountDetails();
        });
    }, [accountingNumber, formData.budgetAccountNumber, formData.postingDate, startBudgetAccountTransition, populateBudgetAccountDetails]);

    useEffect(() => {
        startContactAccountTransition(async () => {
            await populateContactAccountDetails();
        });
    }, [accountingNumber, formData.contactAccountNumber, formData.postingDate, startContactAccountTransition, populateContactAccountDetails]);

    useEffect(() => {
        return () => {
            if (accountNumberChangeTimer.current) {
                clearTimeout(accountNumberChangeTimer.current);
            }
            if (budgetAccountNumberChangeTimer.current) {
                clearTimeout(budgetAccountNumberChangeTimer.current);
            }
            if (contactAccountNumberChangeTimer.current) {
                clearTimeout(contactAccountNumberChangeTimer.current);
            }
        };
    }, [modalState.showEditModal]);

    const validationSchema = useMemo(() => object().shape({
        accountingNumber: validationSchemaHelper.forInteger(validationRuleSet, 'AccountingNumber', { withRequiredValueRule: true, withMinValueRule: true, withMaxValueRule: true }),
        postingJournalLineIdentifier: validationSchemaHelper.forGuid(validationRuleSet, 'PostingJournalLineIdentifier', { withRequiredValueRule: true, withMinLengthRule: true, withMaxLengthRule: true, withPatternRule: true }),
        postingDate: validationSchemaHelper.forDate(validationRuleSet, 'PostingDate', { withRequiredValueRule: true, withMinValueRule: true, withMaxValueRule: true }),
        postingReference: validationSchemaHelper.forString(validationRuleSet, 'PostingReference', { withMinLengthRule: true, withMaxLengthRule: true }),
        accountNumber: validationSchemaHelper.forString(validationRuleSet, 'Account', { withRequiredValueRule: true, withMinLengthRule: true, withMaxLengthRule: true, withPatternRule: true }),
        postingText: validationSchemaHelper.forString(validationRuleSet, 'PostingText', { withRequiredValueRule: true, withMinLengthRule: true, withMaxLengthRule: true }),
        budgetAccountNumber: validationSchemaHelper.forString(validationRuleSet, 'BudgetAccount', { withMinLengthRule: true, withMaxLengthRule: true, withPatternRule: true }),
        debit: validationSchemaHelper.forCurrency(validationRuleSet, 'Debit', { withMinValueRule: true, withMaxValueRule: true }),
        credit: validationSchemaHelper.forCurrency(validationRuleSet, 'Credit', { withMinValueRule: true, withMaxValueRule: true }),
        contactAccountNumber: validationSchemaHelper.forString(validationRuleSet, 'ContactAccount', { withMinLengthRule: true, withMaxLengthRule: true, withPatternRule: true }),
    }), [validationRuleSet, validationSchemaHelper]);

    if (postingJournal === undefined || postingJournal === null || postingJournal.modifiable !== true) {
        return (
            <>
            </>
        );
    }

    return (
        <Stack gap={3}>
            <p className='mb-1 fw-bold'>{postingJournal.postingJournalHeader}</p>
            <ToastContainer position='top-end' className='p-3 posting-journal__toast-container'>
                {toasts.map(toast => (
                    <Toast 
                        key={toast.id} 
                        onClose={() => setToasts(prev => prev.filter(t => t.id !== toast.id))} 
                        show={true}
                        bg='warning'
                        className='posting-journal__toast'
                    >
                        <Toast.Header closeButton className='posting-journal__toast-header' />
                        <Toast.Body className='text-dark'>{toast.body}</Toast.Body>
                    </Toast>
                ))}
            </ToastContainer>
            <Modal show={modalState.showEditModal} onHide={() => setModalState(prev => ({...prev, showEditModal: false}))}>
                <Formik validationSchema={validationSchema} initialValues={{ accountingNumber: accountingNumber, ...formData }} onSubmit={modalState.okCallback}>
                    {({ handleSubmit, handleReset, handleChange, setFieldValue, setFieldTouched, values, touched, errors }) => {
                        const postingDateInvalid = touched.postingDate && Boolean(errors.postingDate);                        

                        return (
                            <Form noValidate onSubmit={handleSubmit}>
                                <Form.Control type='hidden' name='accountingNumber' value={values.accountingNumber} />
                                <Form.Control type='hidden' name='postingJournalLineIdentifier' value={values.postingJournalLineIdentifier} />
                                <Modal.Header closeButton>
                                    <Modal.Title>{modalState.title}</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikPostingDate'>
                                            <Form.Label>{postingJournal.postingDateHeader}</Form.Label>
                                            <DatePicker name='postingDate' locale='da' dateFormat='dd-MM-yyyy' todayButton={staticTextHelper.getTodayText(staticTexts)}
                                                selected={values.postingDate instanceof Date ? dateHelper.getDateOnly(values.postingDate) : null}
                                                minDate={validationRuleSetHelper.getMinValue(validationRuleSet, 'PostingDate', value => dateHelper.getDateOnly(value))}
                                                maxDate={validationRuleSetHelper.getMaxValue(validationRuleSet, 'PostingDate', value => dateHelper.getDateOnly(value))}
                                                onChange={(date) => {
                                                    const dateOnly = dateHelper.getDateOnly(date);
                                                    setFieldValue('postingDate', dateOnly, true);
                                                    setFormData(prev => ({...prev, postingDate: dateOnly}));
                                                }}
                                                onBlur={() => setFieldTouched('postingDate', true, true)}
                                                className={`form-control ${touched.postingDate && errors.postingDate ? 'is-invalid' : ''}`} wrapperClassName='w-100' disabled={isAccountPending || isBudgetAccountPending || isContactAccountPending} />
                                            {postingDateInvalid && (
                                                <Form.Control.Feedback type='invalid' className='d-block'>{errors.postingDate}</Form.Control.Feedback>
                                            )}
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikPostingReference'>
                                            <Form.Label>{postingJournal.postingReferenceHeader}</Form.Label>
                                            <Form.Control type='text' name='postingReference' value={values.postingReference} onChange={handleChange} isValid={touched.postingReference && !errors.postingReference} isInvalid={!!errors.postingReference} />
                                            <Form.Control.Feedback type='invalid'>{errors.postingReference}</Form.Control.Feedback>
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-2'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={4} xl={4} xxl={4} controlId='formikAccountNumber'>
                                            <Form.Label>{postingJournal.accountHeader}</Form.Label>
                                            <Form.Control type='text' name='accountNumber' value={values.accountNumber} readOnly={isAccountPending}
                                                onChange={(e) => {
                                                    const upperValue = e.target.value.toUpperCase();
                                                    setFieldValue('accountNumber', upperValue, true);

                                                    if (accountNumberChangeTimer.current) {
                                                        clearTimeout(accountNumberChangeTimer.current);
                                                    }

                                                    accountNumberChangeTimer.current = setTimeout(() => setFormData(prev => ({...prev, accountNumber: upperValue})), changeTimeout);
                                                }} 
                                                isValid={touched.accountNumber && !errors.accountNumber} isInvalid={!!errors.accountNumber} />
                                            <Form.Control.Feedback type='invalid'>{errors.accountNumber}</Form.Control.Feedback>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={8} xl={8} xxl={8} controlId='formikAccountName'>
                                            <Form.Label>{postingJournal.accountNameLabel}</Form.Label>
                                            <Form.Control type='text' name='accountName' value={computedData.account.name} readOnly={true} disabled={true} />
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={4} xl={4} xxl={4}>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={3} sm={3} md={3} lg={4} xl={4} xxl={4} controlId='formikAccountCredit'>
                                            <Form.Label>{postingJournal.accountCreditLabel}</Form.Label>
                                            <Form.Control type='text' name='accountCredit' value={computedData.account.credit} readOnly={true} disabled={true} />
                                        </Form.Group>
                                        <Form.Group as={Col} xs={3} sm={3} md={3} lg={4} xl={4} xxl={4} controlId='formikAccountAvailable'>
                                            <Form.Label>{postingJournal.accountAvailableLabel}</Form.Label>
                                            <Form.Control type='text' name='accountAvailable' value={computedData.account.available} readOnly={true} disabled={true} />
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={12} sm={12} md={12} lg={12} xl={12} xxl={12} controlId='formikPostingText'>
                                            <Form.Label>{postingJournal.postingTextHeader}</Form.Label>
                                            <Form.Control type='text' name='postingText' value={values.postingText} onChange={handleChange} isValid={touched.postingText && !errors.postingText} isInvalid={!!errors.postingText} />
                                            <Form.Control.Feedback type='invalid'>{errors.postingText}</Form.Control.Feedback>
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-2'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={4} xl={4} xxl={4} controlId='formikBudgetAccountNumber'>
                                            <Form.Label>{postingJournal.budgetAccountHeader}</Form.Label>
                                            <Form.Control type='text' name='budgetAccountNumber' value={values.budgetAccountNumber} readOnly={isBudgetAccountPending}
                                                onChange={(e) => {
                                                    const upperValue = e.target.value.toUpperCase();
                                                    setFieldValue('budgetAccountNumber', upperValue, true);

                                                    if (budgetAccountNumberChangeTimer.current) {
                                                        clearTimeout(budgetAccountNumberChangeTimer.current);
                                                    }

                                                    budgetAccountNumberChangeTimer.current = setTimeout(() => setFormData(prev => ({...prev, budgetAccountNumber: upperValue})), changeTimeout);
                                                }} 
                                                isValid={touched.budgetAccountNumber && !errors.budgetAccountNumber} isInvalid={!!errors.budgetAccountNumber} />
                                            <Form.Control.Feedback type='invalid'>{errors.budgetAccountNumber}</Form.Control.Feedback>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={8} xl={8} xxl={8} controlId='formikBudgetAccountName'>
                                            <Form.Label>{postingJournal.budgetAccountNameLabel}</Form.Label>
                                            <Form.Control type='text' name='budgetAccountName' value={computedData.budgetAccount.name} readOnly={true} disabled={true} />
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={4} xl={4} xxl={4}>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={3} sm={3} md={3} lg={4} xl={4} xxl={4} controlId='formikBudgetAccountPosted'>
                                            <Form.Label>{postingJournal.budgetAccountPostedLabel}</Form.Label>
                                            <Form.Control type='text' name='budgetAccountPosted' value={computedData.budgetAccount.posted} readOnly={true} disabled={true} />
                                        </Form.Group>
                                        <Form.Group as={Col} xs={3} sm={3} md={3} lg={4} xl={4} xxl={4} controlId='formikBudgetAccountAvailable'>
                                            <Form.Label>{postingJournal.budgetAccountAvailableLabel}</Form.Label>
                                            <Form.Control type='text' name='budgetAccountAvailable' value={computedData.budgetAccount.available} readOnly={true} disabled={true} />
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={6} xl={6} xxl={6} controlId='formikDebit'>
                                            <Form.Label>{postingJournal.debitHeader}</Form.Label>
                                            <Form.Control type='number' name='debit' value={values.debit} onChange={handleChange} isValid={touched.debit && !errors.debit} isInvalid={!!errors.debit} />
                                            <Form.Control.Feedback type='invalid'>{errors.debit}</Form.Control.Feedback>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={6} xl={6} xxl={6} controlId='formikCredit'>
                                            <Form.Label>{postingJournal.creditHeader}</Form.Label>
                                            <Form.Control type='number' name='credit' value={values.credit} onChange={handleChange} isValid={touched.credit && !errors.credit} isInvalid={!!errors.credit} />
                                            <Form.Control.Feedback type='invalid'>{errors.credit}</Form.Control.Feedback>
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-2'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={4} xl={4} xxl={4} controlId='formikContactAccountNumber'>
                                            <Form.Label>{postingJournal.contactAccountHeader}</Form.Label>
                                            <Form.Control type='text' name='contactAccountNumber' value={values.contactAccountNumber} readOnly={isContactAccountPending}
                                                onChange={(e) => {
                                                    const upperValue = e.target.value.toUpperCase();
                                                    setFieldValue('contactAccountNumber', upperValue, true);

                                                    if (contactAccountNumberChangeTimer.current) {
                                                        clearTimeout(contactAccountNumberChangeTimer.current);
                                                    }

                                                    contactAccountNumberChangeTimer.current = setTimeout(() => setFormData(prev => ({...prev, contactAccountNumber: upperValue})), changeTimeout);
                                                }} 
                                                isValid={touched.contactAccountNumber && !errors.contactAccountNumber} isInvalid={!!errors.contactAccountNumber} />
                                            <Form.Control.Feedback type='invalid'>{errors.contactAccountNumber}</Form.Control.Feedback>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={8} xl={8} xxl={8} controlId='formikContactAccountName'>
                                            <Form.Label>{postingJournal.contactAccountNameLabel}</Form.Label>
                                            <Form.Control type='text' name='contactAccountName' value={computedData.contactAccount.name} readOnly={true} disabled={true} />
                                        </Form.Group>
                                    </Row>
                                    <Row className='mb-3'>
                                        <Form.Group as={Col} xs={6} sm={6} md={6} lg={4} xl={4} xxl={4}>
                                        </Form.Group>
                                        <Form.Group as={Col} xs={3} sm={3} md={3} lg={4} xl={4} xxl={4} controlId='formikContactAccountBalance'>
                                            <Form.Label>{postingJournal.contactAccountBalanceLabel}</Form.Label>
                                            <Form.Control type='text' name='contactAccountBalance' value={computedData.contactAccount.balance} readOnly={true} disabled={true} />
                                        </Form.Group>
                                        <Form.Group as={Col} xs={3} sm={3} md={3} lg={4} xl={4} xxl={4}>
                                        </Form.Group>
                                    </Row>
                                </Modal.Body>
                                <Modal.Footer>
                                    <SubmitToolbar submitText={modalState.okText} submitVariant='primary' staticTexts={staticTexts} onReset={handleReset} onCancel={() => setModalState(prev => ({...prev, showEditModal: false}))} />
                                </Modal.Footer>
                            </Form>
                        );
                    }}
                </Formik>
            </Modal>
            <DeleteConfirmation
                show={deleteState.show} 
                title={staticTextHelper.getConfirmDeletionText(staticTexts)} 
                deletionQuestion={deleteState.question} 
                verificationInfo={staticTextHelper.getDeleteVerificationInfoText(staticTexts)}
                cancelText={staticTextHelper.getCancelText(staticTexts)}
                deleteText={staticTextHelper.getDeleteText(staticTexts)}
                deleteContext={deleteState.context}
                onClose={() => setDeleteState(prev => ({...prev, show: false}))}
                onDelete={handleDelete} />
            <Table className='p-0' responsive={true}>
                <thead>
                    <tr>
                        <th className='text-nowrap'>{postingJournal.postingDateHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournal.postingReferenceHeader}</th>
                        <th className='d-none d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournal.accountHeader}</th>
                        <th>{postingJournal.postingTextHeader}</th>
                        <th className='d-none d-sm-none d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournal.budgetAccountHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournal.debitHeader}</th>
                        <th className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournal.creditHeader}</th>
                        <th className='d-table-cell d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-none d-xxl-none text-end text-nowrap'>{postingJournal.postingValueHeader}</th>
                        <th className='text-nowrap text-end'>
                            <button type='button' className='btn btn-link p-0 border-0 align-baseline text-decoration-none' onClick={() => handleAddPostingJournalLine(accountingNumber, newGuid(), dateHelper.getCurrentDate(), formHelper.asNullableString(undefined), formHelper.asNullableString(undefined), formHelper.asNullableString(undefined), formHelper.asNullableString(undefined), formHelper.asCurrency(0, true), formHelper.asCurrency(0, true), formHelper.asNullableString(undefined), staticTextHelper.getAddPostingJournalLineText(staticTexts), staticTextHelper.getCreateText(staticTexts))}>
                                <FontAwesomeIcon icon={faPlus} />
                            </button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {postingJournal.postingJournalLines.map(postingJournalLineDisplayer => getPostingJournalLineContent(accountingNumber, postingJournalLineDisplayer))}
                </tbody>
            </Table>
        </Stack>
    );

    function getPostingJournalLineContent(accountingNumber, postingJournalLineDisplayer) {
        const postingJournalLineIdentifier = parseGuid(postingJournalLineDisplayer.identifier ?? postingJournalLineDisplayer.identifierAsText);

        return (
            <tr key={postingJournalLineIdentifier}>
                <td className='text-nowrap'>{postingJournalLineDisplayer.postingDateAsText}</td>
                <td className='d-none d-sm-none d-md-none d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournalLineDisplayer.postingReference}</td>
                <td className='d-none d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournalLineDisplayer.account}</td>
                <td>{postingJournalLineDisplayer.postingText}</td>
                <td className='d-none d-sm-none d-md-table-cell d-lg-table-cell d-xl-table-cell d-xxl-table-cell text-nowrap'>{postingJournalLineDisplayer.budgetAccount}</td>
                <td className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournalLineDisplayer.debitAsText}</td>
                <td className='d-none d-sm-none d-md-none d-lg-none d-xl-table-cell d-xxl-table-cell text-end text-nowrap'>{postingJournalLineDisplayer.creditAsText}</td>
                <td className='d-table-cell d-sm-table-cell d-md-table-cell d-lg-table-cell d-xl-none d-xxl-none text-end text-nowrap'>{postingJournalLineDisplayer.postingValueAsText}</td>
                <td className='text-nowrap text-end'>
                    <span className='fa-stack'>
                        <button type='button' className='btn btn-link p-0 border-0 align-baseline text-decoration-none me-2' onClick={() => handleModifyPostingJournalLine(accountingNumber, postingJournalLineIdentifier, dateHelper.getDateOnly(postingJournalLineDisplayer.postingDate), formHelper.asNullableString(postingJournalLineDisplayer.postingReference), postingJournalLineDisplayer.account, postingJournalLineDisplayer.postingText, formHelper.asNullableString(postingJournalLineDisplayer.budgetAccount), formHelper.asCurrency(postingJournalLineDisplayer.debit, true), formHelper.asCurrency(postingJournalLineDisplayer.credit, true), formHelper.asNullableString(postingJournalLineDisplayer.contactAccount), staticTextHelper.getUpdatePostingJournalLineText(staticTexts), staticTextHelper.getUpdateText(staticTexts))}>
                            <FontAwesomeIcon icon={faPen} />
                        </button>
                        <button type='button' className='btn btn-link p-0 border-0 align-baseline text-decoration-none' onClick={() => confirmDeletion(staticTextHelper.getPostingJournalLineDeletionQuestionText(staticTexts, postingJournalLineDisplayer.postingText), { accoutingNumber: accountingNumber, postingJournalLineIdentifier: postingJournalLineIdentifier })}>
                            <FontAwesomeIcon icon={faTrash} />
                        </button>
                    </span>
                </td>
            </tr>
        );
    }

    function handleAddPostingJournalLine(accountingNumber, postingJournalLineIdentifier, postingDate, postingReference, accountNumber, postingText, budgetAccountNumber, debit, credit, contactAccountNumber, editModalTitle, editModalOkText) {
        setAccountingNumber(accountingNumber);
        setFormData({
            postingJournalLineIdentifier: postingJournalLineIdentifier,
            postingDate: postingDate,
            postingReference: postingReference,
            accountNumber: accountNumber,
            postingText: postingText,
            budgetAccountNumber: budgetAccountNumber,
            debit: debit,
            credit: credit,
            contactAccountNumber: contactAccountNumber,
        });
        setComputedData({
            account: { name: '', credit: '', available: '' },
            budgetAccount: { name: '', posted: '', available: '' },
            contactAccount: { name: '', balance: '' },
        });
        setModalState({
            showEditModal: true,
            title: editModalTitle,
            okText: editModalOkText,
            okCallback: handleCreatePostingJournalLine,
        });
    }

    function handleCreatePostingJournalLine(values) {
        console.debug('handleCreatePostingJournalLine');
        console.debug(`- values=${JSON.stringify(values)}`);

        return undefined;
    }

    function handleModifyPostingJournalLine(accountingNumber, postingJournalLineIdentifier, postingDate, postingReference, accountNumber, postingText, budgetAccountNumber, debit, credit, contactAccountNumber, editModalTitle, editModalOkText) {
        setAccountingNumber(accountingNumber);
        setFormData({
            postingJournalLineIdentifier: postingJournalLineIdentifier,
            postingDate: postingDate,
            postingReference: postingReference,
            accountNumber: accountNumber,
            postingText: postingText,
            budgetAccountNumber: budgetAccountNumber,
            debit: debit,
            credit: credit,
            contactAccountNumber: contactAccountNumber,
        });
        setComputedData({
            account: { name: '', credit: '', available: '' },
            budgetAccount: { name: '', posted: '', available: '' },
            contactAccount: { name: '', balance: '' },
        });
        setModalState({
            showEditModal: true,
            title: editModalTitle,
            okText: editModalOkText,
            okCallback: handleUpdatePostingJournalLine,
        });
    }

    function handleUpdatePostingJournalLine(values) {
        console.debug('handleUpdatePostingJournalLine');
        console.debug(`- values=${JSON.stringify(values)}`);

        return undefined;
    }

    function confirmDeletion(deletionQuestion, deleteContext) {
        setDeleteState({
            show: true,
            question: deletionQuestion,
            context: deleteContext,
        });
    }

    async function handleDelete(deleteContext, verificationKey, verificationCode) {
        console.debug('handleDelete');
        console.debug(`- deleteContext=${deleteContext}`);
        console.debug(`- verificationKey=${verificationKey}`);
        console.debug(`- verificationCode=${verificationCode}`);

        setDeleteState(prev => ({...prev, show: false}));

        return undefined;
    }
}

export default PostingJournal;