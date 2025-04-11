using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class TrustedDomainResolver : ITrustedDomainResolver
{
    #region Private variables

    private readonly IOptions<TrustedDomainOptions> _trustedDomainOptions;

    #endregion

    #region Constructors

    public TrustedDomainResolver(IOptions<TrustedDomainOptions> trustedDomainOptions)
    {
        _trustedDomainOptions = trustedDomainOptions;
    }

    #endregion

    #region Methods

    public bool IsTrustedDomain(Uri uri)
    {
        return _trustedDomainOptions.Value.AsTrustedDomains().Contains(uri.Host, StringComparer.InvariantCultureIgnoreCase);
    }

    #endregion
}