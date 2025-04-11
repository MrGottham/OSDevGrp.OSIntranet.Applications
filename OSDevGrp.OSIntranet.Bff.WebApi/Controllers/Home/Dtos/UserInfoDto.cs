using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class UserInfoDto
{
    [MinLength(1)]
    public string? Name { get; init; }

    [Required]
    public required bool HasAccountingAccess { get; init; }

    [Range(1, 99)]
    public int? DefaultAccountingNumber { get; init; }

    [Required]
    public required IReadOnlyCollection<AccountingDto> Accountings { get; init; } = Array.Empty<AccountingDto>();

    internal static UserInfoDto Map(IUserInfoModel userInfoModel)
    {
        return new UserInfoDto
        {
            Name = userInfoModel.Name,
            HasAccountingAccess = userInfoModel.HasAccountingAccess,
            DefaultAccountingNumber = userInfoModel.DefaultAccountingNumber,
            Accountings = userInfoModel.Accountings.Select(AccountingDto.Map).ToArray()
        };
    }
}