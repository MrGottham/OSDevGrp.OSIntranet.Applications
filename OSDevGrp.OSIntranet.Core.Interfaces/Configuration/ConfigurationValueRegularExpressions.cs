using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Configuration
{
    public static class ConfigurationValueRegularExpressions
    {
        public static readonly Regex JwtKeyRegularExpression = new Regex("^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", RegexOptions.Compiled);
    }
}