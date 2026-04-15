using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.NotImplemented;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class NotImplementedResponseDto
{
    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = Array.Empty<StaticTextDto>();

    internal static NotImplementedResponseDto Map(NotImplementedResponse notImplementedResponse)
    {
        return new NotImplementedResponseDto
        {
            StaticTexts = notImplementedResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}