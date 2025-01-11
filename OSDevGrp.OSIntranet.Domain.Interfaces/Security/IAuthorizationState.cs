using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationState
    {
        string ResponseType { get; }

        string ClientId { get; }

        string ClientSecret { get; }

        Uri RedirectUri { get; }

        IEnumerable<string> Scopes { get; }

        string ExternalState { get; }

        string Nonce { get; }

        IAuthorizationCode AuthorizationCode { get; }

        string ToString(Func<byte[], byte[]> protector);

        IAuthorizationStateBuilder ToBuilder();

        Uri GenerateRedirectUriWithAuthorizationCode();
    }
}