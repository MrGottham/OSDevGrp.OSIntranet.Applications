using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingTextsDto
{
    [Required]
    public required ValueDisplayerDto BalanceBelowZero { get; init; } = null!;

    [Required]
    public required ValueDisplayerDto BackDating { get; init; } = null!;

    internal static AccountingTextsDto Map(IAccountingTexts accountingTexts)
    {
        return new AccountingTextsDto
        {
            BalanceBelowZero = ValueDisplayerDto.Map(accountingTexts.BalanceBelowZero),
            BackDating = ValueDisplayerDto.Map(accountingTexts.BackDating)
        };
    }
}