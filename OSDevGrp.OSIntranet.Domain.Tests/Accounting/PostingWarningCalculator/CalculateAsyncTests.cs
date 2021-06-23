using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingWarningCalculator
{
    [TestFixture]
    public class CalculateAsyncTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void CalculateAsync_WhenPostingLineIsNull_ThrowsArgumentNullException()
        {
            IPostingWarningCalculator sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CalculateAsync((IPostingLine) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingLine"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertAccountWasCalledOnPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IPostingLine> postingLineMock = _fixture.BuildPostingLineMock();
            await sut.CalculateAsync(postingLineMock.Object);

            postingLineMock.Verify(m => m.Account, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertAccountGroupTypeWasCalledOnAccountForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock(isEmpty: true);
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: accountMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            accountMock.Verify(m => m.AccountGroupType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertAccountValuesAtPostingDateWasCalledOnPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IPostingLine> postingLineMock = _fixture.BuildPostingLineMock();
            await sut.CalculateAsync(postingLineMock.Object);

            postingLineMock.Verify(m => m.AccountValuesAtPostingDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssets_AssertCreditWasCalledOnAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            Mock<ICreditInfoValues> accountValuesAtPostingDateMock = _fixture.BuildCreditInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            accountValuesAtPostingDateMock.Verify(m => m.Credit, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssets_AssertBalanceWasCalledOnAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            Mock<ICreditInfoValues> accountValuesAtPostingDateMock = _fixture.BuildCreditInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            accountValuesAtPostingDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsLiabilities_AssertCreditWasNotCalledOnAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities, isEmpty: true).Object;
            Mock<ICreditInfoValues> accountValuesAtPostingDateMock = _fixture.BuildCreditInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            accountValuesAtPostingDateMock.Verify(m => m.Credit, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsLiabilities_AssertBalanceWasNotCalledOnAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities, isEmpty: true).Object;
            Mock<ICreditInfoValues> accountValuesAtPostingDateMock = _fixture.BuildCreditInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            accountValuesAtPostingDateMock.Verify(m => m.Balance, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertBudgetAccountWasCalledOnPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IPostingLine> postingLineMock = _fixture.BuildPostingLineMock();
            await sut.CalculateAsync(postingLineMock.Object);

            postingLineMock.Verify(m => m.BudgetAccount, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertBudgetAccountValuesAtPostingDateWasCalledOnPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IPostingLine> postingLineMock = _fixture.BuildPostingLineMock();
            await sut.CalculateAsync(postingLineMock.Object);

            postingLineMock.Verify(m => m.BudgetAccountValuesAtPostingDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWithBudgetAccountValuesAtPostingDate_AssertBudgetWasCalledOnBudgetAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            Mock<IBudgetInfoValues> budgetAccountValuesAtPostingDateMock = _fixture.BuildBudgetInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            budgetAccountValuesAtPostingDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWithBudgetAccountValuesAtPostingDate_AssertPostedWasCalledOnBudgetAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            Mock<IBudgetInfoValues> budgetAccountValuesAtPostingDateMock = _fixture.BuildBudgetInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            budgetAccountValuesAtPostingDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertContactAccountWasCalledOnPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IPostingLine> postingLineMock = _fixture.BuildPostingLineMock();
            await sut.CalculateAsync(postingLineMock.Object);

            postingLineMock.Verify(m => m.ContactAccount, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_AssertContactAccountValuesAtPostingDateWasCalledOnPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            Mock<IPostingLine> postingLineMock = _fixture.BuildPostingLineMock();
            await sut.CalculateAsync(postingLineMock.Object);

            postingLineMock.Verify(m => m.ContactAccountValuesAtPostingDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWithContactAccountValuesAtPostingDate_AssertBalanceWasNotCalledOnContactAccountValuesAtPostingDateForPostingLine()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object;
            Mock<IContactInfoValues> contactAccountValuesAtPostingDateMock = _fixture.BuildContactInfoValuesMock();
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, contactAccount: contactAccount, contactAccountValuesAtPostingDate: contactAccountValuesAtPostingDateMock.Object).Object;
            await sut.CalculateAsync(postingLine);

            contactAccountValuesAtPostingDateMock.Verify(m => m.Balance, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_ReturnsNotNull()
        {
            IPostingWarningCalculator sut = CreateSut();

            IPostingWarningCollection result = await sut.CalculateAsync(_fixture.BuildPostingLineMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLine_ReturnsPostingWarningCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IPostingWarningCollection result = await sut.CalculateAsync(_fixture.BuildPostingLineMock().Object);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingWarningCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateGreaterThanZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal balance = Math.Abs(_fixture.Create<decimal>());
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(balance: balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateEqualToZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            const decimal balance = 0M;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(balance: balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateLowerThanZeroAndNotExceedingCredit_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal balance = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal credit = Math.Abs(balance) + 250M;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateLowerThanZeroAndMatchingCredit_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal balance = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal credit = Math.Abs(balance);
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateLowerThanZeroAndExceedingCredit_ReturnsPostingWarningCollectionContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal credit = Math.Abs(_fixture.Create<decimal>());
            decimal balance = (credit + 250M) * -1;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            IPostingWarning postingWarning = result.SingleOrDefault(m => m.Reason == PostingWarningReason.AccountIsOverdrawn);
            Assert.That(postingWarning, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.EqualTo(account));
            Assert.That(postingWarning.Amount, Is.EqualTo(Math.Abs(balance) - credit));
            Assert.That(postingWarning.PostingLine, Is.Not.Null);
            Assert.That(postingWarning.PostingLine, Is.EqualTo(postingLine));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereAccountGroupTypeOnAccountIsLiabilities_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities, isEmpty: true).Object;
            decimal credit = Math.Abs(_fixture.Create<decimal>());
            decimal balance = (credit + 250M) * -1;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZeroAndPostedNotExceedingBudget_ReturnsPostingWarningCollectionContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal posted = Math.Abs(_fixture.Create<decimal>());
            decimal budget = posted + 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            IPostingWarning postingWarning = result.SingleOrDefault(m => m.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet);
            Assert.That(postingWarning, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.EqualTo(budgetAccount));
            Assert.That(postingWarning.Amount, Is.EqualTo(budget - posted));
            Assert.That(postingWarning.PostingLine, Is.Not.Null);
            Assert.That(postingWarning.PostingLine, Is.EqualTo(postingLine));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZeroAndPostedMatchingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>());
            decimal posted = budget;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZeroAndPostedExceedingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>());
            decimal posted = budget + 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            const decimal budget = 0M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>()) * -1;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>());
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            const decimal budget = 0M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZeroAndPostedNotExceedingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal posted = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal budget = posted - 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZeroAndPostedMatchingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal posted = budget;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZeroAndPostedExceedingBudget_ReturnsPostingWarningCollectionContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal posted = budget - 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLine);

            IPostingWarning postingWarning = result.SingleOrDefault(m => m.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached);
            Assert.That(postingWarning, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.EqualTo(budgetAccount));
            Assert.That(postingWarning.Amount, Is.EqualTo(Math.Abs(posted) - Math.Abs(budget)));
            Assert.That(postingWarning.PostingLine, Is.Not.Null);
            Assert.That(postingWarning.PostingLine, Is.EqualTo(postingLine));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculateAsync_WhenPostingLineCollectionIsNull_ThrowsArgumentNullException()
        {
            IPostingWarningCalculator sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CalculateAsync((IPostingLineCollection) null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("postingLineCollection"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertAccountWasCalledOnEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new[]
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Account, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertAccountGroupTypeWasCalledOnAccountForEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IAccount>> accountMockCollection = new[]
            {
                _fixture.BuildAccountMock(isEmpty: true),
                _fixture.BuildAccountMock(isEmpty: true),
                _fixture.BuildAccountMock(isEmpty: true),
                _fixture.BuildAccountMock(isEmpty: true),
                _fixture.BuildAccountMock(isEmpty: true),
                _fixture.BuildAccountMock(isEmpty: true),
                _fixture.BuildAccountMock(isEmpty: true)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: accountMockCollection.Select(m => _fixture.BuildPostingLineMock(account: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IAccount> accountMock in accountMockCollection)
            {
                accountMock.Verify(m => m.AccountGroupType, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertAccountValuesAtPostingDateWasCalledOnEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new[]
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.AccountValuesAtPostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertCreditWasCalledOnAccountValuesAtPostingDateForEachPostingLineWhereAccountGroupTypeOnAccountIsAssetsInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            IEnumerable<Mock<ICreditInfoValues>> accountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: accountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<ICreditInfoValues> accountValuesAtPostingDateMock in accountValuesAtPostingDateMockCollection)
            {
                accountValuesAtPostingDateMock.Verify(m => m.Credit, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertBalanceWasCalledOnAccountValuesAtPostingDateForEachPostingLineWhereAccountGroupTypeOnAccountIsAssetsInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            IEnumerable<Mock<ICreditInfoValues>> accountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: accountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<ICreditInfoValues> accountValuesAtPostingDateMock in accountValuesAtPostingDateMockCollection)
            {
                accountValuesAtPostingDateMock.Verify(m => m.Balance, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertCreditWasNotCalledOnAccountValuesAtPostingDateForEachPostingLineWhereAccountGroupTypeOnAccountIsLiabilitiesInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities, isEmpty: true).Object;
            IEnumerable<Mock<ICreditInfoValues>> accountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: accountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<ICreditInfoValues> accountValuesAtPostingDateMock in accountValuesAtPostingDateMockCollection)
            {
                accountValuesAtPostingDateMock.Verify(m => m.Credit, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertBalanceWasNotCalledOnAccountValuesAtPostingDateForEachPostingLineWhereAccountGroupTypeOnAccountIsLiabilitiesInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities, isEmpty: true).Object;
            IEnumerable<Mock<ICreditInfoValues>> accountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock(),
                _fixture.BuildCreditInfoValuesMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: accountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<ICreditInfoValues> accountValuesAtPostingDateMock in accountValuesAtPostingDateMockCollection)
            {
                accountValuesAtPostingDateMock.Verify(m => m.Balance, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertBudgetAccountWasCalledOnEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new[]
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.BudgetAccount, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertBudgetAccountValuesAtPostingDateWasCalledOnEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new[]
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.BudgetAccountValuesAtPostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertBudgetWasCalledOnBudgetAccountValuesAtPostingDateForEachPostingLineWithBudgetAccountValuesAtPostingDateInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetAccountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: budgetAccountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IBudgetInfoValues> budgetAccountValuesAtPostingDateMock in budgetAccountValuesAtPostingDateMockCollection)
            {
                budgetAccountValuesAtPostingDateMock.Verify(m => m.Budget, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertPostedWasCalledOnBudgetAccountValuesAtPostingDateForEachPostingLineWithBudgetAccountValuesAtPostingDateInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IBudgetInfoValues>> budgetAccountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock(),
                _fixture.BuildBudgetInfoValuesMock()
            };
            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: budgetAccountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IBudgetInfoValues> budgetAccountValuesAtPostingDateMock in budgetAccountValuesAtPostingDateMockCollection)
            {
                budgetAccountValuesAtPostingDateMock.Verify(m => m.Posted, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertContactAccountWasCalledOnEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new[]
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ContactAccount, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertContactAccountValuesAtPostingDateWasCalledOnEachPostingLineInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new[]
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ContactAccountValuesAtPostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_AssertBalanceWasNotCalledOnContactAccountValuesAtPostingDateForEachPostingLineWithContactAccountValuesAtPostingDateInPostingLineCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IEnumerable<Mock<IContactInfoValues>> contactAccountValuesAtPostingDateMockCollection = new[]
            {
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock(),
                _fixture.BuildContactInfoValuesMock()
            };
            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, isEmpty: true).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: contactAccountValuesAtPostingDateMockCollection.Select(m => _fixture.BuildPostingLineMock(account: account, contactAccount: contactAccount, contactAccountValuesAtPostingDate: m.Object).Object)).Object;
            await sut.CalculateAsync(postingLineCollection);

            foreach (Mock<IContactInfoValues> contactAccountValuesAtPostingDateMock in contactAccountValuesAtPostingDateMockCollection)
            {
                contactAccountValuesAtPostingDateMock.Verify(m => m.Balance, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_ReturnsNotNull()
        {
            IPostingWarningCalculator sut = CreateSut();

            IPostingWarningCollection result = await sut.CalculateAsync(_fixture.BuildPostingLineCollectionMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollection_ReturnsPostingWarningCollection()
        {
            IPostingWarningCalculator sut = CreateSut();

            IPostingWarningCollection result = await sut.CalculateAsync(_fixture.BuildPostingLineCollectionMock().Object);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingWarningCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateGreaterThanZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal balance = Math.Abs(_fixture.Create<decimal>());
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(balance: balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] {postingLine}).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateEqualToZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            const decimal balance = 0M;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(balance: balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateLowerThanZeroAndNotExceedingCredit_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal balance = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal credit = Math.Abs(balance) + 250M;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateLowerThanZeroAndMatchingCredit_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal balance = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal credit = Math.Abs(balance);
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereAccountGroupTypeOnAccountIsAssetsAndBalanceOnAccountValuesAtPostingDateLowerThanZeroAndExceedingCredit_ReturnsPostingWarningCollectionContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Assets, isEmpty: true).Object;
            decimal credit = Math.Abs(_fixture.Create<decimal>());
            decimal balance = (credit + 250M) * -1;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            IPostingWarning postingWarning = result.SingleOrDefault(m => m.Reason == PostingWarningReason.AccountIsOverdrawn);
            Assert.That(postingWarning, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.EqualTo(account));
            Assert.That(postingWarning.Amount, Is.EqualTo(Math.Abs(balance) - credit));
            Assert.That(postingWarning.PostingLine, Is.Not.Null);
            Assert.That(postingWarning.PostingLine, Is.EqualTo(postingLine));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereAccountGroupTypeOnAccountIsLiabilities_ReturnsPostingWarningCollectionNotContainingPostingWarningForAccountIsOverdrawn()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccount account = _fixture.BuildAccountMock(accountGroupType: AccountGroupType.Liabilities, isEmpty: true).Object;
            decimal credit = Math.Abs(_fixture.Create<decimal>());
            decimal balance = (credit + 250M) * -1;
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock(credit, balance).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, accountValuesAtPostingDate: accountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.AccountIsOverdrawn), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZeroAndPostedNotExceedingBudget_ReturnsPostingWarningCollectionContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal posted = Math.Abs(_fixture.Create<decimal>());
            decimal budget = posted + 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            IPostingWarning postingWarning = result.SingleOrDefault(m => m.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet);
            Assert.That(postingWarning, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.EqualTo(budgetAccount));
            Assert.That(postingWarning.Amount, Is.EqualTo(budget - posted));
            Assert.That(postingWarning.PostingLine, Is.Not.Null);
            Assert.That(postingWarning.PostingLine, Is.EqualTo(postingLine));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZeroAndPostedMatchingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>());
            decimal posted = budget;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZeroAndPostedExceedingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>());
            decimal posted = budget + 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            const decimal budget = 0M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedIncomeHasNotBeenReachedYet()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>()) * -1;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateGreaterThanZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>());
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToZero_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            const decimal budget = 0M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZeroAndPostedNotExceedingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal posted = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal budget = posted - 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZeroAndPostedMatchingBudget_ReturnsPostingWarningCollectionNotContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal posted = budget;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);

            Assert.That(result.Any(postingWarning => postingWarning.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledWithPostingLineCollectionContainingPostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateLowerThanZeroAndPostedExceedingBudget_ReturnsPostingWarningCollectionContainingPostingWarningForExpectedExpensesHaveAlreadyBeenReached()
        {
            IPostingWarningCalculator sut = CreateSut();

            IAccounting accounting = _fixture.BuildAccountingMock(isEmpty: true).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, isEmpty: true).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, isEmpty: true).Object;
            decimal budget = Math.Abs(_fixture.Create<decimal>()) * -1;
            decimal posted = budget - 250M;
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock(budget, posted).Object;
            IPostingLine postingLine = _fixture.BuildPostingLineMock(account: account, budgetAccount: budgetAccount, budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: new[] { postingLine }).Object;
            IPostingWarningCollection result = await sut.CalculateAsync(postingLineCollection);


            IPostingWarning postingWarning = result.SingleOrDefault(m => m.Reason == PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached);
            Assert.That(postingWarning, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.Not.Null);
            Assert.That(postingWarning.Account, Is.EqualTo(budgetAccount));
            Assert.That(postingWarning.Amount, Is.EqualTo(Math.Abs(posted) - Math.Abs(budget)));
            Assert.That(postingWarning.PostingLine, Is.Not.Null);
            Assert.That(postingWarning.PostingLine, Is.EqualTo(postingLine));
        }

        private IPostingWarningCalculator CreateSut()
        {
            return new Domain.Accounting.PostingWarningCalculator();
        }
    }
}