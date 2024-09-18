using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class AuthorizationStateBuilder : IAuthorizationStateBuilder
    {
        #region Private variables

        private readonly string _responseType;
        private readonly string _clientId;
        private readonly Uri _redirectUri;
        private readonly string[] _scopes;
        private string _clientSecret;
        private string _externalState;
        private IAuthorizationCode _authorizationCode;

        #endregion

        #region Construtor

        public AuthorizationStateBuilder(string responseType, string clientId, Uri redirectUri, string[] scopes)
        {
            NullGuard.NotNullOrWhiteSpace(responseType, nameof(responseType))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(redirectUri, nameof(redirectUri))
                .NotNull(scopes, nameof(scopes));

            _responseType = responseType;
            _clientId = clientId;
            _redirectUri = redirectUri;
            _scopes = scopes;
        }

        #endregion

        #region Methods

        public IAuthorizationStateBuilder WithClientSecret(string clientSecret)
        {
            NullGuard.NotNullOrWhiteSpace(clientSecret, nameof(clientSecret));

            _clientSecret = clientSecret;

            return this;
        }

        public IAuthorizationStateBuilder WithExternalState(string externalState)
        {
            NullGuard.NotNullOrWhiteSpace(externalState, nameof(externalState));

            _externalState = externalState;

            return this;
        }

        public IAuthorizationStateBuilder WithAuthorizationCode(IAuthorizationCode authorizationCode)
        {
            NullGuard.NotNull(authorizationCode, nameof(authorizationCode));

            return WithAuthorizationCode(authorizationCode.Value, authorizationCode.Expires);
        }

        public IAuthorizationStateBuilder WithAuthorizationCode(string value, DateTimeOffset expires)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            _authorizationCode = new AuthorizationCode(value, expires);

            return this;
        }

        public IAuthorizationState Build()
        {
            return new AuthorizationState(_responseType, _clientId, _clientSecret, _redirectUri, _scopes, _externalState, _authorizationCode);
        }

        #endregion
    }
}