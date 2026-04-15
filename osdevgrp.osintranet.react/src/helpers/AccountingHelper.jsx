export default class AccountingHelper {
    isAccountingNumberInAccountings(accountingNumber, accountings) {
        if (accountingNumber === undefined || accountingNumber === null || accountings === undefined || accountings === null || Array.isArray(accountings) === false) {
            return false;
        }

        return accountings.some(accounting => accounting.number === accountingNumber);
    }
}