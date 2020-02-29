using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class AccountingMockBuilder
    {
        public static Mock<IAccounting> BuildAccountingMock(this Fixture fixture, int? accountingNumber = null, IAccountCollection accountCollection = null, IBudgetAccountCollection budgetAccountCollection = null, IContactAccountCollection contactAccountCollection = null, bool hasCalculatedAccounting = true, IAccounting calculatedAccounting = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccounting> accountingMock = new Mock<IAccounting>();
            accountingMock.Setup(m => m.Number)
                .Returns(accountingNumber ?? fixture.Create<int>());
            accountingMock.Setup(m => m.Name)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.LetterHead)
                .Returns(fixture.BuildLetterHeadMock().Object);
            accountingMock.Setup(m => m.BalanceBelowZero)
                .Returns(fixture.Create<BalanceBelowZeroType>());
            accountingMock.Setup(m => m.BackDating)
                .Returns(fixture.Create<int>());
            accountingMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            accountingMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountingMock.Setup(m => m.DefaultForPrincipal)
                .Returns(fixture.Create<bool>());
            accountingMock.Setup(m => m.AccountCollection)
                .Returns(accountCollection ?? fixture.BuildAccountCollectionMock(accountingMock.Object).Object);
            accountingMock.Setup(m => m.BudgetAccountCollection)
                .Returns(budgetAccountCollection ?? fixture.BuildBudgetAccountCollectionMock(accountingMock.Object).Object);
            accountingMock.Setup(m => m.ContactAccountCollection)
                .Returns(contactAccountCollection ?? fixture.BuildContactAccountCollectionMock(accountingMock.Object).Object);
            accountingMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountingMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountingMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            accountingMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => hasCalculatedAccounting ? calculatedAccounting ?? accountingMock.Object : null));
            return accountingMock;
        }

        public static Mock<IAccount> BuildAccountMock(this Fixture fixture, IAccounting accounting = null, IAccount calculatedAccount = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccount> accountMock = new Mock<IAccount>();
            accountMock.Setup(m => m.Accounting)
                .Returns(accounting ?? fixture.BuildAccountingMock().Object);
            accountMock.Setup(m => m.AccountNumber)
                .Returns(fixture.Create<string>().ToUpper());
            accountMock.Setup(m => m.AccountName)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.Description)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.Note)
                .Returns(fixture.Create<string>());
            accountMock.Setup(m => m.AccountGroup)
                .Returns(fixture.BuildAccountGroupMock().Object);
            accountMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
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
            accountMock.Setup(m => m.GetHashCode())
                .Returns(accountMock.GetHashCode());
            accountMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && accountMock.GetHashCode() == m.GetHashCode());
            accountMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => calculatedAccount ?? accountMock.Object));
            return accountMock;
        }

        public static Mock<IAccountCollection> BuildAccountCollectionMock(this Fixture fixture, IAccounting accounting = null, IEnumerable<IAccount> accountCollection = null, IAccountCollection calculatedAccountCollection = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            if (accounting == null)
            {
                accounting = fixture.BuildAccountingMock().Object;
            }

            if (accountCollection == null)
            {
                Random random = new Random(fixture.Create<int>());
                int numberOfAccounts = random.Next(1, 10);

                IList<IAccount> accounts = new List<IAccount>(numberOfAccounts);
                while (accounts.Count < numberOfAccounts)
                {
                    accounts.Add(fixture.BuildAccountMock(accounting).Object);
                }

                accountCollection = accounts;
            }

            Mock<IAccountCollection> accountCollectionMock = new Mock<IAccountCollection>();
            accountCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(accountCollection.GetEnumerator());
            accountCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            accountCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => calculatedAccountCollection ?? accountCollectionMock.Object));
            return accountCollectionMock;
        }

        public static Mock<IBudgetAccount> BuildBudgetAccountMock(this Fixture fixture, IAccounting accounting = null, IBudgetAccount calculatedBudgetAccount = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccount> budgetAccountMock = new Mock<IBudgetAccount>();
            budgetAccountMock.Setup(m => m.Accounting)
                .Returns(accounting ?? fixture.BuildAccountingMock().Object);
            budgetAccountMock.Setup(m => m.AccountNumber)
                .Returns(fixture.Create<string>().ToUpper());
            budgetAccountMock.Setup(m => m.AccountName)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.Description)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.Note)
                .Returns(fixture.Create<string>());
            budgetAccountMock.Setup(m => m.BudgetAccountGroup)
                .Returns(fixture.BuildBudgetAccountGroupMock().Object);
            budgetAccountMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
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
            budgetAccountMock.Setup(m => m.GetHashCode())
                .Returns(budgetAccountMock.GetHashCode());
            budgetAccountMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && budgetAccountMock.GetHashCode() == m.GetHashCode());
            budgetAccountMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => calculatedBudgetAccount ?? budgetAccountMock.Object));
            return budgetAccountMock;
        }

        public static Mock<IBudgetAccountCollection> BuildBudgetAccountCollectionMock(this Fixture fixture, IAccounting accounting = null, IEnumerable<IBudgetAccount> budgetAccountCollection = null, IBudgetAccountCollection calculatedBudgetAccountCollection = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            if (accounting == null)
            {
                accounting = fixture.BuildAccountingMock().Object;
            }

            if (budgetAccountCollection == null)
            {
                Random random = new Random(fixture.Create<int>());
                int numberOfBudgetAccounts = random.Next(1, 10);

                IList<IBudgetAccount> budgetAccounts = new List<IBudgetAccount>(numberOfBudgetAccounts);
                while (budgetAccounts.Count < numberOfBudgetAccounts)
                {
                    budgetAccounts.Add(fixture.BuildBudgetAccountMock(accounting).Object);
                }

                budgetAccountCollection = budgetAccounts;
            }

            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = new Mock<IBudgetAccountCollection>();
            budgetAccountCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(budgetAccountCollection.GetEnumerator());
            budgetAccountCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            budgetAccountCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => calculatedBudgetAccountCollection ?? budgetAccountCollectionMock.Object));
            return budgetAccountCollectionMock;
        }

        public static Mock<IContactAccount> BuildContactAccountMock(this Fixture fixture, IAccounting accounting = null, IContactAccount calculatedContactAccount = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IContactAccount> contactAccountMock = new Mock<IContactAccount>();
            contactAccountMock.Setup(m => m.Accounting)
                .Returns(accounting ?? fixture.BuildAccountingMock().Object);
            contactAccountMock.Setup(m => m.AccountNumber)
                .Returns(fixture.Create<string>().ToUpper());
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
                .Returns(fixture.BuildPaymentTermMock().Object);
            contactAccountMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
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
            contactAccountMock.Setup(m => m.GetHashCode())
                .Returns(contactAccountMock.GetHashCode());
            contactAccountMock.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns<object>(m => m != null && contactAccountMock.GetHashCode() == m.GetHashCode());
            contactAccountMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => calculatedContactAccount ?? contactAccountMock.Object));
            return contactAccountMock;
        }

        public static Mock<IContactAccountCollection> BuildContactAccountCollectionMock(this Fixture fixture, IAccounting accounting = null, IEnumerable<IContactAccount> contactAccountCollection = null, IContactAccountCollection calculatedContactAccountCollection = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            if (accounting == null)
            {
                accounting = fixture.BuildAccountingMock().Object;
            }

            if (contactAccountCollection == null)
            {
                Random random = new Random(fixture.Create<int>());
                int numberOfContactAccounts = random.Next(1, 10);

                IList<IContactAccount> contactAccounts = new List<IContactAccount>(numberOfContactAccounts);
                while (contactAccounts.Count < numberOfContactAccounts)
                {
                    contactAccounts.Add(fixture.BuildContactAccountMock(accounting).Object);
                }

                contactAccountCollection = contactAccounts;
            }

            Mock<IContactAccountCollection> contactAccountCollectionMock = new Mock<IContactAccountCollection>();
            contactAccountCollectionMock.Setup(m => m.GetEnumerator())
                .Returns(contactAccountCollection.GetEnumerator());
            contactAccountCollectionMock.Setup(m => m.StatusDate)
                .Returns(fixture.Create<DateTime>().Date);
            contactAccountCollectionMock.Setup(m => m.CalculateAsync(It.IsAny<DateTime>()))
                .Returns(Task.Run(() => calculatedContactAccountCollection ?? contactAccountCollectionMock.Object));
            return contactAccountCollectionMock;
        }

        public static Mock<IAccountGroup> BuildAccountGroupMock(this Fixture fixture, int? number = null, string name = null, AccountGroupType? accountGroupType = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IAccountGroup> accountGroupMock = new Mock<IAccountGroup>();
            accountGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            accountGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            accountGroupMock.Setup(m => m.AccountGroupType)
                .Returns(accountGroupType ?? fixture.Create<AccountGroupType>());
            accountGroupMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            accountGroupMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountGroupMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            accountGroupMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            accountGroupMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return accountGroupMock;
        }

        public static Mock<IBudgetAccountGroup> BuildBudgetAccountGroupMock(this Fixture fixture, int? number = null, string name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IBudgetAccountGroup> budgetAccountGroupMock = new Mock<IBudgetAccountGroup>();
            budgetAccountGroupMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            budgetAccountGroupMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.Deletable)
                .Returns(fixture.Create<bool>());
            budgetAccountGroupMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountGroupMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            budgetAccountGroupMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            budgetAccountGroupMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return budgetAccountGroupMock;
        }

        public static Mock<IPaymentTerm> BuildPaymentTermMock(this Fixture fixture, int? number = null, string name = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IPaymentTerm> paymentTermMock = new Mock<IPaymentTerm>();
            paymentTermMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            paymentTermMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            return paymentTermMock;
        }
    }
}