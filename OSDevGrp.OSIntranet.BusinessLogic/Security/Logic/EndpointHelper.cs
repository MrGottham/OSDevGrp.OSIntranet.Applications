using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class EndpointHelper
    {
        #region Methods

        internal static IValidator ValidateEndpoint(this IValidator validator, Uri endpoint, Type validatingType, string validatingField, bool allowNull = false)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            if (allowNull)
            {
                return validator.Object.ShouldBeKnownValue(endpoint, IsAbsoluteUri, validatingType, validatingField, true);
            }

            return validator.Object.ShouldNotBeNull(endpoint, validatingType, validatingField)
                .Object.ShouldBeKnownValue(endpoint, IsAbsoluteUri, validatingType, validatingField);
        }

        private static Task<bool> IsAbsoluteUri(Uri endpoint)
        {
            NullGuard.NotNull(endpoint, nameof(endpoint));

            return Task.FromResult(endpoint.IsAbsoluteUri);
        }

        #endregion
    }
}