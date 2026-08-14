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

    async getAccountSummary(accountingNumber, accountNumber, isoDateString) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        if (accountNumber === undefined || accountNumber === null || accountNumber.trim() === '') {
            throw new Error('Account number is required.');
        }

        if (isoDateString === undefined || isoDateString === null) {
            throw new Error('ISO date string is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}/accounts/${accountNumber}/summary?statusDate=${isoDateString}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getBudgetAccountSummary(accountingNumber, budgetAccountNumber, isoDateString) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        if (budgetAccountNumber === undefined || budgetAccountNumber === null || budgetAccountNumber.trim() === '') {
            throw new Error('Budget account number is required.');
        }

        if (isoDateString === undefined || isoDateString === null) {
            throw new Error('ISO date string is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}/budgetaccounts/${budgetAccountNumber}/summary?statusDate=${isoDateString}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getContactAccountSummary(accountingNumber, contactAccountNumber, isoDateString) {
        if (accountingNumber === undefined || accountingNumber === null) {
            throw new Error('Accounting number is required.');
        }

        if (contactAccountNumber === undefined || contactAccountNumber === null || contactAccountNumber.trim() === '') {
            throw new Error('Contact account number is required.');
        }

        if (isoDateString === undefined || isoDateString === null) {
            throw new Error('ISO date string is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}/contactaccounts/${contactAccountNumber}/summary?statusDate=${isoDateString}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}