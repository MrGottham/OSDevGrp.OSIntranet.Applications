using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class PostalCodeHelper
    {
        #region Private variables

        private static readonly Regex PostalCodeRegex = new Regex("[0-9]{1,16}", RegexOptions.Compiled);

        #endregion

        #region Methods

        internal static IValidator ValidateRequiredPostalCode(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .ValidatePostalCode(value, validatingType, validatingField, false);
        }

        internal static IValidator ValidateOptionalPostalCode(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.ValidatePostalCode(value, validatingType, validatingField, true);
        }

        internal static IPostalCode GetPostalCode(this string code, string countryCode, IContactRepository contactRepository, ref IPostalCode postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code))
                .NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNull(contactRepository, nameof(contactRepository));

            return postalCode ??= contactRepository.GetPostalCodeAsync(countryCode, code).GetAwaiter().GetResult();
        }

        private static IValidator ValidatePostalCode(this IValidator validator, string value, Type validatingType, string validatingField, bool allowNull)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, allowNull)
                .String.ShouldHaveMaxLength(value, 16, validatingType, validatingField, allowNull)
                .String.ShouldMatchPattern(value, PostalCodeRegex, validatingType, validatingField, allowNull);
        }

        #endregion
    }
}