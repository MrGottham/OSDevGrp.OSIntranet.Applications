using System;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IAcquireTokenForMicrosoftGraphCommand : ICommand
    {
        Uri RedirectUri { get; }

        string Code { get; }
    }
}
