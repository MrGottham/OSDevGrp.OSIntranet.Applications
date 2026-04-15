using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

namespace OSDevGrp.OSIntranet.Core.HealthChecks;

public class LicensesHealthCheckOptions : ConfigurationHealthCheckOptionsBase<LicensesHealthCheckOptions>
{
    #region Properites

    protected override LicensesHealthCheckOptions HealthCheckOptions => this;

    #endregion

    #region Methods

    public LicensesHealthCheckOptions WithAutoMapperLicense(IConfiguration configuration)
    {
        NullGuard.NotNull(configuration, nameof(configuration));

        return AddStringConfigurationValidation(configuration, LicensesConfigurationKeys.AutoMapperLicenseKey);
    }

    #endregion
}