using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Security.GenerateVerification;

public class GenerateVerificationRequest : RequestBase
{
    #region Constructor

    public GenerateVerificationRequest(Guid requestId, Action<string, IReadOnlyCollection<byte>, DateTimeOffset> onVerificationCreated, ISecurityContext securityContext)
        : base(requestId, securityContext)
    {
        OnVerificationCreated = onVerificationCreated; 
    }

    #endregion

    #region Properties

    public Action<string, IReadOnlyCollection<byte>, DateTimeOffset> OnVerificationCreated { get; }

    #endregion
}