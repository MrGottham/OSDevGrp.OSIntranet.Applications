using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class AccessDeniedContentResponseDto
{
    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = Array.Empty<StaticTextDto>();

    internal static AccessDeniedContentResponseDto Map(AccessDeniedContentResponse accessDeniedContentResponse)
    {
        return new AccessDeniedContentResponseDto
        {
            StaticTexts = accessDeniedContentResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}