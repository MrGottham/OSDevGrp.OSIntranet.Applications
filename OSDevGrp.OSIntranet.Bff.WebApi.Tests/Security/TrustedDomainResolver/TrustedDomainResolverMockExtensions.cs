using Moq;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TrustedDomainResolver;

internal static class TrustedDomainResolverMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<ITrustedDomainResolver> trustedDomainResolverMock, bool isTrustedDomain = true)
    {
        trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
            .Returns(isTrustedDomain);
    }

    #endregion
}