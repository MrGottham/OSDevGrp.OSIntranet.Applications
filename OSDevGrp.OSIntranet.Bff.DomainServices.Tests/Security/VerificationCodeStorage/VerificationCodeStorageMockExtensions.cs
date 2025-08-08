using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeStorage;

internal static class VerificationCodeStorageMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IVerificationCodeStorage> verificationCodeStorageMock, Fixture fixture, bool hasVerificationCode = true, string? verificationCode = null)
    {
        verificationCodeStorageMock.Setup(m => m.GetVerificationCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(hasVerificationCode ? verificationCode ?? fixture.Create<string>() : null));
        verificationCodeStorageMock.Setup(m => m.StoreVerificationCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        verificationCodeStorageMock.Setup(m => m.RemoveVerificationCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    #endregion
}