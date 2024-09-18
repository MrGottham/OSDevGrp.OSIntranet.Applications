using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class ScopesHelper
    {
        #region Methods

        internal static IValidator ValidateScopes(this IValidator validator, IEnumerable<string> value, ISupportedScopesProvider supportedScopesProvider, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.ValidateScopes(value?.ToArray(), supportedScopesProvider, validatingType, validatingField);
        }

        private static IValidator ValidateScopes(this IValidator validator, string[] value, ISupportedScopesProvider supportedScopesProvider, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Object.ShouldNotBeNull(value, validatingType, validatingField)
                .Enumerable.ShouldContainItems(value, validatingType, validatingField)
                .Enumerable.ShouldHaveMinItems(value, 1, validatingType, validatingField)
                .Enumerable.ShouldHaveMaxItems(value, supportedScopesProvider.SupportedScopes.Count, validatingType, validatingField)
                .Object.ShouldBeKnownValue(value, scopes => IsSupportedScopesAsync(scopes, supportedScopesProvider), validatingType, validatingField);
        }

        private static Task<bool> IsSupportedScopesAsync(string[] scopes, ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(scopes, nameof(scopes))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            return Task.FromResult(scopes.All(supportedScopesProvider.SupportedScopes.ContainsKey));
        }

        #endregion
    }
}