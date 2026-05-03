using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingLineCollectionTextsBuilder;

internal static class PostingLineCollectionTextsBuilderMockExtensions
{
    #region Methods

    internal static void Setup(this Mock<IPostingLineCollectionTextsBuilder> postingLineCollectionTextsBuilderMock, IPostingLineCollectionTexts? postingLineCollectionTexts = null)
    {
        postingLineCollectionTextsBuilderMock.Setup(m => m.BuildAsync(It.IsAny<IReadOnlyCollection<PostingLineModel>>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(postingLineCollectionTexts ?? new Mock<IPostingLineCollectionTexts>().Object));
    }

    #endregion    
}