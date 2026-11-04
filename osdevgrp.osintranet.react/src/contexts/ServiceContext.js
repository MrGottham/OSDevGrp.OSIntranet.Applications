import { createContext, createElement } from 'react';
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

// createElement instead of JSX because this file is .js, so the react-refresh rule only checks .jsx components.
function ServiceProvider({ children }) {
    return createElement(ServiceContext.Provider, { value: services }, children);
}

export { ServiceContext, ServiceProvider };
