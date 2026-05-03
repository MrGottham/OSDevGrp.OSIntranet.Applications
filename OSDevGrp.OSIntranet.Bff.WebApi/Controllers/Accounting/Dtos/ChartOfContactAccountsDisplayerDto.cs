using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfContactAccountsDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ChartOfContactAccountsLabelMinLength)]
	public required string ChartOfContactAccountsLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNumberLabelMinLength)]
    public required string AccountNumberLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNameLabelMinLength)]
    public required string AccountNameLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceLabelMinLength)]
    public required string BalanceLabel { get; init; }

    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required bool ContactAccountCreationPossible { get; init; }

    [Required]
    public required IReadOnlyCollection<ChartOfContactAccountsLineDisplayerDto> Lines { get; init; }

    internal static ChartOfContactAccountsDisplayerDto Map(IChartOfContactAccountsDisplayer chartOfContactAccountsDisplayer)
    {
        return new ChartOfContactAccountsDisplayerDto
        {
            ChartOfContactAccountsLabel = chartOfContactAccountsDisplayer.ChartOfContactAccountsLabel,
            AccountNumberLabel = chartOfContactAccountsDisplayer.AccountNumberLabel,
            AccountNameLabel = chartOfContactAccountsDisplayer.AccountNameLabel,
            BalanceLabel = chartOfContactAccountsDisplayer.BalanceLabel,
            StatusDate = ValueDisplayerDto.Map(chartOfContactAccountsDisplayer.StatusDate),
            ContactAccountCreationPossible = chartOfContactAccountsDisplayer.ContactAccountCreationPossible,
            Lines = chartOfContactAccountsDisplayer.Lines.Select(ChartOfContactAccountsLineDisplayerDto.Map).ToArray()
        };
    }
}