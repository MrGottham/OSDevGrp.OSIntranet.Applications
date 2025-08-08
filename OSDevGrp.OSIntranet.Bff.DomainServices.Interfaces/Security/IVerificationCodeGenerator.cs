namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IVerificationCodeGenerator
{
    Task<string> GenerateAsync(CancellationToken cancellationToken = default);
}