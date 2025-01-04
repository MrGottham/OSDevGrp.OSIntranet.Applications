using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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

            string subjectIdentifier = await ResolveSubjectIdentifierAsync(claimsPrincipal);
            IUserInfo userInfo = await ResolveUserInfoAsync(claimsPrincipal);

            IIdTokenContentBuilder idTokenContentBuilder = _idTokenContentFactory.Create(subjectIdentifier, userInfo, generateIdTokenCommand.AuthenticationTime);

            string nonce = authorizationState.Nonce;
            if (string.IsNullOrWhiteSpace(nonce) == false)
            {
                idTokenContentBuilder = idTokenContentBuilder.WithNonce(nonce);
            }

            foreach (Claim claim in await ResolveClaimsSupportedByScope(claimsPrincipal, SupportedScopesProvider.SupportedScopes[ScopeHelper.WebApiScope]))
            {
                idTokenContentBuilder = idTokenContentBuilder.WithCustomClaim(claim.Type, claim.Value);
            }

            return _tokenGenerator.Generate(new ClaimsIdentity(idTokenContentBuilder.Build()), TimeSpan.FromMinutes(5));
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

        private static Task<string> ResolveSubjectIdentifierAsync(ClaimsPrincipal claimsPrincipal)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal));

            Claim nameIdentifierClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null || string.IsNullOrWhiteSpace(nameIdentifierClaim.Value))
            {
                throw new IntranetExceptionBuilder(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser).Build();
            }

            return Task.FromResult(nameIdentifierClaim.Value.ComputeSha512Hash());
        }

        private static Task<IEnumerable<Claim>> ResolveClaimsSupportedByScope(ClaimsPrincipal claimsPrincipal, IScope supportedScope)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal))
                .NotNull(supportedScope, nameof(supportedScope));

            return Task.FromResult(supportedScope.Filter(claimsPrincipal.Claims));
        }

        #endregion
    }
}