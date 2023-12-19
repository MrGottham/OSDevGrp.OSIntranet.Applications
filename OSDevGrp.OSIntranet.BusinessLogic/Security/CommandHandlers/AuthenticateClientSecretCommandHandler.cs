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
	internal sealed class AuthenticateClientSecretCommandHandler : AuthenticateCommandHandlerBase<IAuthenticateClientSecretCommand, IClientSecretIdentity>
    {
        #region Constructor

        public AuthenticateClientSecretCommandHandler(ISecurityRepository securityRepository, IExternalTokenClaimCreator externalTokenClaimCreator)
            : base(securityRepository, externalTokenClaimCreator)
        {
        }

        #endregion

        #region Methods

        protected override Task<IClientSecretIdentity> GetIdentityAsync(IAuthenticateClientSecretCommand authenticateClientSecretCommand)
        {
	        NullGuard.NotNull(authenticateClientSecretCommand, nameof(authenticateClientSecretCommand));

            return SecurityRepository.GetClientSecretIdentityAsync(authenticateClientSecretCommand.ClientId);
        }

        protected override ClaimsIdentity CreateAuthenticatedClaimsIdentity(IClientSecretIdentity clientSecretIdentity, IEnumerable<Claim> claims, string authenticationType)
		{
			NullGuard.NotNull(clientSecretIdentity, nameof(clientSecretIdentity))
				.NotNull(claims, nameof(claims))
				.NotNullOrWhiteSpace(authenticationType, nameof(authenticationType));

			clientSecretIdentity.AddClaims(claims);
			clientSecretIdentity.ClearSensitiveData();

			return new ClaimsIdentity(clientSecretIdentity.ToClaimsIdentity().Claims, authenticationType);
		}

        protected override bool IsMatch(IAuthenticateClientSecretCommand authenticateClientSecretCommand, IClientSecretIdentity clientSecretIdentity)
        {
	        NullGuard.NotNull(authenticateClientSecretCommand, nameof(authenticateClientSecretCommand))
		        .NotNull(clientSecretIdentity, nameof(clientSecretIdentity));

	        return string.CompareOrdinal(authenticateClientSecretCommand.ClientSecret, clientSecretIdentity.ClientSecret) == 0;
        }

        #endregion
    }
}