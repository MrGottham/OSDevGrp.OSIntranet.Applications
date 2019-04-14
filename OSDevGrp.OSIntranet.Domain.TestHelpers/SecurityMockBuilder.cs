using System;
using System.Security.Claims;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class SecurityMockBuilder
    {
        public static Mock<IUserIdentity> BuildUserIdentityMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            return new Mock<IUserIdentity>();
        }

        public static Mock<IClientSecretIdentity> BuildClientSecretIdentityMock(this Fixture fixture, string clientSecret = null, IToken token = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IClientSecretIdentity> clientSecretIdentityMock = new Mock<IClientSecretIdentity>();
            clientSecretIdentityMock.Setup(m => m.ClientSecret)
                .Returns(clientSecret ?? fixture.Create<string>());
            clientSecretIdentityMock.Setup(m => m.Token)
                .Returns(token ?? fixture.BuildTokenMock().Object);
            clientSecretIdentityMock.Setup(m => m.ToClaimsIdentity())
                .Returns(new ClaimsIdentity());
            return clientSecretIdentityMock;
        }

        public static Mock<IToken> BuildTokenMock(this Fixture fixture, string value = null, DateTime? expires = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IToken> tokenMock = new Mock<IToken>();
            tokenMock.Setup(m => m.Value)
                .Returns(value ?? fixture.Create<string>());
            tokenMock.Setup(m => m.Expires)
                .Returns(expires ?? DateTime.UtcNow.AddMinutes(new Random(fixture.Create<int>()).Next(30, 60)));
            return tokenMock;
        }
    }
}