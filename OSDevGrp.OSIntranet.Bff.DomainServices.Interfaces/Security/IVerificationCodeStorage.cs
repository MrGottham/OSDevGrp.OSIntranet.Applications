namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IVerificationCodeStorage
{
    Task<string?> GetVerificationCodeAsync(string verificationKey, CancellationToken cancellationToken = default);

    Task StoreVerificationCodeAsync(string verificationKey, string verificationCode, DateTimeOffset expires, CancellationToken cancellationToken = default);

    Task RemoveVerificationCodeAsync(string verificationKey, CancellationToken cancellationToken = default);
}