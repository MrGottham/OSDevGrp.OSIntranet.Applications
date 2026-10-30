using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Notifications;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Notifications.NotificationsFeatureTests;

[TestFixture]
public class ExecuteAsyncTests : PageFeatureTestBase
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<INotificationRetriever>? _notificationRetrieverMock;
    private Fixture? _fixture;

    #endregion

    #region Methods

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _notificationRetrieverMock = new Mock<INotificationRetriever>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsDrainedNotifications()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        NotificationsRequest request = new(Guid.NewGuid(), formatProvider, CreateSecurityContext(_fixture!));
        INotification[] notifications = [new MarkdownNotification("Header", "Content")];
        IQueryFeature<NotificationsRequest, NotificationsResponse> sut = CreateSut(notifications);

        NotificationsResponse result = await sut.ExecuteAsync(request, cancellationToken);

        Assert.That(result.Notifications, Is.EqualTo(notifications));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsStaticTexts()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        NotificationsRequest request = new(Guid.NewGuid(), formatProvider, CreateSecurityContext(_fixture!));
        IQueryFeature<NotificationsRequest, NotificationsResponse> sut = CreateSut([]);

        NotificationsResponse result = await sut.ExecuteAsync(request, cancellationToken);

        Assert.That(result.StaticTexts[StaticTextKey.Notifications], Is.EqualTo("Notifikationer"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_RequestsStaticTextWithExpectedArguments()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        NotificationsRequest request = new(Guid.NewGuid(), formatProvider, CreateSecurityContext(_fixture!));
        IQueryFeature<NotificationsRequest, NotificationsResponse> sut = CreateSut([]);

        await sut.ExecuteAsync(request, cancellationToken);

        _staticTextProviderMock!.Verify(mock => mock.GetStaticTextAsync(
                StaticTextKey.Notifications,
                It.Is<IEnumerable<object>>(arguments => !arguments.Any()),
                It.Is<IFormatProvider>(value => ReferenceEquals(value, formatProvider)),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_RetrievesNotificationsWithCancellationToken()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        NotificationsRequest request = new(Guid.NewGuid(), formatProvider, CreateSecurityContext(_fixture!));
        IQueryFeature<NotificationsRequest, NotificationsResponse> sut = CreateSut([]);

        await sut.ExecuteAsync(request, cancellationToken);

        _notificationRetrieverMock!.Verify(mock => mock.RetrieveAsync(cancellationToken), Times.Once);
    }

    private IQueryFeature<NotificationsRequest, NotificationsResponse> CreateSut(IReadOnlyCollection<INotification> notifications)
    {
        _staticTextProviderMock!.Setup(mock => mock.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("Notifikationer");
        _notificationRetrieverMock!.Setup(mock => mock.RetrieveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(notifications);

        return new NotificationsFeature(_staticTextProviderMock!.Object, _notificationRetrieverMock!.Object);
    }

    #endregion
}