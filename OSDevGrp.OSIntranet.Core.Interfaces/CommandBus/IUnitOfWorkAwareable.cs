using System.Transactions;

namespace OSDevGrp.OSIntranet.Core.Interfaces.CommandBus
{
    public interface IUnitOfWorkAwareable
    {
        TransactionScopeOption TransactionScopeOption { get; }

        TransactionOptions TransactionOptions { get; }
    }
}