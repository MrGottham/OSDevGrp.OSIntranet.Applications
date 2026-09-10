using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingJournalTextsBuilder;

internal static class PostingJournalTextsBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IPostingJournalTextsBuilder> postingJournalTextsBuilderMock, IPostingJournalTexts? postingJournalTexts = null)
    {
        postingJournalTextsBuilderMock.Setup(m => m.BuildAsync(It.IsAny<Tuple<ApplyPostingJournalModel, Predicate<int>>>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(postingJournalTexts ?? new Mock<IPostingJournalTexts>().Object));
    }

    #endregion
}