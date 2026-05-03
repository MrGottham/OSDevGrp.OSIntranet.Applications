using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfBudgetAccountsDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ChartOfBudgetAccountsLabelMinLength)]
	public required string ChartOfBudgetAccountsLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNumberLabelMinLength)]
    public required string AccountNumberLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNameLabelMinLength)]
    public required string AccountNameLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BudgetLabelMinLength)]
    public required string BudgetLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.PostedLabelMinLength)]
    public required string PostedLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AvailableLabelMinLength)]
    public required string AvailableLabel { get; init; }

    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required bool BudgetAccountCreationPossible { get; init; }

    [Required]
    public required IReadOnlyCollection<ChartOfBudgetAccountsSectionDisplayerDto> Sections { get; init; }

    internal static ChartOfBudgetAccountsDisplayerDto Map(IChartOfBudgetAccountsDisplayer chartOfBudgetAccountsDisplayer)
    {
        return new ChartOfBudgetAccountsDisplayerDto
        {
            ChartOfBudgetAccountsLabel = chartOfBudgetAccountsDisplayer.ChartOfBudgetAccountsLabel,
            AccountNumberLabel = chartOfBudgetAccountsDisplayer.AccountNumberLabel,
            AccountNameLabel = chartOfBudgetAccountsDisplayer.AccountNameLabel,
            BudgetLabel = chartOfBudgetAccountsDisplayer.BudgetLabel,
            PostedLabel = chartOfBudgetAccountsDisplayer.PostedLabel,
            AvailableLabel = chartOfBudgetAccountsDisplayer.AvailableLabel,
            StatusDate = ValueDisplayerDto.Map(chartOfBudgetAccountsDisplayer.StatusDate),
            BudgetAccountCreationPossible = chartOfBudgetAccountsDisplayer.BudgetAccountCreationPossible,
            Sections = chartOfBudgetAccountsDisplayer.Sections.Select(ChartOfBudgetAccountsSectionDisplayerDto.Map).ToArray()
        };
    }
}