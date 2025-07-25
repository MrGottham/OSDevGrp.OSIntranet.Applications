import AccountingHelper from "./AccountingHelper";

export default class AuthorizationHelper {
    #accountingHelper = new AccountingHelper();

    authenticatedUser(userInfo) {
        return userInfo !== undefined && userInfo !== null;
    }

    hasAccountingAccess(userInfo) {
        return this.authenticatedUser(userInfo) && userInfo.hasAccountingAccess !== undefined && userInfo.hasAccountingAccess !== null && userInfo.hasAccountingAccess === true;
    }

    isAccountingAdministrator(userInfo) {
        return this.hasAccountingAccess(userInfo) && userInfo.isAccountingAdministrator !== undefined && userInfo.isAccountingAdministrator !== null && userInfo.isAccountingAdministrator === true;
    }

    isAccountingCreator(userInfo) {
        return this.hasAccountingAccess(userInfo) && userInfo.isAccountingCreator !== undefined && userInfo.isAccountingCreator !== null && userInfo.isAccountingCreator === true;
    }

    isAccountingModifier(userInfo, accountingNumber) {
        const result = this.hasAccountingAccess(userInfo) && userInfo.isAccountingModifier !== undefined && userInfo.isAccountingModifier !== null && userInfo.isAccountingModifier === true;
        if (result === false || accountingNumber === undefined || accountingNumber === null) {
            return result;
        }

        return this.#accountingHelper.isAccountingNumberInAccountings(accountingNumber, userInfo.modifiableAccountings);
    }

    isAccountingViewer(userInfo, accountingNumber) {
        const result = this.hasAccountingAccess(userInfo) && userInfo.isAccountingViewer !== undefined && userInfo.isAccountingViewer !== null && userInfo.isAccountingViewer === true;
        if (result === false || accountingNumber === undefined || accountingNumber === null) {
            return result;
        }

        return this.#accountingHelper.isAccountingNumberInAccountings(accountingNumber, userInfo.viewableAccountings);
    }

    hasCommonDataAccess(userInfo) {
        return this.authenticatedUser(userInfo) && userInfo.hasCommonDataAccess !== undefined && userInfo.hasCommonDataAccess !== null && userInfo.hasCommonDataAccess === true;
    }
}