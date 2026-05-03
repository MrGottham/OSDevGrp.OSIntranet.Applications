using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfAccountsSectionDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ChartOfAccountsSectionIdentificationMinLength)]
    public required string Identification { get; init; }

    [Required]
    [MinLength(ValidationValues.ChartOfAccountsSectionDescriptionMinLength)]
    public required string Description { get; init; }

    [Required]
    public required IReadOnlyCollection<ChartOfAccountsLineDisplayerDto> Lines { get; init; }

    internal static ChartOfAccountsSectionDisplayerDto Map(IChartOfAccountsSectionDisplayer chartOfAccountsSectionDisplayer)
    {
        return new ChartOfAccountsSectionDisplayerDto
        {
            Identification = chartOfAccountsSectionDisplayer.Identification,
            Description = chartOfAccountsSectionDisplayer.Description,
            Lines = chartOfAccountsSectionDisplayer.Lines.Select(ChartOfAccountsLineDisplayerDto.Map).ToArray()
        };
    }
}