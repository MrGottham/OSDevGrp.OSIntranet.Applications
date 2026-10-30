using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Notification.NotificationScopes;

[TestFixture]
public class AddDomainServicesTests
{
    #region Methods

    [Test]
    [Category("IntegrationTest")]
    public async Task AddDomainServices_WhenResolvedInDifferentScopes_IsolatesNotificationsBetweenScopes()
    {
        using ServiceProvider serviceProvider = new ServiceCollection()
            .AddDomainServices()
            .BuildServiceProvider();
        using IServiceScope firstScope = serviceProvider.CreateScope();
        using IServiceScope secondScope = serviceProvider.CreateScope();
        INotificationPublisher firstPublisher = firstScope.ServiceProvider.GetRequiredService<INotificationPublisher>();
        INotificationRetriever firstRetriever = firstScope.ServiceProvider.GetRequiredService<INotificationRetriever>();
        INotificationRetriever secondRetriever = secondScope.ServiceProvider.GetRequiredService<INotificationRetriever>();
        INotification notification = new MarkdownNotification("Header", "Content");

        await firstPublisher.PublishAsync(notification);

        Assert.That(await firstRetriever.RetrieveAsync(), Is.EqualTo(new[] { notification }));
        Assert.That(await secondRetriever.RetrieveAsync(), Is.Empty);
    }

    #endregion
}