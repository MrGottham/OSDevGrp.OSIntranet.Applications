using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.CookieConsent;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class CookieConsentResponseDto
{
    [Required]
    [MinLength(1)]
    public required string CookieName { get; init; }

    [Required]
    [MinLength(1)]
    public required string CookieValue { get; init; }

    [Required]
    [Range(0, 365)]
    public required int DaysUntilExpiry { get; init; }

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; }= Array.Empty<StaticTextDto>();

    internal static CookieConsentResponseDto Map(CookieConsentResponse cookieConsentResponse)
    {
        return new CookieConsentResponseDto
        {
            CookieName = cookieConsentResponse.CookieName,
            CookieValue = cookieConsentResponse.CookieValue,
            DaysUntilExpiry = cookieConsentResponse.DaysUntilExpiry,
            StaticTexts = cookieConsentResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}