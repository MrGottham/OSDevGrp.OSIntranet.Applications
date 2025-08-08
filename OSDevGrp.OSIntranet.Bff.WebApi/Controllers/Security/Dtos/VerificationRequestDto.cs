using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class VerificationRequestDto
{
    [Required]
    [RegularExpression(ValidationValues.VerificationKeyRegexPattern)]
    public required string VerificationKey { get; init; }

    [Required]
    [RegularExpression(ValidationValues.VerificationCodeRegexPattern)]
    public required string VerificationCode { get; init; }
}