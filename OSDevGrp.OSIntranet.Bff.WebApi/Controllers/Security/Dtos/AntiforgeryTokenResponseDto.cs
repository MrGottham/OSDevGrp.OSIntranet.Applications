using Microsoft.AspNetCore.Antiforgery;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class AntiforgeryTokenResponseDto
{
	[Required]
	[MinLength(ValidationValues.FormFieldNameMinLength)]
	public required string FormFieldName { get; init; }

	[Required]
	[MinLength(ValidationValues.HeaderNameMinLength)]
	public required string HeaderName { get; init; }

	[Required]
	[MinLength(ValidationValues.RequestTokenMinLength)]
	public required string RequestToken { get; init; }

	internal static AntiforgeryTokenResponseDto Map(AntiforgeryTokenSet antiforgeryTokenSet)
	{
		return new AntiforgeryTokenResponseDto
		{
            FormFieldName = antiforgeryTokenSet.FormFieldName,
            HeaderName = antiforgeryTokenSet.HeaderName!,
            RequestToken = antiforgeryTokenSet.RequestToken!
		};
	}
}