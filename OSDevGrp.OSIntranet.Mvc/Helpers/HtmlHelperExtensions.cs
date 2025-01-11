using System.IO;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        #region Private constants

        private const string AntiForgeryTokenName = "__CSRF";

        #endregion

        #region Private variables

        private static readonly Regex AntiForgeryTokenNameRegex = new(@$"name=""({AntiForgeryTokenName})""", RegexOptions.Compiled);
        private static readonly Regex AntiForgeryTokenValueRegex = new(@"value=""([A-Za-z0-9+=/\-\\_]+)""", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static string AntiForgeryTokenToJsonString(this IHtmlHelper htmlHelper)
        {
            NullGuard.NotNull(htmlHelper, nameof(htmlHelper));

            IHtmlContent htmlContent = htmlHelper.AntiForgeryToken();
            if (htmlContent == null)
            {
                return null;
            }

            using StringWriter writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);

            string htmlContentAsString = writer.GetStringBuilder().ToString();

            Match antiForgeryTokenNameMatch = AntiForgeryTokenNameRegex.Match(htmlContentAsString);
            Match antiForgeryTokenValueMatch = AntiForgeryTokenValueRegex.Match(htmlContentAsString);

            if (string.IsNullOrWhiteSpace(htmlContentAsString) || antiForgeryTokenNameMatch.Success == false || antiForgeryTokenValueMatch.Success == false)
            {
                return null;
            }

            return $"{antiForgeryTokenNameMatch.Groups[1].Value}: '{antiForgeryTokenValueMatch.Groups[1].Value}'";
        }

        #endregion
    }
}