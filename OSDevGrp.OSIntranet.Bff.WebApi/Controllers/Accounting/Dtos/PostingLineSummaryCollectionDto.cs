using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingLineSummaryCollectionDto
{
    [Required]
    [MinLength(ValidationValues.PostingLineSummaryHeaderMinLength)]
    public required string SummaryHeader { get; init; }

    [Required]
    [MinLength(ValidationValues.PostingValueHeaderMinLength)]
    public required string PostingValueHeader { get; init; }

    [Required]
    public required IReadOnlyCollection<PostingLineSummaryDisplayerDto> PostingLines { get; init; }

    internal static PostingLineSummaryCollectionDto Map(IPostingLineCollectionTexts postingLineCollectionTexts)
    {
        return new PostingLineSummaryCollectionDto
        {
            SummaryHeader = postingLineCollectionTexts.SummaryHeader,
            PostingValueHeader = postingLineCollectionTexts.PostingValueHeader,
            PostingLines = postingLineCollectionTexts.PostingLines.Select(PostingLineSummaryDisplayerDto.Map).ToArray()
        };
    }
}