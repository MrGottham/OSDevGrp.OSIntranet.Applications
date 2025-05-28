import ServiceBase from './ServiceBase';

export default class HomeService extends ServiceBase {
    async getLayoutContext() {
        const response = await fetch(this.resolveEndpoint('/api/home/index'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getCookieConsent(applicationName) {
        if (applicationName === undefined || applicationName === null) {
            throw new Error('Application name is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/home/cookie-consent?applicationName=${encodeURIComponent(applicationName)}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getErrorContent(errorMessage) {
        if (errorMessage === undefined || errorMessage === null) {
            throw new Error('Error message is required.');
        }

        const response = await fetch(this.resolveEndpoint(`/api/home/error?errorMessage=${encodeURIComponent(errorMessage)}`), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}