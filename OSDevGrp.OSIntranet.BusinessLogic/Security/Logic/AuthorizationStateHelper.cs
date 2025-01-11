using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class AuthorizationStateHelper
    {
        #region Methods

        internal static IValidator ValidateAuthorizationState(this IValidator validator, string value, Type validatingType, string validatingField, bool shouldBeBase64 = true)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            validator = validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField);

            return shouldBeBase64 == false
                ? validator
                : validator.ValidateBase64String(value, validatingType, validatingField);
        }

        internal static IValidator ValidateAuthorizationState(this IValidator validator, IAuthorizationState authorizationState, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(authorizationState, nameof(authorizationState))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            return validator.ValidateResponseType(authorizationState.ResponseType, ResponseTypeHelper.ResponseTypeForAuthorizationCodeFlowRegex, authorizationState.GetType(), nameof(authorizationState.ResponseType))
                .ValidateClientId(authorizationState.ClientId, securityRepository, authorizationState.GetType(), nameof(authorizationState.ClientId))
                .ValidateRedirectUri(authorizationState.RedirectUri, trustedDomainResolver, authorizationState.GetType(), nameof(authorizationState.RedirectUri))
                .ValidateScopes(authorizationState.Scopes, supportedScopesProvider, authorizationState.GetType(), nameof(authorizationState.Scopes))
                .ValidateState(authorizationState.ExternalState, authorizationState.GetType(), nameof(authorizationState.ExternalState), true)
                .ValidateNonce(authorizationState.Nonce, authorizationState.GetType(), nameof(authorizationState.Nonce), true);
        }

        #endregion
    }
}