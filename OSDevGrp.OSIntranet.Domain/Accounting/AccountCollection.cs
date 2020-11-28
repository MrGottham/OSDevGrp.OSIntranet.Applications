using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public class AccountCollection : AccountCollectionBase<IAccount, IAccountCollection>, IAccountCollection
    {
        #region Properties
        public IAccountCollectionValues ValuesAtStatusDate { get; private set; } = new AccountCollectionValues(0M, 0M);

        public IAccountCollectionValues ValuesAtEndOfLastMonthFromStatusDate { get; private set; } = new AccountCollectionValues(0M, 0M);

        public IAccountCollectionValues ValuesAtEndOfLastYearFromStatusDate { get; private set; } = new AccountCollectionValues(0M, 0M);

        #endregion

        #region Methods

        protected override IAccountCollection Calculate(DateTime statusDate, IEnumerable<IAccount> calculatedAccountCollection)
        {
            NullGuard.NotNull(calculatedAccountCollection, nameof(calculatedAccountCollection));

            IDictionary<AccountGroupType, IEnumerable<IAccount>> accountGroupDictionary = calculatedAccountCollection
                .GroupBy(calculatedAccount => calculatedAccount.AccountGroupType, calculatedAccount => calculatedAccount)
                .ToDictionary(group => group.Key, group => group.AsEnumerable());

            IAccount[] assetAccountCollection = accountGroupDictionary.ContainsKey(AccountGroupType.Assets)
                ? accountGroupDictionary[AccountGroupType.Assets].ToArray()
                : Array.Empty<IAccount>();
            IAccount[] liabilityAccountCollection = accountGroupDictionary.ContainsKey(AccountGroupType.Liabilities)
                ? accountGroupDictionary[AccountGroupType.Liabilities].ToArray()
                : Array.Empty<IAccount>();

            ValuesAtStatusDate = ToAccountCollectionValues(assetAccountCollection, liabilityAccountCollection, account => account.ValuesAtStatusDate);
            ValuesAtEndOfLastMonthFromStatusDate = ToAccountCollectionValues(assetAccountCollection, liabilityAccountCollection, account => account.ValuesAtEndOfLastMonthFromStatusDate);
            ValuesAtEndOfLastYearFromStatusDate = ToAccountCollectionValues(assetAccountCollection, liabilityAccountCollection, account => account.ValuesAtEndOfLastYearFromStatusDate);

            return this;
        }

        private static IAccountCollectionValues ToAccountCollectionValues(IEnumerable<IAccount> assetAccountCollection, IEnumerable<IAccount> liabilityAccountCollection, Func<IAccount, ICreditInfoValues> selector)
        {
            NullGuard.NotNull(assetAccountCollection, nameof(assetAccountCollection))
                .NotNull(liabilityAccountCollection, nameof(liabilityAccountCollection))
                .NotNull(selector, nameof(selector));

            return new AccountCollectionValues(
                assetAccountCollection.AsParallel().Sum(assetAccount => selector(assetAccount).Balance),
                liabilityAccountCollection.AsParallel().Sum(liabilityAccount => selector(liabilityAccount).Balance));
        }

        #endregion
    } 
}