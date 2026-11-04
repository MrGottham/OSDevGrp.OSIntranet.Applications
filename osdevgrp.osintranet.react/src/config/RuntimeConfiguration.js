let bffEndpointByFrontendHost = null;

export async function loadRuntimeConfiguration() {
    const response = await fetch('/runtime-config.json', { cache: 'no-store' });
    if (!response.ok) {
        throw new Error(`Could not load runtime configuration. HTTP status: ${response.status}.`);
    }

    const runtimeConfiguration = await response.json();
    if (runtimeConfiguration === null || typeof runtimeConfiguration !== 'object' || Array.isArray(runtimeConfiguration)) {
        throw new Error('Runtime configuration must be a JSON object.');
    }

    const configuredEndpoints = runtimeConfiguration.bffEndpoints;
    if (configuredEndpoints === null || typeof configuredEndpoints !== 'object' || Array.isArray(configuredEndpoints)) {
        throw new Error('Runtime configuration must contain a bffEndpoints object.');
    }

    const endpointEntries = Object.entries(configuredEndpoints);
    if (endpointEntries.length === 0) {
        throw new Error('Runtime configuration must contain at least one BFF endpoint.');
    }

    bffEndpointByFrontendHost = Object.fromEntries(endpointEntries.map(([frontendHost, endpoint]) => {
        if (frontendHost.trim() === '') {
            throw new Error('Runtime configuration contains an empty frontend hostname.');
        }

        if (typeof endpoint !== 'string' || endpoint.trim() === '') {
            throw new Error(`Runtime configuration contains an invalid BFF endpoint for ${frontendHost}.`);
        }

        const bffUrl = new URL(endpoint);
        if (bffUrl.protocol !== 'https:' || bffUrl.username !== '' || bffUrl.password !== '' || bffUrl.pathname !== '/' || bffUrl.search !== '' || bffUrl.hash !== '') {
            throw new Error(`Runtime configuration must use an HTTPS origin for ${frontendHost}.`);
        }

        return [frontendHost.toLowerCase(), bffUrl.origin];
    }));
}

export function getBffEndpoint(frontendHost) {
    if (bffEndpointByFrontendHost === null) {
        throw new Error('Runtime configuration has not been loaded.');
    }

    const bffEndpoint = bffEndpointByFrontendHost[frontendHost.toLowerCase()];
    if (bffEndpoint === undefined) {
        throw new Error(`No BFF endpoint is configured for frontend host ${frontendHost}.`);
    }

    return bffEndpoint;
}
