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

        public static Mock<IToken> BuildTokenMock(this Fixture fixture, string tokenType = null, string accessToken = null, DateTime? expires = null, bool hasExpired = false, byte[] tokenByteArray = null, string base64Token = null, bool willExpireWithin = false)
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
                .Returns(base64Token ?? fixture.Create<string>());
            tokenMock.Setup(m => m.WillExpireWithin(It.IsAny<TimeSpan>()))
                .Returns(willExpireWithin);
            return tokenMock;
        }

        public static Mock<IRefreshableToken> BuildRefreshableTokenMock(this Fixture fixture, string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null, bool hasExpired = false, byte[] tokenByteArray = null, string base64Token = null, bool willExpireWithin = false)
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
                .Returns(base64Token ?? fixture.Create<string>());
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
    }
}