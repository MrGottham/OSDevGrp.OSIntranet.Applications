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

        internal static IValidator ValidatePhoneNumber(this IValidator validator, string value, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, allowNull)
                .String.ShouldHaveMaxLength(value, 32, validatingType, validatingField, allowNull)
                .String.ShouldMatchPattern(value, PhoneNumberRegex, validatingType, validatingField, allowNull);
        }

        #endregion
    }
}