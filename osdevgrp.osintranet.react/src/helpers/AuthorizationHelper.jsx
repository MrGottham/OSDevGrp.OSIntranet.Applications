export default class AuthorizationHelper {
    authenticatedUser(layoutContext) {
        if (layoutContext === undefined || layoutContext === null) {
            throw new Error('Layout context is required.');
        }

        return layoutContext.userInfo !== undefined && layoutContext.userInfo !== null;
    }

    hasAccountingAccess(layoutContext) {
        if (layoutContext === undefined || layoutContext === null) {
            throw new Error('Layout context is required.');
        }

        return this.authenticatedUser(layoutContext) && layoutContext.userInfo.hasAccountingAccess !== undefined && layoutContext.userInfo.hasAccountingAccess !== null && layoutContext.userInfo.hasAccountingAccess === true;
    }
}