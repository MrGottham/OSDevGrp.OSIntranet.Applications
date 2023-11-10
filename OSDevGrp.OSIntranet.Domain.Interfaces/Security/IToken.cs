using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IToken
    {
        string TokenType { get; }

        string AccessToken { get; }

        DateTime Expires { get; }

        bool HasExpired { get; }

        byte[] ToByteArray();

        string ToBase64String();

        bool WillExpireWithin(TimeSpan timeSpan);
    }
}