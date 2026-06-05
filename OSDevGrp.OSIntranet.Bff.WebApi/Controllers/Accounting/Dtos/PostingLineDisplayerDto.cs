using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingLineDisplayerDto : PostingLineDisplayerBase
{
    [Required]
    [MinLength(ValidationValues.PostingDateMinLength)]
    public required string PostingDate { get; init; }

    [MinLength(AccountingRuleSetSpecifications.PostingReferenceMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.PostingReferenceMaxLength)]
    public string? PostingReference { get; init; }

    [Required]
    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public required string Account { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingTextMinLength)]
    [MaxLength(ValidationValues.PostingTextMaxLength)]
    public required string PostingText { get; init; }

    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public string? BudgetAccount { get; init; }

    [MinLength(ValidationValues.DebitMinLength)]
    public string? Debit { get; init; }

    [MinLength(ValidationValues.CreditMinLength)]
    public string? Credit { get; init; }

    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public string? ContactAccount { get; init; }

    internal static PostingLineDisplayerDto Map(IPostingLineDisplayer postingLineDisplayer)
    {
        return new PostingLineDisplayerDto
        {
            Identification = postingLineDisplayer.Identification,
            PostingDate = postingLineDisplayer.PostingDate,
            PostingReference = postingLineDisplayer.PostingReference,
            Account = postingLineDisplayer.Account,
            PostingText = postingLineDisplayer.PostingText,
            BudgetAccount = postingLineDisplayer.BudgetAccount,
            Debit = postingLineDisplayer.Debit,
            Credit = postingLineDisplayer.Credit,
            PostingValue = postingLineDisplayer.PostingValue,
            ContactAccount = postingLineDisplayer.ContactAccount
        };
    }
}