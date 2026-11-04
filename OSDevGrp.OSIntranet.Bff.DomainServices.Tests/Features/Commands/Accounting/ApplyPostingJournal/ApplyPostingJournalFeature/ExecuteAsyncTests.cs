using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;
using ApplyFeature = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.ApplyPostingJournalFeature;
using ApplyRequest = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting.ApplyPostingJournalRequest;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Commands.Accounting.ApplyPostingJournal.ApplyPostingJournalFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IPostingWarningModelToNotificationConverter>? _converterMock;
    private Mock<INotificationPublisher>? _notificationPublisherMock;
    private List<string>? _calls;
    private List<INotification>? _convertedNotifications;
    private List<INotification>? _publishedNotifications;
    private ApplyPostingJournalModel? _savedJournal;
    private Fixture? _fixture;

    #endregion

    #region Setup

    [SetUp]
    public void SetUp()
    {
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _converterMock = new Mock<IPostingWarningModelToNotificationConverter>();
        _notificationPublisherMock = new Mock<INotificationPublisher>();
        _calls = [];
        _convertedNotifications = [];
        _publishedNotifications = [];
        _savedJournal = null;
        _fixture = new Fixture();
    }

    #endregion

    #region Tests

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_GetsPostingJournalWithRequestAccountingNumber()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _accountingGatewayMock!.Verify(
            mock => mock.GetPostingJournalAsync(postingJournal.AccountingNumber, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AppliesRetrievedPostingJournalWithRequestAccountingNumber()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _accountingGatewayMock!.Verify(
            mock => mock.ApplyPostingJournalAsync(
                postingJournal.AccountingNumber,
                postingJournal,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ForwardsCancellationTokenToApplyPostingJournal()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []));
        CancellationToken cancellationToken = new CancellationTokenSource().Token;

        // Act
        await sut.ExecuteAsync(request, cancellationToken);

        // Assert
        _accountingGatewayMock!.Verify(
            mock => mock.ApplyPostingJournalAsync(
                It.IsAny<int>(),
                It.IsAny<ApplyPostingJournalModel>(),
                It.Is<CancellationToken>(token => token == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLinesWerePosted_SavesOnlyLinesNotPosted()
    {
        // Arrange
        Guid postedIdentifier = _fixture!.Create<Guid>();
        ApplyPostingLineModel postedLine = CreatePostingLine(postedIdentifier);
        ApplyPostingLineModel remainingLine = CreatePostingLine(_fixture.Create<Guid>());
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(postedLine, remainingLine);
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([CreatePostedLine(postedIdentifier)], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(_savedJournal, Is.Not.Null);
        Assert.That(_savedJournal!.ApplyPostingLines, Is.EquivalentTo(new[] { remainingLine }));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLineHasNoIdentifier_KeepsLineInSavedJournal()
    {
        // Arrange
        ApplyPostingLineModel lineWithoutIdentifier = CreatePostingLine(null);
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(lineWithoutIdentifier);
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(_savedJournal!.ApplyPostingLines, Is.EquivalentTo(new[] { lineWithoutIdentifier }));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenPostingLinesWerePosted_DoesNotMutateRetrievedPostingJournal()
    {
        // Arrange
        Guid postedIdentifier = _fixture!.Create<Guid>();
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(postedIdentifier), CreatePostingLine(_fixture.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([CreatePostedLine(postedIdentifier)], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(postingJournal.ApplyPostingLines, Has.Count.EqualTo(2));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_SavesPostingJournalAfterApplyingPostingJournal()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(_calls, Is.EqualTo(new[] { "apply", "save" }));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenWarningsExist_PublishesNotificationsAfterSavingPostingJournal()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        PostingLineModel postedLine = CreatePostedLine(_fixture.Create<Guid>());
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([postedLine], [CreatePostingWarning(postedLine)]));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(_calls, Is.EqualTo(new[] { "apply", "save", "publish" }));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenWarningsExist_ConvertsEachWarningWithRequestFormatProvider()
    {
        // Arrange
        IFormatProvider formatProvider = CultureInfo.GetCultureInfo("da-DK");
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber, formatProvider);
        PostingLineModel postedLine = CreatePostedLine(_fixture.Create<Guid>());
        PostingWarningModel postingWarning = CreatePostingWarning(postedLine);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([postedLine], [postingWarning]));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        _converterMock!.Verify(
            mock => mock.ConvertAsync(
                postingWarning,
                It.Is<IFormatProvider>(provider => ReferenceEquals(provider, formatProvider)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenWarningsExist_PublishesNotificationsInWarningOrder()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        PostingLineModel postedLine = CreatePostedLine(_fixture.Create<Guid>());
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([postedLine], [CreatePostingWarning(postedLine), CreatePostingWarning(postedLine)]));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(_publishedNotifications, Is.EqualTo(_convertedNotifications));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenNoWarningsExist_DoesNotPublishNotifications()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []));

        // Act
        await sut.ExecuteAsync(request, CancellationToken.None);

        // Assert
        Assert.That(_publishedNotifications, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenApplyPostingJournalFails_ThrowsException()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []), applyException: new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.ExecuteAsync(request, CancellationToken.None));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenApplyPostingJournalFails_DoesNotSavePostingJournal()
    {
        // Arrange
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture!.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([], []), applyException: new InvalidOperationException());

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.ExecuteAsync(request, CancellationToken.None));

        // Assert
        Assert.That(_calls, Does.Not.Contain("save"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenSavePostingJournalFails_DoesNotPublishNotifications()
    {
        // Arrange
        PostingLineModel postedLine = CreatePostedLine(_fixture!.Create<Guid>());
        ApplyPostingJournalModel postingJournal = CreatePostingJournal(CreatePostingLine(_fixture.Create<Guid>()));
        ApplyRequest request = CreateRequest(postingJournal.AccountingNumber);
        ApplyFeature sut = CreateSut(postingJournal, CreateResult([postedLine], [CreatePostingWarning(postedLine)]), saveException: new InvalidOperationException());

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.ExecuteAsync(request, CancellationToken.None));

        // Assert
        Assert.That(_publishedNotifications, Is.Empty);
    }

    #endregion

    #region CreateSut

    private ApplyFeature CreateSut(
        ApplyPostingJournalModel postingJournal,
        ApplyPostingJournalResultModel result,
        Exception? applyException = null,
        Exception? saveException = null)
    {
        _accountingGatewayMock!
            .Setup(mock => mock.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(postingJournal);

        _accountingGatewayMock
            .Setup(mock => mock.ApplyPostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback(() => _calls!.Add("apply"))
            .Returns(() => applyException is not null
                ? Task.FromException<ApplyPostingJournalResultModel>(applyException)
                : Task.FromResult(result));

        _accountingGatewayMock
            .Setup(mock => mock.SavePostingJournalAsync(It.IsAny<int>(), It.IsAny<ApplyPostingJournalModel>(), It.IsAny<CancellationToken>()))
            .Callback((int _, ApplyPostingJournalModel journal, CancellationToken _) =>
            {
                _savedJournal = journal;
                _calls!.Add("save");
            })
            .Returns(() => saveException is not null
                ? Task.FromException<ApplyPostingJournalModel>(saveException)
                : Task.FromResult(postingJournal));

        foreach (PostingWarningModel postingWarning in result.PostingWarnings)
        {
            INotification notification = new Mock<INotification>().Object;
            _convertedNotifications!.Add(notification);

            _converterMock!
                .Setup(mock => mock.ConvertAsync(
                    It.Is<PostingWarningModel>(warning => ReferenceEquals(warning, postingWarning)),
                    It.IsAny<IFormatProvider>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(notification);
        }

        _notificationPublisherMock!
            .Setup(mock => mock.PublishAsync(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
            .Callback((INotification notification, CancellationToken _) =>
            {
                _publishedNotifications!.Add(notification);
                _calls!.Add("publish");
            })
            .Returns(Task.CompletedTask);

        return new ApplyFeature(
            new Mock<IPermissionChecker>().Object,
            _accountingGatewayMock.Object,
            _converterMock!.Object,
            _notificationPublisherMock.Object);
    }

    #endregion

    #region Helpers

    private ApplyRequest CreateRequest(int accountingNumber, IFormatProvider? formatProvider = null)
    {
        return new ApplyRequest(
            _fixture!.Create<Guid>(),
            accountingNumber,
            formatProvider ?? CultureInfo.InvariantCulture,
            new Mock<ISecurityContext>().Object);
    }

    private ApplyPostingJournalModel CreatePostingJournal(params ApplyPostingLineModel[] lines)
    {
        return new ApplyPostingJournalModel(_fixture!.Create<int>(), lines.ToList());
    }

    private ApplyPostingLineModel CreatePostingLine(Guid? identifier)
    {
        return new ApplyPostingLineModel(
            _fixture!.Create<string>(),
            null,
            null,
            null,
            null,
            _fixture.Create<string>(),
            identifier,
            _fixture.Create<DateTimeOffset>(),
            null,
            _fixture.Create<int>());
    }

    private PostingLineModel CreatePostedLine(Guid identifier)
    {
        return new PostingLineModel(
            _fixture!.Create<AccountIdentificationModel>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            _fixture.Create<string>(),
            identifier,
            _fixture.Create<DateTimeOffset>(),
            null,
            _fixture.Create<int>());
    }

    private PostingWarningModel CreatePostingWarning(PostingLineModel postingLine)
    {
        return new PostingWarningModel(
            _fixture!.Create<AccountIdentificationModel>(),
            _fixture.Create<double>(),
            postingLine,
            _fixture.Create<PostingWarningReason>());
    }

    private static ApplyPostingJournalResultModel CreateResult(ICollection<PostingLineModel> postingLines, ICollection<PostingWarningModel> postingWarnings)
    {
        return new ApplyPostingJournalResultModel(postingLines, postingWarnings);
    }

    #endregion
}
