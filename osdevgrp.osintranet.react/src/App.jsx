import { ErrorBoundary } from "react-error-boundary";
import { BrowserRouter as Router} from 'react-router';
import './App.css'
import { ServiceProvider } from './contexts/ServiceContext';
import { HelperProvider } from './contexts/HelperContext'
import Fallback from "./components/Fallback";
import Layout from './components/Layout'
import CookieConsent from './components/CookieConsent';
import Home from './components/Home'

function App() {
    return (
        <ErrorBoundary FallbackComponent={Fallback}>
            <Router>
                <ServiceProvider>
                    <HelperProvider>
                        <Layout>
                            <ErrorBoundary FallbackComponent={Fallback}>
                                <CookieConsent />
                                <Home />
                            </ErrorBoundary>
                        </Layout>
                    </HelperProvider>
                </ServiceProvider>
            </Router>
        </ErrorBoundary>
    )
}

export default App