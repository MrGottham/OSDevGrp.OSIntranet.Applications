using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingJournalLineDisplayerDto
{
    [Required]
    public required Guid Identifier { get; init; }

    [Required]
    [MinLength(AccountingRuleSetSpecifications.PostingLineIdentificationMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.PostingLineIdentificationMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.PostingLineIdentificationRegexPattern)]
    public required string IdentifierAsText { get; init; }

    [Required]
    public required DateTimeOffset PostingDate { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingDateMinLength)]
    public required string PostingDateAsText { get; init; }

    [MinLength(AccountingRuleSetSpecifications.PostingReferenceMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.PostingReferenceMaxLength)]
    public string? PostingReference { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNumberMinLength)]
    [MaxLength(ValidationValues.AccountNumberMaxLength)]
    [RegularExpression(ValidationValues.AccountNumberRegexPattern)]
    public required string Account { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingTextMinLength)]
    [MaxLength(ValidationValues.PostingTextMaxLength)]
    public required string PostingText { get; init; }

    [MinLength(ValidationValues.AccountNumberMinLength)]
    [MaxLength(ValidationValues.AccountNumberMaxLength)]
    [RegularExpression(ValidationValues.AccountNumberRegexPattern)]
    public string? BudgetAccount { get; init; }

    [Range(ValidationValues.DebitMinValue, ValidationValues.DebitMaxValue)]
    public decimal? Debit { get; init; }

    [MinLength(ValidationValues.DebitMinLength)]
    public string? DebitAsText { get; init; }

    [Range(ValidationValues.CreditMinValue, ValidationValues.CreditMaxValue)]
    public decimal? Credit { get; init; }

    [MinLength(ValidationValues.CreditMinLength)]
    public string? CreditAsText { get; init; }

    [MinLength(ValidationValues.PostingValueMinLength)]
    public string? PostingValueAsText { get; init; }

    [MinLength(ValidationValues.AccountNumberMinLength)]
    [MaxLength(ValidationValues.AccountNumberMaxLength)]
    [RegularExpression(ValidationValues.AccountNumberRegexPattern)]
    public string? ContactAccount { get; init; }

    internal static PostingJournalLineDisplayerDto Map(IPostingJournalLineDisplayer postingJournalLineDisplayer)
    {
        return new PostingJournalLineDisplayerDto
        {
            Identifier = postingJournalLineDisplayer.PostingJournalLine.Identifier.GetValueOrDefault(),
            IdentifierAsText = postingJournalLineDisplayer.Identification,
            PostingDate = postingJournalLineDisplayer.PostingJournalLine.PostingDate,
            PostingDateAsText = postingJournalLineDisplayer.PostingDate,
            PostingReference = postingJournalLineDisplayer.PostingReference,
            Account = postingJournalLineDisplayer.PostingJournalLine.AccountNumber,
            PostingText = postingJournalLineDisplayer.PostingJournalLine.Details,
            BudgetAccount = postingJournalLineDisplayer.PostingJournalLine.BudgetAccountNumber,
            Debit = postingJournalLineDisplayer.PostingJournalLine.Debit.HasValue ? (decimal) postingJournalLineDisplayer.PostingJournalLine.Debit.Value : null,
            DebitAsText = postingJournalLineDisplayer.Debit,
            Credit = postingJournalLineDisplayer.PostingJournalLine.Credit.HasValue ? (decimal) postingJournalLineDisplayer.PostingJournalLine.Credit.Value : null,
            CreditAsText = postingJournalLineDisplayer.Credit,
            PostingValueAsText = postingJournalLineDisplayer.PostingValue,
            ContactAccount = postingJournalLineDisplayer.PostingJournalLine.ContactAccountNumber
        };
    }
}