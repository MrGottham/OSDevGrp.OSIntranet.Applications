using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Logic
{
    internal static class LetterHeadIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidateLetterHeadIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeBetween(value, 1, 99, validatingType, validatingField);
        }

        internal static ILetterHead GetLetterHead(this int number, ICommonRepository commonRepository, ref ILetterHead letterHead)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository));

            return letterHead ??= commonRepository.GetLetterHeadAsync(number).GetAwaiter().GetResult();
        }

        #endregion
    }
}