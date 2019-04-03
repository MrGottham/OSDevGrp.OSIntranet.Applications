﻿using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
    public interface IClientSecretIdentity : IIdentity
    {
        int Identifier { get; }

        string FriendlyName { get; }

        string ClientId { get; }

        string ClientSecret { get; }

        IToken Token { get; }

        ClaimsIdentity ToClaimsIdentity();

        void ClearSensitiveData();

        void AddToken(IToken token);
    }
}
