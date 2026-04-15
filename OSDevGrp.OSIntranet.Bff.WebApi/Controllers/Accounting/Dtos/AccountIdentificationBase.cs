using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public abstract class AccountIdentificationBase
{
    [Required]
    public required AccountingIdentificationDto Accounting { get; init; }

    [Required]
    [MinLength(ValidationValues.AccountNumberMinLength)]
    [MaxLength(ValidationValues.AccountNumberMaxLength)]
    [RegularExpression(ValidationValues.AccountNumberRegexPattern)]
    public required string AccountNumber { get; init; }
}