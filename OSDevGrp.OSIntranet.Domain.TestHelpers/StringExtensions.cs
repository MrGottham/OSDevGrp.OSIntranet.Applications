using OSDevGrp.OSIntranet.Core;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
	public static class StringExtensions
    {
	    public static bool IsBase64String(this string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            string s = value.Trim();
            return s.Length % 4 == 0 && Regex.IsMatch(s, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", RegexOptions.Compiled);
        }
    }
}