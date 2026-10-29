using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingJournalResponseDto : AccountingIdentificationDto
{
    [Required]
    public required PostingJournalTextsDto DynamicTexts { get; init; }

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; }

    [Required]
    public required ValidationRuleSetDto ValidationRuleSet { get; init; }

    internal static PostingJournalResponseDto Map(PostingJournalResponse postingJournalResponse)
    {
        return new PostingJournalResponseDto
        {
            Number = postingJournalResponse.PostingJournal.AccountingNumber,
            DynamicTexts = PostingJournalTextsDto.Map(postingJournalResponse.DynamicTexts),
            StaticTexts = postingJournalResponse.StaticTexts.Select(staticText => StaticTextDto.Map(staticText)).ToList(),
            ValidationRuleSet = ValidationRuleSetDto.Map(postingJournalResponse.ValidationRuleSet)
        };
    }
}