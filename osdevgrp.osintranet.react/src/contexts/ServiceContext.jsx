import { createContext } from 'react';
import HomeService from '../services/HomeService';
import AccountingService from '../services/AccountingService';
import AuthenticateService from '../services/AuthenticateService';
import SecurityService from '../services/SecurityService';

const services = {
    homeService: new HomeService(),
    accountingService: new AccountingService(),
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