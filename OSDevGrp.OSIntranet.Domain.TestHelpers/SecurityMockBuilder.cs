using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class SecurityMockBuilder
    {
        public static Mock<IUserIdentity> BuildUserIdentityMock(this Fixture fixture, IEnumerable<Claim> claims = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IUserIdentity> userIdentityMock = new Mock<IUserIdentity>();
            userIdentityMock.Setup(m => m.Identifier)
                .Returns(fixture.Create<int>());
            userIdentityMock.Setup(m => m.ExternalUserIdentifier)
                .Returns(fixture.Create<string>());
            userIdentityMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            userIdentityMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            userIdentityMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            userIdentityMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            userIdentityMock.Setup(m => m.ToClaimsIdentity())
                .Returns(new ClaimsIdentity(claims ?? new List<Claim>(0)));
            return userIdentityMock;
        }

        public static Mock<IClientSecretIdentity> BuildClientSecretIdentityMock(this Fixture fixture, string clientId = null, string clientSecret = null, IEnumerable<Claim> claims = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IClientSecretIdentity> clientSecretIdentityMock = new Mock<IClientSecretIdentity>();
            clientSecretIdentityMock.Setup(m => m.Identifier)
                .Returns(fixture.Create<int>());
            clientSecretIdentityMock.Setup(m => m.FriendlyName)
                .Returns(fixture.Create<string>());
            clientSecretIdentityMock.Setup(m => m.ClientId)
                .Returns(clientId ?? fixture.Create<string>());
            clientSecretIdentityMock.Setup(m => m.ClientSecret)
                .Returns(clientSecret ?? fixture.Create<string>());
            clientSecretIdentityMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            clientSecretIdentityMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            clientSecretIdentityMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            clientSecretIdentityMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            clientSecretIdentityMock.Setup(m => m.ToClaimsIdentity())
                .Returns(new ClaimsIdentity(claims ?? new List<Claim>(0)));
            return clientSecretIdentityMock;
        }

        public static Mock<IToken> BuildTokenMock(this Fixture fixture, string tokenType = null, string accessToken = null, DateTime? expires = null, bool hasExpired = false, byte[] tokenByteArray = null, string toBase64String = null, bool willExpireWithin = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IToken> tokenMock = new Mock<IToken>();
            tokenMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            tokenMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            tokenMock.Setup(m => m.Expires)
                .Returns(expires ?? DateTime.UtcNow.AddMinutes(new Random(fixture.Create<int>()).Next(30, 60)));
            tokenMock.Setup(m => m.HasExpired)
                .Returns(hasExpired);
            tokenMock.Setup(m => m.ToByteArray())
                .Returns(tokenByteArray ?? fixture.CreateMany<byte>(random.Next(512, 1024)).ToArray());
            tokenMock.Setup(m => m.ToBase64String())
                .Returns(toBase64String ?? fixture.Create<string>());
            tokenMock.Setup(m => m.WillExpireWithin(It.IsAny<TimeSpan>()))
                .Returns(willExpireWithin);
            return tokenMock;
        }

        public static Mock<IRefreshableToken> BuildRefreshableTokenMock(this Fixture fixture, string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null, bool hasExpired = false, byte[] tokenByteArray = null, string toBase64String = null, bool willExpireWithin = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IRefreshableToken> refreshableTokenMock = new Mock<IRefreshableToken>();
            refreshableTokenMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.RefreshToken)
                .Returns(refreshToken ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.Expires)
                .Returns(expires ?? DateTime.UtcNow.AddMinutes(new Random(fixture.Create<int>()).Next(30, 60)));
            refreshableTokenMock.Setup(m => m.HasExpired)
                .Returns(hasExpired);
            refreshableTokenMock.Setup(m => m.ToByteArray())
                .Returns(tokenByteArray ?? fixture.CreateMany<byte>(random.Next(512, 1024)).ToArray());
            refreshableTokenMock.Setup(m => m.ToBase64String())
                .Returns(toBase64String ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.WillExpireWithin(It.IsAny<TimeSpan>()))
                .Returns(willExpireWithin);
            return refreshableTokenMock;
        }

        public static Mock<ITokenBasedQuery> BuildTokenBasedQueryMock(this Fixture fixture, string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ITokenBasedQuery> tokenBasedQueryMock = new Mock<ITokenBasedQuery>();
            tokenBasedQueryMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            tokenBasedQueryMock.Setup(m => m.AccessToken)
                .Returns(accessToken?? fixture.Create<string>());
            tokenBasedQueryMock.Setup(m => m.Expires)
                .Returns(expires ?? fixture.Create<DateTime>());
            return tokenBasedQueryMock;
        }

        public static Mock<IRefreshableTokenBasedQuery> BuildRefreshableTokenBasedQueryMock(this Fixture fixture, string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IRefreshableTokenBasedQuery> refreshableTokenBasedQueryMock = new Mock<IRefreshableTokenBasedQuery>();
            refreshableTokenBasedQueryMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            refreshableTokenBasedQueryMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            refreshableTokenBasedQueryMock.Setup(m => m.RefreshToken)
                .Returns(refreshToken ?? fixture.Create<string>());
            refreshableTokenBasedQueryMock.Setup(m => m.Expires)
                .Returns(expires ?? fixture.Create<DateTime>());
            return refreshableTokenBasedQueryMock;
        }

        public static Mock<ITokenBasedCommand> BuildTokenBasedCommandMock(this Fixture fixture, string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ITokenBasedCommand> tokenBasedCommandMock = new Mock<ITokenBasedCommand>();
            tokenBasedCommandMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            tokenBasedCommandMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            tokenBasedCommandMock.Setup(m => m.Expires)
                .Returns(expires ?? fixture.Create<DateTime>());
            return tokenBasedCommandMock;
        }

        public static Mock<IRefreshableTokenBasedCommand> BuildRefreshableTokenBasedCommandMock(this Fixture fixture, string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = new Mock<IRefreshableTokenBasedCommand>();
            refreshableTokenBasedCommandMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            refreshableTokenBasedCommandMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            refreshableTokenBasedCommandMock.Setup(m => m.RefreshToken)
                .Returns(refreshToken ?? fixture.Create<string>());
            refreshableTokenBasedCommandMock.Setup(m => m.Expires)
                .Returns(expires ?? fixture.Create<DateTime>());
            return refreshableTokenBasedCommandMock;
        }

        public static Mock<IOpenIdProviderConfiguration> BuildOpenIdProviderConfigurationMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IOpenIdProviderConfiguration> openIdProviderConfigurationMock = new Mock<IOpenIdProviderConfiguration>();
            openIdProviderConfigurationMock.Setup(m => m.Issuer)
                .Returns(new Uri($"https://{fixture.CreateDomainName()}", UriKind.Absolute));
            openIdProviderConfigurationMock.Setup(m => m.AuthorizationEndpoint)
                .Returns(fixture.CreateEndpoint());
            openIdProviderConfigurationMock.Setup(m => m.TokenEndpoint)
                .Returns(fixture.CreateEndpoint());
            openIdProviderConfigurationMock.Setup(m => m.UserInfoEndpoint)
                .Returns(random.Next(100) > 50 ? fixture.CreateEndpoint() : null);
            openIdProviderConfigurationMock.Setup(m => m.JsonWebKeySetEndpoint)
                .Returns(fixture.CreateEndpoint());
            openIdProviderConfigurationMock.Setup(m => m.RegistrationEndpoint)
                .Returns(random.Next(100) > 50 ? fixture.CreateEndpoint() : null);
            openIdProviderConfigurationMock.Setup(m => m.ScopesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.ResponseTypesSupported)
                .Returns(fixture.CreateStringArray(random));
            openIdProviderConfigurationMock.Setup(m => m.ResponseModesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.GrantTypesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.AuthenticationContextClassReferencesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.SubjectTypesSupported)
                .Returns(fixture.CreateStringArray(random));
            openIdProviderConfigurationMock.Setup(m => m.IdTokenSigningAlgValuesSupported)
                .Returns(fixture.CreateStringArray(random));
            openIdProviderConfigurationMock.Setup(m => m.IdTokenEncryptionAlgValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.IdTokenEncryptionEncValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.UserInfoSigningAlgValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.UserInfoEncryptionAlgValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.UserInfoEncryptionEncValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.RequestObjectSigningAlgValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.RequestObjectEncryptionAlgValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.RequestObjectEncryptionEncValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.TokenEndpointAuthenticationMethodsSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.TokenEndpointAuthenticationSigningAlgValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.DisplayValuesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.ClaimTypesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.ClaimsSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.ServiceDocumentationEndpoint)
                .Returns(random.Next(100) > 50 ? fixture.CreateEndpoint() : null);
            openIdProviderConfigurationMock.Setup(m => m.ClaimsLocalesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.UiLocalesSupported)
                .Returns(random.Next(100) > 50 ? fixture.CreateStringArray(random) : null);
            openIdProviderConfigurationMock.Setup(m => m.ClaimsParameterSupported)
                .Returns(random.Next(100) > 50 ? fixture.Create<bool>() : null);
            openIdProviderConfigurationMock.Setup(m => m.RequestParameterSupported)
                .Returns(random.Next(100) > 50 ? fixture.Create<bool>() : null);
            openIdProviderConfigurationMock.Setup(m => m.RequestUriParameterSupported)
                .Returns(random.Next(100) > 50 ? fixture.Create<bool>() : null);
            openIdProviderConfigurationMock.Setup(m => m.RequireRequestUriRegistration)
                .Returns(random.Next(100) > 50 ? fixture.Create<bool>() : null);
            openIdProviderConfigurationMock.Setup(m => m.RegistrationPolicyEndpoint)
                .Returns(random.Next(100) > 50 ? fixture.CreateEndpoint() : null);
            openIdProviderConfigurationMock.Setup(m => m.RegistrationTermsOfServiceEndpoint)
                .Returns(random.Next(100) > 50 ? fixture.CreateEndpoint() : null);
            return openIdProviderConfigurationMock;
        }

        public static Mock<IScope> BuildScopeMock(this Fixture fixture, IEnumerable<string> relatedClaims = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IScope> scopeMock = new Mock<IScope>();
            scopeMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            scopeMock.Setup(m => m.Description)
                .Returns(fixture.Create<string>());
            scopeMock.Setup(m => m.RelatedClaims)
                .Returns(relatedClaims ?? fixture.CreateMany<string>(random.Next(1, 10)).ToArray());
            return scopeMock;
        }

        private static string CreateDomainName(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return $"{fixture.Create<string>().Replace("/", string.Empty)}.local";
        }

        private static Uri CreateEndpoint(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Uri($"https://{CreateDomainName(fixture)}/{fixture.Create<string>().Replace("/", string.Empty)}", UriKind.Absolute);
        }

        private static string[] CreateStringArray(this Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return fixture.CreateMany<string>(random.Next(1, 10)).ToArray();
        }
    }
}