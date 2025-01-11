using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IGenerateAuthorizationCodeCommand : IAuthorizationStateCommand
    {
        IReadOnlyCollection<Claim> Claims { get; }

        IToken IdToken { get; }
    }
}