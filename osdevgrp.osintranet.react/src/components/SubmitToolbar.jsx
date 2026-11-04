import { useContext } from 'react';
import { HelperContext } from '../contexts/HelperContext';
import ButtonToolbar from 'react-bootstrap/ButtonToolbar';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';

function SubmitToolbar({ submitText, submitVariant, staticTexts, onReset, onCancel, isDisabled = false }) {
    const staticTextHelper = useContext(HelperContext).staticTextHelper;

    return (
        <ButtonToolbar>
            <ButtonGroup className='me-2'>
                <Button type='submit' variant={submitVariant} disabled={isDisabled}>{submitText}</Button>
            </ButtonGroup>
            <ButtonGroup className='me-2'>
                <Button type='button' variant='secondary' onClick={onReset} disabled={isDisabled}>{staticTextHelper.getResetText(staticTexts)}</Button>
                <Button type='button' variant='secondary' onClick={onCancel} disabled={isDisabled}>{staticTextHelper.getCancelText(staticTexts)}</Button>
            </ButtonGroup>
        </ButtonToolbar>
    );
}

export default SubmitToolbar;