import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import vueDevTools from 'vite-plugin-vue-devtools';
import fs from 'fs';
import path from 'path';
import childProcess from 'child_process';
import { env } from 'process';

export default defineConfig(({ command }) => {
    const isServe = command === 'serve';

    let httpsConfig;

    // INFO: The dev-cert logic below is only needed for the local HTTPS dev server, never for `vite build` (no server is started, e.g. CI/production builds).
    if (isServe) {
        const baseFolder =
            env.APPDATA !== undefined && env.APPDATA !== ''
                ? `${env.APPDATA}/ASP.NET/https`
                : `${env.HOME}/.aspnet/https`;

        const certificateName = 'budget.client';
        const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
        const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

        if (!fs.existsSync(baseFolder)) {
            fs.mkdirSync(baseFolder, { recursive: true });
        }

        if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
            const dotnetCommand = childProcess.spawnSync('dotnet', ['dev-certs', 'https', '--export-path', certFilePath, '--format', 'Pem', '--no-password'], { stdio: 'inherit' });

            if (dotnetCommand.status !== 0) {
                throw new Error('Error: Could not create certificate');
            }
        }

        httpsConfig = {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath)
        };
    }

    return {
        envPrefix: 'ENV_',
        plugins: [
            plugin(),
            vueDevTools({
                // INFO: 'visualstudio' is only supported on macOS right now, ew.
                launchEditor: 'code'
            })
        ],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        // INFO: Silences Bootstrap 5's legacy Sass deprecation warnings.
        // Remove once migrated to Bootstrap 6.
        css: {
            preprocessorOptions: {
                scss: {
                    quietDeps: true,
                    silenceDeprecations: ['import', 'global-builtin', 'color-functions']
                }
            }
        },
        build: {
            outDir: fileURLToPath(new URL('../Budget.Server/wwwroot', import.meta.url)),
            emptyOutDir: true
        },
        server: isServe ? {
            port: 49835,
            https: httpsConfig,
            // INFO: Keeps the api same-origin in dev, same as prod.
            proxy: {
                '/api': {
                    target: 'https://localhost:7177',
                    secure: false
                }
            }
        } : undefined
    };
});
