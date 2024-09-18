using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class OpenIdProviderConfigurationStaticValuesProvider : IOpenIdProviderConfigurationStaticValuesProvider
    {
        #region Private variables

        private static readonly CultureInfo EnUsCulture = new("en-US");

        #endregion

        #region Constructor

        public OpenIdProviderConfigurationStaticValuesProvider(ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            IReadOnlyDictionary<string, IScope> supportedScopes = supportedScopesProvider.SupportedScopes;

            ScopesSupported = supportedScopes;
            ResponseTypesSupported = ["code", "code id_token", "id_token"];
            ResponseModesSupported = ["query"];
            GrantTypesSupported = ["authorization_code", "client_credentials"];
            SubjectTypesSupported = ["pairwise"];
            IdTokenSigningAlgValuesSupported = [GetRs256Algorithm()];
            UserInfoSigningAlgValuesSupported = [GetRs256Algorithm()];
            RequestObjectSigningAlgValuesSupported = [GetRs256Algorithm(), GetNoneAlgorithm()];
            TokenEndpointAuthenticationMethodsSupported = ["client_secret_basic"];
            DisplayValuesSupported = ["page"];
            ClaimTypesSupported = ["normal", "aggregated"];
            ClaimsSupported = supportedScopes.Values.SelectMany(m => m.RelatedClaims).Distinct().ToArray();
            ClaimsLocalesSupported = [GetEnUsLocale()];
            UiLocalesSupported = [GetEnUsLocale()];
        }

        #endregion

        #region Properties

        public IReadOnlyDictionary<string, IScope> ScopesSupported { get; }

        public IReadOnlyCollection<string> ResponseTypesSupported { get; }

        public IReadOnlyCollection<string> ResponseModesSupported { get; }

        public IReadOnlyCollection<string> GrantTypesSupported { get; }

        public IReadOnlyCollection<string> SubjectTypesSupported { get; }

        public IReadOnlyCollection<string> IdTokenSigningAlgValuesSupported { get; }

        public IReadOnlyCollection<string> UserInfoSigningAlgValuesSupported { get; }

        public IReadOnlyCollection<string> RequestObjectSigningAlgValuesSupported { get; }

        public IReadOnlyCollection<string> TokenEndpointAuthenticationMethodsSupported { get; }

        public IReadOnlyCollection<string> DisplayValuesSupported { get; }

        public IReadOnlyCollection<string> ClaimTypesSupported { get; }

        public IReadOnlyCollection<string> ClaimsSupported { get; }

        public IReadOnlyCollection<string> ClaimsLocalesSupported { get; }

        public IReadOnlyCollection<string> UiLocalesSupported { get; }

        public bool ClaimsParameterSupported => false;

        public bool RequestParameterSupported => false;

        public bool RequestUriParameterSupported => false;

        public bool RequireRequestUriRegistration => false;

        #endregion

        #region Methods

        private static string GetRs256Algorithm()
        {
            return SecurityAlgorithms.RsaSha256;
        }

        private static string GetNoneAlgorithm()
        {
            return "none";
        }

        private static string GetEnUsLocale()
        {
            return EnUsCulture.Name;
        }

        #endregion
    }
}