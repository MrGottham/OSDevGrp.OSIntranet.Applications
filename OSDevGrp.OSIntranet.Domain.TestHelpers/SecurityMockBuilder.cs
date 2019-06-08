using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

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

        public static Mock<IClientSecretIdentity> BuildClientSecretIdentityMock(this Fixture fixture, string clientId = null, string clientSecret = null, IToken token = null, IEnumerable<Claim> claims = null)
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
            clientSecretIdentityMock.Setup(m => m.Token)
                .Returns(token ?? fixture.BuildTokenMock().Object);
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

        public static Mock<IToken> BuildTokenMock(this Fixture fixture, string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IToken> tokenMock = new Mock<IToken>();
            tokenMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            tokenMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            tokenMock.Setup(m => m.Expires)
                .Returns(expires ?? DateTime.UtcNow.AddMinutes(new Random(fixture.Create<int>()).Next(30, 60)));
            return tokenMock;
        }

        public static Mock<IRefreshableToken> BuildRefreshableTokenMock(this Fixture fixture, string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IRefreshableToken> refreshableTokenMock = new Mock<IRefreshableToken>();
            refreshableTokenMock.Setup(m => m.TokenType)
                .Returns(tokenType ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.AccessToken)
                .Returns(accessToken ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.RefreshToken)
                .Returns(refreshToken ?? fixture.Create<string>());
            refreshableTokenMock.Setup(m => m.Expires)
                .Returns(expires ?? DateTime.UtcNow.AddMinutes(new Random(fixture.Create<int>()).Next(30, 60)));
            return refreshableTokenMock;
        }
    }
}