using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class AuthenticateAuthorizationCodeCommandHandler : AuthenticateCommandHandlerBase<IAuthenticateAuthorizationCodeCommand, IIdentity>
    {
        #region Private variables

        private readonly ICommonRepository _commonRepository;
        private readonly IAuthorizationDataConverter _authorizationDataConverter;
        private readonly ITrustedDomainResolver _trustedDomainResolver;

        #endregion

        #region Constructor

        public AuthenticateAuthorizationCodeCommandHandler(ISecurityRepository securityRepository, ICommonRepository commonRepository, IAuthorizationDataConverter authorizationDataConverter, ITrustedDomainResolver trustedDomainResolver, IExternalTokenClaimCreator externalTokenClaimCreator) 
            : base(securityRepository, externalTokenClaimCreator)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository))
                .NotNull(authorizationDataConverter, nameof(authorizationDataConverter))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver));

            _commonRepository = commonRepository;
            _authorizationDataConverter = authorizationDataConverter;
            _trustedDomainResolver = trustedDomainResolver;
        }

        #endregion

        #region Methods

        protected override async Task<IIdentity> GetIdentityAsync(IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand)
        {
            IKeyValueEntry keyValueEntry = await ResolveKeyValueEntryAsync(authenticateAuthorizationCodeCommand.AuthorizationCode);
            if (keyValueEntry == null)
            {
                return null;
            }

            IAuthorizationCode authorizationCode = await _authorizationDataConverter.ToAuthorizationCodeAsync(keyValueEntry, out IReadOnlyCollection<Claim> claims, out IReadOnlyDictionary<string, string> authorizationData);
            if (authorizationCode == null || authorizationCode.Expired || claims == null || claims.Any() == false || authorizationData == null || authorizationData.Any() == false)
            {
                return null;
            }

            return new AuthorizationCodeIdentity(claims, authorizationData);
        }

        protected override bool IsMatch(IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand, IIdentity identity)
        {
            NullGuard.NotNull(authenticateAuthorizationCodeCommand, nameof(authenticateAuthorizationCodeCommand))
                .NotNull(identity, nameof(identity));

            return IsMatch(authenticateAuthorizationCodeCommand, identity as AuthorizationCodeIdentity);
        }

        protected override ClaimsIdentity CreateAuthenticatedClaimsIdentity(IIdentity identity, IEnumerable<Claim> claims, string authenticationType)
        {
            NullGuard.NotNull(identity, nameof(identity))
                .NotNull(claims, nameof(claims))
                .NotNullOrWhiteSpace(authenticationType, nameof(authenticationType));

            return CreateAuthenticatedClaimsIdentity(identity as AuthorizationCodeIdentity, claims, authenticationType);
        }

        private async Task<IKeyValueEntry> ResolveKeyValueEntryAsync(string authorizationCode)
        {
            NullGuard.NotNullOrWhiteSpace(authorizationCode, nameof(authorizationCode));

            IKeyValueEntry keyValueEntry = await _commonRepository.PullKeyValueEntryAsync(authorizationCode);
            if (keyValueEntry == null)
            {
                return null;
            }

            await _commonRepository.DeleteKeyValueEntryAsync(authorizationCode);

            return keyValueEntry;
        }

        private bool IsMatch(IAuthenticateAuthorizationCodeCommand authenticateAuthorizationCodeCommand, AuthorizationCodeIdentity authorizationCodeIdentity)
        {
            NullGuard.NotNull(authenticateAuthorizationCodeCommand, nameof(authenticateAuthorizationCodeCommand))
                .NotNull(authorizationCodeIdentity, nameof(authorizationCodeIdentity));

            return authenticateAuthorizationCodeCommand.IsMatchAsync(authorizationCodeIdentity.AuthorizationData, SecurityRepository, _trustedDomainResolver)
                .GetAwaiter()
                .GetResult();
        }

        private static ClaimsIdentity CreateAuthenticatedClaimsIdentity(AuthorizationCodeIdentity authorizationCodeIdentity, IEnumerable<Claim> claims, string authenticationType)
        {
            NullGuard.NotNull(authorizationCodeIdentity, nameof(authorizationCodeIdentity))
                .NotNull(claims, nameof(claims))
                .NotNullOrWhiteSpace(authenticationType, nameof(authenticationType));

            return new ClaimsIdentity(authorizationCodeIdentity.Claims.Concat(claims), authenticationType);
        }

        #endregion

        #region Private classes

        internal class AuthorizationCodeIdentity : ClaimsIdentity
        {
            #region Constructor

            public AuthorizationCodeIdentity(IReadOnlyCollection<Claim> claims, IReadOnlyDictionary<string, string> authorizationData) 
                : base(claims)
            {
                NullGuard.NotNull(authorizationData, nameof(authorizationData));

                AuthorizationData = authorizationData;
            }

            #endregion

            #region Properties

            internal IReadOnlyDictionary<string, string> AuthorizationData { get; }

            #endregion
        }

        #endregion
    }
}