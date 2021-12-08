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
    public class ApplyCalculationAsyncWithAccountTests
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
        public void ApplyCalculationAsync_WhenCalculatedAccountIsNull_ThrowsArgumentNullException()
        {
            IPostingLineCollection sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IAccount)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnCalculatedAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock(isEmpty: true);
            await sut.ApplyCalculationAsync(calculatedAccountMock.Object);

            calculatedAccountMock.Verify(m => m.Accounting, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertNumberWasCalledOnAccountingFromCalculatedAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IAccount calculatedAccount = _fixture.BuildAccountMock(accountingMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            accountingMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountNumberWasCalledOnCalculatedAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            await sut.ApplyCalculationAsync(calculatedAccountMock.Object);

            calculatedAccountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IAccount calculatedAccount = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Accounting, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IAccount calculatedAccount = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Account, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasCalledOnEachPostingLineMatchingCalculatedAccount()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IAccount account = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account),
                _fixture.BuildPostingLineMock(account: account)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IAccount calculatedAccount = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.Is<IAccount>(value => value != null && value == calculatedAccount)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasNotCalledOnAnyPostingLineNotMatchingCalculatedAccount()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IAccount calculatedAccount = _fixture.BuildAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.IsAny<IAccount>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}