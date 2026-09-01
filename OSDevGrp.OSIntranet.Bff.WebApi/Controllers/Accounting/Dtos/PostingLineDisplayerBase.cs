using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public abstract class PostingLineDisplayerBase
{
    [Required]
    [MinLength(AccountingRuleSetSpecifications.PostingLineIdentificationMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.PostingLineIdentificationMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.PostingLineIdentificationRegexPattern)]
    public required string Identification { get; init; }

    [MinLength(ValidationValues.PostingValueMinLength)]
    public string? PostingValue { get; init; }
}