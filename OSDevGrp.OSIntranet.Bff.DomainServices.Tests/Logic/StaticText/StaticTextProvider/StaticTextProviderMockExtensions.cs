using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;

internal static class StaticTextProviderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IStaticTextProvider> staticTextProviderMock, Fixture fixture)
    {
        staticTextProviderMock.Setup(m => m.GetStaticTextAsync(It.IsAny<StaticTextKey>(), It.IsAny<IEnumerable<object>>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns<StaticTextKey, IEnumerable<object>, IFormatProvider, CancellationToken>((staticTextKey, _, _, _) => Task.FromResult($"{staticTextKey}: {fixture!.Create<string>()}"));
    }

    #endregion    
}