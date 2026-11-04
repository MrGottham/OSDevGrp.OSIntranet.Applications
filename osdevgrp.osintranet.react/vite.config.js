import { defineConfig, loadEnv } from 'vite';
import plugin from '@vitejs/plugin-react';
import path from 'path';
import fs from 'fs';
import child_process from 'child_process';
import process from 'process';

function buildServerProxy(target) {
    return {
        '^/api': {
            target: target,
            changeOrigin: true,
            secure: false
        }
    }
}

function buildServerPort(env) {
    return parseInt(env.SERVER_PORT || '5003')
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
export default defineConfig(({ command, mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    const server = {
        port: buildServerPort(env)
    };

    if (command === 'serve') {
        const bffEndpoint = env.VITE_BFF_ENDPOINT;
        if (bffEndpoint === undefined || bffEndpoint === null || bffEndpoint.trim() === '') {
            throw new Error('Endpoint to the Backend for Frontend application is not defined.');
        }

        server.proxy = buildServerProxy(bffEndpoint);
    }

    if (/^true$/i.test(env.RUNNING_IN_CONTAINER)) {
        return {
            plugins: [plugin()],
            server
        }
    }

    const serverHttps = buildServerHttps(env);

    return {
        plugins: [plugin()],
        server: {
            ...server,
            https: serverHttps,
            cors: {
                "origin": "https://localhost:5003",
                "methods": "GET,HEAD,PUT,PATCH,POST,DELETE",
                "preflightContinue": false,
                "optionsSuccessStatus": 204
            }
        }
    }
})