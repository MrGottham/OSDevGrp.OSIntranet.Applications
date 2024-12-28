using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IGenerateIdTokenCommand : ICommand
    {
        ClaimsIdentity ClaimsIdentity { get; }

        DateTimeOffset AuthenticationTime { get; }

        string Nonce { get; }
    }
}