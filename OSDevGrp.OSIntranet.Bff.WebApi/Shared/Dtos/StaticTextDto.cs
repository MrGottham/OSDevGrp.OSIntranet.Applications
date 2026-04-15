using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class StaticTextDto
{
    [Required]
    [MinLength(ValidationValues.StaticTextKeyMinLength)]
    public required string Key { get; init; }

    [Required]
    [MinLength(ValidationValues.StaticTextValueMinLength)]
    public required string Text { get; init; }

    internal static StaticTextDto Map(KeyValuePair<StaticTextKey, string> staticText)
    {
        return new StaticTextDto
        {
            Key = staticText.Key.ToString(),
            Text = staticText.Value
        };
    }
}