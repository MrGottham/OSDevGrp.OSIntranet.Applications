using System.Transactions;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;

namespace OSDevGrp.OSIntranet.Core.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        #region Properties

        public abstract TransactionScopeOption TransactionScopeOption { get; }

        public abstract TransactionOptions TransactionOptions { get; } 

        #endregion
    }
}