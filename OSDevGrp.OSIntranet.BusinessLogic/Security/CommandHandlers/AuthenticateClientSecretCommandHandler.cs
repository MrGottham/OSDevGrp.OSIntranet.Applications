using System;
using System.Security.Principal;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class AuthenticateClientSecretCommandHandler : AuthenticateCommandHandlerBase<IAuthenticateClientSecretCommand, IClientSecretIdentity>
    {
        #region Private variables

        private readonly ITokenHelper _tokenHelper;

        #endregion

        #region Constructor

        public AuthenticateClientSecretCommandHandler(ISecurityRepository securityRepository, ITokenHelper tokenHelper)
            : base(securityRepository)
        {
            NullGuard.NotNull(tokenHelper, nameof(tokenHelper));

            _tokenHelper = tokenHelper;
        }

        #endregion

        #region Methods

        protected override async Task<IIdentity> GetIdentityAsync(IAuthenticateClientSecretCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IClientSecretIdentity clientSecretIdentity = await SecurityRepository.GetClientSecretIdentityAsync(command.ClientId);

            return clientSecretIdentity;
        }

        protected override IClientSecretIdentity CreateAuthenticatedIdentity(IAuthenticateClientSecretCommand command, IIdentity identity)
        {
            NullGuard.NotNull(command, nameof(command))
                .NotNull(identity, nameof(identity));

            IClientSecretIdentity clientSecretIdentity = (IClientSecretIdentity) identity;

            clientSecretIdentity.AddClaims(command.Claims);
            clientSecretIdentity.ClearSensitiveData();

            //TODO: Handle this
            //clientSecretIdentity.AddToken(_tokenHelper.Generate(clientSecretIdentity));

            return clientSecretIdentity;
        }

        protected override bool IsMatch(IAuthenticateClientSecretCommand command, IIdentity identity)
        {
            NullGuard.NotNull(command, nameof(command))
                .NotNull(identity, nameof(identity));

            return string.Compare(command.ClientSecret, ((IClientSecretIdentity) identity).ClientSecret, StringComparison.Ordinal) == 0;
        }

        #endregion
    }
}
