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
        const headers = await this.generateAntiforgeryHeader();

        const response = await fetch(this.resolveEndpoint('/api/security/verification'), { method: 'POST', headers: headers, credentials: 'include' });
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

        const headers = { ...this.generateContentTypeHeaderForJson(), ...await this.generateAntiforgeryHeader() };

        const body = {
            'verificationKey': `${verificationKey}`,
            'verificationCode': `${verificationCode}`
        };

        const response = await fetch(this.resolveEndpoint('/api/security/verification/verify'), { method: 'POST', headers: headers, body: JSON.stringify(body), credentials: 'include' });
        if (response.ok) {
            return await response.json();
        }

        throw await this.generateError(response);
    }
}