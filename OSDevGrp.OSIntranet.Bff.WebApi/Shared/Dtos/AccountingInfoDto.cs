using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class AccountingInfoDto : AccountingIdentificationDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(256)]
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