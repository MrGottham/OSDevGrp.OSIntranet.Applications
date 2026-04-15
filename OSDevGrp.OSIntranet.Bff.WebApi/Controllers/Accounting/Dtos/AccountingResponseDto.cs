using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingResponseDto : AccountingDto
{
    [Required]
    public required IReadOnlyCollection<LetterHeadInfoDto> LetterHeads { get; init; } = [];

    [Required]
    public required AccountingTextsDto DynamicTexts { get; init; }

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = [];

    [Required]
    public required ValidationRuleSetDto ValidationRuleSet { get; init; }

    internal static AccountingResponseDto Map(AccountingResponse accountingResponse)
    {
        return new AccountingResponseDto
        {
            Number = accountingResponse.Model.Number,
            Name = accountingResponse.Model.Name,
            LetterHead = LetterHeadInfoDto.Map(accountingResponse.Model.LetterHead),
            BalanceBelowZero = Enum.Parse<BalanceBelowZeroType>(accountingResponse.Model.BalanceBelowZero.ToString()),
            BackDating = accountingResponse.Model.BackDating,
            StatusDate = accountingResponse.Model.StatusDate,
            Accounts = accountingResponse.Model.Accounts.Select(AccountDto.Map).ToArray(),
            BudgetAccounts = accountingResponse.Model.BudgetAccounts.Select(BudgetAccountDto.Map).ToArray(),
            ContactAccounts = accountingResponse.Model.ContactAccounts.Select(ContactAccountDto.Map).ToArray(),
            Modifiable = accountingResponse.Model.Modifiable,
            Deletable = accountingResponse.Model.Deletable,
            LetterHeads = accountingResponse.LetterHeads.Select(LetterHeadInfoDto.Map).ToArray(),
            DynamicTexts = AccountingTextsDto.Map(accountingResponse.DynamicTexts),
            StaticTexts = accountingResponse.StaticTexts.Select(StaticTextDto.Map).ToArray(),
            ValidationRuleSet = ValidationRuleSetDto.Map(accountingResponse.ValidationRuleSet)
        };
    }
}