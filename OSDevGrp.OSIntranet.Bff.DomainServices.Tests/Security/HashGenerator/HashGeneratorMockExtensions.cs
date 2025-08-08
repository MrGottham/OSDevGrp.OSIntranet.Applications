using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.HashGenerator;

internal static class HashGeneratorMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IHashGenerator> hashGeneratorMock, Fixture fixture, string? hash = null)
    {
        hashGeneratorMock.Setup(m => m.GenerateAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(hash ?? fixture.Create<string>()));
    }

    #endregion
}