using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class AuthorizationStateFactory : IAuthorizationStateFactory
    {
        #region Methods

        public IAuthorizationStateBuilder Create(string responseType, string clientId, Uri redirectUri, IEnumerable<string> scopes)
        {
            NullGuard.NotNullOrWhiteSpace(responseType, nameof(responseType))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNull(redirectUri, nameof(redirectUri))
                .NotNull(scopes, nameof(scopes));

            return new AuthorizationStateBuilder(responseType, clientId, redirectUri, scopes.ToArray());
        }

        public IAuthorizationState FromBase64String(string base64String, Func<byte[], byte[]> unprotect)
        {
            NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String))
                .NotNull(unprotect, nameof(unprotect));

            return AuthorizationState.FromBase64String(base64String, unprotect);
        }

        #endregion
    }
}