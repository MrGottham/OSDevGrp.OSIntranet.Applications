using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class StaticTextDto
{
    [Required]
    [MinLength(1)]
    public required string Key { get; init; }

    [Required]
    [MinLength(1)]
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