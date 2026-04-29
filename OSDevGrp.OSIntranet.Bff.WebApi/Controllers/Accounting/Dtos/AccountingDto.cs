using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingDto : AccountingInfoDto
{
    [Required]
    public required LetterHeadInfoDto LetterHead { get; init; }

    [Required]
    public required BalanceBelowZeroType BalanceBelowZero { get; init; }

    [Required]
    [Range(AccountingRuleSetSpecifications.BackDatingMinValue, AccountingRuleSetSpecifications.BackDatingMaxValue)]
    public required int BackDating { get; init; }

    [Required]
    public required DateTimeOffset StatusDate { get; init; }

    [Required]
    public required bool Modifiable { get; init; }

    [Required]
    public required bool Deletable { get; init; }

    internal static AccountingDto Map(AccountingModel accountingModel)
    {
        return new AccountingDto
        {
            Number = accountingModel.Number,
            Name = accountingModel.Name,
            LetterHead = LetterHeadInfoDto.Map(accountingModel.LetterHead),
            BalanceBelowZero = Enum.Parse<BalanceBelowZeroType>(accountingModel.BalanceBelowZero.ToString()),
            BackDating = accountingModel.BackDating,
            StatusDate = accountingModel.StatusDate,
            Modifiable = accountingModel.Modifiable,
            Deletable = accountingModel.Deletable
        };
    }
}