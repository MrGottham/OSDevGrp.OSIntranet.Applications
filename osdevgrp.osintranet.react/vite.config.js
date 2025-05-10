import { defineConfig, loadEnv } from 'vite';
import plugin from '@vitejs/plugin-react';
import path from 'path';
import fs from 'fs';
import child_process from 'child_process';

function buildServerProxy(target) {
    return {
        '^/api': {
            target: target,
            changeOrigin: true
        }
    }
}

function buildServerPort(env) {
    return parseInt(env.SERVER_PORT || '5001')
}

function buildServerHttps(env) {
    const baseFolder = env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

    const certificateName = 'osdevgrp.osintranet.react';
    const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(baseFolder)) {
        fs.mkdirSync(baseFolder, { recursive: true });
    }

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        if (0 !== child_process.spawnSync('dotnet', [
            'dev-certs',
            'https',
            '--export-path',
            certFilePath,
            '--format',
            'Pem',
            '--no-password',
        ], { stdio: 'inherit', }).status) {
            throw new Error("Could not create certificate.");
        }
    }

    return {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath),
    }
}

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    const bffEndpoint = env.VITE_BFF_ENDPOINT;
    if (bffEndpoint === undefined || bffEndpoint === null) {
        throw new Error('Endpoint to the Backend for Frontend application is not defined.');
    }

    const serverProxy = buildServerProxy(bffEndpoint);
    const serverPort = buildServerPort(env);

    if (/^true$/i.test(env.RUNNING_IN_CONTAINER)) {
        return {
            plugins: [plugin()],
            server: {
                proxy: serverProxy,
                port: serverPort
            }
        }
    }

    const serverHttps = buildServerHttps(env);

    return {
        plugins: [plugin()],
        server: {
            proxy: serverProxy,
            port: serverPort,
            https: serverHttps,
            cors: {
                "origin": "https://localhost:5001",
                "methods": "GET,HEAD,PUT,PATCH,POST,DELETE",
                "preflightContinue": false,
                "optionsSuccessStatus": 204
            }
        }
    }
})