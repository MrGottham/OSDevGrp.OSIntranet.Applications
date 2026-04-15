import ServiceBase from './ServiceBase';

export default class SecurityService extends ServiceBase {
    async getAccessDeniedContent() {
        const response = await fetch(this.resolveEndpoint('/api/security/accessdenied/content'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async getUserInfo() {
        const response = await fetch(this.resolveEndpoint('/api/security/userinfo'), { credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async generateVerification() {
        const response = await fetch(this.resolveEndpoint('/api/security/verification'), { method: 'POST', credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }

    async verifyVerificationCode(verificationKey, verificationCode) {
        if (verificationKey === undefined || verificationKey === null) {
            throw new Error('Verification key is required.');
        }

        if (verificationCode === undefined || verificationCode === null) {
            throw new Error('Verification code is required.');
        }

        const body = {
            'verificationKey': `${verificationKey}`,
            'verificationCode': `${verificationCode}`
        };

        const response = await fetch(this.resolveEndpoint('/api/security/verification/verify'), { method: 'POST', headers: this.makeHeadersForPostWithJsonContent(), body: JSON.stringify(body), credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}