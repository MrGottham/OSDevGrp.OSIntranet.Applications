import ServiceBase from './ServiceBase';

export default class AccountingService extends ServiceBase {
    async getAccountings() {
        const response = await fetch(this.resolveEndpoint('/api/accounting'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getAccounting(accountingNumber) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}