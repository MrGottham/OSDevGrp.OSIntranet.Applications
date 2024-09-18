using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries
{
    public interface IGetOpenIdProviderConfigurationQuery : IQuery
    {
        Uri AuthorizationEndpoint { get; }

        Uri TokenEndpoint { get; }

        Uri JsonWebKeySetEndpoint { get; }

        Uri UserInfoEndpoint { get; }

        Uri RegistrationEndpoint { get; }

        Uri ServiceDocumentationEndpoint { get; }

        Uri RegistrationPolicyEndpoint { get; }

        Uri RegistrationTermsOfServiceEndpoint { get; }

        IValidator Validate(IValidator validator);
    }
}