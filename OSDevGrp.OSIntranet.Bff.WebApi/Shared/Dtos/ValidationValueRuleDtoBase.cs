using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public abstract class ValidationValueRuleDtoBase : ValidationRuleDtoBase
{
    [Required]
    [MinLength(ValidationValues.ValidationValueMinLength)]
    public required string Value { get; init; }

    protected static string Map<TValue>(TValue value) where TValue : struct, IComparable<TValue> => value.ToString() ?? string.Empty;
}