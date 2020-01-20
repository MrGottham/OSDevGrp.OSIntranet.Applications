using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class ExpiresHelper
    {
        internal static IValidator ValidateExpires(this IValidator validator, DateTime value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.DateTime.ShouldBeFutureDateTime(value, validatingType, validatingField);
        }
    }
}