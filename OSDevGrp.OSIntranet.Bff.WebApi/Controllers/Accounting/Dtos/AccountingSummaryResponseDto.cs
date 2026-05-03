using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingSummaryResponseDto : AccountingInfoDto
{
    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required BalanceSheetDisplayerDto BalanceSheetAtStatusDate { get; init; }

    [Required]
    public required BudgetStatementDisplayerDto BudgetStatementForMonthOfStatusDate { get; init; }

    [Required]
    public required ObligeePartiesDisplayerDto ObligeePartiesAtStatusDate { get; init; }

    [Required]
    public required PostingLineSummaryCollectionDto PostingLineSummaryCollection { get; init; }

    internal static AccountingSummaryResponseDto Map(AccountingSummaryResponse accountingSummaryResponse)
    {
        return new AccountingSummaryResponseDto
        {
            Number = accountingSummaryResponse.Accounting.Number,
            Name = accountingSummaryResponse.Accounting.Name,
            StatusDate = ValueDisplayerDto.Map(accountingSummaryResponse.DynamicTexts.StatusDate),
            BalanceSheetAtStatusDate = BalanceSheetDisplayerDto.Map(accountingSummaryResponse.DynamicTexts.BalanceSheetAtStatusDate),
            BudgetStatementForMonthOfStatusDate = BudgetStatementDisplayerDto.Map(accountingSummaryResponse.DynamicTexts.BudgetStatementForMonthOfStatusDate),
            ObligeePartiesAtStatusDate = ObligeePartiesDisplayerDto.Map(accountingSummaryResponse.DynamicTexts.ObligeePartiesAtStatusDate),
            PostingLineSummaryCollection = PostingLineSummaryCollectionDto.Map(accountingSummaryResponse.DynamicTexts.PostingLineCollection)
        };
    }
}