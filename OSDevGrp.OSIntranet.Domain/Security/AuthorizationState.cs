﻿using OSDevGrp.OSIntranet.Core;
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

        public AuthorizationState(string responseType, string clientId, string clientSecret, Uri redirectUri, string[] scopes, string externalState, IAuthorizationCode authorizationCode)
        {
            NullGuard.NotNullOrWhiteSpace(responseType, nameof(responseType))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(redirectUri, nameof(redirectUri))
                .NotNull(scopes, nameof(scopes));

            ResponseType = responseType.Trim();
            ClientId = clientId.Trim();
            ClientSecret = string.IsNullOrWhiteSpace(clientSecret) == false ? clientSecret.Trim() : null;
            RedirectUri = redirectUri;
            Scopes = scopes.Where(scope => string.IsNullOrWhiteSpace(scope) == false).ToArray();
            ExternalState = string.IsNullOrWhiteSpace(externalState) == false ? externalState : null;
            AuthorizationCode = authorizationCode;
        }

        #endregion

        #region Properties

        public string ResponseType { get; }

        public string ClientId { get; }

        public string ClientSecret { get; }

        public Uri RedirectUri { get; }

        public IEnumerable<string> Scopes { get; }

        public string ExternalState { get; }

        public IAuthorizationCode AuthorizationCode { get; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public override bool Equals(object obj)
        {
            return obj is IAuthorizationState && ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public string ToString(Func<byte[], byte[]> protector)
        {
            NullGuard.NotNull(protector, nameof(protector));

            return Convert.ToBase64String(protector(ToByteArray()));
        }

        public IAuthorizationStateBuilder ToBuilder()
        {
            IAuthorizationStateBuilder authorizationStateBuilder = new AuthorizationStateBuilder(ResponseType, ClientId, RedirectUri, Scopes.ToArray());
            if (string.IsNullOrWhiteSpace(ClientSecret) == false)
            {
                authorizationStateBuilder.WithClientSecret(ClientSecret);
            }
            if (string.IsNullOrWhiteSpace(ExternalState) == false)
            {
                authorizationStateBuilder.WithExternalState(ExternalState);
            }
            if (AuthorizationCode != null && string.IsNullOrWhiteSpace(AuthorizationCode.Value) == false)
            {
                authorizationStateBuilder.WithAuthorizationCode(AuthorizationCode.Value, AuthorizationCode.Expires);
            }
            return authorizationStateBuilder;
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
                ClientSecret = ClientSecret,
                RedirectUri = RedirectUri.AbsoluteUri,
                Scopes = Scopes.ToArray(),
                ExternalState = string.IsNullOrWhiteSpace(ExternalState) == false ? ExternalState : null,
                AuthorizationCode = AuthorizationCode != null
                    ? new LocalAuthorizationCode
                    {
                        Value = AuthorizationCode.Value,
                        Expires = AuthorizationCode.Expires
                    }
                    : null
            };

            return DomainHelper.ToByteArray(localAuthorizationState);
        }

        private static IAuthorizationState FromByteArray(byte[] bytes)
        {
            NullGuard.NotNull(bytes, nameof(bytes));

            LocalAuthorizationState localAuthorizationState = DomainHelper.FromByteArray<LocalAuthorizationState>(bytes);

            IAuthorizationStateBuilder authorizationStateBuilder = new AuthorizationStateBuilder(localAuthorizationState.ResponseType, localAuthorizationState.ClientId, new Uri(localAuthorizationState.RedirectUri, UriKind.Absolute), localAuthorizationState.Scopes);
            if (string.IsNullOrWhiteSpace(localAuthorizationState.ClientSecret) == false)
            {
                authorizationStateBuilder.WithClientSecret(localAuthorizationState.ClientSecret);
            }
            if (string.IsNullOrWhiteSpace(localAuthorizationState.ExternalState) == false)
            {
                authorizationStateBuilder.WithExternalState(localAuthorizationState.ExternalState);
            }
            if (localAuthorizationState.AuthorizationCode != null && string.IsNullOrWhiteSpace(localAuthorizationState.AuthorizationCode.Value) == false)
            {
                authorizationStateBuilder.WithAuthorizationCode(localAuthorizationState.AuthorizationCode.Value, localAuthorizationState.AuthorizationCode.Expires);
            }
            return authorizationStateBuilder.Build();
        }

        #endregion

        #region Private classes

        private class LocalAuthorizationState
        {
            public string ResponseType { get; init; }

            public string ClientId { get; init; }

            public string ClientSecret { get; init; }

            public string RedirectUri { get; init; }

            public string[] Scopes { get; init; }

            public string ExternalState { get; init; }

            public LocalAuthorizationCode AuthorizationCode { get; init; }
        }

        private class LocalAuthorizationCode
        {
            public string Value { get; init; }

            public DateTimeOffset Expires { get; init; }
        }

        #endregion
    }
}