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
            Number = accountingResponse.Accounting.Number,
            Name = accountingResponse.Accounting.Name,
            LetterHead = LetterHeadInfoDto.Map(accountingResponse.Accounting.LetterHead),
            BalanceBelowZero = Enum.Parse<BalanceBelowZeroType>(accountingResponse.Accounting.BalanceBelowZero.ToString()),
            BackDating = accountingResponse.Accounting.BackDating,
            StatusDate = accountingResponse.Accounting.StatusDate,
            Modifiable = accountingResponse.Accounting.Modifiable,
            Deletable = accountingResponse.Accounting.Deletable,
            LetterHeads = accountingResponse.LetterHeads.Select(LetterHeadInfoDto.Map).ToArray(),
            DynamicTexts = AccountingTextsDto.Map(accountingResponse.DynamicTexts),
            StaticTexts = accountingResponse.StaticTexts.Select(StaticTextDto.Map).ToArray(),
            ValidationRuleSet = ValidationRuleSetDto.Map(accountingResponse.ValidationRuleSet)
        };
    }
}