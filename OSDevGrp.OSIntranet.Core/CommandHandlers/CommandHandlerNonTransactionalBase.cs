using System.Transactions;

namespace OSDevGrp.OSIntranet.Core.CommandHandlers
{
    public abstract class CommandHandlerNonTransactionalBase : CommandHandlerBase
    {
        #region Properties

        public override TransactionScopeOption TransactionScopeOption
        {
            get => TransactionScopeOption.Suppress;
        }

        public override TransactionOptions TransactionOptions
        {
            get => new TransactionOptions();
        }

        #endregion
    }
}