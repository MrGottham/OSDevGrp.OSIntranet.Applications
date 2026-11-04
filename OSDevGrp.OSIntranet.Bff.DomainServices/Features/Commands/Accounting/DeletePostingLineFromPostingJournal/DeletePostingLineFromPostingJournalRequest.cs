using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

public sealed class DeletePostingLineFromPostingJournalRequest : PostingJournalLineIdentificationRequestBase, IHumanVerifiableRequest
{
    #region Constructor

    public DeletePostingLineFromPostingJournalRequest(
        Guid requestId,
        int accountingNumber,
        Guid identifier,
        string verificationKey,
        string verificationCode,
        IFormatProvider formatProvider,
        ISecurityContext securityContext)
        : base(requestId, accountingNumber, identifier, formatProvider, securityContext)
    {
        VerificationKey = verificationKey ?? throw new ArgumentNullException(nameof(verificationKey));
        VerificationCode = verificationCode ?? throw new ArgumentNullException(nameof(verificationCode));
    }

    #endregion

    #region Properties

    public string VerificationKey { get; }

    public string VerificationCode { get; }

    #endregion
}