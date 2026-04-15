using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText;

internal static class AccountingTextsBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IAccountingTextsBuilder> accountingTextsBuilderMock, IAccountingTexts? accountingTexts = null)
    {
        accountingTextsBuilderMock.Setup(m => m.BuildAsync(It.IsAny<AccountingModel>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingTexts ?? new Mock<IAccountingTexts>().Object));
    }

    #endregion    
}