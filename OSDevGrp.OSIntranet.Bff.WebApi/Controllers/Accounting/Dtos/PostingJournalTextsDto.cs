using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingJournalTextsDto
{
    [Required]
    [MinLength(ValidationValues.PostingJournalHeaderMinLength)]
    public required string PostingJournalHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingDateHeaderMinLength)]
    public required string PostingDateHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingReferenceHeaderMinLength)]
    public required string PostingReferenceHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountHeaderMinLength)]
    public required string AccountHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingTextHeaderMinLength)]
    public required string PostingTextHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.BudgetAccountHeaderMinLength)]
    public required string BudgetAccountHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.DebitHeaderMinLength)]
    public required string DebitHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.CreditHeaderMinLength)]
    public required string CreditHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingValueHeaderMinLength)]
    public required string PostingValueHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.ContactAccountHeaderMinLength)]
    public required string ContactAccountHeader { get; init; }

    [Required]
    [Range(AccountingRuleSetSpecifications.AccountingNumberMinValue, AccountingRuleSetSpecifications.AccountingNumberMaxValue)]
    public required int AccountingNumber { get; init; }

    [Required]
    public required IReadOnlyCollection<PostingJournalLineDisplayerDto> PostingJournalLines { get; init; }

    [Required]
    public required bool Modifiable { get; init; }

    internal static PostingJournalTextsDto Map(IPostingJournalTexts postingJournalTexts)
    {
        return new PostingJournalTextsDto
        {
            PostingJournalHeader = postingJournalTexts.PostingJournalHeader,
            PostingDateHeader = postingJournalTexts.PostingDateHeader,
            PostingReferenceHeader = postingJournalTexts.PostingReferenceHeader,
            AccountHeader = postingJournalTexts.AccountHeader,
            PostingTextHeader = postingJournalTexts.PostingTextHeader,
            BudgetAccountHeader = postingJournalTexts.BudgetAccountHeader,
            DebitHeader = postingJournalTexts.DebitHeader,
            CreditHeader = postingJournalTexts.CreditHeader,
            PostingValueHeader = postingJournalTexts.PostingValueHeader,
            ContactAccountHeader = postingJournalTexts.ContactAccountHeader,
            AccountingNumber = postingJournalTexts.AccountingNumber,
            PostingJournalLines = postingJournalTexts.PostingJournalLines.Select(PostingJournalLineDisplayerDto.Map).ToArray(),
            Modifiable = postingJournalTexts.Modifiable
        };
    }
}