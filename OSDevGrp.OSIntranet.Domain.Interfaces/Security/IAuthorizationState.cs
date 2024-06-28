using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationState
    {
        string ResponseType { get; }

        string ClientId { get; }

        Uri RedirectUri { get; }

        IEnumerable<string> Scopes { get; }

        string ExternalState { get; }

        string ToString(Func<byte[], byte[]> protector);
    }
}