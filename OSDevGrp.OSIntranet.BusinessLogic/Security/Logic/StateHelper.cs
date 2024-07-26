using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class StateHelper
    {
        #region Methods

        internal static IValidator ValidateState(this IValidator validator, string value, Type validatingType, string validatingField, bool allowNull = false, bool shouldBeBase64 = false)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (allowNull == false)
            {
                validator = validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField);
            }

            validator = validator.String.ShouldHaveMinLength(value, 1, validatingType, validatingField, allowNull);

            if (shouldBeBase64)
            {
                validator = validator.ValidateBase64String(value, validatingType, validatingField, allowNull);
            }

            return validator;
        }

        #endregion
    }
}