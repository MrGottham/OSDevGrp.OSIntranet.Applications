using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;

public class AccountingIdentificationDto
{
    [Required]
    [Range(1, 99)]
    public required int Number { get; init; }
}