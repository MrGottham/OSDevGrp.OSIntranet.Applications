namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IVerificationCodeVerifier
{
    Task<bool> VerifyAsync(string verificationKey, string verificationCode, CancellationToken cancellationToken = default);
}