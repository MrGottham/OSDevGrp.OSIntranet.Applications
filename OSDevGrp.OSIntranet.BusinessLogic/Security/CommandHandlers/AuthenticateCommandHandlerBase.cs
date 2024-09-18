using AutoMapper.Internal;
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
    internal abstract class AuthenticateCommandHandlerBase<TAuthenticateCommand, TIdentity> : CommandHandlerTransactionalBase, ICommandHandler<TAuthenticateCommand, ClaimsPrincipal> where TAuthenticateCommand : IAuthenticateCommand where TIdentity : IIdentity
    {
        #region Constructor

        protected AuthenticateCommandHandlerBase(ISecurityRepository securityRepository, IExternalTokenClaimCreator externalTokenClaimCreator)
        {
	        NullGuard.NotNull(securityRepository, nameof(securityRepository))
		        .NotNull(externalTokenClaimCreator, nameof(externalTokenClaimCreator));

            SecurityRepository = securityRepository;
            ExternalTokenClaimCreator = externalTokenClaimCreator;
        }

		#endregion

		#region Properties

		protected ISecurityRepository SecurityRepository { get; }

		protected IExternalTokenClaimCreator ExternalTokenClaimCreator { get; }

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

	        IEnumerable<Claim> claims = CombineClaims(authenticateCommand.Claims, ResolveExternalTokenClaim(authenticateCommand.AuthenticationSessionItems,  authenticateCommand.Protector));

	        return new ClaimsPrincipal(CreateAuthenticatedClaimsIdentity(identity, claims, authenticateCommand.AuthenticationType));
        }

        protected abstract Task<TIdentity> GetIdentityAsync(TAuthenticateCommand authenticateCommand);

        protected abstract ClaimsIdentity CreateAuthenticatedClaimsIdentity(TIdentity identity, IEnumerable<Claim> claims, string authenticationType);

        protected virtual bool IsMatch(TAuthenticateCommand authenticateCommand, TIdentity identity)
        {
	        NullGuard.NotNull(authenticateCommand, nameof(authenticateCommand))
		        .NotNull(identity, nameof(identity));

	        return true;
        }

        private Claim ResolveExternalTokenClaim(IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector)
        {
	        NullGuard.NotNull(authenticationSessionItems, nameof(authenticationSessionItems))
		        .NotNull(protector, nameof(protector));

	        return ExternalTokenClaimCreator.CanBuild(authenticationSessionItems)
		        ? ExternalTokenClaimCreator.Build(authenticationSessionItems, protector)
		        : null;
        }

        private static IEnumerable<Claim> CombineClaims(IReadOnlyCollection<Claim> claims, Claim externalTokenClaim)
        {
	        NullGuard.NotNull(claims, nameof(claims));

	        return externalTokenClaim == null
		        ? claims
		        : claims.Concat(new[] { externalTokenClaim }).ToArray();
        }

        #endregion
    }
}