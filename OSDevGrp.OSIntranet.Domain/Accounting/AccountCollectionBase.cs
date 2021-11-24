using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountCollectionBase<TAccount, TAccountCollection> : HashSet<TAccount>, IAccountCollectionBase<TAccount, TAccountCollection> where TAccount : IAccountBase<TAccount> where TAccountCollection : IAccountCollectionBase<TAccount>
    {
        #region Properties

        public DateTime StatusDate { get; private set; }

        #endregion

        #region Methods

        public new void Add(TAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            if (base.Add(account))
            {
                return;
            }

            throw new IntranetExceptionBuilder(ErrorCode.ObjectAlreadyExists, account.GetType().Name).Build(); 
        }

        public void Add(IEnumerable<TAccount> accountCollection)
        {
            NullGuard.NotNull(accountCollection, nameof(accountCollection));

            foreach(TAccount account in accountCollection)
            {
                Add(account);
            }
        }

        public async Task<TAccountCollection> CalculateAsync(DateTime statusDate)
        {
            if (statusDate.Date == StatusDate)
            {
                return AlreadyCalculated();
            }

            StatusDate = statusDate.Date;

            TAccount[] calculatedAccountCollection = await Task.WhenAll(this.AsParallel().Select(account => account.CalculateAsync(StatusDate)).ToArray());

            return Calculate(StatusDate, calculatedAccountCollection);
        }

        protected abstract TAccountCollection Calculate(DateTime statusDate, IEnumerable<TAccount> calculatedAccountCollection);

        protected abstract TAccountCollection AlreadyCalculated();

        #endregion
    }
}