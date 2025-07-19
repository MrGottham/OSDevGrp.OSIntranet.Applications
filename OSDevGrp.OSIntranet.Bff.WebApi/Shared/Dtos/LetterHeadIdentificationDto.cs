using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class LetterHeadIdentificationDto
{
    [Required]
    [Range(LetterHeadRuleSetSpecifications.LetterHeadNumberMinValue, LetterHeadRuleSetSpecifications.LetterHeadNumberMaxValue)]
    public required int Number { get; init; }
}