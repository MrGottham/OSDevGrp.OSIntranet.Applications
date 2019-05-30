using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IIdentityCommand : IIdentityIdentificationCommand
    {
        IEnumerable<Claim> Claims { get; set; }
    }
}