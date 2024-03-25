using AutoFixture;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    public abstract class OpenIdProviderConfigurationBuilderTestBase
    {
        protected static IOpenIdProviderConfigurationBuilder CreateSut(Fixture fixture, Uri issuer = null, Uri authorizationEndpoint = null, Uri tokenEndpoint = null, Uri jsonWebKeySetEndpoint = null, string[] responseTypesSupported = null, string[] subjectTypesSupported = null, string[] idTokenSigningAlgValuesSupported = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Domain.Security.OpenIdProviderConfigurationBuilder(
                issuer ?? CreateIssuer(fixture),
                authorizationEndpoint ?? CreateAuthorizationEndpoint(fixture),
                tokenEndpoint ?? CreateTokenEndpoint(fixture),
                jsonWebKeySetEndpoint ?? CreateJsonWebKeySetEndpoint(fixture),
                responseTypesSupported ?? CreateResponseTypesSupported(fixture),
                subjectTypesSupported ?? CreateSubjectTypesSupported(fixture),
                idTokenSigningAlgValuesSupported ?? CreateIdTokenSigningAlgValuesSupported(fixture));
        }

        protected static Uri CreateIssuer(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Uri($"https://{CreateDomainName(fixture)}", UriKind.Absolute);
        }

        protected static Uri CreateAuthorizationEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static Uri CreateTokenEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static Uri CreateUserInfoEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static Uri CreateJsonWebKeySetEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static Uri CreateRegistrationEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static string[] CreateScopesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateResponseTypesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateResponseModesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateGrantTypesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateAuthenticationContextClassReferencesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateSubjectTypesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateIdTokenSigningAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateIdTokenEncryptionAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateIdTokenEncryptionEncValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateUserInfoSigningAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateUserInfoEncryptionAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateUserInfoEncryptionEncValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateRequestObjectSigningAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateRequestObjectEncryptionAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateRequestObjectEncryptionEncValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateTokenEndpointAuthenticationMethodsSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateTokenEndpointAuthenticationSigningAlgValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateDisplayValuesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateClaimTypesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateClaimsSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static Uri CreateServiceDocumentationEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static string[] CreateClaimsLocalesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static string[] CreateUiLocalesSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateStringArray(fixture);
        }

        protected static bool CreateClaimsParameterSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.Create<bool>();
        }

        protected static bool CreateRequestParameterSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.Create<bool>();
        }

        protected static bool CreateRequestUriParameterSupported(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.Create<bool>();
        }

        protected static bool CreateRequireRequestUriRegistration(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return fixture.Create<bool>();
        }

        protected static Uri CreateRegistrationPolicyEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        protected static Uri CreateRegistrationTermsOfServiceEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return CreateEndpoint(fixture);
        }

        private static string CreateDomainName(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return $"{fixture.Create<string>().Replace("/", string.Empty)}.local";
        }

        private static Uri CreateEndpoint(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Uri($"https://{CreateDomainName(fixture)}/{fixture.Create<string>().Replace("/", string.Empty)}");
        }

        private static string[] CreateStringArray(Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            return fixture.CreateMany<string>(random.Next(1, 10)).ToArray();
        }
    }
}