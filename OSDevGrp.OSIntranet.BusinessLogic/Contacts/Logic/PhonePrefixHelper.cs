using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class PhonePrefixHelper
    {
        #region Private variables

        private static readonly Regex PhonePrefixRegex = new Regex(@"\+[0-9]{1,3}", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static IValidator ValidatePhonePrefix(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .String.ShouldHaveMaxLength(value, 4, validatingType, validatingField)
                .String.ShouldMatchPattern(value, PhonePrefixRegex, validatingType, validatingField);
        }
        
        #endregion
    }
}