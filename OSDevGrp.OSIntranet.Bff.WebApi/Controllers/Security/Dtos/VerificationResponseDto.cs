using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;

public class VerificationResponseDto
{
    [Required]
    public required bool Verified { get; init; }

    internal static VerificationResponseDto Map(VerificationResponse verificationResponse)
    {
        return new VerificationResponseDto
        {
            Verified = verificationResponse.Verified
        };
    }
}