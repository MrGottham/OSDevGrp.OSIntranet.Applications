using System.Security.Principal;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public abstract class AuthenticateCommandHandlerBase<TCommand, TResult> : CommandHandlerNonTransactionalBase, ICommandHandler<TCommand, TResult> where TCommand : ICommand where TResult : IIdentity
    {
        #region Protected variables

        protected readonly ISecurityRepository SecurityRepository;

        #endregion

        #region Constructor

        protected AuthenticateCommandHandlerBase(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            SecurityRepository = securityRepository;
        }

        #endregion

        #region Methods

        public async Task<TResult> ExecuteAsync(TCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IIdentity identity = await GetIdentityAsync(command);
            if (identity == null || IsMatch(command, identity) == false)
            {
                return default(TResult);
            }

            return CreateAuthenticatedIdentity(identity);
        }

        protected abstract Task<IIdentity> GetIdentityAsync(TCommand command);

        protected abstract TResult CreateAuthenticatedIdentity(IIdentity identity);

        protected virtual bool IsMatch(TCommand command, IIdentity identity)
        {
            NullGuard.NotNull(command, nameof(command))
                .NotNull(identity, nameof(identity));

            return true;
        }

        #endregion
    }
}
