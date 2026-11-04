using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class ContactAccountSummaryResponseDto : ContactAccountInfoDto
{
    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required ContactAccountValuesDisplayerDto ValuesAtStatusDate { get; init; }

    [Required]
    public required ContactAccountValuesDisplayerDto ValuesAtEndOfLastMonthFromStatusDate { get; init; }

    [Required]
    public required ContactAccountValuesDisplayerDto ValuesAtEndOfLastYearFromStatusDate { get; init; }

    internal static ContactAccountSummaryResponseDto Map(ContactAccountSummaryResponse contactAccountSummaryResponse)
    {
        return new ContactAccountSummaryResponseDto
        {
            Accounting = AccountingIdentificationDto.Map(contactAccountSummaryResponse.Model.Accounting),
            AccountNumber = contactAccountSummaryResponse.Model.AccountNumber,
            AccountName = contactAccountSummaryResponse.Model.AccountName,
            StatusDate = ValueDisplayerDto.Map(contactAccountSummaryResponse.DynamicTexts.StatusDate),
            ValuesAtStatusDate = ContactAccountValuesDisplayerDto.Map(contactAccountSummaryResponse.DynamicTexts.ValuesAtStatusDate),
            ValuesAtEndOfLastMonthFromStatusDate = ContactAccountValuesDisplayerDto.Map(contactAccountSummaryResponse.DynamicTexts.ValuesAtEndOfLastMonthFromStatusDate),
            ValuesAtEndOfLastYearFromStatusDate = ContactAccountValuesDisplayerDto.Map(contactAccountSummaryResponse.DynamicTexts.ValuesAtEndOfLastYearFromStatusDate)
        };
    }
}