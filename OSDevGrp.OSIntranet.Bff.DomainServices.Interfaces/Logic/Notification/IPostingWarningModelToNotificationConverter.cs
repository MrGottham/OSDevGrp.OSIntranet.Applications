using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;

public interface IPostingWarningModelToNotificationConverter
{
    Task<INotification> ConvertAsync(PostingWarningModel postingWarningModel, IFormatProvider formatProvider, CancellationToken cancellationToken = default);
}