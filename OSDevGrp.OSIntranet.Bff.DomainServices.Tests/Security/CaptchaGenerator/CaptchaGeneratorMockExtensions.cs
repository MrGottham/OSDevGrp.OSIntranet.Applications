using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.CaptchaGenerator;

internal static class CaptchaGeneratorMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ICaptchaGenerator> captchaGeneratorMock, Fixture fixture, Random random, byte[]? captchaImage = null)
    {
        captchaGeneratorMock.Setup(m => m.GenerateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(captchaImage ?? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()));
    }

    #endregion
}