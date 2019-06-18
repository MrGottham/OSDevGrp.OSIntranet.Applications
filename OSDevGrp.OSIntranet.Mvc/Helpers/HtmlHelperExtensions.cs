using System.IO;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Helpers
{
    internal static class HtmlHelperExtensions
    {
        internal static string AntiForgeryTokenToJsonString(this IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            IHtmlContent htmlContent = htmlHelper.AntiForgeryToken();
            if (htmlContent == null)
            {
                return null;
            }

            using (StringWriter writer = new StringWriter())
            {
                htmlContent.WriteTo(writer, HtmlEncoder.Default);

                Regex nameRegex = new Regex(@"name=""([A-Za-z_]+)""", RegexOptions.Compiled);
                Regex valueRegex = new Regex(@"value=""([A-Za-z0-9+=/\-\\_]+)""", RegexOptions.Compiled);

                string htmlContentAsString = writer.GetStringBuilder().ToString();
                if (string.IsNullOrWhiteSpace(htmlContentAsString) || nameRegex.IsMatch(htmlContentAsString) == false || valueRegex.IsMatch(htmlContentAsString) == false)
                {
                    return null;
                }

                return $"{nameRegex.Match(htmlContentAsString).Groups[1].Value}: '{valueRegex.Match(htmlContentAsString).Groups[1].Value}'";
            }
        }
    }
}