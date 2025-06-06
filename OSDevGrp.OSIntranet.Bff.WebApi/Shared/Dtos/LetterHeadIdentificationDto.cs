using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class LetterHeadIdentificationDto
{
    [Required]
    [Range(ValidationValues.LetterHeadIdentificationMinValue, ValidationValues.LetterHeadIdentificationMaxValue)]
    public required int Number { get; init; }
}