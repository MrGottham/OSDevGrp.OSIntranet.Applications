using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeGenerator;

internal static class VerificationCodeGeneratorMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IVerificationCodeGenerator> verificationCodeGeneratorMock, Fixture fixture, string? verificationCode = null)
    {
        verificationCodeGeneratorMock.Setup(m => m.GenerateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(verificationCode ?? fixture.Create<string>()));
    }

    #endregion
}