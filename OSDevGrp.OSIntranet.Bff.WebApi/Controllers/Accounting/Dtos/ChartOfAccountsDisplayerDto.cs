using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ChartOfAccountsDisplayerDto
{
    [Required]
    [MinLength(ValidationValues.ChartOfAccountsLabelMinLength)]
    public required string ChartOfAccountsLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNumberLabelMinLength)]
    public required string AccountNumberLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNameLabelMinLength)]
    public required string AccountNameLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.CreditLabelMinLength)]
    public required string CreditLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceLabelMinLength)]
    public required string BalanceLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AvailableLabelMinLength)]
    public required string AvailableLabel { get; init; }

    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required bool AccountCreationPossible { get; init; }

    [Required]
    public required IReadOnlyCollection<ChartOfAccountsSectionDisplayerDto> Sections { get; init; }

    internal static ChartOfAccountsDisplayerDto Map(IChartOfAccountsDisplayer chartOfAccountsDisplayer)
    {
        return new ChartOfAccountsDisplayerDto
        {
            ChartOfAccountsLabel = chartOfAccountsDisplayer.ChartOfAccountsLabel,
            AccountNumberLabel = chartOfAccountsDisplayer.AccountNumberLabel,
            AccountNameLabel = chartOfAccountsDisplayer.AccountNameLabel,
            CreditLabel = chartOfAccountsDisplayer.CreditLabel,
            BalanceLabel = chartOfAccountsDisplayer.BalanceLabel,
            AvailableLabel = chartOfAccountsDisplayer.AvailableLabel,
            StatusDate = ValueDisplayerDto.Map(chartOfAccountsDisplayer.StatusDate),
            AccountCreationPossible = chartOfAccountsDisplayer.AccountCreationPossible,
            Sections = chartOfAccountsDisplayer.Sections.Select(ChartOfAccountsSectionDisplayerDto.Map).ToArray()
        };
    }
}