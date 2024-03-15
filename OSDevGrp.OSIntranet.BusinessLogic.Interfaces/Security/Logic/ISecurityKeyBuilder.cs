using Microsoft.IdentityModel.Tokens;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface ISecurityKeyBuilder : IDisposable
    {
        SecurityKey Build();
    }
}