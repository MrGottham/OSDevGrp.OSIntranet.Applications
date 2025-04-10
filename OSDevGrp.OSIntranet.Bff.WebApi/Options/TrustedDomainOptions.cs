namespace OSDevGrp.OSIntranet.Bff.WebApi.Options;

public class TrustedDomainOptions
{
    public string? TrustedDomainCollection { get; set; }

    internal IEnumerable<string> AsTrustedDomains() => TrustedDomainCollection?.Split(';').Where(trustedDomain => string.IsNullOrWhiteSpace(trustedDomain) == false).ToArray() ?? Array.Empty<string>();
}