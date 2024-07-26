using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationCode
    {
        string Value { get; }

        DateTime Expires { get; }

        bool Expired { get; }
    }
}