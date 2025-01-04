using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class IdTokenHelper
    {
        #region Methods

        internal static IValidator ValidateIdToken(this IValidator validator, IToken value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Object.ShouldNotBeNull(value, validatingType, validatingField);
        }

        #endregion
    }
}