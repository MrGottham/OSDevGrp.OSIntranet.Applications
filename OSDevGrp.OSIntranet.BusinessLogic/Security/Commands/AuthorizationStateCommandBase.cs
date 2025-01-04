using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal abstract class AuthorizationStateCommandBase : IAuthorizationStateCommand
    {
        #region Constructor

        protected AuthorizationStateCommandBase(string authorizationState, Func<byte[], byte[]> unprotect)
        {
            NullGuard.NotNullOrWhiteSpace(authorizationState, nameof(authorizationState))
                .NotNull(unprotect, nameof(unprotect));

            AuthorizationState = authorizationState;
            Unprotect = unprotect;
        }

        #endregion

        #region Properties

        public string AuthorizationState { get; }

        public Func<byte[], byte[]> Unprotect { get; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.ValidateAuthorizationState(AuthorizationState, GetType(), nameof(AuthorizationState))
                .Object.ShouldNotBeNull(Unprotect, GetType(), nameof(Unprotect));
        }

        public IAuthorizationState ToDomain(IAuthorizationStateFactory authorizationStateFactory, IValidator validator, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(authorizationStateFactory, nameof(authorizationStateFactory))
                .NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            IAuthorizationState authorizationState = authorizationStateFactory.FromBase64String(AuthorizationState, Unprotect);

            validator.ValidateAuthorizationState(authorizationState, securityRepository, trustedDomainResolver, supportedScopesProvider);

            return authorizationState;
        }

        #endregion
    }
}