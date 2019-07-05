using System;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    internal static class CountryCodeHelper
    {
        #region Methods

        internal static IValidator ValidateCountryCode(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .String.ShouldHaveMaxLength(value, 4, validatingType, validatingField)
                .String.ShouldMatchPattern(value, new Regex("[A-Z]{1,4}", RegexOptions.Compiled), validatingType, validatingField);
        }

        internal static ICountry GetCountry(this string countryCode, IContactRepository contactRepository, ref ICountry country)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNull(contactRepository, nameof(contactRepository));

            return country ?? (country = contactRepository.GetCountryAsync(countryCode).GetAwaiter().GetResult());
        }

        #endregion
    }
}
