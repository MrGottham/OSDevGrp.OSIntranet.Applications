using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingLineCollectionDto
{
    [Required]
    [MinLength(ValidationValues.LatestPostingsHeaderMinLength)]
    public required string LatestPostingsHeader { get; init; }

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
    public required IReadOnlyCollection<PostingLineDisplayerDto> PostingLines { get; init; }

    internal static PostingLineCollectionDto Map(IPostingLineCollectionTexts postingLineCollectionTexts)
    {
        return new PostingLineCollectionDto
        {
            LatestPostingsHeader = postingLineCollectionTexts.LatestPostingsHeader,
            PostingDateHeader = postingLineCollectionTexts.PostingDateHeader,
            PostingReferenceHeader = postingLineCollectionTexts.PostingReferenceHeader,
            AccountHeader = postingLineCollectionTexts.AccountHeader,
            PostingTextHeader = postingLineCollectionTexts.PostingTextHeader,
            BudgetAccountHeader = postingLineCollectionTexts.BudgetAccountHeader,
            DebitHeader = postingLineCollectionTexts.DebitHeader,
            CreditHeader = postingLineCollectionTexts.CreditHeader,
            PostingValueHeader = postingLineCollectionTexts.PostingValueHeader,
            ContactAccountHeader = postingLineCollectionTexts.ContactAccountHeader,
            PostingLines = postingLineCollectionTexts.PostingLines.Select(PostingLineDisplayerDto.Map).ToArray()
        };
    }
}