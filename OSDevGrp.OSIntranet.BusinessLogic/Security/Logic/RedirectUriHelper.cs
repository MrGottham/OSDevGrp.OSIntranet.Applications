using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class RedirectUriHelperHelper
    {
        #region Methods

        internal static IValidator ValidateRedirectUri(this IValidator validator, Uri value, ITrustedDomainResolver trustedDomainResolver, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.ValidateEndpoint(value, validatingType, validatingField)
                .Object.ShouldBeKnownValue(value, redirectUri => IsTrustedDomainAsync(redirectUri, trustedDomainResolver), validatingType, validatingField);
        }

        private static Task<bool> IsTrustedDomainAsync(Uri redirectUri, ITrustedDomainResolver trustedDomainResolver)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver));

            return Task.FromResult(trustedDomainResolver.IsTrustedDomain(redirectUri));
        }

        #endregion
    }
}