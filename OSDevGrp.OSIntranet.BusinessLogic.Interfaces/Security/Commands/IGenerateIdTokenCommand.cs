using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IGenerateIdTokenCommand : IAuthorizationStateCommand
    {
        ClaimsPrincipal ClaimsPrincipal { get; }

        DateTimeOffset AuthenticationTime { get; }
    }
}