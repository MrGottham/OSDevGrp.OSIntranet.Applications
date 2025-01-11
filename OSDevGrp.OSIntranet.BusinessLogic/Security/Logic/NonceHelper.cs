using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class NonceHelper
    {
        #region Methods

        internal static IValidator ValidateNonce(this IValidator validator, string value, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (allowNull == false)
            {
                validator = validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField);
            }

            return validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, allowNull);
        }

        #endregion
    }
}