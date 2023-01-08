using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class AccountCollectionBase<TAccount, TAccountCollection> : HashSet<TAccount>, IAccountCollectionBase<TAccount, TAccountCollection> where TAccount : IAccountBase<TAccount> where TAccountCollection : IAccountCollectionBase<TAccount>
    {
        #region Private variables

        private bool _isCalculating;

        #endregion

        #region Properties

        public DateTime StatusDate { get; private set; }

        public bool IsProtected { get; private set; }

        public bool Deletable => IsProtected == false && this.All(account => account.Deletable);

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
                while (_isCalculating)
                {
                    await Task.Delay(250);
                }

                return AlreadyCalculated();
            }

            StatusDate = statusDate.Date;

            _isCalculating = true;
            try
            {
                List<TAccount> calculatedAccountCollection = new List<TAccount>();
                foreach (TAccount account in this)
                {
                    calculatedAccountCollection.Add(await account.CalculateAsync(StatusDate));
                }

                return Calculate(StatusDate, calculatedAccountCollection.AsReadOnly());
            }
            finally
            {
                _isCalculating = false;
            }
        }

        public void ApplyProtection()
        {
            foreach (TAccount account in this)
            {
                account.ApplyProtection();
            }

            IsProtected = true;
        }

        public void AllowDeletion() => throw new NotSupportedException();

        public void DisallowDeletion() => throw new NotSupportedException();

        protected abstract TAccountCollection Calculate(DateTime statusDate, IReadOnlyCollection<TAccount> calculatedAccountCollection);

        protected abstract TAccountCollection AlreadyCalculated();

        #endregion
    }
}