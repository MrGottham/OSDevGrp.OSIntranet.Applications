using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class GenerateAuthorizationCodeCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<IGenerateAuthorizationCodeCommand, IAuthorizationState>
    {
        #region Private variables

        private readonly IValidator _validator;
        private readonly IAuthorizationStateFactory _authorizationStateFactory;
        private readonly ISecurityRepository _securityRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly ITrustedDomainResolver _trustedDomainResolver;
        private readonly ISupportedScopesProvider _supportedScopesProvider;
        private readonly IClaimsSelector _claimsSelector;
        private readonly IAuthorizationCodeGenerator _authorizationCodeGenerator;
        private readonly IAuthorizationDataConverter _authorizationDataConverter;

        #endregion

        #region Constructor

        public GenerateAuthorizationCodeCommandHandler(IValidator validator, IAuthorizationStateFactory authorizationStateFactory, ISecurityRepository securityRepository, ICommonRepository commonRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider, IClaimsSelector claimsSelector, IAuthorizationCodeGenerator authorizationCodeGenerator, IAuthorizationDataConverter authorizationDataConverter)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(authorizationStateFactory, nameof(authorizationStateFactory))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(commonRepository, nameof(commonRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider))
                .NotNull(claimsSelector, nameof(claimsSelector))
                .NotNull(authorizationCodeGenerator, nameof(authorizationCodeGenerator))
                .NotNull(authorizationDataConverter, nameof(authorizationDataConverter));

            _validator = validator;
            _authorizationStateFactory = authorizationStateFactory;
            _securityRepository = securityRepository;
            _commonRepository = commonRepository;
            _trustedDomainResolver = trustedDomainResolver;
            _supportedScopesProvider = supportedScopesProvider;
            _claimsSelector = claimsSelector;
            _authorizationCodeGenerator = authorizationCodeGenerator;
            _authorizationDataConverter = authorizationDataConverter;
        }

        #endregion

        #region Methods

        public async Task<IAuthorizationState> ExecuteAsync(IGenerateAuthorizationCodeCommand generateAuthorizationCodeCommand)
        {
            NullGuard.NotNull(generateAuthorizationCodeCommand, nameof(generateAuthorizationCodeCommand));

            generateAuthorizationCodeCommand.Validate(_validator);

            IAuthorizationState authorizationState = generateAuthorizationCodeCommand.ToDomain(_authorizationStateFactory, _validator, _securityRepository, _trustedDomainResolver, _supportedScopesProvider);

            string clientSecret = await ResolveClientSecretAsync(authorizationState.ClientId);
            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                return null;
            }

            IReadOnlyCollection<Claim> selectedClaims = await ResolveSelectedClaimsAsync(_supportedScopesProvider.SupportedScopes, authorizationState.Scopes, generateAuthorizationCodeCommand.Claims);
            if (selectedClaims == null || selectedClaims.Count == 0)
            {
                return null;
            }

            IAuthorizationCode authorizationCode = await _authorizationCodeGenerator.GenerateAsync();
            IKeyValueEntry keyValueEntry = await _authorizationDataConverter.ToKeyValueEntryAsync(authorizationCode, selectedClaims);
            await _commonRepository.PushKeyValueEntryAsync(keyValueEntry);

            return PopulateAuthorizationState(authorizationState, clientSecret, authorizationCode);
        }

        private async Task<string> ResolveClientSecretAsync(string clientId)
        {
            NullGuard.NotNullOrWhiteSpace(clientId, nameof(clientId));

            IClientSecretIdentity clientSecretIdentity = await _securityRepository.GetClientSecretIdentityAsync(clientId);
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