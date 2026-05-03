using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingLineCollectionTextsBuilder;

[TestFixture]
public class BuildAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.LatestPostings, 1)]
    [TestCase(StaticTextKey.PostingDate, 1)]
    [TestCase(StaticTextKey.PostingReference, 1)]
    [TestCase(StaticTextKey.Account, 1)]
    [TestCase(StaticTextKey.PostingText, 1)]
    [TestCase(StaticTextKey.BudgetAccount, 1)]
    [TestCase(StaticTextKey.Debit, 1)]
    [TestCase(StaticTextKey.Credit, 1)]
    [TestCase(StaticTextKey.PostingValue, 1)]
    [TestCase(StaticTextKey.ContactAccount, 1)]
    public async Task BuildAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithExpectedStaticTextKeys(StaticTextKey staticTextKey, int expectedCalls)
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(postingLineModels, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Exactly(expectedCalls));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsPostingLineCollectionTexts()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<PostingLineCollectionTexts>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereLatestPostingsHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.LatestPostingsHeader, Does.StartWith($"{StaticTextKey.LatestPostings}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingDateHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.PostingDateHeader, Does.StartWith($"{StaticTextKey.PostingDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingReferenceHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.PostingReferenceHeader, Does.StartWith($"{StaticTextKey.PostingReference}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereAccountHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.AccountHeader, Does.StartWith($"{StaticTextKey.Account}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingTextHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.PostingTextHeader, Does.StartWith($"{StaticTextKey.PostingText}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereBudgetAccountHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.BudgetAccountHeader, Does.StartWith($"{StaticTextKey.BudgetAccount}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereDebitHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.DebitHeader, Does.StartWith($"{StaticTextKey.Debit}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereCreditHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.CreditHeader, Does.StartWith($"{StaticTextKey.Credit}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingValueHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.PostingValueHeader, Does.StartWith($"{StaticTextKey.PostingValue}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereContactAccountHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.ContactAccountHeader, Does.StartWith($"{StaticTextKey.ContactAccount}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWhereSummaryHeaderIsEqualToStaticTextFromStaticTextProvider()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.SummaryHeader, Does.StartWith($"{StaticTextKey.LatestPostings}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingLinesIsNotEmpty()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(result.PostingLines, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalled_ReturnsAccountingTextsWherePostingLinesContainsEachPostingLine()
    {
        IPostingLineCollectionTextsBuilder sut = CreateSut();

        PostingLineModel[] postingLineModels = _fixture!.CreatePostingLineModels(_random!);
        IPostingLineCollectionTexts result = await sut.BuildAsync(postingLineModels, CultureInfo.InvariantCulture);

        Assert.That(postingLineModels.All(postingLineModel => result.PostingLines.SingleOrDefault(postingLineDisplayer => Guid.Parse(postingLineDisplayer.Identification) == postingLineModel.Identifier) != null));
    }

    private IPostingLineCollectionTextsBuilder CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.DynamicText.PostingLineCollectionTextsBuilder(_staticTextProviderMock!.Object);
    }
}