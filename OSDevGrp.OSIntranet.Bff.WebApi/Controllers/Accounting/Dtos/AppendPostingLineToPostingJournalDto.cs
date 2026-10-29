using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AppendPostingLineToPostingJournalDto : PostingJournalLineModifierDtoBase
{
    [Required]
    public required Guid Identifier { get; init; }
}