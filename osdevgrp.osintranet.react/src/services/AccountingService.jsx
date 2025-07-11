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
        const response = await fetch(this.resolveEndpoint(`/api/accounting/${accountingNumber}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}