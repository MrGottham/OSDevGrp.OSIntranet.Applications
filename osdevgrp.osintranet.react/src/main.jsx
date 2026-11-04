import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import 'react-datepicker/dist/react-datepicker.css';
import { loadRuntimeConfiguration } from './config/RuntimeConfiguration';

const rootElement = document.getElementById('root');
if (rootElement === null) {
    throw new Error('The application root element was not found.');
}

async function renderApplication() {
    const { default: App } = await import('./App.jsx');

    createRoot(rootElement).render(
        <StrictMode>
            <App />
        </StrictMode>,
    );
}

if (import.meta.env.DEV) {
    renderApplication();
} else {
    loadRuntimeConfiguration()
        .then(renderApplication)
        .catch(error => {
            console.error('Application startup failed.', error);
            rootElement.textContent = 'The application could not load its runtime configuration.';
        });
}