using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class AuthorizationState : IAuthorizationState
    {
        #region Constructor

        public AuthorizationState(string responseType, string clientId, Uri redirectUri, string[] scopes, string externalState)
        {
            NullGuard.NotNullOrWhiteSpace(responseType, nameof(responseType))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(redirectUri, nameof(redirectUri))
                .NotNull(scopes, nameof(scopes));

            ResponseType = responseType.Trim();
            ClientId = clientId.Trim();
            RedirectUri = redirectUri;
            Scopes = scopes.Where(scope => string.IsNullOrWhiteSpace(scope) == false).ToArray();
            ExternalState = string.IsNullOrWhiteSpace(externalState) == false ? externalState : null;
        }

        #endregion

        #region Properties

        public string ResponseType { get; }

        public string ClientId { get; }

        public Uri RedirectUri { get; }

        public IEnumerable<string> Scopes { get; }

        public string ExternalState { get; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public string ToString(Func<byte[], byte[]> protector)
        {
            NullGuard.NotNull(protector, nameof(protector));

            return Convert.ToBase64String(protector(ToByteArray()));
        }

        internal static IAuthorizationState FromBase64String(string base64String, Func<byte[], byte[]> unprotect)
        {
            NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String))
                .NotNull(unprotect, nameof(unprotect));

            return FromByteArray(unprotect(Convert.FromBase64String(base64String)));
        }

        private byte[] ToByteArray()
        {
            LocalAuthorizationState localAuthorizationState = new LocalAuthorizationState
            {
                ResponseType = ResponseType,
                ClientId = ClientId,
                RedirectUri = RedirectUri.AbsoluteUri,
                Scopes = Scopes.ToArray(),
                ExternalState = string.IsNullOrWhiteSpace(ExternalState) == false ? ExternalState : null
            };

            return DomainHelper.ToByteArray(localAuthorizationState);
        }

        private static IAuthorizationState FromByteArray(byte[] bytes)
        {
            NullGuard.NotNull(bytes, nameof(bytes));

            LocalAuthorizationState localAuthorizationState = DomainHelper.FromByteArray<LocalAuthorizationState>(bytes);

            IAuthorizationStateBuilder authorizationStateBuilder = new AuthorizationStateBuilder(localAuthorizationState.ResponseType, localAuthorizationState.ClientId, new Uri(localAuthorizationState.RedirectUri, UriKind.Absolute), localAuthorizationState.Scopes);
            if (string.IsNullOrWhiteSpace(localAuthorizationState.ExternalState) == false)
            {
                authorizationStateBuilder.WithExternalState(localAuthorizationState.ExternalState);
            }

            return authorizationStateBuilder.Build();
        }

        #endregion

        #region Private classes

        private class LocalAuthorizationState
        {
            public string ResponseType { get; init; }

            public string ClientId { get; init; }

            public string RedirectUri { get; init; }

            public string[] Scopes { get; init; }

            public string ExternalState { get; init; }
        }

        #endregion
    }
}