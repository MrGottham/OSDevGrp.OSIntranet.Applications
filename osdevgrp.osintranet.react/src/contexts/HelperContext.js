import { createContext, createElement } from 'react';
import AccountingHelper from '../helpers/AccountingHelper';
import AuthorizationHelper from '../helpers/AuthorizationHelper';
import StaticTextHelper from '../helpers/StaticTextHelper';
import UrlHelper from '../helpers/UrlHelper';
import DateHelper from '../helpers/DateHelper';
import FormHelper from '../helpers/FormHelper';
import ValidationSchemaHelper from '../helpers/ValidationSchemaHelper';
import ValidationRuleSetHelper from '../helpers/ValidationRuleSetHelper';

const helpers = {
    accountingHelper: new AccountingHelper(),
    authorizationHelper: new AuthorizationHelper(),
    staticTextHelper: new StaticTextHelper(),
    urlHelper: new UrlHelper(),
    dateHelper: new DateHelper(),
    formHelper: new FormHelper(),
    validationSchemaHelper: new ValidationSchemaHelper(),
    validationRuleSetHelper: new ValidationRuleSetHelper()
};

const HelperContext = createContext();

// createElement instead of JSX because this file is .js, so the react-refresh rule only checks .jsx components.
function HelperProvider({ children }) {
    return createElement(HelperContext.Provider, { value: helpers }, children);
}

export { HelperContext, HelperProvider };
