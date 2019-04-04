using System.Collections.Generic;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IAuthenticateUserCommand : ICommand
    {
        string ExternalUserIdentifier { get; }

        IEnumerable<Claim> Claims { get; }
    }
}
