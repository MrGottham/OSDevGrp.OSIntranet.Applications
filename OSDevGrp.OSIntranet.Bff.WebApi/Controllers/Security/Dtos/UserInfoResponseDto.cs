using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class UserInfoResponseDto : UserInfoDto
{
    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = Array.Empty<StaticTextDto>();

    internal static UserInfoResponseDto Map(UserInfoResponse userInfoResponse)
    {
        return new UserInfoResponseDto
        {
            NameIdentifier = userInfoResponse.UserInfo.NameIdentifier,
            Name = userInfoResponse.UserInfo.Name,
            MailAddress = userInfoResponse.UserInfo.MailAddress,
            HasAccountingAccess = userInfoResponse.UserInfo.HasAccountingAccess,
            DefaultAccountingNumber = userInfoResponse.UserInfo.DefaultAccountingNumber,
            Accountings = userInfoResponse.UserInfo.Accountings.Select(AccountingInfoDto.Map).ToArray(),
            IsAccountingAdministrator = userInfoResponse.UserInfo.IsAccountingAdministrator,
            IsAccountingCreator = userInfoResponse.UserInfo.IsAccountingCreator,
            IsAccountingModifier = userInfoResponse.UserInfo.IsAccountingModifier,
            ModifiableAccountings = userInfoResponse.UserInfo.ModifiableAccountings.Select(AccountingInfoDto.Map).ToArray(),
            IsAccountingViewer = userInfoResponse.UserInfo.IsAccountingViewer,
            ViewableAccountings = userInfoResponse.UserInfo.ViewableAccountings.Select(AccountingInfoDto.Map).ToArray(),
            HasCommonDataAccess = userInfoResponse.UserInfo.HasCommonDataAccess,
            StaticTexts = userInfoResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}