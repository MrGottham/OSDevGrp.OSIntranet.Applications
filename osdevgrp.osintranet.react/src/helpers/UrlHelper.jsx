export default class UrlHelper {
    getNotImplementedUrl() {
        return '/home/notimplemented';
    }

    getAccountingsUrl() {
        return '/accountings';
    }

    getAccountingUrl(accountingNumber) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        return `/accountings/${accountingNumber}`;
    }
}