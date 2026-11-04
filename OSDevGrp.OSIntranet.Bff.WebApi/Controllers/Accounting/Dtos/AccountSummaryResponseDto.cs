using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountSummaryResponseDto : AccountInfoDto
{
    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required AccountValuesDisplayerDto ValuesAtStatusDate { get; init; }

    [Required]
    public required AccountValuesDisplayerDto ValuesAtEndOfLastMonthFromStatusDate { get; init; }

    [Required]
    public required AccountValuesDisplayerDto ValuesAtEndOfLastYearFromStatusDate { get; init; }

    internal static AccountSummaryResponseDto Map(AccountSummaryResponse accountSummaryResponse)
    {
        return new AccountSummaryResponseDto
        {
            Accounting = AccountingIdentificationDto.Map(accountSummaryResponse.Model.Accounting),
            AccountNumber = accountSummaryResponse.Model.AccountNumber,
            AccountName = accountSummaryResponse.Model.AccountName,
            StatusDate = ValueDisplayerDto.Map(accountSummaryResponse.DynamicTexts.StatusDate),
            ValuesAtStatusDate = AccountValuesDisplayerDto.Map(accountSummaryResponse.DynamicTexts.ValuesAtStatusDate),
            ValuesAtEndOfLastMonthFromStatusDate = AccountValuesDisplayerDto.Map(accountSummaryResponse.DynamicTexts.ValuesAtEndOfLastMonthFromStatusDate),
            ValuesAtEndOfLastYearFromStatusDate = AccountValuesDisplayerDto.Map(accountSummaryResponse.DynamicTexts.ValuesAtEndOfLastYearFromStatusDate)
        };
    }
}