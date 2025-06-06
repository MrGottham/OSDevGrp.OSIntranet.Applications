using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class LetterHeadInfoDto : LetterHeadIdentificationDto
{
    [Required]
    [MinLength(ValidationValues.LetterHeadNameMinLength)]
    [MaxLength(ValidationValues.LetterHeadNameMaxLength)]
    public required string Name { get; init; }

    internal static LetterHeadInfoDto Map(LetterHeadIdentificationModel letterHeadIdentificationModel)
    {
        return new LetterHeadInfoDto
        {
            Number = letterHeadIdentificationModel.Number,
            Name = letterHeadIdentificationModel.Name
        };
    }
}