using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class GenerateAuthorizationCodeCommandHandler : AuthorizationStateCommandHandlerBase<IGenerateAuthorizationCodeCommand, IAuthorizationState>
    {
        #region Private variables

        private readonly ICommonRepository _commonRepository;
        private readonly IClaimsSelector _claimsSelector;
        private readonly IAuthorizationCodeGenerator _authorizationCodeGenerator;
        private readonly IAuthorizationDataConverter _authorizationDataConverter;

        #endregion

        #region Constructor

        public GenerateAuthorizationCodeCommandHandler(IValidator validator, IAuthorizationStateFactory authorizationStateFactory, ISecurityRepository securityRepository, ICommonRepository commonRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider, IClaimsSelector claimsSelector, IAuthorizationCodeGenerator authorizationCodeGenerator, IAuthorizationDataConverter authorizationDataConverter) 
            : base(validator, authorizationStateFactory, securityRepository, trustedDomainResolver, supportedScopesProvider)
        {
            NullGuard.NotNull(commonRepository, nameof(commonRepository))
                .NotNull(claimsSelector, nameof(claimsSelector))
                .NotNull(authorizationCodeGenerator, nameof(authorizationCodeGenerator))
                .NotNull(authorizationDataConverter, nameof(authorizationDataConverter));

            _commonRepository = commonRepository;
            _claimsSelector = claimsSelector;
            _authorizationCodeGenerator = authorizationCodeGenerator;
            _authorizationDataConverter = authorizationDataConverter;
        }

        #endregion

        #region Methods

        protected override async Task<IAuthorizationState> HandleAuthorizationStateAsync(IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand, IAuthorizationState authorizationState)
        {
            NullGuard.NotNull(generateAuthorizationCodeCommand, nameof(generateAuthorizationCodeCommand))
                .NotNull(authorizationState, nameof(authorizationState));

            string clientId = authorizationState.ClientId;
            string clientSecret = await ResolveClientSecretAsync(clientId);
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                return null;
            }

            IReadOnlyCollection<Claim> selectedClaims = await ResolveSelectedClaimsAsync(SupportedScopesProvider.SupportedScopes, authorizationState.Scopes, generateAuthorizationCodeCommand.Claims);
            if (selectedClaims == null || selectedClaims.Count == 0)
            {
                return null;
            }

            IReadOnlyDictionary<string, string> authorizationData = await ResolveAuthorizationData(clientId, clientSecret, authorizationState.RedirectUri, generateAuthorizationCodeCommand.IdToken);

            IAuthorizationCode authorizationCode = await _authorizationCodeGenerator.GenerateAsync();
            IKeyValueEntry keyValueEntry = await _authorizationDataConverter.ToKeyValueEntryAsync(authorizationCode, selectedClaims, authorizationData);
            await _commonRepository.PushKeyValueEntryAsync(keyValueEntry);

            return PopulateAuthorizationState(authorizationState, clientSecret, authorizationCode);
        }

        private async Task<string> ResolveClientSecretAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            IClientSecretIdentity clientSecretIdentity = await SecurityRepository.GetClientSecretIdentityAsync(clientId);
            if (clientSecretIdentity == null || string.IsNullOrWhiteSpace(clientSecretIdentity.ClientSecret))
            {
                return null;
            }

            return clientSecretIdentity.ClientSecret;
        }

        private Task<IReadOnlyCollection<Claim>> ResolveSelectedClaimsAsync(IReadOnlyDictionary<string, IScope> supportedScopes, IEnumerable<string> scopes, IReadOnlyCollection<Claim> claims)
        {
            NullGuard.NotNull(supportedScopes, nameof(supportedScopes))
                .NotNull(scopes, nameof(scopes))
                .NotNull(claims, nameof(claims));

            return Task.FromResult(_claimsSelector.Select(supportedScopes, scopes, claims));
        }

        private Task<IReadOnlyDictionary<string, string>> ResolveAuthorizationData(string clientId, string clientSecret, Uri redirectUri, IToken idToken)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret))
                .NotNull(redirectUri, nameof(redirectUri))
                .NotNull(idToken, nameof(idToken));

            IDictionary<string, string> authorizationData = new Dictionary<string, string>
            {
                {AuthorizationDataConverter.ClientIdKey, clientId},
                {AuthorizationDataConverter.ClientSecretKey, clientSecret},
                {AuthorizationDataConverter.RedirectUriKey, redirectUri.AbsoluteUri},
                {AuthorizationDataConverter.IdTokenKey, idToken.ToBase64String()}
            };

            return Task.FromResult<IReadOnlyDictionary<string, string>>(authorizationData.AsReadOnly());
        }

        private IAuthorizationState PopulateAuthorizationState(IAuthorizationState authorizationState, string clientSecret, IAuthorizationCode authorizationCode)
        {
            NullGuard.NotNull(authorizationState, nameof(authorizationState))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret))
                .NotNull(authorizationCode, nameof(authorizationCode));

            return authorizationState.ToBuilder()
                .WithClientSecret(clientSecret)
                .WithAuthorizationCode(authorizationCode)
                .Build();
        }

        #endregion
    }
}