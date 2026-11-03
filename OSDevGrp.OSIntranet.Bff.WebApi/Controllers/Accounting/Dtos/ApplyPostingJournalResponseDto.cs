using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Notifications;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ApplyPostingJournalResponseDto : AccountingIdentificationDto
{
    [Required]
    public required IReadOnlyCollection<NotificationDto> Notifications { get; init; }

    internal static ApplyPostingJournalResponseDto Map(int accountingNumber, NotificationsResponse notificationsResponse)
    {
        return new ApplyPostingJournalResponseDto
        {
            Number = accountingNumber,
            Notifications = notificationsResponse.Notifications.Select(notification => NotificationDto.Map(notification)).ToList()
        };
    }
}