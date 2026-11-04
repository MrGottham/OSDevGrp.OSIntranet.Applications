namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

public interface IHumanVerifiableRequest : IRequest
{
    IFormatProvider FormatProvider { get; }

    string VerificationKey { get; }

    string VerificationCode { get; }
}