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
    internal class PrepareAuthorizationCodeFlowCommandHandler : CommandHandlerNonTransactionalBase, ICommandHandler<IPrepareAuthorizationCodeFlowCommand, string>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly ISecurityRepository _securityRepository;
        private readonly ITrustedDomainResolver _trustedDomainResolver;
        private readonly ISupportedScopesProvider _supportedScopesProvider;
        private readonly IAuthorizationStateFactory _authorizationStateFactory;

        #endregion

        #region Constructor

        public PrepareAuthorizationCodeFlowCommandHandler(IValidator validator, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider, IAuthorizationStateFactory authorizationStateFactory)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider))
                .NotNull(authorizationStateFactory, nameof(authorizationStateFactory));

            _validator = validator;
            _securityRepository = securityRepository;
            _trustedDomainResolver = trustedDomainResolver;
            _supportedScopesProvider = supportedScopesProvider;
            _authorizationStateFactory = authorizationStateFactory;
        }

        #endregion

        #region Methods

        public Task<string> ExecuteAsync(IPrepareAuthorizationCodeFlowCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            return Task.Run(() =>
            {
                command.Validate(_validator, _securityRepository, _trustedDomainResolver, _supportedScopesProvider);

                IAuthorizationState authorizationState = command.ToDomain(_authorizationStateFactory);

                return authorizationState.ToString(command.Protector);
            });
        }

        #endregion
    }
}