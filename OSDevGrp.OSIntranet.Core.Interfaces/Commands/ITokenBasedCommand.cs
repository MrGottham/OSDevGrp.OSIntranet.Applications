using System;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Commands
{
    public interface ITokenBasedCommand : ICommand
    {
        string TokenType { get; set; }

        string AccessToken { get; set; }

        DateTime Expires { get; set; }
    }
}