using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public abstract class AccountIdentificationBase
{
    [Required]
    public required AccountingIdentificationDto Accounting { get; init; }

    [Required]
    [MinLength(AccountingRuleSetSpecifications.AccountNumberMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountNumberMaxLength)]
    [RegularExpression(AccountingRuleSetSpecifications.AccountNumberRegexPattern)]
    public required string AccountNumber { get; init; }
}