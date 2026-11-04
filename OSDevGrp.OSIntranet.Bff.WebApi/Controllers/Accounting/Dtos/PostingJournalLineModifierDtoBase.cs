using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public abstract class PostingJournalLineModifierDtoBase
{
    [Required]
    public required DateTimeOffset PostingDate { get; init; }

    [MinLength(AccountingRuleSetSpecifications.PostingReferenceMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.PostingReferenceMaxLength)]
    public string? PostingReference { get; init; }

    [Required]
    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public required string Account { get; init; }

    [Required]
    [MinLength(AccountingRuleSetSpecifications.PostingTextMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.PostingTextMaxLength)]
    public required string PostingText { get; init; }

    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public string? BudgetAccount { get; init; }

    [Range(AccountingRuleSetSpecifications.DebitMinValue, AccountingRuleSetSpecifications.DebitMaxValue)]
    public decimal? Debit { get; init; }

    [Range(AccountingRuleSetSpecifications.CreditMinValue, AccountingRuleSetSpecifications.CreditMaxValue)]
    public decimal? Credit { get; init; }

    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public string? ContactAccount { get; init; }
}