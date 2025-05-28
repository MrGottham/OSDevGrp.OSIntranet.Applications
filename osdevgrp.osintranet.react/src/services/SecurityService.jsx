import ServiceBase from './ServiceBase';

export default class SecurityService extends ServiceBase {
    async getAccessDeniedContent() {
        const response = await fetch(this.resolveEndpoint('/api/security/accessdenied/content'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}