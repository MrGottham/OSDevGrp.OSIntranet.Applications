using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeVerifier;

internal static class VerificationCodeVerifierMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IVerificationCodeVerifier> verificationCodeVerifierMock, bool verifyResult = true)
    {
        verificationCodeVerifierMock.Setup(m => m.VerifyAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(verifyResult));
    }

    #endregion
}