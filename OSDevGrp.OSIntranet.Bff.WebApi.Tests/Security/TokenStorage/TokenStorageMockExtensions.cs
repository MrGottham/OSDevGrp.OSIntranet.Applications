using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenStorage;

internal static class TokenStorageMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ITokenStorage> tokenStorageMock, Fixture fixture, Random random, IToken? token = null)
    {
        tokenStorageMock.Setup(m => m.GetTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(token ?? fixture.CreateToken(random)));
        tokenStorageMock.Setup(m => m.StoreTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<IToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        tokenStorageMock.Setup(m => m.DeleteTokenAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    #endregion
}