using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class NotificationDto
{
    [Required]
    public required Guid Identifier { get; init; }

    [Required]
    [MinLength(ValidationValues.NotificationFormatMinLength)]
    [RegularExpression(ValidationValues.NotificationFormatRegexPattern)]
    public required string Format { get; init; }

    [Required]
    [MinLength(ValidationValues.NotificationTypeMinLength)]
    [RegularExpression(ValidationValues.NotificationTypeRegexPattern)]
    public required string Type { get; init; }

    [Required]
    [MinLength(ValidationValues.NotificationHeaderMinLength)]
    public required string Header { get; init; }

    [Required]
    [MinLength(ValidationValues.NotificationContentMinLength)]
    public required string Content { get; init; }

    internal static NotificationDto Map(INotification notification)
    {
        return new NotificationDto
        {
            Identifier = notification.Identifier,
            Format = notification.Format.ToString(),
            Type = notification.Type.ToString(),
            Header = notification.Header,
            Content = notification.Content
        };
    }
}