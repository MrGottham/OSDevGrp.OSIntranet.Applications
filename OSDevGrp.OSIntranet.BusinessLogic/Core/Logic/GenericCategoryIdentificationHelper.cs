using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Core.Logic
{
    internal static class GenericCategoryIdentificationHelper
    {
        #region Methods

        internal static IValidator ValidateGenericCategoryIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField);
        }

        #endregion
    }
}