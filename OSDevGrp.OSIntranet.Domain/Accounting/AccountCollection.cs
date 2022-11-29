using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<IAccountGroupStatus>> GroupByAccountGroupAsync()
        {
            Task<IAccountGroupStatus>[] groupCalculationTasks = this.AsParallel()
                .GroupBy(account => account.AccountGroup.Number, account => account)
                .Select(group =>
                {
                    IAccountGroup accountGroup = group.First().AccountGroup;
                    IAccountCollection accountCollection = new AccountCollection
                    {
                        group.AsEnumerable().AsParallel().OrderBy(account => account.AccountNumber).ToArray()
                    };

                    return accountGroup.CalculateAsync(StatusDate, accountCollection);
                })
                .ToArray();

            IAccountGroupStatus[] accountGroupStatusCollection = await Task.WhenAll(groupCalculationTasks);

            return accountGroupStatusCollection.OrderBy(accountGroupStatus => accountGroupStatus.Number).ToArray();
        }

        protected override IAccountCollection Calculate(DateTime statusDate, IReadOnlyCollection<IAccount> calculatedAccountCollection)
        {
            NullGuard.NotNull(calculatedAccountCollection, nameof(calculatedAccountCollection));

            IReadOnlyDictionary<AccountGroupType, IEnumerable<IAccount>> accountGroupDictionary = GroupByAccountGroupType(calculatedAccountCollection);

            IReadOnlyCollection<IAccount> assetAccountCollection = ResolveAssets(accountGroupDictionary);
            IReadOnlyCollection<IAccount> liabilityAccountCollection = ResolveLiabilities(accountGroupDictionary);

            ValuesAtStatusDate = ToAccountCollectionValues(assetAccountCollection, liabilityAccountCollection, account => account.ValuesAtStatusDate);
            ValuesAtEndOfLastMonthFromStatusDate = ToAccountCollectionValues(assetAccountCollection, liabilityAccountCollection, account => account.ValuesAtEndOfLastMonthFromStatusDate);
            ValuesAtEndOfLastYearFromStatusDate = ToAccountCollectionValues(assetAccountCollection, liabilityAccountCollection, account => account.ValuesAtEndOfLastYearFromStatusDate);

            return this;
        }

        protected override IAccountCollection AlreadyCalculated() => this;

        private static IReadOnlyDictionary<AccountGroupType, IEnumerable<IAccount>> GroupByAccountGroupType(IEnumerable<IAccount> accountCollection)
        {
            NullGuard.NotNull(accountCollection, nameof(accountCollection));

            return new ReadOnlyDictionary<AccountGroupType, IEnumerable<IAccount>>(accountCollection.AsParallel()
                .GroupBy(account => account.AccountGroupType, account => account)
                .ToDictionary(group => group.Key, group => group.AsEnumerable()));
        }

        private static IReadOnlyCollection<IAccount> ResolveAssets(IReadOnlyDictionary<AccountGroupType, IEnumerable<IAccount>> accountGroupDictionary)
        {
            NullGuard.NotNull(accountGroupDictionary, nameof(accountGroupDictionary));

            return accountGroupDictionary.ContainsKey(AccountGroupType.Assets)
                ? accountGroupDictionary[AccountGroupType.Assets].ToArray()
                : Array.Empty<IAccount>();
        }

        private static IReadOnlyCollection<IAccount> ResolveLiabilities(IReadOnlyDictionary<AccountGroupType, IEnumerable<IAccount>> accountGroupDictionary)
        {
            NullGuard.NotNull(accountGroupDictionary, nameof(accountGroupDictionary));

            return accountGroupDictionary.ContainsKey(AccountGroupType.Liabilities)
                ? accountGroupDictionary[AccountGroupType.Liabilities].ToArray()
                : Array.Empty<IAccount>();
        }

        private static IAccountCollectionValues ToAccountCollectionValues(IReadOnlyCollection<IAccount> assetAccountCollection, IReadOnlyCollection<IAccount> liabilityAccountCollection, Func<IAccount, ICreditInfoValues> selector)
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