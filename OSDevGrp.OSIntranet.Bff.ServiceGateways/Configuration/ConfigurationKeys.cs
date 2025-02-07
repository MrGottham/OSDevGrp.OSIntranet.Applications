namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Configuration;

internal static class ConfigurationKeys
{
    #region Internal constants

    internal const string ServiceGatewaysSectionName = "ServiceGateways";

    internal const string WebApiSectionName = "WebApi";

    #endregion

    #region Internal variables

    internal static readonly string ServiceGatewaysWebApiEndpointAddressKey = $"{ServiceGatewaysSectionName}:{WebApiSectionName}:EndpointAddress";

    internal static readonly string ServiceGatewaysWebApiClientIdKey = $"{ServiceGatewaysSectionName}:{WebApiSectionName}:ClientId";

    internal static readonly string ServiceGatewaysWebApiClientSecretKey = $"{ServiceGatewaysSectionName}:{WebApiSectionName}:ClientSecret";

    #endregion
}