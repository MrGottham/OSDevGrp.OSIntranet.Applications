using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class GenerateIdTokenCommandHandler : AuthorizationStateCommandHandlerBase<IGenerateIdTokenCommand, IToken>
    {
        #region Private variables

        private readonly IUserInfoFactory _userInfoFactory;
        private readonly IIdTokenContentFactory _idTokenContentFactory;
        private readonly ITokenGenerator _tokenGenerator;

        #endregion

        #region Constructor

        public GenerateIdTokenCommandHandler(IValidator validator, IAuthorizationStateFactory authorizationStateFactory, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider, IUserInfoFactory userInfoFactory, IIdTokenContentFactory idTokenContentFactory, ITokenGenerator tokenGenerator)
            : base(validator, authorizationStateFactory, securityRepository, trustedDomainResolver, supportedScopesProvider)
        {
            NullGuard.NotNull(userInfoFactory, nameof(userInfoFactory))
                .NotNull(idTokenContentFactory, nameof(idTokenContentFactory))
                .NotNull(tokenGenerator, nameof(tokenGenerator));

            _userInfoFactory = userInfoFactory;
            _idTokenContentFactory = idTokenContentFactory;
            _tokenGenerator = tokenGenerator;
        }

        #endregion

        #region Methods

        protected override async Task<IToken> HandleAuthorizationStateAsync(IGenerateIdTokenCommand generateIdTokenCommand, IAuthorizationState authorizationState)
        {
            NullGuard.NotNull(generateIdTokenCommand, nameof(generateIdTokenCommand))
                .NotNull(authorizationState, nameof(authorizationState));

            ClaimsPrincipal claimsPrincipal = generateIdTokenCommand.ClaimsPrincipal;

            string nameIdentifier = await ResolveNameIdentifierAsync(claimsPrincipal);
            IUserInfo userInfo = await ResolveUserInfoAsync(claimsPrincipal);

            IReadOnlyDictionary<string, IScope> supportedScopes = SupportedScopesProvider.SupportedScopes;

            IIdTokenContentBuilder idTokenContentBuilder = _idTokenContentFactory.Create(nameIdentifier, userInfo, generateIdTokenCommand.AuthenticationTime, supportedScopes, authorizationState.Scopes.ToArray());

            string nonce = authorizationState.Nonce;
            if (string.IsNullOrWhiteSpace(nonce) == false)
            {
                idTokenContentBuilder = idTokenContentBuilder.WithNonce(nonce);
            }

            idTokenContentBuilder = idTokenContentBuilder.WithCustomClaimsFilteredByScope(supportedScopes[ScopeHelper.WebApiScope], claimsPrincipal.Claims)
                .WithCustomClaimsFilteredByClaimType(ClaimHelper.MicrosoftTokenClaimType, claimsPrincipal.Claims)
                .WithCustomClaimsFilteredByClaimType(ClaimHelper.GoogleTokenClaimType, claimsPrincipal.Claims);

            return _tokenGenerator.Generate(new ClaimsIdentity(idTokenContentBuilder.Build()), TimeSpan.FromMinutes(5), authorizationState.ClientId);
        }

        private Task<IUserInfo> ResolveUserInfoAsync(ClaimsPrincipal claimsPrincipal)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal));

            IUserInfo userInfo = _userInfoFactory.FromPrincipal(claimsPrincipal);
            if (userInfo == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser).Build();
            }

            return Task.FromResult(userInfo);
        }

        private static Task<string> ResolveNameIdentifierAsync(ClaimsPrincipal claimsPrincipal)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal));

            Claim nameIdentifierClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null || string.IsNullOrWhiteSpace(nameIdentifierClaim.Value))
            {
                throw new IntranetExceptionBuilder(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser).Build();
            }

            return Task.FromResult(nameIdentifierClaim.Value);
        }

        #endregion
    }
}