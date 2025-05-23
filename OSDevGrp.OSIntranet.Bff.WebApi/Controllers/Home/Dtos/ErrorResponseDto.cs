using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class ErrorResponseDto
{
    [Required]
    [MinLength(1)]
    public required string ErrorMessage { get; init; }

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; }= Array.Empty<StaticTextDto>();

    internal static ErrorResponseDto Map(ErrorResponse errorResponse)
    {
        return new ErrorResponseDto
        {
            ErrorMessage = errorResponse.ErrorMessage,
            StaticTexts = errorResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}