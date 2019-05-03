using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IToken
    {
        string TokenType { get; }

        string AccessToken { get; }

        DateTime Expires { get; }

        string ToBase64();
    }
}
