using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
	internal abstract class AuthenticateCommandHandlerBase<TAuthenticateCommand, TIdentity> : CommandHandlerNonTransactionalBase, ICommandHandler<TAuthenticateCommand, ClaimsPrincipal> where TAuthenticateCommand : IAuthenticateCommand where TIdentity : IIdentity
    {
        #region Private variables

        private readonly IExternalTokenClaimCreator _externalTokenClaimCreator;

        #endregion

        #region Constructor

        protected AuthenticateCommandHandlerBase(ISecurityRepository securityRepository, IExternalTokenClaimCreator externalTokenClaimCreator)
        {
	        NullGuard.NotNull(securityRepository, nameof(securityRepository))
		        .NotNull(externalTokenClaimCreator, nameof(externalTokenClaimCreator));

            SecurityRepository = securityRepository;
            _externalTokenClaimCreator = externalTokenClaimCreator;
        }

		#endregion

		#region Properties

		protected ISecurityRepository SecurityRepository { get; }

        #endregion

        #region Methods

        public async Task<ClaimsPrincipal> ExecuteAsync(TAuthenticateCommand authenticateCommand)
        {
	        NullGuard.NotNull(authenticateCommand, nameof(authenticateCommand));

	        TIdentity identity = await GetIdentityAsync(authenticateCommand);
	        if (identity == null || IsMatch(authenticateCommand, identity) == false)
	        {
		        return default;
	        }

	        IReadOnlyCollection<Claim> claims = ResolveClaims(
		        authenticateCommand.Claims.ToList(),
		        authenticateCommand.AuthenticationSessionItems.ToDictionary(m => m.Key, m => m.Value),
		        authenticateCommand.Protector);

	        return new ClaimsPrincipal(CreateAuthenticatedClaimsIdentity(identity, claims, authenticateCommand.AuthenticationType));
        }

        protected abstract Task<TIdentity> GetIdentityAsync(TAuthenticateCommand authenticateCommand);

        protected abstract ClaimsIdentity CreateAuthenticatedClaimsIdentity(TIdentity identity, IReadOnlyCollection<Claim> claims, string authenticationType);

        protected virtual bool IsMatch(TAuthenticateCommand authenticateCommand, TIdentity identity)
        {
	        NullGuard.NotNull(authenticateCommand, nameof(authenticateCommand))
		        .NotNull(identity, nameof(identity));

	        return true;
        }

        private IReadOnlyCollection<Claim> ResolveClaims(IList<Claim> claims, IDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
        {
	        NullGuard.NotNull(claims, nameof(claims))
		        .NotNull(authenticationSessionItems, nameof(authenticationSessionItems))
		        .NotNull(protector, nameof(protector));

	        Claim externalTokenClaim = ResolveExternalTokenClaim(authenticationSessionItems, protector);
	        if (externalTokenClaim != null)
	        {
		        claims.Add(externalTokenClaim);
	        }

	        return claims.AsReadOnly();
        }

        private Claim ResolveExternalTokenClaim(IDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
        {
	        NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems))
		        .NotNull(protector, nameof(protector));

	        return _externalTokenClaimCreator.CanBuild(authenticationSessionItems)
		        ? _externalTokenClaimCreator.Build(authenticationSessionItems, protector)
		        : null;
        }

        #endregion
    }
}