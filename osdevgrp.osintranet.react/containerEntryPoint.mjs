import { constants as osConstants } from 'node:os';
import { spawn } from 'node:child_process';
import { writeFile } from 'node:fs/promises';

const bffEndpointMappingsJson = process.env.BFF_ENDPOINT_MAPPINGS;
if (bffEndpointMappingsJson === undefined || bffEndpointMappingsJson.trim() === '') {
    throw new Error('BFF_ENDPOINT_MAPPINGS is required.');
}

const bffEndpointMappings = JSON.parse(bffEndpointMappingsJson);
await writeFile('/app/runtime-config.json', JSON.stringify({ bffEndpoints: bffEndpointMappings }), { mode: 0o644 });

const [command, ...args] = process.argv.slice(2);
if (command === undefined) {
    throw new Error('A container command must be provided.');
}

const childProcess = spawn(command, args, { stdio: 'inherit', env: process.env });
let childProcessFailedToStart = false;

childProcess.once('error', error => {
    childProcessFailedToStart = true;
    console.error('The container command could not be started.', error);
    process.exitCode = 1;
});

childProcess.once('exit', (code, signal) => {
    if (childProcessFailedToStart === false) {
        const signalCode = signal === null ? undefined : osConstants.signals[signal];
        process.exitCode = code ?? (signalCode === undefined ? 1 : 128 + signalCode);
    }
});

for (const signal of ['SIGINT', 'SIGTERM']) {
    process.on(signal, () => childProcess.kill(signal));
}
