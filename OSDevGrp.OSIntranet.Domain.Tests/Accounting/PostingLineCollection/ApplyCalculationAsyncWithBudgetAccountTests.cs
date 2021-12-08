using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
{
    [TestFixture]
    public class ApplyCalculationAsyncWithBudgetAccountTests
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
        public void ApplyCalculationAsync_WhenCalculatedBudgetAccountIsNull_ThrowsArgumentNullException()
        {
            IPostingLineCollection sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IBudgetAccount)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedBudgetAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnCalculatedBudgetAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock(isEmpty: true);
            await sut.ApplyCalculationAsync(calculatedBudgetAccountMock.Object);

            calculatedBudgetAccountMock.Verify(m => m.Accounting, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertNumberWasCalledOnAccountingFromCalculatedBudgetAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accountingMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            accountingMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountNumberWasCalledOnCalculatedBudgetAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ApplyCalculationAsync(calculatedBudgetAccountMock.Object);

            calculatedBudgetAccountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Accounting, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertBudgetAccountWasCalledTwiceOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.BudgetAccount, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasCalledOnEachPostingLineMatchingCalculatedBudgetAccount()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: budgetAccount)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.Is<IBudgetAccount>(value => value != null && value == calculatedBudgetAccount)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasNotCalledOnAnyPostingLineNotMatchingCalculatedBudgetAccount()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, budgetAccount: _fixture.BuildBudgetAccountMock(accounting).Object)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.IsAny<IBudgetAccount>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}