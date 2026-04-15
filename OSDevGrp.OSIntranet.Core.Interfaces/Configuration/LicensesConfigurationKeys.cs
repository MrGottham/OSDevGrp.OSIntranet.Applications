namespace OSDevGrp.OSIntranet.Core.Interfaces.Configuration;

public static class LicensesConfigurationKeys
{
    public const string LicensesSectionName = "Licenses";
    public const string AutoMapperSectionName = "AutoMapper";

    public static readonly string AutoMapperLicenseKey = $"{LicensesSectionName}:{AutoMapperSectionName}:LicenseKey";
}