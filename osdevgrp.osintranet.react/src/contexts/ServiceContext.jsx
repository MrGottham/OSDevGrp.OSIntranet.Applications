import { createContext } from 'react';
import HomeService from '../services/HomeService';
import AuthenticateService from '../services/AuthenticateService'

const services = {
    homeService: new HomeService(),
    authenticateService: new AuthenticateService()
};

const ServiceContext = createContext();

function ServiceProvider({ children }) {
    return (
        <ServiceContext.Provider value={ services }>{children}</ServiceContext.Provider>
    )
}

export { ServiceContext, ServiceProvider }