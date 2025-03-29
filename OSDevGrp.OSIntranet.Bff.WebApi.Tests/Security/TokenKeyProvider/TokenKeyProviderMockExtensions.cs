using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyProvider;

internal static class TokenKeyProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ITokenKeyProvider> tokenKeyProviderMock, Fixture fixture, string? tokenKey = null)
    {
        tokenKeyProviderMock.Setup(m => m.ResolveAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(tokenKey ?? fixture.Create<string>()));
    }

    #endregion
}