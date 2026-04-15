using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountInfoDto : AccountIdentificationDto
{
    [Required]
    [MinLength(ValidationValues.AccountNameMinLength)]
    [MaxLength(ValidationValues.AccountNameMaxLength)]
    public required string AccountName { get; init; }
}