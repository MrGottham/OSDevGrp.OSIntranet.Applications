using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal static class IdentityIdentifierHelper
    {
        #region Methods

        internal static IValidator ValidateIdentityIdentifier(this IValidator validator, int value, Type validatingType, string validatingField)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(validatingType, nameof(validatingType))
                .NotNullOrWhiteSpace(validatingField, nameof(validatingField));

            return validator.Integer.ShouldBeGreaterThanZero(value, validatingType, validatingField);
        }

        internal static IUserIdentity GetUserIdentity(this int identifier, ISecurityRepository securityRepository, ref IUserIdentity userIdentity)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            return userIdentity ?? (userIdentity = securityRepository.GetUserIdentityAsync(identifier).GetAwaiter().GetResult());
        }

        internal static IClientSecretIdentity GetClientSecretIdentity(this int identifier, ISecurityRepository securityRepository, ref IClientSecretIdentity clientSecretIdentity)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            return clientSecretIdentity ?? (clientSecretIdentity = securityRepository.GetClientSecretIdentityAsync(identifier).GetAwaiter().GetResult());
        }

        #endregion
    }
}