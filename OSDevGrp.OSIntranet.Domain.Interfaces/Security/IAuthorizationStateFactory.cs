using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationStateFactory
    {
        IAuthorizationStateBuilder Create(string responseType, string clientId, Uri redirectUri, IEnumerable<string> scopes);

        IAuthorizationState FromBase64String(string base64String, Func<byte[], byte[]> unprotect);
    }
}