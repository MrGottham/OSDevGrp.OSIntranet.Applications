using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class AuthenticationTimeHelper
    {
        #region Methods

        internal static IValidator ValidateAuthenticationTime(this IValidator validator, DateTimeOffset value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.DateTime.ShouldBePastDateTime(value.UtcDateTime, validatingType, validatingField);
        }

        #endregion
    }
}