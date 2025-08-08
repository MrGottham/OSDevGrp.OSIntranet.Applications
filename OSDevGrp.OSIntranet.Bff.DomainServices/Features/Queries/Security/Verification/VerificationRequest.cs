using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;

public class VerificationRequest : RequestBase
{
    #region Constructor

    public VerificationRequest(Guid requestId, string verificationKey, string verificationCode, ISecurityContext securityContext) 
        : base(requestId, securityContext)
    {
        VerificationKey = verificationKey;
        VerificationCode = verificationCode;
    }

    #endregion

    #region Properties

    public string VerificationKey { get; }

    public string VerificationCode { get; }

    #endregion
}