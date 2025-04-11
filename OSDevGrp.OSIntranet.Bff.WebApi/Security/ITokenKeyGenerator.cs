namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public interface ITokenKeyGenerator : IDisposable
{
    Task<string> GenerateAsync(IReadOnlyCollection<string> values, CancellationToken cancellationToken = default);
}