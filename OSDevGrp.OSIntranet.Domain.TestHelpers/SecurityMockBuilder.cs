using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Core.TestHelpers;
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
                .Returns(fixture.CreateEndpoint(withoutPathAndQuery: true));
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

        public static Mock<IScope> BuildScopeMock(this Fixture fixture, string name = null, string description = null, IEnumerable<string> relatedClaims = null, IEnumerable<Claim> filteredClaims = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            string[] claimTypes = (relatedClaims ?? fixture.CreateMany<string>(random.Next(1, 10))).ToArray();
            filteredClaims ??= claimTypes.Take(random.Next(0, claimTypes.Length - 1)).Select(claimType => fixture.CreateClaim(claimType)).ToArray();

            Mock<IScope> scopeMock = new Mock<IScope>();
            scopeMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            scopeMock.Setup(m => m.Description)
                .Returns(description ?? fixture.Create<string>());
            scopeMock.Setup(m => m.RelatedClaims)
                .Returns(claimTypes);
            scopeMock.Setup(m => m.Filter(It.IsAny<IEnumerable<Claim>>()))
                .Returns(filteredClaims);
            return scopeMock;
        }

        public static Mock<IAuthorizationState> BuildAuthorizationStateMock(this Fixture fixture, string responseType = null, string clientId = null, bool hasClientSecret = false, string clientSecret = null, Uri redirectUri = null, string[] scopes = null, bool hasExternalState = true, string externalState = null, bool hasNonce = true, string nonce = null, bool hasAuthorizationCode = false, IAuthorizationCode authorizationCode = null, string toStringValue = null, IAuthorizationStateBuilder toAuthorizationStateBuilder = null, Uri redirectUriWithAuthorizationCode = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());
            toStringValue ??= Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray());

            Mock<IAuthorizationState> authorizationStateMock = new Mock<IAuthorizationState>();
            authorizationStateMock.Setup(m => m.ResponseType)
                .Returns(responseType ?? fixture.Create<string>());
            authorizationStateMock.Setup(m => m.ClientId)
                .Returns(clientId ?? fixture.Create<string>());
            authorizationStateMock.Setup(m => m.ClientSecret)
                .Returns(hasClientSecret ? clientSecret ?? fixture.Create<string>() : null);
            authorizationStateMock.Setup(m => m.RedirectUri)
                .Returns(redirectUri ?? fixture.CreateEndpoint());
            authorizationStateMock.Setup(m => m.Scopes)
                .Returns(scopes ?? fixture.CreateStringArray(random));
            authorizationStateMock.Setup(m => m.ExternalState)
                .Returns(hasExternalState ? externalState ?? fixture.Create<string>() : null);
            authorizationStateMock.Setup(m => m.Nonce)
                .Returns(hasNonce ? nonce ?? fixture.Create<string>() : null);
            authorizationStateMock.Setup(m => m.AuthorizationCode)
                .Returns(hasAuthorizationCode ? authorizationCode ?? fixture.BuildAuthorizationCodeMock().Object : null);
            authorizationStateMock.Setup(m => m.ToString())
                .Returns(toStringValue);
            authorizationStateMock.Setup(m => m.ToString(It.IsAny<Func<byte[], byte[]>>()))
                .Returns(toStringValue);
            authorizationStateMock.Setup(m => m.ToBuilder())
                .Returns(toAuthorizationStateBuilder ?? fixture.BuildAuthorizationStateBuilderMock(authorizationState: authorizationStateMock.Object).Object);
            authorizationStateMock.Setup(m => m.GenerateRedirectUriWithAuthorizationCode())
                .Returns(redirectUriWithAuthorizationCode ?? fixture.CreateEndpoint());
            return authorizationStateMock;
        }

        public static Mock<IAuthorizationCode> BuildAuthorizationCodeMock(this Fixture fixture, bool hasValue = true, string value = null, DateTimeOffset? expires = null, bool expired = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<IAuthorizationCode> authorizationCodeMock = new Mock<IAuthorizationCode>();
            authorizationCodeMock.Setup(m => m.Value)
                .Returns(hasValue ? value ?? fixture.Create<string>() : null);
            authorizationCodeMock.Setup(m => m.Expires)
                .Returns(expires?.UtcDateTime ?? DateTime.UtcNow.AddSeconds(random.Next(60, 600)));
            authorizationCodeMock.Setup(m => m.Expired)
                .Returns(expired);
            return authorizationCodeMock;
        }

        public static Mock<IAuthorizationStateBuilder> BuildAuthorizationStateBuilderMock(this Fixture fixture, IAuthorizationState authorizationState = null)
        {
            Mock<IAuthorizationStateBuilder> authorizationStateBuilderMock = new Mock<IAuthorizationStateBuilder>();
            authorizationStateBuilderMock.Setup(m => m.WithClientSecret(It.IsAny<string>()))
                .Returns(authorizationStateBuilderMock.Object);
            authorizationStateBuilderMock.Setup(m => m.WithExternalState(It.IsAny<string>()))
                .Returns(authorizationStateBuilderMock.Object);
            authorizationStateBuilderMock.Setup(m => m.WithNonce(It.IsAny<string>()))
                .Returns(authorizationStateBuilderMock.Object);
            authorizationStateBuilderMock.Setup(m => m.WithAuthorizationCode(It.IsAny<IAuthorizationCode>()))
                .Returns(authorizationStateBuilderMock.Object);
            authorizationStateBuilderMock.Setup(m => m.WithAuthorizationCode(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .Returns(authorizationStateBuilderMock.Object);
            authorizationStateBuilderMock.Setup(m => m.Build())
                .Returns(authorizationState ?? fixture.BuildAuthorizationStateMock(toAuthorizationStateBuilder: authorizationStateBuilderMock.Object).Object);
            return authorizationStateBuilderMock;
        }

        public static Mock<IUserInfo> BuildUserInfoMock(this Fixture fixture, string subject = null, string fullName = null, string givenName = null, string surname = null, string middleName = null, string nickName = null, string preferredUsername = null, Uri profile = null, Uri picture = null, Uri webpage = null, string email = null, bool? emailVerified = null, string gender = null, DateTimeOffset? birthdate = null, string timeZone = null, string locale = null, string phoneNumber = null, bool? phoneNumberVerified = null, IUserAddress userAddress = null, DateTimeOffset? updatedAt = null, string toJson = null, IEnumerable<Claim> toClaims = null)
        {
            Random random = new Random(fixture.Create<int>());

            Mock<IUserInfo> userInfoMock = new Mock<IUserInfo>();
            userInfoMock.Setup(m => m.Subject)
                .Returns(subject ?? fixture.Create<string>());
            userInfoMock.Setup(m => m.FullName)
                .Returns(fullName ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.GivenName)
                .Returns(givenName ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.Surname)
                .Returns(surname ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.MiddleName)
                .Returns(middleName ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.NickName)
                .Returns(nickName ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.PreferredUsername)
                .Returns(preferredUsername ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.Profile)
                .Returns(profile ?? (random.Next(100) > 50 ? fixture.CreateEndpoint() : null));
            userInfoMock.Setup(m => m.Picture)
                .Returns(picture ?? (random.Next(100) > 50 ? fixture.CreateEndpoint() : null));
            userInfoMock.Setup(m => m.Webpage)
                .Returns(webpage ?? (random.Next(100) > 50 ? fixture.CreateEndpoint() : null));
            userInfoMock.Setup(m => m.Email)
                .Returns(email ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.EmailVerified)
                .Returns(emailVerified ?? (random.Next(100) > 50 ? fixture.Create<bool>() : null));
            userInfoMock.Setup(m => m.Gender)
                .Returns(gender ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.Birthdate)
                .Returns(birthdate ?? (random.Next(100) > 50 ? DateTimeOffset.UtcNow.AddYears(random.Next(25, 50) * -1).AddDays(random.Next(0, 365) * -1) : null));
            userInfoMock.Setup(m => m.TimeZone)
                .Returns(timeZone ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.Locale)
                .Returns(locale ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.PhoneNumber)
                .Returns(phoneNumber ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.PhoneNumberVerified)
                .Returns(phoneNumberVerified ?? (random.Next(100) > 50 ? fixture.Create<bool>() : null));
            userInfoMock.Setup(m => m.Address)
                .Returns(userAddress ?? (random.Next(100) > 50 ? fixture.BuildUserAddressMock().Object : null));
            userInfoMock.Setup(m => m.UpdatedAt)
                .Returns(updatedAt ?? (random.Next(100) > 50 ? DateTimeOffset.UtcNow.AddDays(random.Next(0, 365) * -1) : null));
            userInfoMock.Setup(m => m.ToJson())
                .Returns(toJson ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userInfoMock.Setup(m => m.ToClaims())
                .Returns(toClaims ?? fixture.CreateClaims(random));
            return userInfoMock;
        }

        public static Mock<IUserAddress> BuildUserAddressMock(this Fixture fixture, string mailingAddress = null, string streetAddress = null, string city = null, string region = null, string postalCode = null, string country = null, bool? isEmpty = null, string toJson = null)
        {
            Random random = new Random(fixture.Create<int>());

            Mock<IUserAddress> userAddressMock = new Mock<IUserAddress>();
            userAddressMock.Setup(m => m.MailingAddress)
                .Returns(mailingAddress ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userAddressMock.Setup(m => m.StreetAddress)
                .Returns(streetAddress ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userAddressMock.Setup(m => m.City)
                .Returns(city ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userAddressMock.Setup(m => m.Region)
                .Returns(region ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userAddressMock.Setup(m => m.PostalCode)
                .Returns(postalCode ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userAddressMock.Setup(m => m.Country)
                .Returns(country ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            userAddressMock.Setup(m => m.IsEmpty())
                .Returns(isEmpty ?? fixture.Create<bool>());
            userAddressMock.Setup(m => m.ToJson())
                .Returns(toJson ?? (random.Next(100) > 50 ? fixture.Create<string>() : null));
            return userAddressMock;
        }

        private static string[] CreateStringArray(this Fixture fixture, Random random)
        {
            NullGuard.NotNull(fixture, nameof(fixture))
                .NotNull(random, nameof(random));

            return fixture.CreateMany<string>(random.Next(1, 10)).ToArray();
        }
    }
}