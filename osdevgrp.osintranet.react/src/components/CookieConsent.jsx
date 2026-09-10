import { useContext, useState, useEffect, useCallback } from 'react';
import { useErrorBoundary } from 'react-error-boundary';
import {CookieConsent as CookieConsentForReact, OPTIONS } from 'react-cookie-consent';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';

function CookieConsent() {
    const { showBoundary } = useErrorBoundary();
    const homeService = useContext(ServiceContext).homeService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [cookieConsent, setCookieConsent] = useState();
    const populateContent = useCallback(async () => {
        const json = await homeService.getCookieConsent('OSDevGrp.OSIntranet.React');
        setCookieConsent(json);
    }, [homeService]);

    useEffect(() => {
        async function fetchContent() {
            populateContent()
                .catch(error => showBoundary(error));
        }
        fetchContent();
    }, [populateContent, showBoundary]);

    if (cookieConsent === undefined) {
        return (
            <>
            </>
        );
    }

    return (
        <CookieConsentForReact disableStyles={true} location={OPTIONS.BOTTOM} buttonText={staticTextHelper.getAllowNecessaryCookiesText(cookieConsent.staticTexts)} cookieName={cookieConsent.cookieName} cookieValue={cookieConsent.cookieValue} expires={cookieConsent.daysUntilExpiry} buttonClasses='btn btn-primary right' containerClasses='alert alert-warning mb-6'>
            <p><b>{staticTextHelper.getWebsiteUsingCookiesText(cookieConsent.staticTexts)}</b></p>
            <p className='small'>{staticTextHelper.getCookieConsentInformationText(cookieConsent.staticTexts)}</p>
        </CookieConsentForReact>
    );
}

export default CookieConsent;