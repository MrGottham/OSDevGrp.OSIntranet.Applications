using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IToken
    {
        string Value { get; }

        DateTime Expires { get; }
    }
}
