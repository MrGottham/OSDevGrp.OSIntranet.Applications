using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Configuration;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Options;

internal static class ConfigurationExtensions
{
    #region Methods

    internal static WebApiOptions? GetWebApiOptions(this IConfiguration configuration)
    {
        return configuration.GetWebApiSection().Get<WebApiOptions>();
    }

    internal static IConfigurationSection GetWebApiSection(this IConfiguration configuration)
    {
        return configuration.GetSection($"{ConfigurationKeys.ServiceGatewaysSectionName}:{ConfigurationKeys.WebApiSectionName}");
    }

    #endregion
}