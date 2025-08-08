namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

public interface IHashGenerator : IDisposable
{
    Task<string> GenerateAsync(byte[] buffer, CancellationToken cancellationToken = default);
}