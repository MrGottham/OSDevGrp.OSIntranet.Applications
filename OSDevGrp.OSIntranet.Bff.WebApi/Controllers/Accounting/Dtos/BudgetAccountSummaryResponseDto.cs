using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.BudgetAccountSummary;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class BudgetAccountSummaryResponseDto : BudgetAccountInfoDto
{
    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required BudgetAccountValuesDisplayerDto ValuesForMonthOfStatusDate { get; init; }

    [Required]
    public required BudgetAccountValuesDisplayerDto ValuesForLastMonthOfStatusDate { get; init; }

    [Required]
    public required BudgetAccountValuesDisplayerDto ValuesForYearToDateOfStatusDate { get; init; }

    [Required]
    public required BudgetAccountValuesDisplayerDto ValuesForLastYearOfStatusDate { get; init; }

    internal static BudgetAccountSummaryResponseDto Map(BudgetAccountSummaryResponse budgetAccountSummaryResponse)
    {
        return new BudgetAccountSummaryResponseDto
        {
            Accounting = AccountingIdentificationDto.Map(budgetAccountSummaryResponse.Model.Accounting),
            AccountNumber = budgetAccountSummaryResponse.Model.AccountNumber,
            AccountName = budgetAccountSummaryResponse.Model.AccountName,
            StatusDate = ValueDisplayerDto.Map(budgetAccountSummaryResponse.DynamicTexts.StatusDate),
            ValuesForMonthOfStatusDate = BudgetAccountValuesDisplayerDto.Map(budgetAccountSummaryResponse.DynamicTexts.ValuesForMonthOfStatusDate),
            ValuesForLastMonthOfStatusDate = BudgetAccountValuesDisplayerDto.Map(budgetAccountSummaryResponse.DynamicTexts.ValuesForLastMonthOfStatusDate),
            ValuesForYearToDateOfStatusDate = BudgetAccountValuesDisplayerDto.Map(budgetAccountSummaryResponse.DynamicTexts.ValuesForYearToDateOfStatusDate),
            ValuesForLastYearOfStatusDate = BudgetAccountValuesDisplayerDto.Map(budgetAccountSummaryResponse.DynamicTexts.ValuesForLastYearOfStatusDate)
        };
    }
}