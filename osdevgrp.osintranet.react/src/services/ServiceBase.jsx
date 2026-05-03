export default class ServiceBase {
    #bffEndpoint = null;

    constructor() {
        if (import.meta.env.DEV) {
            return;
        }

        const bffEndpoint = import.meta.env.VITE_BFF_ENDPOINT;
        if (bffEndpoint === undefined || bffEndpoint === null) {
            throw new Error('Endpoint to the Backend for Frontend application is not defined.');
        }

        this.#bffEndpoint = bffEndpoint;
    }

    resolveEndpoint(path) {
        if (this.#bffEndpoint === undefined || this.#bffEndpoint === null) {
            return path;
        }

        return this.#bffEndpoint + path;
    }

    generateContentTypeHeaderForJson() {
        return {
            'Content-Type': 'application/json; charset=utf-8' 
        };
    }

    async generateAntiforgeryHeader() {
        const response = await fetch(this.resolveEndpoint('/api/security/antiforgery/token'), { method: 'GET', credentials: 'include' });
        if (response.ok) {
            const json = await response.json();

            return {
               [json.headerName]: json.requestToken
            };
        }

        throw await this.generateError(response);
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