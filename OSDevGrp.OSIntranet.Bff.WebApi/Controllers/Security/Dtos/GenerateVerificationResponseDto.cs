using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class GenerateVerificationResponseDto
{
    [Required]
    [RegularExpression(ValidationValues.VerificationKeyRegexPattern)]
    public required string VerificationKey { get; init; }

    [Required]
    [MinLength(ValidationValues.VerificationImageMinLength)]
    [RegularExpression(ValidationValues.VerificationImageRegexPattern)]
    public required string VerificationImage { get; init; }

    [Required]
    public required DateTimeOffset Expires { get; init; }
}