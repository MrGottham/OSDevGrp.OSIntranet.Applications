using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class MailAddressHelper
    {
        #region Private variables

        private static readonly Regex MailAddressRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static IValidator ValidateMailAddress(this IValidator validator, string value, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, allowNull)
                .String.ShouldHaveMaxLength(value, 256, validatingType, validatingField, allowNull)
                .String.ShouldMatchPattern(value, MailAddressRegex, validatingType, validatingField, allowNull);
        }

        #endregion
    }
}