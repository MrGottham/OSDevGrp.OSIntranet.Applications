export default class ServiceBase {
    _bffEndpoint = null;

    constructor() {
        if (import.meta.env.DEV) {
            return;
        }

        const bffEndpoint = import.meta.env.VITE_BFF_ENDPOINT;
        if (bffEndpoint === undefined || bffEndpoint === null) {
            throw new Error('Endpoint to the Backend for Frontend application is not defined.');
        }

        this._bffEndpoint = bffEndpoint;
    }

    resolveEndpoint(path) {
        if (this._bffEndpoint === undefined || this._bffEndpoint === null) {
            return path;
        }

        return this._bffEndpoint + path;
    }
}