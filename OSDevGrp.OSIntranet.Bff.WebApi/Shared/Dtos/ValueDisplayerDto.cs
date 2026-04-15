using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class ValueDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ValueDisplayerLabelMinLength)]
    public required string Label { get; init; }

    [MinLength(ValidationValues.ValueDisplayerValueMinLength)]
    public string? Value { get; init; }

    internal static ValueDisplayerDto Map(IValueDisplayer valueDisplayer)
    {
        return new ValueDisplayerDto
        {
            Label = valueDisplayer.Label,
            Value = string.IsNullOrWhiteSpace(valueDisplayer.Value) == false ? valueDisplayer.Value : null
        };
    }
}