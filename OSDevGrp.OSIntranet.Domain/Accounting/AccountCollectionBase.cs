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
        #region Constructor

        protected AccountCollectionBase()
        {
        }

        #endregion

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
                this.Add(account);
            }
        }

        public async Task<TAccountCollection> CalculateAsync(DateTime statusDate)
        {
            StatusDate = statusDate.Date;

            await Task.WhenAll(this.Select(account => account.CalculateAsync(StatusDate)).ToArray());

            return Calculate(StatusDate);
        }

        protected abstract TAccountCollection Calculate(DateTime statusDate);

        #endregion
    }
}