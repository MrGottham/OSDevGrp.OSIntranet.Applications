using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Notification.PostingWarningModelToNotificationConverterTests;

[TestFixture]
public class ConvertAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    #region Methods

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ConvertAsync_WhenCalled_ReturnsNotificationWithMarkdownFormat()
    {
        PostingWarningModel postingWarningModel = CreatePostingWarningModel(PostingWarningReason.AccountIsOverdrawn);
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        IPostingWarningModelToNotificationConverter sut = CreateSut();

        INotification result = await sut.ConvertAsync(postingWarningModel, formatProvider, cancellationToken);

        Assert.That(result.Format, Is.EqualTo(NotificationFormat.Markdown));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ConvertAsync_WhenCalled_ReturnsHeaderFromStaticTextProvider()
    {
        PostingWarningModel postingWarningModel = CreatePostingWarningModel(PostingWarningReason.AccountIsOverdrawn);
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        IPostingWarningModelToNotificationConverter sut = CreateSut();

        INotification result = await sut.ConvertAsync(postingWarningModel, formatProvider, cancellationToken);

        Assert.That(result.Header, Is.EqualTo("Static text"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ConvertAsync_WhenCalled_ReturnsContentFromStaticTextProvider()
    {
        PostingWarningModel postingWarningModel = CreatePostingWarningModel(PostingWarningReason.AccountIsOverdrawn);
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        IPostingWarningModelToNotificationConverter sut = CreateSut();

        INotification result = await sut.ConvertAsync(postingWarningModel, formatProvider, cancellationToken);

        Assert.That(result.Content, Is.EqualTo("Static text"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ConvertAsync_WhenCalled_GetsHeaderStaticTextWithExpectedFormatProviderAndCancellationToken()
    {
        PostingWarningModel postingWarningModel = CreatePostingWarningModel(PostingWarningReason.AccountIsOverdrawn);
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        IPostingWarningModelToNotificationConverter sut = CreateSut();

        await sut.ConvertAsync(postingWarningModel, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(mock => mock.GetStaticTextAsync(
                StaticTextKey.Warning,
                It.Is<IEnumerable<object>>(arguments => !arguments.Any()),
                It.Is<IFormatProvider>(value => ReferenceEquals(value, formatProvider)),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(PostingWarningReason.AccountIsOverdrawn, StaticTextKey.AccountIsOverdrawn)]
    [TestCase(PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet, StaticTextKey.ExpectedIncomeHasNotBeenReachedYet)]
    [TestCase(PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached, StaticTextKey.ExpectedExpensesHaveAlreadyBeenReached)]
    public async Task ConvertAsync_WhenCalled_GetsWarningContentWithExpectedStaticTextAndFormattedArguments(PostingWarningReason reason, StaticTextKey expectedContentStaticTextKey)
    {
        PostingWarningModel postingWarningModel = CreatePostingWarningModel(reason);
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        string expectedAmount = postingWarningModel.Amount.ToString("C", formatProvider);
        object[] expectedContentArguments =
        [
            postingWarningModel.Account.AccountName,
            expectedAmount,
            Environment.NewLine,
            Environment.NewLine,
            postingWarningModel.PostingLine.PostingDate.ToString("d", formatProvider),
            postingWarningModel.PostingLine.Details
        ];
        IPostingWarningModelToNotificationConverter sut = CreateSut();

        await sut.ConvertAsync(postingWarningModel, formatProvider, cancellationToken);

        _staticTextProviderMock!.Verify(mock => mock.GetStaticTextAsync(
                expectedContentStaticTextKey,
                It.Is<IEnumerable<object>>(arguments => arguments.SequenceEqual(expectedContentArguments)),
                It.Is<IFormatProvider>(value => ReferenceEquals(value, formatProvider)),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ConvertAsync_WhenReasonIsUnsupported_ThrowsNotSupportedExceptionIdentifyingReason()
    {
        PostingWarningModel postingWarningModel = CreatePostingWarningModel((PostingWarningReason) 999);
        IPostingWarningModelToNotificationConverter sut = CreateSut();

        NotSupportedException exception = (await Assert.ThrowsAsync<NotSupportedException>(async () =>
            await sut.ConvertAsync(postingWarningModel, CultureInfo.InvariantCulture)))!;

        Assert.That(exception.Message, Does.Contain("999"));
        _staticTextProviderMock!.Verify(mock => mock.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private IPostingWarningModelToNotificationConverter CreateSut()
    {
        _staticTextProviderMock!.Setup(mock => mock.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Static text");

        return new PostingWarningModelToNotificationConverter(_staticTextProviderMock!.Object);
    }

    private PostingWarningModel CreatePostingWarningModel(PostingWarningReason reason)
    {
        Fixture fixture = _fixture!;
        Random random = _random!;
        AccountIdentificationModel account = new(
            fixture.CreateAccountingIdentificationModel(random),
            "Test account",
            fixture.CreateAccountNumber());
        PostingLineModel postingLine = fixture.CreatePostingLineModel(random, account);

        return new PostingWarningModel(account, 1234.56, postingLine, reason);
    }

    #endregion
}