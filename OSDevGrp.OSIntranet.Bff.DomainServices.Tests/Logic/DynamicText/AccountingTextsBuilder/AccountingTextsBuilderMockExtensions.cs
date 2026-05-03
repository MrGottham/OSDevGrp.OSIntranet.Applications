using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.AccountingTextsBuilder;

internal static class AccountingTextsBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IAccountingTextsBuilder> accountingTextsBuilderMock, IAccountingTexts? accountingTexts = null)
    {
        accountingTextsBuilderMock.Setup(m => m.BuildAsync(It.IsAny<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingTexts ?? new Mock<IAccountingTexts>().Object));
        accountingTextsBuilderMock.Setup(m => m.BuildAsync(It.IsAny<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>>>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountingTexts ?? new Mock<IAccountingTexts>().Object));
    }

    #endregion    
}