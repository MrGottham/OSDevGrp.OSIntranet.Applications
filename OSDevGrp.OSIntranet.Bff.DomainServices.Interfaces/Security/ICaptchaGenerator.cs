namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface ICaptchaGenerator
{
    Task<byte[]> GenerateAsync(string code, CancellationToken cancellationToken = default);
}