using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IOpenIdProviderConfigurationStaticValuesProvider
    {
        IReadOnlyDictionary<string, IScope> ScopesSupported { get; }

        IReadOnlyCollection<string> ResponseTypesSupported { get; }

        IReadOnlyCollection<string> ResponseModesSupported { get; }

        IReadOnlyCollection<string> GrantTypesSupported { get; }

        IReadOnlyCollection<string> SubjectTypesSupported { get; }

        IReadOnlyCollection<string> IdTokenSigningAlgValuesSupported { get; }

        IReadOnlyCollection<string> UserInfoSigningAlgValuesSupported { get; }

        IReadOnlyCollection<string> RequestObjectSigningAlgValuesSupported { get; }

        IReadOnlyCollection<string> TokenEndpointAuthenticationMethodsSupported { get; }

        IReadOnlyCollection<string> DisplayValuesSupported { get; }

        IReadOnlyCollection<string> ClaimTypesSupported { get; }

        IReadOnlyCollection<string> ClaimsSupported { get; }

        IReadOnlyCollection<string> ClaimsLocalesSupported { get; }

        IReadOnlyCollection<string> UiLocalesSupported { get; }

        bool ClaimsParameterSupported { get; }

        bool RequestParameterSupported { get; }

        bool RequestUriParameterSupported { get; }

        bool RequireRequestUriRegistration { get; }
    }
}