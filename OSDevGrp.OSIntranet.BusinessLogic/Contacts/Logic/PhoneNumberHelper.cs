using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class PhoneNumberHelper
    {
        #region Private variables

        private static readonly Regex PhoneNumberRegex = new Regex(@"^[\+]?[0-9\s]+$", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static IValidator ValidatePhoneNumber(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, true)
                .String.ShouldMatchPattern(value, PhoneNumberRegex, validatingType, validatingField, true);
        }

        #endregion
    }
}