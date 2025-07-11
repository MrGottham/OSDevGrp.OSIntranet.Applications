export default class StaticTextHelper {
    getCopyrightText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Copyright');
    }

    getBuildInfoText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'BuildInfo');
    }

    getStartText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Start');
    }

    getLoginText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Login');
    }

    getLogoutText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Logout');
    }

    getAccessDeniedText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'AccessDenied');
    }

    getMissingPermissionToPageText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'MissingPermissionToPage');
    }

    getCheckYourCredentialsText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'CheckYourCredentials');
    }

    getSomethingWentWrongText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'SomethingWentWrong');
    }

    getWebsiteUsingCookiesText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'WebsiteUsingCookies');
    }

    getCookieConsentInformationText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'CookieConsentInformation');
    }

    getAllowNecessaryCookiesText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'AllowNecessaryCookies');
    }

    getFunctionalityNotImplmentedText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'FunctionalityNotImplmented');
    }

    getFunctionalityNotImplmentedDetailsText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'FunctionalityNotImplmentedDetails');
    }

    getMailAddressText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'MailAddress');
    }

    getPermissionsText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Permissions');
    }

    getAdministratorText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Administrator');
    }

    getCreatorText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Creator');
    }

    getModifierText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Modifier');
    }

    getViewerText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Viewer');
    }

    getFinancialManagementText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'FinancialManagement');
    }

    getPrimaryAccountingText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'PrimaryAccounting');
    }

    getAccountingsText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Accountings');
    }

    getCreateNewAccountingText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'CreateNewAccounting');
    }

    getUpdateAccountingText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'UpdateAccounting');
    }

    getDeleteAccountingText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'DeleteAccounting');
    }

    getMasterDataText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'MasterData');
    }

    getLetterHeadText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'LetterHead');
    }

    getCommonDataText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'CommonData');
    }

    getStaticTextByKey(staticTexts, key) {
        const found = staticTexts.find(item => item.key === key);
        if (found) {
            return found.text;
        }

        throw Error('No static text was found for the key named: ' + key);
    }
}