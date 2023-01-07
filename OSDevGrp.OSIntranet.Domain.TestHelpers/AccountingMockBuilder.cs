using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class AccountingMockBuilder
    {
        public static Mock<IAccounting> BuildAccountingMock(this Fixture fixture, int? accountingNumber = null, bool hasLetterHead = true, ILetterHead letterHead = null, BalanceBelowZeroType? balanceBelowZero = null, int? backDating = null, DateTime? statusDate = null, IAccountCollection accountCollection = null, IBudgetAccountCollection budgetAccountCollection = null, IContactAccountCollection contactAccountCollection = null, bool hasCalculatedAccounting = true, IAccounting calculatedAccounting = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccounting> accountingMock = new Mock<IAccounting>();
            accountingMock.Setup(m => m.Number)
                .Returns(accountingNumber ?? fixture.Create<int>());
            accountingMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.LetterHead)
                .Returns(hasLetterHead ? letterHead ?? fixture.BuildLetterHeadMock().Object : null);
            accountingMock.Setup(m => m.BalanceBelowZero)
                .Returns(balanceBelowZero ?? fixture.Create<BalanceBelowZeroType>());
            accountingMock.Setup(m => m.BackDating)
                .Returns(backDating ?? fixture.Create<int>());
            accountingMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            accountingMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountingMock.Setup(m => m.DefaultForPrincipal)
                .Returns(fixture.Create<bool>());
            accountingMock.Setup(m => m.AccountCollection)
                .Returns(accountCollection ?? fixture.BuildAccountCollectionMock(accountingMock.Object, isEmpty: isEmpty).Object);
            accountingMock.Setup(m => m.BudgetAccountCollection)
                .Returns(budgetAccountCollection ?? fixture.BuildBudgetAccountCollectionMock(accountingMock.Object, isEmpty: isEmpty).Object);
            accountingMock.Setup(m => m.ContactAccountCollection)
                .Returns(contactAccountCollection ?? fixture.BuildContactAccountCollectionMock(accountingMock.Object, isEmpty: isEmpty).Object);
            accountingMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountingMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountingMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedAccounting ? calculatedAccounting ?? accountingMock.Object : null));
            accountingMock.Setup(m => m.GetPostingLinesAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(fixture.BuildPostingLineCollectionMock(isEmpty: isEmpty).Object));
            return accountingMock;
        }

        public static Mock<IAccount> BuildAccountMock(this Fixture fixture, IAccounting accounting = null, string accountNumber = null, IAccountGroup accountGroup = null, AccountGroupType? accountGroupType = null, ICreditInfoCollection creditInfoCollection = null, ICreditInfoValues valuesAtStatusDate = null, ICreditInfoValues valuesAtEndOfLastMonthFromStatusDate = null, ICreditInfoValues valuesAtEndOfLastYearFromStatusDate = null, DateTime? statusDate = null, IPostingLineCollection postingLineCollection = null, bool hasCalculatedAccount = true, IAccount calculatedAccount = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccount> accountMock = new Mock<IAccount>();
            accountMock.Setup(m => m.Accounting)
                .Returns(accounting ?? fixture.BuildAccountingMock(isEmpty: true).Object);
            accountMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? fixture.Create<string>().ToUpper());
            accountMock.Setup(m => m.AccountName)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.Description)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.Note)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.AccountGroup)
                .Returns(accountGroup ?? fixture.BuildAccountGroupMock().Object);
            accountMock.Setup(m => m.AccountGroupType)
                .Returns(accountGroupType ?? fixture.Create<AccountGroupType>());
            accountMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(valuesAtStatusDate ?? fixture.BuildCreditInfoValuesMock().Object);
            accountMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(valuesAtEndOfLastMonthFromStatusDate ?? fixture.BuildCreditInfoValuesMock().Object);
            accountMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(valuesAtEndOfLastYearFromStatusDate ?? fixture.BuildCreditInfoValuesMock().Object);
            accountMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            accountMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.CreditInfoCollection)
                .Returns(creditInfoCollection ?? fixture.BuildCreditInfoCollectionMock(account: accountMock.Object, isEmpty: isEmpty).Object);
            accountMock.Setup(m => m.PostingLineCollection)
                .Returns(postingLineCollection ?? fixture.BuildPostingLineCollectionMock(account: accountMock.Object, isEmpty: isEmpty).Object);
            accountMock.Setup(m => m.GetHashCode())
                .Returns(accountMock.GetHashCode());
            accountMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && accountMock.GetHashCode() == m.GetHashCode());
            accountMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedAccount ? calculatedAccount ?? accountMock.Object : null));
            return accountMock;
        }

        public static Mock<IAccountCollection> BuildAccountCollectionMock(this Fixture fixture, IAccounting accounting = null, IEnumerable<IAccount> accountCollection = null, bool hasCalculatedAccountCollection = true, IAccountCollection calculatedAccountCollection = null, IEnumerable<IAccountGroupStatus> groupByAccountGroupCollection = null, bool isEmpty = false, IAccountCollectionValues valuesAtStatusDate = null, IAccountCollectionValues valuesAtEndOfLastMonthFromStatusDate = null, IAccountCollectionValues valuesAtEndOfLastYearFromStatusDate = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            accounting ??= fixture.BuildAccountingMock(isEmpty: true).Object;

            if (accountCollection == null)
            {
                IList<IAccount> accounts = new List<IAccount>();
                if (isEmpty == false)
                {
                    Random random = new Random(fixture.Create<int>());
                    int numberOfAccounts = random.Next(1, 10);

                    while (accounts.Count < numberOfAccounts)
                    {
                        accounts.Add(fixture.BuildAccountMock(accounting, isEmpty: true).Object);
                    }
                }
                accountCollection = accounts;
            }

            IList<IAccount> accountList = accountCollection.ToList();

            groupByAccountGroupCollection ??= isEmpty
                ? Array.Empty<IAccountGroupStatus>()
                : new[]
                {
                    fixture.BuildAccountGroupStatusMock(accountCollection: fixture.BuildAccountCollectionMock(accounting, accountList, isEmpty: true).Object).Object,
                    fixture.BuildAccountGroupStatusMock(accountCollection: fixture.BuildAccountCollectionMock(accounting, accountList, isEmpty: true).Object).Object,
                    fixture.BuildAccountGroupStatusMock(accountCollection: fixture.BuildAccountCollectionMock(accounting, accountList, isEmpty: true).Object).Object
                };

            Mock<IAccountCollection> accountCollectionMock = new Mock<IAccountCollection>();
            accountCollectionMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(valuesAtStatusDate ?? fixture.BuildAccountCollectionValuesMock().Object);
            accountCollectionMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(valuesAtEndOfLastMonthFromStatusDate ?? fixture.BuildAccountCollectionValuesMock().Object);
            accountCollectionMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(valuesAtEndOfLastYearFromStatusDate ?? fixture.BuildAccountCollectionValuesMock().Object);
            accountCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            accountCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(accountList.GetEnumerator());
            accountCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedAccountCollection ? calculatedAccountCollection ?? accountCollectionMock.Object : null));
            accountCollectionMock.Setup(m => m.GroupByAccountGroupAsync())
                .Returns(Task.FromResult(groupByAccountGroupCollection));
            return accountCollectionMock;
        }

        public static Mock<IAccountCollectionValues> BuildAccountCollectionValuesMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccountCollectionValues> accountCollectionValuesMock = new Mock<IAccountCollectionValues>();
            accountCollectionValuesMock.Setup(m => m.Assets)
                .Returns(fixture.Create<decimal>());
            accountCollectionValuesMock.Setup(m => m.Liabilities)
                .Returns(fixture.Create<decimal>());
            return accountCollectionValuesMock;
        }

        public static Mock<IBudgetAccount> BuildBudgetAccountMock(this Fixture fixture, IAccounting accounting = null, string accountNumber = null, IBudgetAccountGroup budgetAccountGroup = null, IBudgetInfoValues valuesForMonthOfStatusDate = null, IBudgetInfoValues valuesForLastMonthOfStatusDate = null, IBudgetInfoValues valuesForYearToDateOfStatusDate = null, IBudgetInfoValues valuesForLastYearOfStatusDate = null, DateTime? statusDate = null, IBudgetInfoCollection budgetInfoCollection = null, IPostingLineCollection postingLineCollection = null, bool hasCalculatedBudgetAccount = true, IBudgetAccount calculatedBudgetAccount = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccount> budgetAccountMock = new Mock<IBudgetAccount>();
            budgetAccountMock.Setup(m => m.Accounting)
                .Returns(accounting ?? fixture.BuildAccountingMock(isEmpty: true).Object);
            budgetAccountMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? fixture.Create<string>().ToUpper());
            budgetAccountMock.Setup(m => m.AccountName)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.Description)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.Note)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.BudgetAccountGroup)
                .Returns(budgetAccountGroup ?? fixture.BuildBudgetAccountGroupMock().Object);
            budgetAccountMock.Setup(m => m.ValuesForMonthOfStatusDate)
                .Returns(valuesForMonthOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountMock.Setup(m => m.ValuesForLastMonthOfStatusDate)
                .Returns(valuesForLastMonthOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountMock.Setup(m => m.ValuesForYearToDateOfStatusDate)
                .Returns(valuesForYearToDateOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountMock.Setup(m => m.ValuesForLastYearOfStatusDate)
                .Returns(valuesForLastYearOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            budgetAccountMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            budgetAccountMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.BudgetInfoCollection)
                .Returns(budgetInfoCollection ?? fixture.BuildBudgetInfoCollectionMock(budgetAccount: budgetAccountMock.Object, isEmpty: isEmpty).Object);
            budgetAccountMock.Setup(m => m.PostingLineCollection)
                .Returns(postingLineCollection ?? fixture.BuildPostingLineCollectionMock(budgetAccount: budgetAccountMock.Object, isEmpty: isEmpty).Object);
            budgetAccountMock.Setup(m => m.GetHashCode())
                .Returns(budgetAccountMock.GetHashCode());
            budgetAccountMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && budgetAccountMock.GetHashCode() == m.GetHashCode());
            budgetAccountMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedBudgetAccount ? calculatedBudgetAccount ?? budgetAccountMock.Object : null));
            return budgetAccountMock;
        }

        public static Mock<IBudgetAccountCollection> BuildBudgetAccountCollectionMock(this Fixture fixture, IAccounting accounting = null, IEnumerable<IBudgetAccount> budgetAccountCollection = null, bool hasCalculatedBudgetAccountCollection = true, IBudgetAccountCollection calculatedBudgetAccountCollection = null, IEnumerable<IBudgetAccountGroupStatus> groupByBudgetAccountGroupCollection = null, bool isEmpty = false, IBudgetInfoValues valuesForMonthOfStatusDate = null, IBudgetInfoValues valuesForLastMonthOfStatusDate = null, IBudgetInfoValues valuesForYearToDateOfStatusDate = null, IBudgetInfoValues valuesForLastYearOfStatusDate = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            accounting ??= fixture.BuildAccountingMock(isEmpty: true).Object;

            if (budgetAccountCollection == null)
            {
                IList<IBudgetAccount> budgetAccounts = new List<IBudgetAccount>();
                if (isEmpty == false)
                {
                    Random random = new Random(fixture.Create<int>());
                    int numberOfBudgetAccounts = random.Next(1, 10);

                    while (budgetAccounts.Count < numberOfBudgetAccounts)
                    {
                        budgetAccounts.Add(fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object);
                    }
                }
                budgetAccountCollection = budgetAccounts;
            }

            IList<IBudgetAccount> budgetAccountList = budgetAccountCollection.ToList();

            groupByBudgetAccountGroupCollection ??= isEmpty
                ? Array.Empty<IBudgetAccountGroupStatus>()
                : new[]
                {
                    fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: fixture.BuildBudgetAccountCollectionMock(accounting, budgetAccountList, isEmpty: true).Object).Object,
                    fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: fixture.BuildBudgetAccountCollectionMock(accounting, budgetAccountList, isEmpty: true).Object).Object,
                    fixture.BuildBudgetAccountGroupStatusMock(budgetAccountCollection: fixture.BuildBudgetAccountCollectionMock(accounting, budgetAccountList, isEmpty: true).Object).Object
                };

            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = new Mock<IBudgetAccountCollection>();
            budgetAccountCollectionMock.Setup(m => m.ValuesForMonthOfStatusDate)
                .Returns(valuesForMonthOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountCollectionMock.Setup(m => m.ValuesForLastMonthOfStatusDate)
                .Returns(valuesForLastMonthOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountCollectionMock.Setup(m => m.ValuesForYearToDateOfStatusDate)
                .Returns(valuesForYearToDateOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountCollectionMock.Setup(m => m.ValuesForLastYearOfStatusDate)
                .Returns(valuesForLastYearOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            budgetAccountCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(budgetAccountList.GetEnumerator());
            budgetAccountCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedBudgetAccountCollection ? calculatedBudgetAccountCollection ?? budgetAccountCollectionMock.Object : null));
            budgetAccountCollectionMock.Setup(m => m.GroupByBudgetAccountGroupAsync())
                .Returns(Task.FromResult(groupByBudgetAccountGroupCollection));
            return budgetAccountCollectionMock;
        }

        public static Mock<IContactAccount> BuildContactAccountMock(this Fixture fixture, IAccounting accounting = null, string accountNumber = null, IPaymentTerm paymentTerm = null, ContactAccountType? contactAccountType = null, IContactInfoValues valuesAtStatusDate = null, IContactInfoValues valuesAtEndOfLastMonthFromStatusDate = null, IContactInfoCollection contactInfoCollection = null, IContactInfoValues valuesAtEndOfLastYearFromStatusDate = null, DateTime? statusDate = null, IPostingLineCollection postingLineCollection = null, bool hasCalculatedContactAccount = true, IContactAccount calculatedContactAccount = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IContactAccount> contactAccountMock = new Mock<IContactAccount>();
            contactAccountMock.Setup(m => m.Accounting)
                .Returns(accounting ?? fixture.BuildAccountingMock(isEmpty: true).Object);
            contactAccountMock.Setup(m => m.AccountNumber)
                .Returns(accountNumber ?? fixture.Create<string>().ToUpper());
            contactAccountMock.Setup(m => m.AccountName)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.Description)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.Note)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.MailAddress)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.PrimaryPhone)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.SecondaryPhone)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.PaymentTerm)
                .Returns(paymentTerm ?? fixture.BuildPaymentTermMock().Object);
            contactAccountMock.Setup(m => m.ContactAccountType)
                .Returns(contactAccountType ?? fixture.Create<ContactAccountType>());
            contactAccountMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(valuesAtStatusDate ?? fixture.BuildContactInfoValuesMock().Object);
            contactAccountMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(valuesAtEndOfLastMonthFromStatusDate ?? fixture.BuildContactInfoValuesMock().Object);
            contactAccountMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(valuesAtEndOfLastYearFromStatusDate ?? fixture.BuildContactInfoValuesMock().Object);
            contactAccountMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            contactAccountMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            contactAccountMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactAccountMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactAccountMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            contactAccountMock.Setup(m => m.ContactInfoCollection)
                .Returns(contactInfoCollection ?? fixture.BuildContactInfoCollectionMock(contactAccount: contactAccountMock.Object, isEmpty: isEmpty).Object);
            contactAccountMock.Setup(m => m.PostingLineCollection)
                .Returns(postingLineCollection ?? fixture.BuildPostingLineCollectionMock(contactAccount: contactAccountMock.Object, isEmpty: isEmpty).Object);
            contactAccountMock.Setup(m => m.GetHashCode())
                .Returns(contactAccountMock.GetHashCode());
            contactAccountMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && contactAccountMock.GetHashCode() == m.GetHashCode());
            contactAccountMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedContactAccount ? calculatedContactAccount ?? contactAccountMock.Object : null));
            return contactAccountMock;
        }

        public static Mock<IContactAccountCollection> BuildContactAccountCollectionMock(this Fixture fixture, IAccounting accounting = null, IEnumerable<IContactAccount> contactAccountCollection = null, IContactAccountCollection calculatedContactAccountCollection = null, IContactAccountCollection findDebtorsContactAccountCollection = null, IContactAccountCollection findCreditorContactAccountCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            accounting ??= fixture.BuildAccountingMock(isEmpty: true).Object;

            if (contactAccountCollection == null)
            {
                IList<IContactAccount> contactAccounts = new List<IContactAccount>();
                if (isEmpty == false)
                {
                    Random random = new Random(fixture.Create<int>());
                    int numberOfContactAccounts = random.Next(1, 10);

                    while (contactAccounts.Count < numberOfContactAccounts)
                    {
                        contactAccounts.Add(fixture.BuildContactAccountMock(accounting, isEmpty: true).Object);
                    }
                }
                contactAccountCollection = contactAccounts;
            }

            Mock<IContactAccountCollection> contactAccountCollectionMock = new Mock<IContactAccountCollection>();
            contactAccountCollectionMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(fixture.BuildContactAccountCollectionValuesMock().Object);
            contactAccountCollectionMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(fixture.BuildContactAccountCollectionValuesMock().Object);
            contactAccountCollectionMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(fixture.BuildContactAccountCollectionValuesMock().Object);
            contactAccountCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            contactAccountCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(contactAccountCollection.GetEnumerator());
            contactAccountCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedContactAccountCollection ?? contactAccountCollectionMock.Object));
            contactAccountCollectionMock.Setup(m => m.FindDebtorsAsync())
                .Returns(Task.FromResult(findDebtorsContactAccountCollection?? contactAccountCollectionMock.Object));
            contactAccountCollectionMock.Setup(m => m.FindCreditorsAsync())
                .Returns(Task.FromResult(findCreditorContactAccountCollection ?? contactAccountCollectionMock.Object));
            return contactAccountCollectionMock;
        }

        public static Mock<IContactAccountCollectionValues> BuildContactAccountCollectionValuesMock(this Fixture fixture)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IContactAccountCollectionValues> contactAccountCollectionValuesMock = new Mock<IContactAccountCollectionValues>();
            contactAccountCollectionValuesMock.Setup(m => m.Debtors)
                .Returns(fixture.Create<decimal>());
            contactAccountCollectionValuesMock.Setup(m => m.Creditors)
                .Returns(fixture.Create<decimal>());
            return contactAccountCollectionValuesMock;
        }

        public static Mock<ICreditInfo> BuildCreditInfoMock(this Fixture fixture, DateTime? infoOffset = null, bool isMonthOfStatusDate = false, bool isLastMonthOfStatusDate = false, bool isYearToDateOfStatusDate = false, bool isLastYearOfStatusDate = false, IAccount account = null, ICreditInfo calculatedCreditInfo = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());
            DateTime infoDate = infoOffset ?? DateTime.Today.AddDays(random.Next(1, 365) * -1);

            Mock<ICreditInfo> creditInfoMock = new Mock<ICreditInfo>();
            creditInfoMock.Setup(m => m.Year)
                .Returns((short) infoDate.Year);
            creditInfoMock.Setup(m => m.Month)
                .Returns((short) infoDate.Month);
            creditInfoMock.Setup(m => m.FromDate)
                .Returns(new DateTime(infoDate.Year, infoDate.Month, 1).Date);
            creditInfoMock.Setup(m => m.ToDate)
                .Returns(new DateTime(infoDate.Year, infoDate.Month, DateTime.DaysInMonth(infoDate.Year, infoDate.Month)).Date);
            creditInfoMock.Setup(m => m.IsMonthOfStatusDate)
                .Returns(isMonthOfStatusDate);
            creditInfoMock.Setup(m => m.IsLastMonthOfStatusDate)
                .Returns(isLastMonthOfStatusDate);
            creditInfoMock.Setup(m => m.IsYearToDateOfStatusDate)
                .Returns(isYearToDateOfStatusDate);
            creditInfoMock.Setup(m => m.IsLastYearOfStatusDate)
                .Returns(isLastYearOfStatusDate);
            creditInfoMock.Setup(m => m.Account)
                .Returns(account ?? fixture.BuildAccountMock(isEmpty: true).Object);
            creditInfoMock.Setup(m => m.Credit)
                .Returns(fixture.Create<decimal>());
            creditInfoMock.Setup(m => m.Balance)
                .Returns(fixture.Create<decimal>());
            creditInfoMock.Setup(m => m.Available)
                .Returns(fixture.Create<decimal>());
            creditInfoMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            creditInfoMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            creditInfoMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            creditInfoMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            creditInfoMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            creditInfoMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            creditInfoMock.Setup(m => m.GetHashCode())
                .Returns(creditInfoMock.GetHashCode());
            creditInfoMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && creditInfoMock.GetHashCode() == m.GetHashCode());
            creditInfoMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedCreditInfo ?? creditInfoMock.Object));
            return creditInfoMock;
        }

        public static Mock<ICreditInfoValues> BuildCreditInfoValuesMock(this Fixture fixture, decimal? credit = null, decimal? balance = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ICreditInfoValues> creditInfoValuesMock = new Mock<ICreditInfoValues>();
            creditInfoValuesMock.Setup(m => m.Credit)
                .Returns(credit ?? fixture.Create<decimal>());
            creditInfoValuesMock.Setup(m => m.Balance)
                .Returns(balance ?? fixture.Create<decimal>());
            creditInfoValuesMock.Setup(m => m.Available)
                .Returns(fixture.Create<decimal>());
            return creditInfoValuesMock;
        }

        public static Mock<ICreditInfoCollection> BuildCreditInfoCollectionMock(this Fixture fixture, DateTime? infoOffset = null, IAccount account = null, bool hasCreditInfoForFind = true, ICreditInfo creditInfoForFind = null, IEnumerable<ICreditInfo> creditInfoCollection = null, ICreditInfoCollection calculatedCreditInfoCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            account ??= fixture.BuildAccountMock(isEmpty: true).Object;

            if (creditInfoCollection == null)
            {
                DateTime infoDate = infoOffset ?? DateTime.Today;
                creditInfoCollection = isEmpty ? new List<ICreditInfo>(0) : new List<ICreditInfo>
                {
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(3), account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(2), account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(1), account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate, true, isYearToDateOfStatusDate: true, account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(-1), isLastMonthOfStatusDate: true, isYearToDateOfStatusDate: true, account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(-2), isYearToDateOfStatusDate: true, account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(-3), isYearToDateOfStatusDate: true, account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(-4), isLastYearOfStatusDate: true, account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(-5), isLastYearOfStatusDate: true, account: account).Object,
                    fixture.BuildCreditInfoMock(infoDate.AddMonths(-6), isLastYearOfStatusDate: true, account: account).Object
                };
            }

            Mock<ICreditInfoCollection> creditInfoCollectionMock = new Mock<ICreditInfoCollection>();
            creditInfoCollectionMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(fixture.BuildCreditInfoValuesMock().Object);
            creditInfoCollectionMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(fixture.BuildCreditInfoValuesMock().Object);
            creditInfoCollectionMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(fixture.BuildCreditInfoValuesMock().Object);
            creditInfoCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            creditInfoCollectionMock.Setup(m => m.First())
                .Throws(new NotSupportedException());
            creditInfoCollectionMock.Setup(m => m.Prev(It.IsAny<ICreditInfo>()))
                .Throws(new NotSupportedException());
            creditInfoCollectionMock.Setup(m => m.Next(It.IsAny<ICreditInfo>()))
                .Throws(new NotSupportedException());
            creditInfoCollectionMock.Setup(m => m.Last())
                .Throws(new NotSupportedException());
            creditInfoCollectionMock.Setup(m => m.Find(It.IsAny<DateTime>()))
                .Returns(hasCreditInfoForFind ? creditInfoForFind ?? fixture.BuildCreditInfoMock(account: account).Object : null);
            creditInfoCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(creditInfoCollection.GetEnumerator());
            creditInfoCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedCreditInfoCollection ?? creditInfoCollectionMock.Object));
            return creditInfoCollectionMock;
        }

        public static Mock<IBudgetInfo> BuildBudgetInfoMock(this Fixture fixture, DateTime? infoOffset = null, bool isMonthOfStatusDate = false, bool isLastMonthOfStatusDate = false, bool isYearToDateOfStatusDate = false, bool isLastYearOfStatusDate = false, IBudgetAccount budgetAccount = null, IBudgetInfo calculatedBudgetInfo = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());
            DateTime infoDate = infoOffset ?? DateTime.Today.AddDays(random.Next(1, 365) * -1);

            Mock<IBudgetInfo> budgetInfoMock = new Mock<IBudgetInfo>();
            budgetInfoMock.Setup(m => m.Year)
                .Returns((short) infoDate.Year);
            budgetInfoMock.Setup(m => m.Month)
                .Returns((short) infoDate.Month);
            budgetInfoMock.Setup(m => m.FromDate)
                .Returns(new DateTime(infoDate.Year, infoDate.Month, 1).Date);
            budgetInfoMock.Setup(m => m.ToDate)
                .Returns(new DateTime(infoDate.Year, infoDate.Month, DateTime.DaysInMonth(infoDate.Year, infoDate.Month)).Date);
            budgetInfoMock.Setup(m => m.IsMonthOfStatusDate)
                .Returns(isMonthOfStatusDate);
            budgetInfoMock.Setup(m => m.IsLastMonthOfStatusDate)
                .Returns(isLastMonthOfStatusDate);
            budgetInfoMock.Setup(m => m.IsYearToDateOfStatusDate)
                .Returns(isYearToDateOfStatusDate);
            budgetInfoMock.Setup(m => m.IsLastYearOfStatusDate)
                .Returns(isLastYearOfStatusDate);
            budgetInfoMock.Setup(m => m.BudgetAccount)
                .Returns(budgetAccount ?? fixture.BuildBudgetAccountMock(isEmpty: true).Object);
            budgetInfoMock.Setup(m => m.Income)
                .Returns(fixture.Create<decimal>());
            budgetInfoMock.Setup(m => m.Expenses)
                .Returns(fixture.Create<decimal>());
            budgetInfoMock.Setup(m => m.Budget)
                .Returns(fixture.Create<decimal>());
            budgetInfoMock.Setup(m => m.Posted)
                .Returns(fixture.Create<decimal>());
            budgetInfoMock.Setup(m => m.Available)
                .Returns(fixture.Create<decimal>());
            budgetInfoMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            budgetInfoMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            budgetInfoMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetInfoMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetInfoMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetInfoMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetInfoMock.Setup(m => m.GetHashCode())
                .Returns(budgetInfoMock.GetHashCode());
            budgetInfoMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && budgetInfoMock.GetHashCode() == m.GetHashCode());
            budgetInfoMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedBudgetInfo ?? budgetInfoMock.Object));
            return budgetInfoMock;
        }

        public static Mock<IBudgetInfoValues> BuildBudgetInfoValuesMock(this Fixture fixture, decimal? budget = null, decimal? posted = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetInfoValues> budgetInfoValuesMock = new Mock<IBudgetInfoValues>();
            budgetInfoValuesMock.Setup(m => m.Budget)
                .Returns(budget ?? fixture.Create<decimal>());
            budgetInfoValuesMock.Setup(m => m.Posted)
                .Returns(posted ?? fixture.Create<decimal>());
            budgetInfoValuesMock.Setup(m => m.Available)
                .Returns(fixture.Create<decimal>());
            return budgetInfoValuesMock;
        }

        public static Mock<IBudgetInfoCollection> BuildBudgetInfoCollectionMock(this Fixture fixture, DateTime? infoOffset = null, IBudgetAccount budgetAccount = null, bool hasBudgetInfoForFind = true, IBudgetInfo budgetInfoForFind = null, IEnumerable<IBudgetInfo> budgetInfoCollection = null, IBudgetInfoCollection calculatedBudgetInfoCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            budgetAccount ??= fixture.BuildBudgetAccountMock(isEmpty: true).Object;

            if (budgetInfoCollection == null)
            {
                DateTime infoDate = infoOffset ?? DateTime.Today;
                budgetInfoCollection = isEmpty ? new List<IBudgetInfo>(0) : new List<IBudgetInfo>
                {
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(3), budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(2), budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(1), budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate, true, isYearToDateOfStatusDate: true, budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(-1), isLastMonthOfStatusDate: true, isYearToDateOfStatusDate: true, budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(-2), isYearToDateOfStatusDate: true, budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(-3), isYearToDateOfStatusDate: true, budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(-4), isLastYearOfStatusDate: true, budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(-5), isLastYearOfStatusDate: true, budgetAccount: budgetAccount).Object,
                    fixture.BuildBudgetInfoMock(infoDate.AddMonths(-6), isLastYearOfStatusDate: true, budgetAccount: budgetAccount).Object
                };
            }

            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = new Mock<IBudgetInfoCollection>();
            budgetInfoCollectionMock.Setup(m => m.ValuesForMonthOfStatusDate)
                .Returns(fixture.BuildBudgetInfoValuesMock().Object);
            budgetInfoCollectionMock.Setup(m => m.ValuesForLastMonthOfStatusDate)
                .Returns(fixture.BuildBudgetInfoValuesMock().Object);
            budgetInfoCollectionMock.Setup(m => m.ValuesForYearToDateOfStatusDate)
                .Returns(fixture.BuildBudgetInfoValuesMock().Object);
            budgetInfoCollectionMock.Setup(m => m.ValuesForLastYearOfStatusDate)
                .Returns(fixture.BuildBudgetInfoValuesMock().Object);
            budgetInfoCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            budgetInfoCollectionMock.Setup(m => m.First())
                .Throws(new NotSupportedException());
            budgetInfoCollectionMock.Setup(m => m.Prev(It.IsAny<IBudgetInfo>()))
                .Throws(new NotSupportedException());
            budgetInfoCollectionMock.Setup(m => m.Next(It.IsAny<IBudgetInfo>()))
                .Throws(new NotSupportedException());
            budgetInfoCollectionMock.Setup(m => m.Last())
                .Throws(new NotSupportedException());
            budgetInfoCollectionMock.Setup(m => m.Find(It.IsAny<DateTime>()))
                .Returns(hasBudgetInfoForFind ? budgetInfoForFind ?? fixture.BuildBudgetInfoMock(budgetAccount: budgetAccount).Object : null);
            budgetInfoCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(budgetInfoCollection.GetEnumerator());
            budgetInfoCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedBudgetInfoCollection ?? budgetInfoCollectionMock.Object));
            return budgetInfoCollectionMock;
        }

        public static Mock<IContactInfo> BuildContactInfoMock(this Fixture fixture, DateTime? infoOffset = null, bool isMonthOfStatusDate = false, bool isLastMonthOfStatusDate = false, bool isYearToDateOfStatusDate = false, bool isLastYearOfStatusDate = false, IContactAccount contactAccount = null, IContactInfo calculatedContactInfo = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());
            DateTime infoDate = infoOffset ?? DateTime.Today.AddDays(random.Next(1, 365) * -1);

            Mock<IContactInfo> contactInfoMock = new Mock<IContactInfo>();
            contactInfoMock.Setup(m => m.Year)
                .Returns((short) infoDate.Year);
            contactInfoMock.Setup(m => m.Month)
                .Returns((short) infoDate.Month);
            contactInfoMock.Setup(m => m.FromDate)
                .Returns(new DateTime(infoDate.Year, infoDate.Month, 1).Date);
            contactInfoMock.Setup(m => m.ToDate)
                .Returns(new DateTime(infoDate.Year, infoDate.Month, DateTime.DaysInMonth(infoDate.Year, infoDate.Month)).Date);
            contactInfoMock.Setup(m => m.IsMonthOfStatusDate)
                .Returns(isMonthOfStatusDate);
            contactInfoMock.Setup(m => m.IsLastMonthOfStatusDate)
                .Returns(isLastMonthOfStatusDate);
            contactInfoMock.Setup(m => m.IsYearToDateOfStatusDate)
                .Returns(isYearToDateOfStatusDate);
            contactInfoMock.Setup(m => m.IsLastYearOfStatusDate)
                .Returns(isLastYearOfStatusDate);
            contactInfoMock.Setup(m => m.ContactAccount)
                .Returns(contactAccount ?? fixture.BuildContactAccountMock(isEmpty: true).Object);
            contactInfoMock.Setup(m => m.Balance)
                .Returns(fixture.Create<decimal>());
            contactInfoMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            contactInfoMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            contactInfoMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactInfoMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            contactInfoMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            contactInfoMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            contactInfoMock.Setup(m => m.GetHashCode())
                .Returns(contactInfoMock.GetHashCode());
            contactInfoMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && contactInfoMock.GetHashCode() == m.GetHashCode());
            contactInfoMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedContactInfo ?? contactInfoMock.Object));
            return contactInfoMock;
        }

        public static Mock<IContactInfoValues> BuildContactInfoValuesMock(this Fixture fixture, decimal? balance = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IContactInfoValues> contactInfoValuesMock = new Mock<IContactInfoValues>();
            contactInfoValuesMock.Setup(m => m.Balance)
                .Returns(balance ?? fixture.Create<decimal>());
            return contactInfoValuesMock;
        }

        public static Mock<IContactInfoCollection> BuildContactInfoCollectionMock(this Fixture fixture, DateTime? infoOffset = null, IContactAccount contactAccount = null, IContactInfoValues valuesAtStatusDate = null, bool hasContactInfoForFind = true, IContactInfo contactInfoForFind = null, IEnumerable<IContactInfo> contactInfoCollection = null, IContactInfoCollection calculatedContactInfoCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            contactAccount ??= fixture.BuildContactAccountMock(isEmpty: true).Object;

            if (contactInfoCollection == null)
            {
                DateTime infoDate = infoOffset ?? DateTime.Today;
                contactInfoCollection = isEmpty ? new List<IContactInfo>(0) : new List<IContactInfo>
                {
                    fixture.BuildContactInfoMock(infoDate.AddMonths(3), contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(2), contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(1), contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate, true, isYearToDateOfStatusDate: true, contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(-1), isLastMonthOfStatusDate: true, isYearToDateOfStatusDate: true, contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(-2), isYearToDateOfStatusDate: true, contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(-3), isYearToDateOfStatusDate: true, contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(-4), isLastYearOfStatusDate: true, contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(-5), isLastYearOfStatusDate: true, contactAccount: contactAccount).Object,
                    fixture.BuildContactInfoMock(infoDate.AddMonths(-6), isLastYearOfStatusDate: true, contactAccount: contactAccount).Object
                };
            }

            Mock<IContactInfoCollection> contactInfoCollectionMock = new Mock<IContactInfoCollection>();
            contactInfoCollectionMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(valuesAtStatusDate ?? fixture.BuildContactInfoValuesMock().Object);
            contactInfoCollectionMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(fixture.BuildContactInfoValuesMock().Object);
            contactInfoCollectionMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(fixture.BuildContactInfoValuesMock().Object);
            contactInfoCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            contactInfoCollectionMock.Setup(m => m.First())
                .Throws(new NotSupportedException());
            contactInfoCollectionMock.Setup(m => m.Prev(It.IsAny<IContactInfo>()))
                .Throws(new NotSupportedException());
            contactInfoCollectionMock.Setup(m => m.Next(It.IsAny<IContactInfo>()))
                .Throws(new NotSupportedException());
            contactInfoCollectionMock.Setup(m => m.Last())
                .Throws(new NotSupportedException());
            contactInfoCollectionMock.Setup(m => m.Find(It.IsAny<DateTime>()))
                .Returns(hasContactInfoForFind ? contactInfoForFind ?? fixture.BuildContactInfoMock(contactAccount: contactAccount).Object : null);
            contactInfoCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(contactInfoCollection.GetEnumerator());
            contactInfoCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedContactInfoCollection ?? contactInfoCollectionMock.Object));
            return contactInfoCollectionMock;
        }

        public static Mock<IPostingLine> BuildPostingLineMock(this Fixture fixture, Guid? identifier = null, DateTime? postingDate = null, IAccount account = null, ICreditInfoValues accountValuesAtPostingDate = null, IBudgetAccount budgetAccount = null, IBudgetInfoValues budgetAccountValuesAtPostingDate = null, IContactAccount contactAccount = null, IContactInfoValues contactAccountValuesAtPostingDate = null, int? sortOrder = null, DateTime? statusDate = null, IPostingLine calculatedPostingLine = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());
            int numberOfDays = (int)DateTime.Today.Subtract(DateTime.Today.AddYears(-3)).TotalDays;

            IAccounting accounting = account?.Accounting ??
                                     budgetAccount?.Accounting ??
                                     contactAccount?.Accounting ??
                                     fixture.BuildAccountingMock(isEmpty: true).Object;
            account ??= fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            budgetAccount ??= random.Next(100) > 25 ? fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object : null;
            contactAccount ??= random.Next(100) > 75 ? fixture.BuildContactAccountMock(accounting, isEmpty: true).Object : null;

            decimal debit = Math.Abs(fixture.Create<decimal>());
            decimal credit = Math.Abs(fixture.Create<decimal>());

            Mock<IPostingLine> postingLineMock = new Mock<IPostingLine>();
            postingLineMock.Setup(m => m.Identifier)
                .Returns(identifier ?? Guid.NewGuid());
            postingLineMock.Setup(m => m.Accounting)
                .Returns(accounting);
            postingLineMock.Setup(m => m.PostingDate)
                .Returns(postingDate ?? DateTime.Today.AddDays(random.Next(0, numberOfDays) * -1));
            postingLineMock.Setup(m => m.Reference)
                .Returns(random.Next(100) > 25 ? fixture.Create<string>() : null);
            postingLineMock.Setup(m => m.Account)
                .Returns(account);
            postingLineMock.Setup(m => m.AccountValuesAtPostingDate)
                .Returns(accountValuesAtPostingDate ?? fixture.BuildCreditInfoValuesMock().Object);
            postingLineMock.Setup(m => m.Details)
                .Returns(fixture.Create<string>());
            postingLineMock.Setup(m => m.BudgetAccount)
                .Returns(budgetAccount);
            postingLineMock.Setup(m => m.BudgetAccountValuesAtPostingDate)
                .Returns(budgetAccount != null ? budgetAccountValuesAtPostingDate ?? fixture.BuildBudgetInfoValuesMock().Object : null);
            postingLineMock.Setup(m => m.Debit)
                .Returns(debit);
            postingLineMock.Setup(m => m.Credit)
                .Returns(credit);
            postingLineMock.Setup(m => m.PostingValue)
                .Returns(debit - credit);
            postingLineMock.Setup(m => m.ContactAccount)
                .Returns(contactAccount);
            postingLineMock.Setup(m => m.ContactAccountValuesAtPostingDate)
                .Returns(contactAccount != null ? contactAccountValuesAtPostingDate ?? fixture.BuildContactInfoValuesMock().Object : null);
            postingLineMock.Setup(m => m.SortOrder)
                .Returns(sortOrder ?? Math.Abs(fixture.Create<int>()));
            postingLineMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            postingLineMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            postingLineMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            postingLineMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            postingLineMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            postingLineMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedPostingLine ?? postingLineMock.Object));
            postingLineMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IAccounting>()))
                .Returns(Task.FromResult(postingLineMock.Object));
            postingLineMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IAccount>()))
                .Returns(Task.FromResult(postingLineMock.Object));
            postingLineMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IBudgetAccount>()))
                .Returns(Task.FromResult(postingLineMock.Object));
            postingLineMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IContactAccount>()))
                .Returns(Task.FromResult(postingLineMock.Object));
            postingLineMock.Setup(m => m.GetHashCode())
                .Returns(postingLineMock.GetHashCode());
            postingLineMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && postingLineMock.GetHashCode() == m.GetHashCode());
            return postingLineMock;
        }

        public static Mock<IPostingLineCollection> BuildPostingLineCollectionMock(this Fixture fixture, IAccount account = null, IBudgetAccount budgetAccount = null, IContactAccount contactAccount = null, decimal? calculatedPostingValue = null, IEnumerable<IPostingLine> postingLineCollection = null, IPostingLineCollection orderedPostingLineCollection = null, IPostingLineCollection topPostingLineCollection = null, IPostingLineCollection calculatedPostingLineCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            if (postingLineCollection == null)
            {
                IList<IPostingLine> postingLines = new List<IPostingLine>();
                if (isEmpty == false)
                {
                    Random random = new Random(fixture.Create<int>());
                    int numberOfPostingLines = random.Next(25, 50);

                    while (postingLines.Count < numberOfPostingLines)
                    {
                        postingLines.Add(fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, contactAccount: contactAccount).Object);
                    }
                }
                postingLineCollection = postingLines;
            }

            Mock<IPostingLineCollection> postingLineCollectionMock = new Mock<IPostingLineCollection>();
            postingLineCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            postingLineCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(postingLineCollection.GetEnumerator());
            postingLineCollectionMock.Setup(m => m.Between(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(postingLineCollectionMock.Object);
            postingLineCollectionMock.Setup(m => m.Ordered())
                .Returns(orderedPostingLineCollection ?? postingLineCollectionMock.Object);
            postingLineCollectionMock.Setup(m => m.Top(It.IsAny<int>()))
                .Returns(topPostingLineCollection ?? postingLineCollectionMock.Object);
            postingLineCollectionMock.Setup(m => m.CalculatePostingValue(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int?>()))
                .Returns(calculatedPostingValue ?? fixture.Create<decimal>());
            postingLineCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(calculatedPostingLineCollection ?? postingLineCollectionMock.Object));
            postingLineCollectionMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IAccounting>()))
                .Returns(Task.FromResult(postingLineCollectionMock.Object));
            postingLineCollectionMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IAccount>()))
                .Returns(Task.FromResult(postingLineCollectionMock.Object));
            postingLineCollectionMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IBudgetAccount>()))
                .Returns(Task.FromResult(postingLineCollectionMock.Object));
            postingLineCollectionMock.Setup(m => m.ApplyCalculationAsync(It.IsAny<IContactAccount>()))
                .Returns(Task.FromResult(postingLineCollectionMock.Object));
            return postingLineCollectionMock;
        }

        public static Mock<IPostingWarning> BuildPostingWarningMock(this Fixture fixture, IPostingLine postingLine = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPostingWarning> postingWarningMock = new Mock<IPostingWarning>();
            postingWarningMock.Setup(m => m.Reason)
                .Returns(fixture.Create<PostingWarningReason>());
            postingWarningMock.Setup(m => m.Account)
                .Returns(fixture.BuildAccountMock(isEmpty: true).Object);
            postingWarningMock.Setup(m => m.Amount)
                .Returns(fixture.Create<decimal>());
            postingWarningMock.Setup(m => m.PostingLine)
                .Returns(postingLine ?? fixture.BuildPostingLineMock().Object);
            return postingWarningMock;
        }

        public static Mock<IPostingWarningCollection> BuildPostingWarningCollectionMock(this Fixture fixture, IEnumerable<IPostingWarning> postingWarningCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            if (postingWarningCollection == null)
            {
                IList<IPostingWarning> postingWarnings = new List<IPostingWarning>();
                if (isEmpty == false)
                {
                    Random random = new Random(fixture.Create<int>());
                    int numberOfPostingWarnings = random.Next(5, 10);

                    while (postingWarnings.Count < numberOfPostingWarnings)
                    {
                        postingWarnings.Add(fixture.BuildPostingWarningMock().Object);
                    }
                }
                postingWarningCollection = postingWarnings;
            }

            Mock<IPostingWarningCollection> postingWarningCollectionMock = new Mock<IPostingWarningCollection>();
            postingWarningCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(postingWarningCollection.GetEnumerator());
            postingWarningCollectionMock.Setup(m => m.Ordered())
                .Returns(postingWarningCollectionMock.Object);
            return postingWarningCollectionMock;
        }

        public static Mock<IPostingJournal> BuildPostingJournalMock(this Fixture fixture, IPostingLineCollection postingLineCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPostingJournal> postingJournalMock = new Mock<IPostingJournal>();
            postingJournalMock.Setup(m => m.PostingLineCollection)
                .Returns(postingLineCollection ?? fixture.BuildPostingLineCollectionMock(isEmpty: isEmpty).Object);
            return postingJournalMock;
        }

        public static Mock<IPostingJournalResult> BuildPostingJournalResultMock(this Fixture fixture, IPostingLineCollection postingLineCollection = null, IPostingWarningCollection postingWarningCollection = null, bool hasCalculatedPostingJournalResult = true, IPostingJournalResult calculatedPostingJournalResult = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPostingJournalResult> postingJournalResultMock = new Mock<IPostingJournalResult>();
            postingJournalResultMock.Setup(m => m.PostingLineCollection)
                .Returns(postingLineCollection ?? fixture.BuildPostingLineCollectionMock(isEmpty: isEmpty).Object);
            postingJournalResultMock.Setup(m => m.PostingWarningCollection)
                .Returns(postingWarningCollection ?? fixture.BuildPostingWarningCollectionMock(isEmpty: isEmpty).Object);
            postingJournalResultMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            postingJournalResultMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedPostingJournalResult ? calculatedPostingJournalResult ?? postingJournalResultMock.Object : null));
            return postingJournalResultMock;
        }

        public static Mock<IPostingWarningCalculator> BuildPostingWarningCalculatorMock(this Fixture fixture, IPostingWarningCollection postingWarningCollection = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPostingWarningCalculator> postingWarningCalculatorMock = new Mock<IPostingWarningCalculator>();
            postingWarningCalculatorMock.Setup(m => m.CalculateAsync(It.IsAny<IPostingLine>()))
                .Returns(Task.FromResult(postingWarningCollection ?? fixture.BuildPostingWarningCollectionMock(isEmpty: isEmpty).Object));
            postingWarningCalculatorMock.Setup(m => m.CalculateAsync(It.IsAny<IPostingLineCollection>()))
                .Returns(Task.FromResult(postingWarningCollection ?? fixture.BuildPostingWarningCollectionMock(isEmpty: isEmpty).Object));
            return postingWarningCalculatorMock;
        }

        public static Mock<IAccountGroup> BuildAccountGroupMock(this Fixture fixture, int? number = null, string name = null, AccountGroupType? accountGroupType = null, bool? deletable = null, DateTime? createdDateTime = null, string createdByIdentifier = null, DateTime? modifiedDateTime = null, string modifiedByIdentifier = null, bool hasCalculatedAccountGroupStatus = true, IAccountGroupStatus calculatedAccountGroupStatus = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccountGroup> accountGroupMock = new Mock<IAccountGroup>();
            accountGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            accountGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            accountGroupMock.Setup(m => m.AccountGroupType)
                .Returns(accountGroupType ?? fixture.Create<AccountGroupType>());
            accountGroupMock.Setup(m => m.IsProtected)
                .Returns(fixture.Create<bool>());
            accountGroupMock.Setup(m => m.Deletable)
                .Returns(deletable ?? fixture.Create<bool>());
            accountGroupMock.Setup(m => m.CreatedDateTime)
                .Returns(createdDateTime ?? fixture.Create<DateTime>());
            accountGroupMock.Setup(m => m.CreatedByIdentifier)
                .Returns(createdByIdentifier ?? fixture.Create<string>());
            accountGroupMock.Setup(m => m.ModifiedDateTime)
                .Returns(modifiedDateTime ?? fixture.Create<DateTime>());
            accountGroupMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(modifiedByIdentifier ?? fixture.Create<string>());
            accountGroupMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>(), It.IsAny<IAccountCollection>()))
                .Returns(Task.FromResult(hasCalculatedAccountGroupStatus ? calculatedAccountGroupStatus ?? fixture.BuildAccountGroupStatusMock(isEmpty: true).Object : null));
            return accountGroupMock;
        }

        public static Mock<IAccountGroupStatus> BuildAccountGroupStatusMock(this Fixture fixture, int? number = null, string name = null, AccountGroupType? accountGroupType = null, IAccountCollection accountCollection = null, IAccountCollectionValues valuesAtStatusDate = null, IAccountCollectionValues valuesAtEndOfLastMonthFromStatusDate = null, IAccountCollectionValues valuesAtEndOfLastYearFromStatusDate = null, DateTime? statusDate = null, bool hasCalculatedAccountGroupStatus = true, IAccountGroupStatus calculatedAccountGroupStatus = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccountGroupStatus> accountGroupStatusMock = new Mock<IAccountGroupStatus>();
            accountGroupStatusMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            accountGroupStatusMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            accountGroupStatusMock.Setup(m => m.AccountGroupType)
                .Returns(accountGroupType ?? fixture.Create<AccountGroupType>());
            accountGroupStatusMock.Setup(m => m.AccountCollection)
                .Returns(accountCollection ?? fixture.BuildAccountCollectionMock(isEmpty: isEmpty).Object);
            accountGroupStatusMock.Setup(m => m.ValuesAtStatusDate)
                .Returns(valuesAtStatusDate ?? fixture.BuildAccountCollectionValuesMock().Object);
            accountGroupStatusMock.Setup(m => m.ValuesAtEndOfLastMonthFromStatusDate)
                .Returns(valuesAtEndOfLastMonthFromStatusDate ?? fixture.BuildAccountCollectionValuesMock().Object);
            accountGroupStatusMock.Setup(m => m.ValuesAtEndOfLastYearFromStatusDate)
                .Returns(valuesAtEndOfLastYearFromStatusDate ?? fixture.BuildAccountCollectionValuesMock().Object);
            accountGroupStatusMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            accountGroupStatusMock.Setup(m => m.IsProtected)
                .Returns(fixture.Create<bool>());
            accountGroupStatusMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountGroupStatusMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountGroupStatusMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountGroupStatusMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountGroupStatusMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            accountGroupStatusMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedAccountGroupStatus ? calculatedAccountGroupStatus ?? accountGroupStatusMock.Object : null));
            return accountGroupStatusMock;
        }

        public static Mock<IBudgetAccountGroup> BuildBudgetAccountGroupMock(this Fixture fixture, int? number = null, string name = null, bool? deletable = null, DateTime? createdDateTime = null, string createdByIdentifier = null, DateTime? modifiedDateTime = null, string modifiedByIdentifier = null, bool hasCalculatedBudgetAccountGroupStatus = true, IBudgetAccountGroupStatus calculatedBudgetAccountGroupStatus = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccountGroup> budgetAccountGroupMock = new Mock<IBudgetAccountGroup>();
            budgetAccountGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            budgetAccountGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.IsProtected)
                .Returns(fixture.Create<bool>());
            budgetAccountGroupMock.Setup(m => m.Deletable)
                .Returns(deletable ?? fixture.Create<bool>());
            budgetAccountGroupMock.Setup(m => m.CreatedDateTime)
                .Returns(createdDateTime ?? fixture.Create<DateTime>());
            budgetAccountGroupMock.Setup(m => m.CreatedByIdentifier)
                .Returns(createdByIdentifier ?? fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.ModifiedDateTime)
                .Returns(modifiedDateTime ?? fixture.Create<DateTime>());
            budgetAccountGroupMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(modifiedByIdentifier ?? fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>(), It.IsAny<IBudgetAccountCollection>()))
                .Returns(Task.FromResult(hasCalculatedBudgetAccountGroupStatus ? calculatedBudgetAccountGroupStatus ?? fixture.BuildBudgetAccountGroupStatusMock(isEmpty: true).Object : null));
            return budgetAccountGroupMock;
        }

        public static Mock<IBudgetAccountGroupStatus> BuildBudgetAccountGroupStatusMock(this Fixture fixture, int? number = null, string name = null, IBudgetAccountCollection budgetAccountCollection = null, IBudgetInfoValues valuesForMonthOfStatusDate = null, IBudgetInfoValues valuesForLastMonthOfStatusDate = null, IBudgetInfoValues valuesForYearToDateOfStatusDate = null, IBudgetInfoValues valuesForLastYearOfStatusDate = null, DateTime? statusDate = null, bool hasCalculatedBudgetAccountGroupStatus = true, IBudgetAccountGroupStatus calculatedBudgetAccountGroupStatus = null, bool isEmpty = false)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = new Mock<IBudgetAccountGroupStatus>();
            budgetAccountGroupStatusMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            budgetAccountGroupStatusMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            budgetAccountGroupStatusMock.Setup(m => m.BudgetAccountCollection)
                .Returns(budgetAccountCollection ?? fixture.BuildBudgetAccountCollectionMock(isEmpty: isEmpty).Object);
            budgetAccountGroupStatusMock.Setup(m => m.ValuesForMonthOfStatusDate)
                .Returns(valuesForMonthOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountGroupStatusMock.Setup(m => m.ValuesForLastMonthOfStatusDate)
                .Returns(valuesForLastMonthOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountGroupStatusMock.Setup(m => m.ValuesForYearToDateOfStatusDate)
                .Returns(valuesForYearToDateOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountGroupStatusMock.Setup(m => m.ValuesForLastYearOfStatusDate)
                .Returns(valuesForLastYearOfStatusDate ?? fixture.BuildBudgetInfoValuesMock().Object);
            budgetAccountGroupStatusMock.Setup(m => m.StatusDate)
                .Returns(statusDate?.Date ?? fixture.Create<DateTime>().Date);
            budgetAccountGroupStatusMock.Setup(m => m.IsProtected)
                .Returns(fixture.Create<bool>());
            budgetAccountGroupStatusMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            budgetAccountGroupStatusMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountGroupStatusMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetAccountGroupStatusMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountGroupStatusMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetAccountGroupStatusMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(hasCalculatedBudgetAccountGroupStatus ? calculatedBudgetAccountGroupStatus ?? budgetAccountGroupStatusMock.Object : null));
            return budgetAccountGroupStatusMock;
        }

        public static Mock<IPaymentTerm> BuildPaymentTermMock(this Fixture fixture, int? number = null, string name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPaymentTerm> paymentTermMock = new Mock<IPaymentTerm>();
            paymentTermMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            paymentTermMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            paymentTermMock.Setup(m => m.IsProtected)
                .Returns(fixture.Create<bool>());
            paymentTermMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            paymentTermMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            paymentTermMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            paymentTermMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            paymentTermMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return paymentTermMock;
        }
    }
}