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
    public class ApplyCalculationAsyncWithContactAccountTests
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
        public void ApplyCalculationAsync_WhenCalculatedContactAccountIsNull_ThrowsArgumentNullException()
        {
            IPostingLineCollection sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IContactAccount)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedContactAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnCalculatedContactAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IContactAccount> calculatedContactAccountMock = _fixture.BuildContactAccountMock(isEmpty: true);
            await sut.ApplyCalculationAsync(calculatedContactAccountMock.Object);

            calculatedContactAccountMock.Verify(m => m.Accounting, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertNumberWasCalledOnAccountingFromCalculatedContactAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IAccounting> accountingMock = _fixture.BuildAccountingMock();
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accountingMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedContactAccount);

            accountingMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountNumberWasCalledOnCalculatedContactAccount()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IContactAccount> calculatedContactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ApplyCalculationAsync(calculatedContactAccountMock.Object);

            calculatedContactAccountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedContactAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Accounting, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertContactAccountWasCalledTwiceOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedContactAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ContactAccount, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasCalledOnEachPostingLineMatchingCalculatedContactAccount()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: contactAccount)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedContactAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.Is<IContactAccount>(value => value != null && value == calculatedContactAccount)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasNotCalledOnAnyPostingLineNotMatchingCalculatedContactAccount()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object),
                _fixture.BuildPostingLineMock(account: _fixture.BuildAccountMock(accounting).Object, contactAccount: _fixture.BuildContactAccountMock(accounting).Object)
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(accounting, accountNumber).Object;
            await sut.ApplyCalculationAsync(calculatedContactAccount);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.IsAny<IContactAccount>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}