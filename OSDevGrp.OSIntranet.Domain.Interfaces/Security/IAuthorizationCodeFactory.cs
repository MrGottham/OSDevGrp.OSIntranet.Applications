using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IAuthorizationCodeFactory
    {
        IAuthorizationCodeBuilder Create(string value, DateTimeOffset expires);
    }
}