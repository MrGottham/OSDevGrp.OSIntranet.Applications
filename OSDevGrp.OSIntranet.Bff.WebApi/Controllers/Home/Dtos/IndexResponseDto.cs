using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;

public class IndexResponseDto
{
    [Required]
    [MinLength(1)]
    public required string Title { get; init; }

    public UserInfoDto? UserInfo { get; init; }

    [Required]
    public required IReadOnlyCollection<StaticTextDto> StaticTexts { get; init; }= Array.Empty<StaticTextDto>();

    internal static IndexResponseDto Map(IndexResponse indexResponse)
    {
        string? title = indexResponse.StaticTexts[indexResponse.TitleSelector];
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException($"The static text key '{indexResponse.TitleSelector}' was  not found in the static texts.");
        }

        return new IndexResponseDto
        {
            Title = title,
            UserInfo = indexResponse.UserInfo != null ? UserInfoDto.Map(indexResponse.UserInfo) : null,
            StaticTexts = indexResponse.StaticTexts.Select(StaticTextDto.Map).ToArray()
        };
    }
}