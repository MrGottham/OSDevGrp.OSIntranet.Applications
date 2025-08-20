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

    getAccountingDeletionQuestionText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'AccountingDeletionQuestion');
    }

    getAccountingNumberText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'AccountingNumber');
    }

    getAccountingNameText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'AccountingName');
    }

    getBalanceBelowZeroText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'BalanceBelowZero');
    }

    getDebtorsText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Debtors');
    }

    getCreditorsText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Creditors');
    }

    getBackDatingText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'BackDating');
    }

    getMasterDataText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'MasterData');
    }

    getLetterHeadText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'LetterHead');
    }

    getCurrentStatusText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'CurrentStatus');
    }

    getCommonDataText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'CommonData');
    }

    getCreateText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Create');
    }

    getUpdateText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Update');
    }

    getDeleteText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Delete');
    }

    getConfirmDeletionText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'ConfirmDeletion');
    }

    getDeleteVerificationInfoText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'DeleteVerificationInfo');
    }

    getResetText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Reset');
    }

    getCancelText(staticTexts) {
        return this.getStaticTextByKey(staticTexts, 'Cancel');
    }

    getStaticTextByKey(staticTexts, key) {
        if (staticTexts === undefined || staticTexts === null || Array.isArray(staticTexts) === false) {
            throw new Error('An array of static texts is required.');
        }

        if (key === undefined || key === null || key.trim() === '') {
            throw new Error('Key for static text is required.');
        }

        const found = staticTexts.find(item => item.key === key);
        if (found) {
            return found.text;
        }

        throw Error('No static text was found for the key named: ' + key);
    }
}