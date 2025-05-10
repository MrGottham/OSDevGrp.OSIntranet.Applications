import { createContext } from 'react';
import HomeService from '../services/HomeService';

const services = {
    homeService: new HomeService()
};

const ServiceContext = createContext();

function ServiceProvider({ children }) {
    return (
        <ServiceContext.Provider value={ services }>{children}</ServiceContext.Provider>
    )
}

export { ServiceContext, ServiceProvider }