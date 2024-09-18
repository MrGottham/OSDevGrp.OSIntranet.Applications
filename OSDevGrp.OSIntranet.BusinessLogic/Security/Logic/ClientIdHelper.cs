using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class ClientIdHelper
    {
        #region Methods

        internal static IValidator ValidateClientId(this IValidator validator, string value, ISecurityRepository securityRepository, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.String.ShouldNotBeNullOrWhiteSpace(value, validatingType, validatingField)
                .String.ShouldHaveMinLength(value, 1, validatingType, validatingField)
                .Object.ShouldBeKnownValue(value, clientId => ExistingClientIdAsync(clientId, securityRepository), validatingType, validatingField);
        }

        private static async Task<bool> ExistingClientIdAsync(string clientId, ISecurityRepository securityRepository)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(securityRepository, nameof(securityRepository));

            return await securityRepository.GetClientSecretIdentityAsync(clientId) != null;
        }

        #endregion
    }
}