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
        #region Methods

        internal static IValidator ValidatePostalCode(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .String.ShouldHaveMaxLength(value, 16, validatingType, validatingField)
                .String.ShouldMatchPattern(value, new Regex("[0-9]{1,16}", RegexOptions.Compiled), validatingType, validatingField);
        }

        internal static IPostalCode GetPostalCode(this string code, string countryCode, IContactRepository contactRepository, ref IPostalCode postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code))
                .NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNull(contactRepository, nameof(contactRepository));

            return postalCode ?? (postalCode = contactRepository.GetPostalCodeAsync(countryCode, code).GetAwaiter().GetResult());
        }

        #endregion
    }
}
