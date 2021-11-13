using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class MapperCache
    {
        #region Properties

        internal IDictionary<int, IAccounting> AccountingDictionary { get; } = new ConcurrentDictionary<int, IAccounting>();

        internal IDictionary<int, IAccount> AccountDictionary { get; } = new ConcurrentDictionary<int, IAccount>();

        internal IDictionary<int, IBudgetAccount> BudgetAccountDictionary { get; } = new ConcurrentDictionary<int, IBudgetAccount>();

        internal IDictionary<int, IContactAccount> ContactAccountDictionary { get; } = new ConcurrentDictionary<int, IContactAccount>();

        internal IDictionary<Guid, IPostingLine> PostingLineDictionary { get; } = new ConcurrentDictionary<Guid, IPostingLine>();

        internal object SyncRoot { get; } = new object();

        #endregion
    }
}