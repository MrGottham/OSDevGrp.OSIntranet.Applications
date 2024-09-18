using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
    internal class GetOpenIdProviderConfigurationQuery : IGetOpenIdProviderConfigurationQuery
    {
        #region Constructor

        public GetOpenIdProviderConfigurationQuery(Uri authorizationEndpoint, Uri tokenEndpoint, Uri jsonWebKeySetEndpoint, Uri userInfoEndpoint, Uri registrationEndpoint, Uri serviceDocumentationEndpoint, Uri registrationPolicyEndpoint, Uri registrationTermsOfServiceEndpoint)
        {
            NullGuard.NotNull(authorizationEndpoint, nameof(authorizationEndpoint))
                .NotNull(tokenEndpoint, nameof(tokenEndpoint))
                .NotNull(jsonWebKeySetEndpoint, nameof(jsonWebKeySetEndpoint));

            AuthorizationEndpoint = authorizationEndpoint;
            TokenEndpoint = tokenEndpoint;
            JsonWebKeySetEndpoint = jsonWebKeySetEndpoint;
            UserInfoEndpoint = userInfoEndpoint;
            RegistrationEndpoint = registrationEndpoint;
            ServiceDocumentationEndpoint = serviceDocumentationEndpoint;
            RegistrationPolicyEndpoint = registrationPolicyEndpoint;
            RegistrationTermsOfServiceEndpoint = registrationTermsOfServiceEndpoint;
        }

        #endregion

        #region Properties

        public Uri AuthorizationEndpoint { get; }

        public Uri TokenEndpoint { get; }

        public Uri JsonWebKeySetEndpoint { get; }

        public Uri UserInfoEndpoint { get; }

        public Uri RegistrationEndpoint { get; }

        public Uri ServiceDocumentationEndpoint { get; }

        public Uri RegistrationPolicyEndpoint { get; }

        public Uri RegistrationTermsOfServiceEndpoint { get; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.ValidateEndpoint(AuthorizationEndpoint, GetType(), nameof(AuthorizationEndpoint))
                .ValidateEndpoint(TokenEndpoint, GetType(), nameof(TokenEndpoint))
                .ValidateEndpoint(JsonWebKeySetEndpoint, GetType(), nameof(JsonWebKeySetEndpoint))
                .ValidateEndpoint(UserInfoEndpoint, GetType(), nameof(UserInfoEndpoint), true)
                .ValidateEndpoint(RegistrationEndpoint, GetType(), nameof(RegistrationEndpoint), true)
                .ValidateEndpoint(ServiceDocumentationEndpoint, GetType(), nameof(ServiceDocumentationEndpoint), true)
                .ValidateEndpoint(RegistrationPolicyEndpoint, GetType(), nameof(RegistrationPolicyEndpoint), true)
                .ValidateEndpoint(RegistrationTermsOfServiceEndpoint, GetType(), nameof(RegistrationTermsOfServiceEndpoint), true);
        }

        #endregion
    }
}