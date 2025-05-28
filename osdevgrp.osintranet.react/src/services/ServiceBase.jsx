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

    async generateError(response) {
        if (response.status === 400) {
            const problemDetails = await response.json();
            if (problemDetails === undefined || problemDetails === null) {
                return new Error(response.statusText);
            }

            return this.problemDetailsToError(problemDetails, response.statusText);
        }

        if (response.status === 401) {
            const problemDetails = await response.json();
            if (problemDetails === undefined || problemDetails === null) {
                return new Error(response.statusText);
            }

            return this.problemDetailsToError(problemDetails, response.statusText);
        }

        if (response.status === 500) {
            const problemDetails = await response.json();
            if (problemDetails === undefined || problemDetails === null) {
                return new Error(response.statusText);
            }

            return this.problemDetailsToError(problemDetails, response.statusText);
        }

        return new Error(response.statusText);
    }

    problemDetailsToError(problemDetails, fallbackMessage) {
        if (problemDetails.detail !== undefined && problemDetails.detail !== null && problemDetails.detail.length > 0) {
            return new Error(problemDetails.detail);
        }

        if (problemDetails.title !== undefined && problemDetails.title !== null && problemDetails.title.length > 0) {
            return new Error(problemDetails.title);
        }

        return new Error(fallbackMessage);
    }
}