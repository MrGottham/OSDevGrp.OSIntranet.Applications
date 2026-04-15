namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;

public class VerificationResponse : ResponseBase
{
    #region Constructor

    public VerificationResponse(bool verified)
    {
        Verified = verified;
    }

    #endregion

    #region Properties

    public bool Verified { get; }

    #endregion
}