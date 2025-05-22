import { BrowserRouter as Router} from 'react-router';
import './App.css'
import { ServiceProvider } from './contexts/ServiceContext';
import { HelperProvider } from './contexts/HelperContext'
import Layout from './components/Layout'
import CookieConsent from './components/CookieConsent';
import Home from './components/Home'

function App() {
    return (
        <Router>
            <ServiceProvider>
                <HelperProvider>
                    <Layout>
                        <CookieConsent />
                        <Home />
                    </Layout>
                </HelperProvider>
            </ServiceProvider>
        </Router>
    )
}

export default App