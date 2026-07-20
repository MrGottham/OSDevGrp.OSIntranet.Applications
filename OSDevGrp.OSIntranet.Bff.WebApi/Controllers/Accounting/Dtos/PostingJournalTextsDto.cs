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
    [MinLength(ValidationValues.AccountNameLabelMinLength)]
    public required string AccountNameLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.CreditLabelMinLength)]
    public required string AccountCreditLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceLabelMinLength)]
    public required string AccountBalanceLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AvailableLabelMinLength)]
    public required string AccountAvailableLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingTextHeaderMinLength)]
    public required string PostingTextHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.BudgetAccountHeaderMinLength)]
    public required string BudgetAccountHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNameLabelMinLength)]
    public required string BudgetAccountNameLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BudgetLabelMinLength)]
    public required string BudgetAccountBudgetLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.PostedLabelMinLength)]
    public required string BudgetAccountPostedLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.AvailableLabelMinLength)]
    public required string BudgetAccountAvailableLabel { get; init; }

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
    [MinLength(ValidationValues.AccountNameLabelMinLength)]
    public required string ContactAccountNameLabel { get; init; }

    [Required]
    [MinLength(ValidationValues.BalanceLabelMinLength)]
    public required string ContactAccountBalanceLabel { get; init; }

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
            AccountNameLabel = postingJournalTexts.AccountNameLabel,
            AccountCreditLabel = postingJournalTexts.AccountCreditLabel,
            AccountBalanceLabel = postingJournalTexts.AccountBalanceLabel,
            AccountAvailableLabel = postingJournalTexts.AccountAvailableLabel,
            PostingTextHeader = postingJournalTexts.PostingTextHeader,
            BudgetAccountHeader = postingJournalTexts.BudgetAccountHeader,
            BudgetAccountNameLabel = postingJournalTexts.BudgetAccountNameLabel,
            BudgetAccountBudgetLabel = postingJournalTexts.BudgetAccountBudgetLabel,
            BudgetAccountPostedLabel = postingJournalTexts.BudgetAccountPostedLabel,
            BudgetAccountAvailableLabel = postingJournalTexts.BudgetAccountAvailableLabel,
            DebitHeader = postingJournalTexts.DebitHeader,
            CreditHeader = postingJournalTexts.CreditHeader,
            PostingValueHeader = postingJournalTexts.PostingValueHeader,
            ContactAccountHeader = postingJournalTexts.ContactAccountHeader,
            ContactAccountNameLabel = postingJournalTexts.ContactAccountNameLabel,
            ContactAccountBalanceLabel = postingJournalTexts.ContactAccountBalanceLabel,
            AccountingNumber = postingJournalTexts.AccountingNumber,
            PostingJournalLines = postingJournalTexts.PostingJournalLines.Select(PostingJournalLineDisplayerDto.Map).ToArray(),
            Modifiable = postingJournalTexts.Modifiable
        };
    }
}