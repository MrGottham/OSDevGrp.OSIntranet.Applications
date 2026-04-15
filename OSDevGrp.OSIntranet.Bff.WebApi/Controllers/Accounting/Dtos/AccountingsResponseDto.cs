using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingsResponseDto
{
    [Required]
    public required bool CreationAllowed { get; init; }

    [Required]
    public required IReadOnlyCollection<AccountingDto> Accountings { get; init; } = [];

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = [];

    internal static AccountingsResponseDto Map(AccountingsResponse accountingsResponse)
    {
        return new AccountingsResponseDto
        {
            CreationAllowed = accountingsResponse.CreationAllowed,
            Accountings = accountingsResponse.Accountings.Select(AccountingDto.Map).ToArray(),
            StaticTexts = accountingsResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}