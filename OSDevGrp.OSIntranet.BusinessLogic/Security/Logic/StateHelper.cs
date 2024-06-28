using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class StateHelper
    {
        #region Private variables

        private static readonly Regex StateRegex = new("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        #endregion

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
                validator = validator.String.ShouldMatchPattern(value, StateRegex, validatingType, validatingField, allowNull);
            }

            return validator;
        }

        #endregion
    }
}