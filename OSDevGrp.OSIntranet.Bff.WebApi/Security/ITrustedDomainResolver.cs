namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

public interface ITrustedDomainResolver
{
    bool IsTrustedDomain(Uri uri);
}