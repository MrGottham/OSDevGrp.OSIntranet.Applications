import { createContext } from 'react';
import HomeService from '../services/HomeService';
import AuthenticateService from '../services/AuthenticateService';
import SecurityService from '../services/SecurityService';

const services = {
    homeService: new HomeService(),
    authenticateService: new AuthenticateService(),
    securityService: new SecurityService()
};

const ServiceContext = createContext();

function ServiceProvider({ children }) {
    return (
        <ServiceContext.Provider value={ services }>{children}</ServiceContext.Provider>
    )
}

export { ServiceContext, ServiceProvider }