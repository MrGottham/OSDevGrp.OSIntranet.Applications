using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal abstract class AuthorizationStateCommandHandlerBase<TAuthorizationStateCommand, TResult> : CommandHandlerNonTransactionalBase, ICommandHandler<TAuthorizationStateCommand, TResult> where TAuthorizationStateCommand : IAuthorizationStateCommand
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IAuthorizationStateFactory _authorizationStateFactory;
        private readonly ITrustedDomainResolver _trustedDomainResolver;

        #endregion

        #region Constructor

        protected AuthorizationStateCommandHandlerBase(IValidator validator, IAuthorizationStateFactory authorizationStateFactory, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(authorizationStateFactory, nameof(authorizationStateFactory))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            _validator = validator;
            _authorizationStateFactory = authorizationStateFactory;
            _trustedDomainResolver = trustedDomainResolver;

            SecurityRepository = securityRepository;
            SupportedScopesProvider = supportedScopesProvider;
        }

        #endregion

        #region Properties

        protected ISecurityRepository SecurityRepository { get; }

        protected ISupportedScopesProvider SupportedScopesProvider { get; }

        #endregion

        #region Methods

        public async Task<TResult> ExecuteAsync(TAuthorizationStateCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            command.Validate(_validator);

            IAuthorizationState authorizationState = command.ToDomain(_authorizationStateFactory, _validator, SecurityRepository, _trustedDomainResolver, SupportedScopesProvider);

            return await HandleAuthorizationStateAsync(command, authorizationState);
        }

        protected abstract Task<TResult> HandleAuthorizationStateAsync(TAuthorizationStateCommand command, IAuthorizationState authorizationState);

        #endregion
    }
}