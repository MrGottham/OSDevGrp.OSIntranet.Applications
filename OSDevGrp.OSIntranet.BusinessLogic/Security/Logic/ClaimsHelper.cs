using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class ClaimsHelper
    {
        #region Methods

        internal static IValidator ValidateClaims(this IValidator validator, IEnumerable<Claim> value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.ValidateClaims(value?.ToArray(), validatingType, validatingField);
        }

        private static IValidator ValidateClaims(this IValidator validator, Claim[] value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Object.ShouldNotBeNull(value, validatingType, validatingField)
                .Enumerable.ShouldContainItems(value, validatingType, validatingField)
                .Enumerable.ShouldHaveMinItems(value, 1, validatingType, validatingField);
        }

        #endregion
    }
}