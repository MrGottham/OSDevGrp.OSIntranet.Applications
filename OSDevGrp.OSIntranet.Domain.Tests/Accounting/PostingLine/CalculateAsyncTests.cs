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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            accountingMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            IPostingLine sut = CreateSut(account: accountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            accountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCreditInfoCollectionWasCalledOnCalculatedAccount()
        {
            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            calculatedAccountMock.Verify(m => m.CreditInfoCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertFindWasCalledOnCreditInfoCollectionFromCalculatedAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            Mock<ICreditInfoCollection> creditInfoCollectionMock = _fixture.BuildCreditInfoCollectionMock();
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollectionMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(postingDate, account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            creditInfoCollectionMock.Verify(m => m.Find(It.Is<DateTime>(value => value == postingDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnCalculatedAccount()
        {
            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            calculatedAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromCalculatedAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount calculatedAccount = _fixture.BuildAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, account, sortOrder: sortOrder);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_AssertCalculateAsyncWasCalledOnBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_AssertBudgetInfoCollectionWasCalledOnCalculatedBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            calculatedBudgetAccountMock.Verify(m => m.BudgetInfoCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_AssertFindWasCalledOnBudgetInfoCollectionFromCalculatedBudgetAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = _fixture.BuildBudgetInfoCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, budgetInfoCollection: budgetInfoCollectionMock.Object).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(postingDate, account, budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetInfoCollectionMock.Verify(m => m.Find(It.Is<DateTime>(value => value == postingDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_AssertPostingLineCollectionWasCalledOnCalculatedBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            calculatedBudgetAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromCalculatedBudgetAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, postingLineCollection: postingLineCollectionMock.Object).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, account, budgetAccount, sortOrder: sortOrder);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(postingDate.Year, postingDate.Month, 1)),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_AssertCalculateAsyncWasCalledOnContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            contactAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_AssertPostingLineCollectionWasCalledOnCalculatedContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> calculatedContactAccountMock = _fixture.BuildContactAccountMock(accounting);
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, calculatedContactAccount: calculatedContactAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            calculatedContactAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromCalculatedContactAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting, postingLineCollection: postingLineCollectionMock.Object).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, calculatedContactAccount: calculatedContactAccount).Object;
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, account, contactAccount: contactAccount, sortOrder: sortOrder);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
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
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineWhereAccountingEqualToCalculatedAccounting()
        {
            IAccounting calculatedAccounting = _fixture.BuildAccountingMock().Object;
            IAccounting accounting = _fixture.BuildAccountingMock(calculatedAccounting: calculatedAccounting).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Accounting, Is.EqualTo(calculatedAccounting));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineWhereAccountEqualToCalculatedAccount()
        {
            IAccount calculatedAccount = _fixture.BuildAccountMock().Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Account, Is.EqualTo(calculatedAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineWhereAccountValuesAtPostingDateNotEqualToNull()
        {
            IAccount account = _fixture.BuildAccountMock().Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.AccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithAccountWhereCalculatedAccountDoesNotHaveCreditInfoForPostingDate_ReturnsSamePostingLineWhereCreditOnAccountValuesAtPostingDateEqualToZero()
        {
            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock(hasCreditInfoForFind: false, isEmpty: true).Object;
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollection).Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.AccountValuesAtPostingDate.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithAccountWhereCalculatedAccountHasCreditInfoForPostingDate_ReturnsSamePostingLineWhereCreditOnAccountValuesAtPostingDateEqualToCreditOnCreditInfoForPostingDate()
        {
            ICreditInfo creditInfo = _fixture.BuildCreditInfoMock().Object;
            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock(creditInfoForFind: creditInfo, isEmpty: true).Object;
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollection).Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.AccountValuesAtPostingDate.Credit, Is.EqualTo(creditInfo.Credit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingLineWhereBalanceOnAccountValuesAtPostingDateEqualToCalculatedPostingValueFromPostingLineCollection()
        {
            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue, isEmpty: true).Object;
            IAccount calculatedAccount = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.AccountValuesAtPostingDate.Balance, Is.EqualTo(calculatedPostingValue));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountEqualToCalculatedBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccount, Is.EqualTo(calculatedBudgetAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateNotEqualToNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccountWhereCalculatedBudgetAccountDoesNotHaveBudgetInfoForPostingDate_ReturnsSamePostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToZero()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock(hasBudgetInfoForFind: false, isEmpty: true).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(budgetInfoCollection: budgetInfoCollection).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccountValuesAtPostingDate.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccountWhereCalculatedBudgetAccountHasBudgetInfoForPostingDate_ReturnsSamePostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToBudgetOnBudgetInfoForPostingDate()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IBudgetInfo budgetInfo = _fixture.BuildBudgetInfoMock().Object;
            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock(budgetInfoForFind: budgetInfo, isEmpty: true).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(budgetInfoCollection: budgetInfoCollection).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccountValuesAtPostingDate.Budget, Is.EqualTo(budgetInfo.Budget));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithBudgetAccount_ReturnsSamePostingLineWherePostedOnBudgetAccountValuesAtPostingDateEqualToCalculatedPostingValueFromPostingLineCollection()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue, isEmpty: true).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccountValuesAtPostingDate.Posted, Is.EqualTo(calculatedPostingValue));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountEqualToNull()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccount, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutBudgetAccount_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateEqualToNull()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereContactAccountEqualToCalculatedContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, calculatedContactAccount: calculatedContactAccount).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.ContactAccount, Is.EqualTo(calculatedContactAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateNotEqualToNull()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithContactAccount_ReturnsSamePostingLineWhereBalanceOnContactAccountValuesAtPostingDateEqualToCalculatedPostingValueFromPostingLineCollection()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue, isEmpty: true).Object;
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, calculatedContactAccount: calculatedContactAccount).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.ContactAccountValuesAtPostingDate.Balance, Is.EqualTo(calculatedPostingValue));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutContactAccount_ReturnsSamePostingLineWhereContactAccountEqualToNull()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

            Assert.That(result.ContactAccount, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledOnPostingLineWithoutContactAccount_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateEqualToNull()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await sut.CalculateAsync(statusDate);

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
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnAccounting()
        {
            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount account = _fixture.BuildAccountMock(accountingMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountingMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            IPostingLine sut = CreateSut(account: accountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCreditInfoCollectionWasCalledOnlyOnceOnCalculatedAccount()
        {
            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            calculatedAccountMock.Verify(m => m.CreditInfoCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertFindWasCalledOnlyOnceOnCreditInfoCollectionFromCalculatedAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            Mock<ICreditInfoCollection> creditInfoCollectionMock = _fixture.BuildCreditInfoCollectionMock();
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollectionMock.Object).Object;
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccount).Object;
            IPostingLine sut = CreateSut(postingDate, account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            creditInfoCollectionMock.Verify(m => m.Find(It.Is<DateTime>(value => value == postingDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertPostingLineCollectionWasCalledOnlyOnceOnCalculatedAccount()
        {
            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            IAccount account = _fixture.BuildAccountMock(calculatedAccount: calculatedAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            calculatedAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculatePostingValueWasCalledOnlyOnceOnPostingLineCollectionFromCalculatedAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, postingLineCollection: postingLineCollectionMock.Object).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, account, budgetAccount, sortOrder: sortOrder);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(postingDate.Year, postingDate.Month, 1)),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithBudgetAccount_AssertCalculateAsyncWasCalledOnlyOnceOnBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            budgetAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithBudgetAccount_AssertBudgetInfoCollectionWasCalledOnlyOnceOnCalculatedBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            calculatedBudgetAccountMock.Verify(m => m.BudgetInfoCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithBudgetAccount_AssertFindWasCalledOnlyOnceOnBudgetInfoCollectionFromCalculatedBudgetAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = _fixture.BuildBudgetInfoCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, budgetInfoCollection: budgetInfoCollectionMock.Object).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            IPostingLine sut = CreateSut(postingDate, account, budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            budgetInfoCollectionMock.Verify(m => m.Find(It.Is<DateTime>(value => value == postingDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithBudgetAccount_AssertPostingLineCollectionWasCalledOnlyOnceOnCalculatedBudgetAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock(accounting);
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account, budgetAccount: budgetAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            calculatedBudgetAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithBudgetAccount_AssertCalculatePostingValueWasCalledOnlyOnceOnPostingLineCollectionFromCalculatedBudgetAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, postingLineCollection: postingLineCollectionMock.Object).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, calculatedBudgetAccount: calculatedBudgetAccount).Object;
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, account, budgetAccount, sortOrder: sortOrder);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(postingDate.Year, postingDate.Month, 1)),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithContactAccount_AssertCalculateAsyncWasCalledOnlyOnceOnContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock(accounting);
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccountMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            contactAccountMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithContactAccount_AssertPostingLineCollectionWasCalledOnlyOnceOnCalculatedContactAccount()
        {
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IContactAccount> calculatedContactAccountMock = _fixture.BuildContactAccountMock(accounting);
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, calculatedContactAccount: calculatedContactAccountMock.Object).Object;
            IPostingLine sut = CreateSut(account: account, contactAccount: contactAccount);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            calculatedContactAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDateOnPostingLineWithContactAccount_AssertCalculatePostingValueWasCalledOnlyOnceOnPostingLineCollectionFromCalculatedContactAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IAccounting accounting = _fixture.BuildAccountingMock().Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting, postingLineCollection: postingLineCollectionMock.Object).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, calculatedContactAccount: calculatedContactAccount).Object;
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, account, contactAccount: contactAccount, sortOrder: sortOrder);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IPostingLine result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLine CreateSut(DateTime? postingDate = null, IAccount account = null, IBudgetAccount budgetAccount = null, IContactAccount contactAccount = null, int? sortOrder = null)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, Math.Min(DateTime.Today.Year, InfoBase<ICreditInfo>.MaxYear));
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, Math.Min(DateTime.Today.Month, InfoBase<ICreditInfo>.MaxMonth));
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), postingDate ?? new DateTime(year, month, day), _fixture.Create<string>(), account ?? _fixture.BuildAccountMock().Object, _fixture.Create<string>(), budgetAccount, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), contactAccount, sortOrder ?? Math.Abs(_fixture.Create<int>()));
        }
    }
}