using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class AccountingInfoDto : AccountingIdentificationDto
{
    [Required]
    [MinLength(AccountingRuleSetSpecifications.AccountingNameMinLength)]
    [MaxLength(AccountingRuleSetSpecifications.AccountingNameMaxLength)]
    public required string Name { get; init; }

    internal static AccountingInfoDto Map(KeyValuePair<int, string> accounting)
    {
        return new AccountingInfoDto
        {
            Number = accounting.Key,
            Name = accounting.Value
        };
    }
}