using System.Transactions;

namespace OSDevGrp.OSIntranet.Core.CommandHandlers
{
    public abstract class CommandHandlerTransactionalBase : CommandHandlerBase
    {
        #region Properties

        public override TransactionScopeOption TransactionScopeOption
        {
            get => TransactionScopeOption.Required;
        }

        public override TransactionOptions TransactionOptions
        {
            get => new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        }

        #endregion
    }
}