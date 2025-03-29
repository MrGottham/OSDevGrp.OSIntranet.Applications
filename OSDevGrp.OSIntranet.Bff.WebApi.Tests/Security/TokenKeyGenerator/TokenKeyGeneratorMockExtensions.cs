using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyGenerator;

internal static class TokenKeyGeneratorMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ITokenKeyGenerator> tokenKeyGeneratorMock, Fixture fixture, string? tokenKey = null)
    {
        tokenKeyGeneratorMock.Setup(m => m.GenerateAsync(It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(tokenKey ?? fixture.Create<string>()));
    }

    #endregion
}