using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class PostingLineSummaryDisplayerDto : PostingLineDisplayerBase
{
    [Required]
    [MinLength(ValidationValues.PostingLineSummaryMinLength)]
    public required string Summary { get; init; }

    internal static PostingLineSummaryDisplayerDto Map(IPostingLineDisplayer postingLineDisplayer)
    {
        return new PostingLineSummaryDisplayerDto
        {
            Identification = postingLineDisplayer.Identification,
            Summary = postingLineDisplayer.Summary,
            PostingValue = postingLineDisplayer.PostingValue
        };
    }
}