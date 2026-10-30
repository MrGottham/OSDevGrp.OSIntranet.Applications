using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Notification.NotificationPublisherTests;

[TestFixture]
public class PublishAsyncTests
{
    #region Methods

    [Test]
    [Category("UnitTest")]
    public async Task PublishAsync_WhenCalled_PublishesNotification()
    {
        NotificationCollection notificationCollection = new();
        INotificationPublisher sut = CreateSut(notificationCollection);
        INotification notification = new MarkdownNotification("Header", "Content");
        INotificationRetriever notificationRetriever = new NotificationRetriever(notificationCollection);
        using CancellationTokenSource cancellationTokenSource = new();

        await sut.PublishAsync(notification, cancellationTokenSource.Token);

        Assert.That(await notificationRetriever.RetrieveAsync(), Is.EqualTo(new[] { notification }));
    }

    [Test]
    [Category("UnitTest")]
    public void PublishAsync_WhenCancellationHasBeenRequested_ThrowsOperationCanceledException()
    {
        INotificationPublisher sut = CreateSut(new NotificationCollection());
        using CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        Assert.That(async () => await sut.PublishAsync(new MarkdownNotification("Header", "Content"), cancellationTokenSource.Token), Throws.TypeOf<OperationCanceledException>());
    }

    private INotificationPublisher CreateSut(NotificationCollection notificationCollection)
    {
        return new NotificationPublisher(notificationCollection);
    }

    #endregion
}