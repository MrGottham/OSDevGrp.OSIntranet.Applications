using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
	internal sealed class AuthenticateUserCommandHandler : AuthenticateCommandHandlerBase<IAuthenticateUserCommand, IUserIdentity>
    {
        #region Constructor

        public AuthenticateUserCommandHandler(ISecurityRepository securityRepository, IExternalTokenClaimCreator externalTokenClaimCreator)
            : base(securityRepository, externalTokenClaimCreator)
        {
        }

        #endregion

        #region Methods

        protected override Task<IUserIdentity> GetIdentityAsync(IAuthenticateUserCommand authenticateUserCommand)
        {
	        NullGuard.NotNull(authenticateUserCommand, nameof(authenticateUserCommand));

	        return SecurityRepository.GetUserIdentityAsync(authenticateUserCommand.ExternalUserIdentifier);
        }

        protected override ClaimsIdentity CreateAuthenticatedClaimsIdentity(IUserIdentity userIdentity, IEnumerable<Claim> claims, string authenticationType)
        {
	        NullGuard.NotNull(userIdentity, nameof(userIdentity))
		        .NotNull(claims, nameof(claims))
		        .NotNullOrWhiteSpace(authenticationType, nameof(authenticationType));

            userIdentity.AddClaims(claims);
            userIdentity.ClearSensitiveData();

            return new ClaimsIdentity(userIdentity.ToClaimsIdentity().Claims, authenticationType);
        }

        #endregion
	}
}