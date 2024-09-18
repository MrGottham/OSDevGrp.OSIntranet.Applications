using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.QueryHandlers
{
    internal class GetOpenIdProviderConfigurationQueryHandler : IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IOptions<TokenGeneratorOptions> _tokenGeneratorOptions;
        private readonly IOpenIdProviderConfigurationStaticValuesProvider _openIdProviderConfigurationStaticValuesProvider;

        #endregion

        #region Constructor

        public GetOpenIdProviderConfigurationQueryHandler(IValidator validator, IOptions<TokenGeneratorOptions> tokenGeneratorOptions, IOpenIdProviderConfigurationStaticValuesProvider openIdProviderConfigurationStaticValuesProvider)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions))
                .NotNull(openIdProviderConfigurationStaticValuesProvider, nameof(openIdProviderConfigurationStaticValuesProvider));

            _validator = validator;
            _tokenGeneratorOptions = tokenGeneratorOptions;
            _openIdProviderConfigurationStaticValuesProvider = openIdProviderConfigurationStaticValuesProvider;
        }

        #endregion

        #region Methods

        public Task<IOpenIdProviderConfiguration> QueryAsync(IGetOpenIdProviderConfigurationQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return Task.Run(() =>
            {
                query.Validate(_validator);

                TokenGeneratorOptions tokenGeneratorOptions = _tokenGeneratorOptions.Value;
                IReadOnlyDictionary<string, IScope> scopesSupported = _openIdProviderConfigurationStaticValuesProvider.ScopesSupported;

                IOpenIdProviderConfigurationBuilder openIdProviderConfigurationBuilder = OpenIdProviderConfigurationFactory.Create(
                        ResolveIssuer(tokenGeneratorOptions),
                        query.AuthorizationEndpoint,
                        query.TokenEndpoint,
                        query.JsonWebKeySetEndpoint,
                        _openIdProviderConfigurationStaticValuesProvider.ResponseTypesSupported.ToArray(),
                        _openIdProviderConfigurationStaticValuesProvider.SubjectTypesSupported.ToArray(),
                        _openIdProviderConfigurationStaticValuesProvider.IdTokenSigningAlgValuesSupported.ToArray())
                    .WithScopesSupported(scopesSupported.Select(m => m.Value).Select(m => m.Name).ToArray())
                    .WithResponseModesSupported(_openIdProviderConfigurationStaticValuesProvider.ResponseModesSupported.ToArray())
                    .WithGrantTypesSupported(_openIdProviderConfigurationStaticValuesProvider.GrantTypesSupported.ToArray())
                    .WithUserInfoSigningAlgValuesSupported(_openIdProviderConfigurationStaticValuesProvider.UserInfoSigningAlgValuesSupported.ToArray())
                    .WithRequestObjectSigningAlgValuesSupported(_openIdProviderConfigurationStaticValuesProvider.RequestObjectSigningAlgValuesSupported.ToArray())
                    .WithTokenEndpointAuthenticationMethodsSupported(_openIdProviderConfigurationStaticValuesProvider.TokenEndpointAuthenticationMethodsSupported.ToArray())
                    .WithDisplayValuesSupported(_openIdProviderConfigurationStaticValuesProvider.DisplayValuesSupported.ToArray())
                    .WithClaimTypesSupported(_openIdProviderConfigurationStaticValuesProvider.ClaimTypesSupported.ToArray())
                    .WithClaimsSupported(_openIdProviderConfigurationStaticValuesProvider.ClaimsSupported.ToArray())
                    .WithClaimsLocalesSupported(_openIdProviderConfigurationStaticValuesProvider.ClaimsLocalesSupported.ToArray())
                    .WithUiLocalesSupported(_openIdProviderConfigurationStaticValuesProvider.UiLocalesSupported.ToArray())
                    .WithClaimsParameterSupported(_openIdProviderConfigurationStaticValuesProvider.ClaimsParameterSupported)
                    .WithRequestParameterSupported(_openIdProviderConfigurationStaticValuesProvider.RequestParameterSupported)
                    .WithRequestUriParameterSupported(_openIdProviderConfigurationStaticValuesProvider.RequestUriParameterSupported)
                    .WithRequireRequestUriRegistration(_openIdProviderConfigurationStaticValuesProvider.RequireRequestUriRegistration);

                AddUriWhenNotNull(query.UserInfoEndpoint, openIdProviderConfigurationBuilder.WithUserInfoEndpoint);
                AddUriWhenNotNull(query.RegistrationEndpoint, openIdProviderConfigurationBuilder.WithRegistrationEndpoint);
                AddUriWhenNotNull(query.ServiceDocumentationEndpoint, openIdProviderConfigurationBuilder.WithServiceDocumentationEndpoint);
                AddUriWhenNotNull(query.RegistrationPolicyEndpoint, openIdProviderConfigurationBuilder.WithRegistrationPolicyEndpoint);
                AddUriWhenNotNull(query.RegistrationTermsOfServiceEndpoint, openIdProviderConfigurationBuilder.WithRegistrationTermsOfServiceEndpoint);

                return openIdProviderConfigurationBuilder.Build();
            });
        }

        private static Uri ResolveIssuer(TokenGeneratorOptions tokenGeneratorOptions)
        {
            NullGuard.NotNull(tokenGeneratorOptions, nameof(tokenGeneratorOptions));

            return Uri.TryCreate(tokenGeneratorOptions.Issuer, UriKind.Absolute, out Uri issuer) ? issuer : null;
        }

        private static void AddUriWhenNotNull(Uri uri, Func<Uri, IOpenIdProviderConfigurationBuilder> uriSetter)
        {
            NullGuard.NotNull(uriSetter, nameof(uriSetter));

            if (uri == null)
            {
                return;
            }

            uriSetter(uri);
        }

        #endregion
    }
}