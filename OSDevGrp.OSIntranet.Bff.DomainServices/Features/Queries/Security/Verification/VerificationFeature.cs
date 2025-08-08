using System.Security.Claims;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;

internal class VerificationFeature : IQueryFeature<VerificationRequest, VerificationResponse>, IPermissionVerifiable<VerificationRequest>
{
    #region Private variables

    private readonly IPermissionChecker _permissionChecker;
    private readonly IVerificationCodeVerifier _verificationCodeVerifier;

    #endregion

    #region Constructor

    public VerificationFeature(IPermissionChecker permissionChecker, IVerificationCodeVerifier verificationCodeVerifier)
    {
        _permissionChecker = permissionChecker;
        _verificationCodeVerifier = verificationCodeVerifier;
    }

    #endregion

    #region Methods

    public Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, VerificationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => VerifyPermission(securityContext.User), cancellationToken);
    }

    public async Task<VerificationResponse> ExecuteAsync(VerificationRequest request, CancellationToken cancellationToken = default)
    {
        bool verified = await _verificationCodeVerifier.VerifyAsync(request.VerificationKey, request.VerificationCode, cancellationToken);

        return new VerificationResponse(verified);
    }

    private bool VerifyPermission(ClaimsPrincipal user)
    {
        return _permissionChecker.IsAuthenticated(user);
    }

    #endregion
}