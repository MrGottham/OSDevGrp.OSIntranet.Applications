import ServiceBase from "./ServiceBase";

export default class AuthenticateService extends ServiceBase {
    getLoginUrl(location) {
        if (location === undefined || location === null) {
            throw new Error('Location is required.');
        }

        const loginEndpoint = this.resolveEndpoint(`/api/security/login`);

        return `${loginEndpoint}?returnUrl=${encodeURIComponent(this.getReturnUrl(location))}`;
    }

    getLogoutUrl(location) {
        if (location === undefined || location === null) {
            throw new Error('Location is required.');
        }

        const logoutEndpoint = this.resolveEndpoint(`/api/security/logout`);

        return `${logoutEndpoint}?returnUrl=${encodeURIComponent(this.getReturnUrl(location))}`;
    }

    getReturnUrl(location) {
        if (location === undefined || location === null) {
            throw new Error('Location is required.');
        }

        if (location.port === undefined || location.port === null) {
            return `${location.protocol}//${location.hostname}`;
        }

        return `${location.protocol}//${location.hostname}:${location.port}`;
    }
}