using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class AccountingDto
{
    [Required]
    [Range(1, 99)]
    public required int Number { get; init; }

    [Required]
    [MinLength(1)]
    public required string Name { get; init; }

    internal static AccountingDto Map(KeyValuePair<int, string> accounting)
    {
        return new AccountingDto
        {
            Number = accounting.Key,
            Name = accounting.Value
        };
    }
}