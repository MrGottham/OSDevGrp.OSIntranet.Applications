namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public interface ITokenKeyGenerator
{
    Task<string> GenerateAsync(IReadOnlyCollection<string> values, CancellationToken cancellationToken = default);
}