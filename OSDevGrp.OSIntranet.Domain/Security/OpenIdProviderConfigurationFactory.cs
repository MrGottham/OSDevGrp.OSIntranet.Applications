using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public static class OpenIdProviderConfigurationFactory
    {
        #region Methods

        public static IOpenIdProviderConfigurationBuilder Create(Uri issuer, Uri authorizationEndpoint, Uri tokenEndpoint, Uri jsonWebKeySetEndpoint, string[] responseTypesSupported, string[] subjectTypesSupported, string[] idTokenSigningAlgValuesSupported)
        {
            NullGuard.NotNull(issuer, nameof(issuer))
                .NotNull(authorizationEndpoint, nameof(authorizationEndpoint))
                .NotNull(tokenEndpoint, nameof(tokenEndpoint))
                .NotNull(jsonWebKeySetEndpoint, nameof(jsonWebKeySetEndpoint))
                .NotNull(responseTypesSupported, nameof(responseTypesSupported))
                .NotNull(subjectTypesSupported, nameof(subjectTypesSupported))
                .NotNull(idTokenSigningAlgValuesSupported, nameof(idTokenSigningAlgValuesSupported));

            return new OpenIdProviderConfigurationBuilder(
                issuer,
                authorizationEndpoint,
                tokenEndpoint,
                jsonWebKeySetEndpoint,
                responseTypesSupported,
                subjectTypesSupported,
                idTokenSigningAlgValuesSupported);
        }

        #endregion
    }
}