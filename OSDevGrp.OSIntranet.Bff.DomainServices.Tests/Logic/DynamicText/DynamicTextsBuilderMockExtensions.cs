using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;

internal static class DynamicTextsBuilderMockExtensions
{
    #region Methods

    internal static void Setup<TModel, TDynamicTexts>(this Mock<IDynamicTextsBuilder<TModel, TDynamicTexts>> dynamicTextsBuilderMock, TDynamicTexts? dynamicTexts = null) where TModel : class where TDynamicTexts : class, IDynamicTexts
    {
        dynamicTextsBuilderMock.Setup(m => m.BuildAsync(It.IsAny<TModel>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(dynamicTexts ?? new Mock<TDynamicTexts>().Object));
    }

    #endregion    
}