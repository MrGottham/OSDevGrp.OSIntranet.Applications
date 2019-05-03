using System;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IRefreshTokenForMicrosoftGraphCommand : ICommand
    {
        Uri RedirectUri { get; }

        IRefreshableToken RefreshableToken { get; }
    }
}
