using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfBudgetAccountsSectionDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ChartOfBudgetAccountsSectionIdentificationMinLength)]
    public required string Identification { get; init; }

    [Required]
    [MinLength(ValidationValues.ChartOfBudgetAccountsSectionDescriptionMinLength)]
    public required string Description { get; init; }

    [Required]
    public required IReadOnlyCollection<ChartOfBudgetAccountsLineDisplayerDto> Lines { get; init; }

    internal static ChartOfBudgetAccountsSectionDisplayerDto Map(IChartOfBudgetAccountsSectionDisplayer chartOfBudgetAccountsSectionDisplayer)
    {
        return new ChartOfBudgetAccountsSectionDisplayerDto
        {
            Identification = chartOfBudgetAccountsSectionDisplayer.Identification,
            Description = chartOfBudgetAccountsSectionDisplayer.Description,
            Lines = chartOfBudgetAccountsSectionDisplayer.Lines.Select(ChartOfBudgetAccountsLineDisplayerDto.Map).ToArray()
        };
    }
}