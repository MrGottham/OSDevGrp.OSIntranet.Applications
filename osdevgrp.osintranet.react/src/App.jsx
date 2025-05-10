import './App.css'
import { ServiceProvider } from './contexts/ServiceContext';
import { HelperProvider } from './contexts/HelperContext'
import Layout from './components/Layout'

function App() {
    return (
        <ServiceProvider>
            <HelperProvider>
                <Layout>
                    <p>Content!</p>
                </Layout>
            </HelperProvider>
        </ServiceProvider>
    )
}

export default App