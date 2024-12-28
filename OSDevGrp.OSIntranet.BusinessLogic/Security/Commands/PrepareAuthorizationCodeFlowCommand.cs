using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal class PrepareAuthorizationCodeFlowCommand : IPrepareAuthorizationCodeFlowCommand
    {
        #region Constructor

        public PrepareAuthorizationCodeFlowCommand(string responseType, string clientId, Uri redirectUri, string[] scopes, string state, string nonce, Func<byte[], byte[]> protector)
        {
            NullGuard.NotNullOrWhiteSpace(responseType, nameof(responseType))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(redirectUri, nameof(redirectUri))
                .NotNull(scopes, nameof(scopes))
                .NotNull(protector, nameof(protector));

            ResponseType = responseType;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Scopes = scopes;
            State = string.IsNullOrWhiteSpace(state) ? null : state;
            Nonce = string.IsNullOrWhiteSpace(nonce) ? null : nonce;
            Protector = protector;
        }

        #endregion

        #region Properties

        public string ResponseType { get; }

        public string ClientId { get; }

        public Uri RedirectUri { get; }

        public IEnumerable<string> Scopes { get; }

        public string State { get; }

        public string Nonce { get; }

        public Func<byte[], byte[]> Protector { get; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver))
                .NotNull(supportedScopesProvider, nameof(supportedScopesProvider));

            return validator.ValidateResponseType(ResponseType, ResponseTypeHelper.ResponseTypeForAuthorizationCodeFlowRegex, GetType(), nameof(ResponseType))
                .ValidateClientId(ClientId, securityRepository, GetType(), nameof(ClientId))
                .ValidateRedirectUri(RedirectUri, trustedDomainResolver, GetType(), nameof(RedirectUri))
                .ValidateScopes(Scopes, supportedScopesProvider, GetType(), nameof(Scopes))
                .ValidateState(State, GetType(), nameof(State), true)
                .ValidateNonce(Nonce, GetType(), nameof(Nonce), true)
                .Object.ShouldNotBeNull(Protector, GetType(), nameof(Protector));
        }

        public IAuthorizationState ToDomain(IAuthorizationStateFactory authorizationStateFactory)
        {
            NullGuard.NotNull(authorizationStateFactory, nameof(authorizationStateFactory));

            IAuthorizationStateBuilder authorizationStateBuilder = authorizationStateFactory.Create(ResponseType, ClientId, RedirectUri, Scopes);
            if (string.IsNullOrWhiteSpace(State) == false)
            {
                authorizationStateBuilder = authorizationStateBuilder.WithExternalState(State);
            }
            if (string.IsNullOrWhiteSpace(Nonce) == false)
            {
                authorizationStateBuilder = authorizationStateBuilder.WithNonce(Nonce);
            }

            return authorizationStateBuilder.Build();
        }

        #endregion
    }
}