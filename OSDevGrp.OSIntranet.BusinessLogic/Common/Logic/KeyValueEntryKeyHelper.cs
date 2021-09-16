using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Logic
{
    internal static class KeyValueEntryKeyHelper
    {
        #region Methods

        internal static IValidator ValidateKeyValueEntryKey(this IValidator validator, string value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .String.ShouldHaveMaxLength(value, 256, validatingType, validatingField);
        }

        internal static IKeyValueEntry GetKeyValueEntry(this string key, ICommonRepository commonRepository, ref IKeyValueEntry keyValueEntry)
        {
            NullGuard.NotNullOrWhiteSpace(key, nameof(key))
                .NotNull(commonRepository, nameof(commonRepository));

            return keyValueEntry ??= commonRepository.PullKeyValueEntryAsync(key).GetAwaiter().GetResult();
        }

        #endregion
    }
}