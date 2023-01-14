using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountCollectionBase<TAccount> : IEnumerable<TAccount>, ICalculable, IProtectable where TAccount : IAccountBase<TAccount>
    {
        void Add(TAccount account);

        void Add(IEnumerable<TAccount> accountCollection);
    }

    public interface IAccountCollectionBase<TAccount, TAccountCollection> : IAccountCollectionBase<TAccount>, ICalculable<TAccountCollection> where TAccount : IAccountBase<TAccount> where TAccountCollection : IAccountCollectionBase<TAccount>
    {
    }
}