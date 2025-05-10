import { createContext } from 'react'
import StaticTextHelper from '../helpers/StaticTextHelper';

const helpers = {
    staticTextHelper: new StaticTextHelper()
};

const HelperContext = createContext();

function HelperProvider({ children }) {
    return (
        <HelperContext.Provider value={helpers}>{children}</HelperContext.Provider>
    )
}

export { HelperContext, HelperProvider }