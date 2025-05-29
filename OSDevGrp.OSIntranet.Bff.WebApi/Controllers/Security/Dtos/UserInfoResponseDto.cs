using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class UserInfoResponseDto
{
    [Required]
    public required UserInfoDto UserInfo { get; init; }

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; } = Array.Empty<StaticTextDto>();

    internal static UserInfoResponseDto Map(UserInfoResponse userInfoResponse)
    {
        return new UserInfoResponseDto
        {
            UserInfo = UserInfoDto.Map(userInfoResponse.UserInfo),
            StaticTexts = userInfoResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}