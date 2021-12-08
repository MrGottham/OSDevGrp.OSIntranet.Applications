using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class CalculateAsyncTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccounting_AssertStatusDateWasNotCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            accountingMock.Verify(m => m.StatusDate, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccounting_AssertCalculateAsyncWasNotCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            accountingMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccount_AssertStatusDateWasCalledOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            IPostingLine sut = CreateSut(account: accountMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            accountMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccountWhereStatusDateDoesNotMatchStatusDateFromArgument_AssertCalculateAsyncWasCalledOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock(statusDate: DateTime.MinValue);
            IPostingLine sut = CreateSut(account: accountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            accountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccountWhereStatusDateMatchesStatusDateFromArgument_AssertCalculateAsyncWasNotCalledOnAccount()
        {
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            Mock<IAccount> accountMock = _fixture.BuildAccountMock(statusDate: statusDate);
            IPostingLine sut = CreateSut(account: accountMock.Object);

            await sut.CalculateAsync(statusDate);

            accountMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_AssertStatusDateWasCalledOnBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            budgetAccountMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccountWhereStatusDateDoesNotMatchStatusDateFromArgument_AssertCalculateAsyncWasCalledOnBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting, statusDate: DateTime.MinValue);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccountWhereStatusDateMatchesStatusDateFromArgument_AssertCalculateAsyncWasNotCalledOnBudgetAccount()
        {
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting, statusDate: statusDate);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            await sut.CalculateAsync(statusDate);

            budgetAccountMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_AssertStatusDateWasCalledOnContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            contactAccountMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccountWhereStatusDateDoesNotMatchStatusDateFromArgument_AssertCalculateAsyncWasCalledOnContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting, statusDate: DateTime.MinValue);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            contactAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccountWhereStatusDateMatchesStatusDateFromArgument_AssertCalculateAsyncWasNotCalledOnContactAccount()
        {
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting, statusDate: statusDate);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            await sut.CalculateAsync(statusDate);

            contactAccountMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccounting_ReturnsSamePostingLineWhereAccountingIsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock(statusDate: DateTime.MinValue).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccounting_ReturnsSamePostingLineWhereAccountingHasNotBeenChanged()
        {
            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IAccounting accounting = _fixture.BuildAccountingMock(statusDate: DateTime.MinValue, calculatedAccounting: calculatedAccounting).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.Accounting, Is.SameAs(accounting));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccount_ReturnsSamePostingLineWhereAccountIsNotNull()
        {
            IAccount account = _fixture.BuildAccountMock(statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.Account, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccount_ReturnsSamePostingLineWhereAccountHasNotBeenChanged()
        {
            IAccount calculatedAccount = _fixture.BuildAccountMock().Object;
            IAccount account = _fixture.BuildAccountMock(statusDate: DateTime.MinValue, calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.Account, Is.SameAs(account));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccount_ReturnsSamePostingLineWhereAccountValuesAtPostingDateIsNotNull()
        {
            IAccount account = _fixture.BuildAccountMock(statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithAccount_ReturnsSamePostingLineWhereAccountValuesAtPostingDateHasNotBeenChanged()
        {
            IAccount account = _fixture.BuildAccountMock(statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account);

            ICreditInfoValues accountValuesAtPostingDate = sut.AccountValuesAtPostingDate;
            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountValuesAtPostingDate, Is.SameAs(accountValuesAtPostingDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountIsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccount, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountHasNotBeenChanged()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock().Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, statusDate: DateTime.MinValue, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccount, Is.SameAs(budgetAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateIsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateHasNotBeenChanged()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            IBudgetInfoValues budgetAccountValuesAtPostingDate = sut.BudgetAccountValuesAtPostingDate;
            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.SameAs(budgetAccountValuesAtPostingDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountIsNull()
        {
            IAccount account = _fixture.BuildAccountMock().Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccount, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateIsNull(bool hasBudgetAccountValuesAtPostingDate)
        {
            IAccount account = _fixture.BuildAccountMock().Object;
            IPostingLine sut = CreateSut(account: account, budgetAccountValuesAtPostingDate: hasBudgetAccountValuesAtPostingDate ? _fixture.BuildBudgetInfoValuesMock().Object : null);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereContactAccountIsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccount, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereContactAccountHasNotBeenChanged()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock().Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, statusDate: DateTime.MinValue, calculatedContactAccount: calculatedContactAccount).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccount, Is.SameAs(contactAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateIsNotNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateHasNotBeenChanged()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, statusDate: DateTime.MinValue).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            IContactInfoValues contactAccountValuesAtPostingDate = sut.ContactAccountValuesAtPostingDate;
            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.SameAs(contactAccountValuesAtPostingDate));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutContactAccount_ReturnsSamePostingLineWhereContactAccountIsNull()
        {
            IAccount account = _fixture.BuildAccountMock().Object;
            IPostingLine sut = CreateSut(account: account);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccount, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutContactAccount_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateIsNull(bool hasContactAccountValuesAtPostingDate)
        {
            IAccount account = _fixture.BuildAccountMock().Object;
            IPostingLine sut = CreateSut(account: account, contactAccountValuesAtPostingDate: hasContactAccountValuesAtPostingDate ? _fixture.BuildContactInfoValuesMock().Object : null);

            IPostingLine result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineWhereStatusDateEqualDateFromCall()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithAccounting_AssertStatusDateWasNotCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountingMock.Verify(m => m.StatusDate, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithAccounting_AssertCalculateAsyncWasNotCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountingMock.Verify(m => m.CalculateAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithAccount_AssertStatusDateWasCalledOnlyOnceOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            IPostingLine sut = CreateSut(account: accountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithAccountWhereStatusDateDoesNotMatchStatusDateFromArgument_AssertCalculateAsyncWasCalledOnlyOnceOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock(statusDate: DateTime.MinValue);
            IPostingLine sut = CreateSut(account: accountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithBudgetAccount_AssertStatusDateWasCalledOnlyOnceOnBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            budgetAccountMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithBudgetAccountWhereStatusDateDoesNotMatchStatusDateFromArgument_AssertCalculateAsyncWasCalledOnlyOnceOnBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting, statusDate: DateTime.MinValue);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            budgetAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithContactAccount_AssertStatusDateWasCalledOnlyOnceOnContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            contactAccountMock.Verify(m => m.StatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesOnPostingLineWithContactAccountWhereStatusDateDoesNotMatchStatusDateFromArgument_AssertCalculateAsyncWasCalledOnlyOnceOnContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting, statusDate: DateTime.MinValue);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            contactAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimes_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLine CreateSut(DateTime? postingDate = null, IAccount account = null, IBudgetAccount budgetAccount = null, IContactAccount contactAccount = null, int? sortOrder = null, ICreditInfoValues accountValuesAtPostingDate = null, IBudgetInfoValues budgetAccountValuesAtPostingDate = null, IContactInfoValues contactAccountValuesAtPostingDate = null)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, Math.Min(DateTime.Today.Year, InfoBase<ICreditInfo>.MaxYear));
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, Math.Min(DateTime.Today.Month, InfoBase<ICreditInfo>.MaxMonth));
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), postingDate ?? new DateTime(year, month, day), _fixture.Create<string>(), account ?? _fixture.BuildAccountMock().Object, _fixture.Create<string>(), budgetAccount, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), contactAccount, sortOrder ?? Math.Abs(_fixture.Create<int>()), accountValuesAtPostingDate, budgetAccountValuesAtPostingDate, contactAccountValuesAtPostingDate);
        }
    }
}