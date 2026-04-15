namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IHumanVerifiableRequest : IRequest
{
    string VerificationKey { get; }

    string VerificationCode { get; }
}