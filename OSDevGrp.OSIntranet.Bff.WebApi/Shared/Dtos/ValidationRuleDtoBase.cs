using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public abstract class ValidationRuleDtoBase
{
    [Required]
    [MinLength(ValidationValues.ValidationRuleNameMinLength)]
    public required string Name { get; init; }

    [Required]
    [MinLength(ValidationValues.ValidationErrorMinLength)]
    public required string ValidationError { get; init; }
}