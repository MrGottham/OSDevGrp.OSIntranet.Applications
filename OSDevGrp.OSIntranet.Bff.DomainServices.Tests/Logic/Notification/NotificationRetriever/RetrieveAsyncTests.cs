using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Notification.NotificationRetrieverTests;

[TestFixture]
public class RetrieveAsyncTests
{
    #region Methods

    [Test]
    [Category("UnitTest")]
    public async Task RetrieveAsync_WhenNotificationsHaveBeenPublished_ReturnsNotificationsInPublicationOrder()
    {
        NotificationCollection notificationCollection = new();
        INotificationRetriever sut = CreateSut(notificationCollection);
        INotification[] notifications =
        [
            new MarkdownNotification("Header 1", "Content 1"),
            new MarkdownNotification("Header 2", "Content 2"),
            new MarkdownNotification("Header 3", "Content 3")
        ];
        using CancellationTokenSource cancellationTokenSource = new();

        foreach (INotification notification in notifications)
        {
            notificationCollection.Publish(notification);
        }

        IReadOnlyCollection<INotification> result = await sut.RetrieveAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.EqualTo(notifications));
    }

    [Test]
    [Category("UnitTest")]
    public async Task RetrieveAsync_WhenCalled_DrainsCollection()
    {
        NotificationCollection notificationCollection = new();
        INotificationRetriever sut = CreateSut(notificationCollection);
        notificationCollection.Publish(new MarkdownNotification("Header", "Content"));
        using CancellationTokenSource cancellationTokenSource = new();

        await sut.RetrieveAsync(cancellationTokenSource.Token);

        Assert.That(await sut.RetrieveAsync(cancellationTokenSource.Token), Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task RetrieveAsync_WhenPublishingAndRetrievingConcurrently_DoesNotLoseOrDuplicateNotifications()
    {
        const int notificationCount = 100;
        NotificationCollection notificationCollection = new();
        INotificationRetriever sut = CreateSut(notificationCollection);
        INotificationPublisher notificationPublisher = new NotificationPublisher(notificationCollection);

        Task[] publishingTasks = Enumerable.Range(0, notificationCount)
            .Select(index => Task.Run(async () => await notificationPublisher.PublishAsync(new MarkdownNotification("Header", index.ToString()))))
            .ToArray();
        Task<IReadOnlyCollection<INotification>>[] retrievingTasks = Enumerable.Range(0, 2)
            .Select(_ => Task.Run(async () => await sut.RetrieveAsync()))
            .ToArray();

        await Task.WhenAll(publishingTasks);
        await Task.WhenAll(retrievingTasks);

        INotification[] retrievedNotifications = retrievingTasks.SelectMany(task => task.Result)
            .Concat(await sut.RetrieveAsync())
            .ToArray();

        Assert.That(retrievedNotifications, Has.Length.EqualTo(notificationCount));
        Assert.That(retrievedNotifications.Select(notification => notification.Content).Distinct().Count(), Is.EqualTo(notificationCount));
    }

    [Test]
    [Category("UnitTest")]
    public void RetrieveAsync_WhenCancellationHasBeenRequested_ThrowsOperationCanceledException()
    {
        INotificationRetriever sut = CreateSut(new NotificationCollection());
        using CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.Cancel();

        Assert.That(async () => await sut.RetrieveAsync(cancellationTokenSource.Token), Throws.TypeOf<OperationCanceledException>());
    }

    private INotificationRetriever CreateSut(NotificationCollection notificationCollection)
    {
        return new NotificationRetriever(notificationCollection);
    }

    #endregion
}