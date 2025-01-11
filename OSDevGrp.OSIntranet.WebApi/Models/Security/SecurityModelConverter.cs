using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.WebApi.Models.Security
{
    internal class SecurityModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IValueConverter<DateTimeOffset, int> _totalSecondsCalculator = new TotalSecondsCalculator(() => DateTimeOffset.Now);
        private readonly IValueConverter<Uri, string> _uriToStringConverter = new UriToStringConverter();
        private readonly IValueConverter<IEnumerable<string>, IEnumerable<string>> _stringCollectionConverter = new StringCollectionConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IToken, AccessTokenModel>()
                .ForMember(dest => dest.TokenType, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.TokenType) == false);
                })
                .ForMember(dest => dest.AccessToken, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.AccessToken) == false);
                })
                .ForMember(dest => dest.IdToken, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresIn, opt => opt.ConvertUsing(_totalSecondsCalculator, src => src.Expires));

            mapperConfiguration.CreateMap<JsonWebKeySet, JsonWebKeySetModel>()
                .ForMember(dest => dest.Keys, opt => opt.Condition(src => src.Keys is {Count: > 0}));

            mapperConfiguration.CreateMap<JsonWebKey, JsonWebKeyModel>()
                .ForMember(dest => dest.Kty, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.Kty) == false))
                .ForMember(dest => dest.Use, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.Use) == false))
                .ForMember(dest => dest.Alg, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.Alg) == false))
                .ForMember(dest => dest.Kid, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.Kid) == false))
                .ForMember(dest => dest.N, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.N) == false))
                .ForMember(dest => dest.E, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.E) == false))
                .ForMember(dest => dest.D, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.D) == false))
                .ForMember(dest => dest.DP, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.DP) == false))
                .ForMember(dest => dest.DQ, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.DQ) == false))
                .ForMember(dest => dest.P, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.P) == false))
                .ForMember(dest => dest.Q, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.Q) == false))
                .ForMember(dest => dest.QI, opt => opt.Condition(src => string.IsNullOrWhiteSpace(src.QI) == false));

            mapperConfiguration.CreateMap<IOpenIdProviderConfiguration, OpenIdProviderConfigurationModel>()
                .ForMember(dest => dest.Issuer, opt => opt.ConvertUsing(_uriToStringConverter, src => src.Issuer))
                .ForMember(dest => dest.AuthorizationEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.AuthorizationEndpoint))
                .ForMember(dest => dest.TokenEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.TokenEndpoint))
                .ForMember(dest => dest.UserInfoEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.UserInfoEndpoint))
                .ForMember(dest => dest.JsonWebKeySetEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.JsonWebKeySetEndpoint))
                .ForMember(dest => dest.RegistrationEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.RegistrationEndpoint))
                .ForMember(dest => dest.ScopesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.ScopesSupported))
                .ForMember(dest => dest.ResponseTypesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.ResponseTypesSupported))
                .ForMember(dest => dest.ResponseModesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.ResponseModesSupported))
                .ForMember(dest => dest.GrantTypesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.GrantTypesSupported))
                .ForMember(dest => dest.AuthenticationContextClassReferencesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.AuthenticationContextClassReferencesSupported))
                .ForMember(dest => dest.SubjectTypesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.SubjectTypesSupported))
                .ForMember(dest => dest.IdTokenSigningAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.IdTokenSigningAlgValuesSupported))
                .ForMember(dest => dest.IdTokenEncryptionAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.IdTokenEncryptionAlgValuesSupported))
                .ForMember(dest => dest.IdTokenEncryptionEncValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.IdTokenEncryptionEncValuesSupported))
                .ForMember(dest => dest.UserInfoSigningAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.UserInfoSigningAlgValuesSupported))
                .ForMember(dest => dest.UserInfoEncryptionAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.UserInfoEncryptionAlgValuesSupported))
                .ForMember(dest => dest.UserInfoEncryptionEncValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.UserInfoEncryptionEncValuesSupported))
                .ForMember(dest => dest.RequestObjectSigningAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.RequestObjectSigningAlgValuesSupported))
                .ForMember(dest => dest.RequestObjectEncryptionAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.RequestObjectEncryptionAlgValuesSupported))
                .ForMember(dest => dest.RequestObjectEncryptionEncValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.RequestObjectEncryptionEncValuesSupported))
                .ForMember(dest => dest.TokenEndpointAuthenticationMethodsSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.TokenEndpointAuthenticationMethodsSupported))
                .ForMember(dest => dest.TokenEndpointAuthenticationSigningAlgValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.TokenEndpointAuthenticationSigningAlgValuesSupported))
                .ForMember(dest => dest.DisplayValuesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.DisplayValuesSupported))
                .ForMember(dest => dest.ClaimTypesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.ClaimTypesSupported))
                .ForMember(dest => dest.ClaimsSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.ClaimsSupported))
                .ForMember(dest => dest.ServiceDocumentationEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.ServiceDocumentationEndpoint))
                .ForMember(dest => dest.ClaimsLocalesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.ClaimsLocalesSupported))
                .ForMember(dest => dest.UiLocalesSupported, opt => opt.ConvertUsing(_stringCollectionConverter, src => src.UiLocalesSupported))
                .ForMember(dest => dest.RegistrationPolicyEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.RegistrationPolicyEndpoint))
                .ForMember(dest => dest.RegistrationTermsOfServiceEndpoint, opt => opt.ConvertUsing(_uriToStringConverter, src => src.RegistrationTermsOfServiceEndpoint));
        }

        private class TotalSecondsCalculator : IValueConverter<DateTimeOffset, int>
        {
            #region Private variables

            private readonly Func<DateTimeOffset> _fromDateResolver;

            #endregion

            #region Constructor

            public TotalSecondsCalculator(Func<DateTimeOffset> fromDateResolver)
            {
                NullGuard.NotNull(fromDateResolver, nameof(fromDateResolver));

                _fromDateResolver = fromDateResolver;
            }

            #endregion

            #region Methods

            public int Convert(DateTimeOffset toDateTime, ResolutionContext resolutionContext)
            {
                NullGuard.NotNull(resolutionContext, nameof(resolutionContext));

                return (int) toDateTime.UtcDateTime.Subtract(_fromDateResolver().UtcDateTime).TotalSeconds;
            }

            #endregion
        }

        private class UriToStringConverter : IValueConverter<Uri, string>
        {
            #region Methods

            public string Convert(Uri uri, ResolutionContext resolutionContext)
            {
                NullGuard.NotNull(resolutionContext, nameof(resolutionContext));

                if (uri == null || uri.IsAbsoluteUri == false || string.IsNullOrWhiteSpace(uri.AbsoluteUri))
                {
                    return null;
                }

                string value = uri.AbsoluteUri;
                return value.EndsWith("/") ? value.Substring(0, value.Length - 1) : value;
            }

            #endregion
        }

        private class StringCollectionConverter : IValueConverter<IEnumerable<string>, IEnumerable<string>>
        {
            #region Methods

            public IEnumerable<string> Convert(IEnumerable<string> stringCollection, ResolutionContext resolutionContext)
            {
                NullGuard.NotNull(resolutionContext, nameof(resolutionContext));

                string[] stringArray = stringCollection?.Where(value => string.IsNullOrWhiteSpace(value) == false).ToArray() ?? [];
                return stringArray.Length == 0 ? null : stringArray;
            }

            #endregion
        }

        #endregion
    }
}