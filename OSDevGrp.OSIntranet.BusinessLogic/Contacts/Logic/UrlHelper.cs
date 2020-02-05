using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class UrlHelper
    {
        #region Private variables

        private static readonly Regex UrlRegex = new Regex(@"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static IValidator ValidateUrl(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
                .String.ShouldHaveMaxLength(value, 256, validatingType, validatingField, true)
                .String.ShouldMatchPattern(value, UrlRegex, validatingType, validatingField, true);
        }

        #endregion
    }
}