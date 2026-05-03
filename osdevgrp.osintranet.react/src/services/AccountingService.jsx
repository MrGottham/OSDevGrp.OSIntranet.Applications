import ServiceBase from './ServiceBase';

export default class AccountingService extends ServiceBase {
    async getAccountings() {
        const response = await fetch(this.resolveEndpoint('/api/accounting'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getAccountingPreCreation() {
        const response = await fetch(this.resolveEndpoint('/api/accounting/create'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getAccounting(accountingNumber, numberOfPostingLines) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        if (numberOfPostingLines === undefined || numberOfPostingLines === null) {
            throw new Error('Number of posting lines is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}?numberOfPostingLines=${numberOfPostingLines}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getAccountingSummary(accountingNumber, numberOfPostingLines) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        if (numberOfPostingLines === undefined || numberOfPostingLines === null) {
            throw new Error('Number of posting lines is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}/summary?numberOfPostingLines=${numberOfPostingLines}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}