using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

            userIdentity.AddClaims(Transform(claims));
            userIdentity.ClearSensitiveData();

            return new ClaimsIdentity(userIdentity.ToClaimsIdentity().Claims, authenticationType);
        }

        private static IEnumerable<Claim> Transform(IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(claims, nameof(claims));

            return claims.Select(Transform);
        }

        private static Claim Transform(Claim claim)
        {
            NullGuard.NotNull(claim, nameof(claim));

            return claim.Type == ClaimTypes.NameIdentifier ? new Claim(claim.Type, string.IsNullOrWhiteSpace(claim.Value) ? string.Empty : claim.Value.ComputeSha512Hash()) : claim;
        }

        #endregion
    }
}