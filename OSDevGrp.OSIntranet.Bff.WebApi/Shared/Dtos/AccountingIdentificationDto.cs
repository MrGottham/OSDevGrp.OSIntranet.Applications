using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class AccountingIdentificationDto
{
    [Required]
    [Range(AccountingRuleSetSpecifications.AccountingNumberMinValue, AccountingRuleSetSpecifications.AccountingNumberMaxValue)]
    public required int Number { get; init; }

    internal static AccountingIdentificationDto Map(AccountingIdentificationModel accountingIdentificationModel)
    {
        return new AccountingIdentificationDto
        {
            Number = accountingIdentificationModel.Number
        };
    }
}