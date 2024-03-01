namespace OSDevGrp.OSIntranet.Repositories.Interfaces.Configuration
{
    public static class ExternalDataConfigurationKeys
    {
        public const string ExternalDataSectionName = "ExternalData";
        public const string DashboardSectionName = "Dashboard";

        public static readonly string DashboardEndpointAddress = $"{ExternalDataSectionName}:{DashboardSectionName}:EndpointAddress";
    }
}