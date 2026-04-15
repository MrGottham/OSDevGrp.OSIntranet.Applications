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
    public required IReadOnlyCollection<AccountDto> Accounts { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<BudgetAccountDto> BudgetAccounts { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<ContactAccountDto> ContactAccounts { get; init; } = [];

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
            Accounts = accountingModel.Accounts.Select(AccountDto.Map).ToArray(),
            BudgetAccounts = accountingModel.BudgetAccounts.Select(BudgetAccountDto.Map).ToArray(),
            ContactAccounts = accountingModel.ContactAccounts.Select(ContactAccountDto.Map).ToArray(),
            Modifiable = accountingModel.Modifiable,
            Deletable = accountingModel.Deletable
        };
    }
}