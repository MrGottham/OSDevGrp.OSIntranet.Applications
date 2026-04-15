using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingPreCreationResponseDto
{
    [Required]
    public required IReadOnlyCollection<LetterHeadInfoDto> LetterHeads { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = [];

    [Required]
    public required ValidationRuleSetDto ValidationRuleSet { get; init; }

    internal static AccountingPreCreationResponseDto Map(AccountingPreCreationResponse accountingPreCreationResponse)
    {
        return new AccountingPreCreationResponseDto
        {
            LetterHeads = accountingPreCreationResponse.LetterHeads.Select(LetterHeadInfoDto.Map).ToArray(),
            StaticTexts = accountingPreCreationResponse.StaticTexts.Select(StaticTextDto.Map).ToArray(),
            ValidationRuleSet = ValidationRuleSetDto.Map(accountingPreCreationResponse.ValidationRuleSet)
        };
    }
}