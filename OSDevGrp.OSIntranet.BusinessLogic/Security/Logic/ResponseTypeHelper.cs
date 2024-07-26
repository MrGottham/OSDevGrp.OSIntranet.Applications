using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class ResponseTypeHelper
    {
        #region Internal variables

        internal static readonly Regex ResponseTypeForAuthorizationCodeFlowRegex = new("^(code){1}$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        #endregion

        #region Methods

        internal static IValidator ValidateResponseType(this IValidator validator, string value, Regex responseTypePattern, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(responseTypePattern, nameof(responseTypePattern))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .String.ShouldMatchPattern(value, responseTypePattern, validatingType, validatingField);
        }

        #endregion
    }
}