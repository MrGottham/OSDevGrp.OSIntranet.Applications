using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Configuration
{
    public static class ConfigurationValueRegularExpressions
    {
        public static readonly Regex JwtKeyTypeRegularExpression = new("^(RSA)$", RegexOptions.Compiled);
        public static readonly Regex JwtKeyBase64UrlRegularExpression = new("^[A-Za-z0-9_-]+$", RegexOptions.Compiled);
    }
}