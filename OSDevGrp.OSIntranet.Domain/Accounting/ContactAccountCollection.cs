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
    public class ContactAccountCollection : AccountCollectionBase<IContactAccount, IContactAccountCollection>, IContactAccountCollection
    {
        #region Properties

        public IContactAccountCollectionValues ValuesAtStatusDate { get; private set; } = new ContactAccountCollectionValues(0M, 0M);

        public IContactAccountCollectionValues ValuesAtEndOfLastMonthFromStatusDate { get; private set; } = new ContactAccountCollectionValues(0M, 0M);

        public IContactAccountCollectionValues ValuesAtEndOfLastYearFromStatusDate { get; private set; } = new ContactAccountCollectionValues(0M, 0M);

        #endregion

        #region Methods

        public Task<IContactAccountCollection> FindDebtorsAsync()
        {
            IReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>> contactAccountGroupDictionary = GroupByContactAccountType(this);

            IContactAccountCollection debtorAccountCollection = new ContactAccountCollection();
            debtorAccountCollection.Add(ResolveDebtors(contactAccountGroupDictionary).OrderBy(contactAccount => contactAccount.AccountName).ToArray());

            return debtorAccountCollection.CalculateAsync(StatusDate);
        }

        public Task<IContactAccountCollection> FindCreditorsAsync()
        {
            IReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>> contactAccountGroupDictionary = GroupByContactAccountType(this);

            IContactAccountCollection debtorAccountCollection = new ContactAccountCollection();
            debtorAccountCollection.Add(ResolveCreditors(contactAccountGroupDictionary).OrderBy(contactAccount => contactAccount.AccountName).ToArray());

            return debtorAccountCollection.CalculateAsync(StatusDate);
        }

        protected override IContactAccountCollection Calculate(DateTime statusDate, IEnumerable<IContactAccount> calculatedContactAccountCollection)
        {
            NullGuard.NotNull(calculatedContactAccountCollection, nameof(calculatedContactAccountCollection));

            IReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>> contactAccountGroupDictionary = GroupByContactAccountType(calculatedContactAccountCollection);

            IContactAccount[] debtorAccountCollection = ResolveDebtors(contactAccountGroupDictionary);
            IContactAccount[] creditorAccountCollection = ResolveCreditors(contactAccountGroupDictionary);

            ValuesAtStatusDate = ToContactAccountCollectionValues(debtorAccountCollection, creditorAccountCollection, contactAccount => contactAccount.ValuesAtStatusDate);
            ValuesAtEndOfLastMonthFromStatusDate = ToContactAccountCollectionValues(debtorAccountCollection, creditorAccountCollection, contactAccount => contactAccount.ValuesAtEndOfLastMonthFromStatusDate);
            ValuesAtEndOfLastYearFromStatusDate = ToContactAccountCollectionValues(debtorAccountCollection, creditorAccountCollection, contactAccount => contactAccount.ValuesAtEndOfLastYearFromStatusDate);

            return this;
        }

        protected override IContactAccountCollection AlreadyCalculated() => this;

        private static IReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>> GroupByContactAccountType(IEnumerable<IContactAccount> contactAccountCollection)
        {
            NullGuard.NotNull(contactAccountCollection, nameof(contactAccountCollection));

            return new ReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>>(contactAccountCollection
                .GroupBy(contactAccount => contactAccount.ContactAccountType, contactAccount => contactAccount)
                .ToDictionary(group => group.Key, group => group.AsEnumerable()));
        }

        private static IContactAccount[] ResolveDebtors(IReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>> contactAccountGroupDictionary)
        {
            NullGuard.NotNull(contactAccountGroupDictionary, nameof(contactAccountGroupDictionary));

            return contactAccountGroupDictionary.ContainsKey(ContactAccountType.Debtor)
                ? contactAccountGroupDictionary[ContactAccountType.Debtor].ToArray()
                : Array.Empty<IContactAccount>();
        }

        private static IContactAccount[] ResolveCreditors(IReadOnlyDictionary<ContactAccountType, IEnumerable<IContactAccount>> contactAccountGroupDictionary)
        {
            NullGuard.NotNull(contactAccountGroupDictionary, nameof(contactAccountGroupDictionary));

            return contactAccountGroupDictionary.ContainsKey(ContactAccountType.Creditor)
                ? contactAccountGroupDictionary[ContactAccountType.Creditor].ToArray()
                : Array.Empty<IContactAccount>();
        }

        private static IContactAccountCollectionValues ToContactAccountCollectionValues(IEnumerable<IContactAccount> debtorAccountCollection, IEnumerable<IContactAccount> creditorAccountCollection, Func<IContactAccount, IContactInfoValues> selector)
        {
            NullGuard.NotNull(debtorAccountCollection, nameof(debtorAccountCollection))
                .NotNull(creditorAccountCollection, nameof(creditorAccountCollection))
                .NotNull(selector, nameof(selector));

            return new ContactAccountCollectionValues(
                debtorAccountCollection.AsParallel().Sum(debtorAccount => selector(debtorAccount).Balance),
                creditorAccountCollection.AsParallel().Sum(creditorAccount => selector(creditorAccount).Balance));
        }

        #endregion
    }
}