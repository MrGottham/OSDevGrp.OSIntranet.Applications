using System.Security.Principal;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class AuthenticateUserCommandHandler : AuthenticateCommandHandlerBase<IAuthenticateUserCommand, IUserIdentity>
    {
        #region Constructor

        public AuthenticateUserCommandHandler(ISecurityRepository securityRepository)
            : base(securityRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task<IIdentity> GetIdentityAsync(IAuthenticateUserCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IUserIdentity userIdentity = await SecurityRepository.GetUserIdentityAsync(command.ExternalUserIdentifier);

            return userIdentity;
        }

        protected override IUserIdentity CreateAuthenticatedIdentity(IIdentity identity)
        {
            NullGuard.NotNull(identity, nameof(identity));

            IUserIdentity userIdentity = (IUserIdentity) identity;

            userIdentity.ClearSensitiveData();

            return userIdentity;
        }

        #endregion
    }
}
