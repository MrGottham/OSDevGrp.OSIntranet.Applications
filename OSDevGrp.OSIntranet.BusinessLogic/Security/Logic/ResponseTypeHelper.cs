using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class ResponseTypeHelper
    {
        #region Private variables

        private static readonly Regex ResponseTypeRegex = new("^(code){1}$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        #endregion

        #region Methods

        internal static IValidator ValidateResponseType(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .String.ShouldMatchPattern(value, ResponseTypeRegex, validatingType, validatingField);
        }

        #endregion
    }
}