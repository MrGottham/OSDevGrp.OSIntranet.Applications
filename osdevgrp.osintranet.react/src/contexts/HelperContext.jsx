import { createContext } from 'react';
import AuthorizationHelper from '../helpers/AuthorizationHelper';
import StaticTextHelper from '../helpers/StaticTextHelper';

const helpers = {
    authorizationHelper: new AuthorizationHelper(),
    staticTextHelper: new StaticTextHelper()
};

const HelperContext = createContext();

function HelperProvider({ children }) {
    return (
        <HelperContext.Provider value={helpers}>{children}</HelperContext.Provider>
    )
}

export { HelperContext, HelperProvider }