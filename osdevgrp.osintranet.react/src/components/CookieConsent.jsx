import { useContext, useState, useEffect } from 'react';
import {CookieConsent as CookieConsentForReact, OPTIONS } from 'react-cookie-consent';
import { ServiceContext } from '../contexts/ServiceContext';
import { HelperContext } from '../contexts/HelperContext';

function CookieConsent() {
    const homeService = useContext(ServiceContext).homeService;
    const staticTextHelper = useContext(HelperContext).staticTextHelper;
    const [cookieConsent, setCookieConsent] = useState();

    useEffect(() => {
        populateCookieConsent();
    }, []);

    if (cookieConsent === undefined) {
        return (
            <>
            </>
        );
    }

    return (
        <CookieConsentForReact disableStyles={true} location={OPTIONS.BOTTOM} buttonText={staticTextHelper.getStaticTextByKey(cookieConsent.staticTexts, 'AllowNecessaryCookies')} cookieName={cookieConsent.cookieName} cookieValue={cookieConsent.cookieValue} expires={cookieConsent.daysUntilExpiry} buttonClasses='btn btn-primary right' containerClasses='alert alert-warning mb-6'>
            <p><b>{staticTextHelper.getStaticTextByKey(cookieConsent.staticTexts, 'WebsiteUsingCookies')}</b></p>
            <p className='small'>{staticTextHelper.getStaticTextByKey(cookieConsent.staticTexts, 'CookieConsentInformation')}</p>
        </CookieConsentForReact>
    );

    async function populateCookieConsent() {
        const json = await homeService.getCookieConsent('OSDevGrp.OSIntranet.React');
        setCookieConsent(json);
    }
}

export default CookieConsent;