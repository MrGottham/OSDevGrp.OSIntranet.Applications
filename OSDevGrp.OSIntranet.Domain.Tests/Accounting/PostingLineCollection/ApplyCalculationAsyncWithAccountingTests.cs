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
    public class ApplyCalculationAsyncWithAccountingTests
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
        public void ApplyCalculationAsync_WhenCalculatedAccountingIsNull_ThrowsArgumentNullException()
        {
            IPostingLineCollection sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IAccounting)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedAccounting"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertNumberWasCalledOnCalculatedAccounting()
        {
            IPostingLineCollection sut = CreateSut();

            Mock<IAccounting> calculatedAccountingMock = _fixture.BuildAccountingMock();
            await sut.ApplyCalculationAsync(calculatedAccountingMock.Object);

            calculatedAccountingMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertAccountingWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
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

            IAccounting calculatedAccounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccounting);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.Accounting, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasCalledOnEachPostingLineMatchingCalculatedAccounting()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
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

            IAccounting calculatedAccounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccounting);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.Is<IAccounting>(value => value != null && value == calculatedAccounting)), Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_AssertApplyCalculationAsyncWasNotCalledOnAnyPostingLineNotMatchingCalculatedAccounting()
        {
            IPostingLineCollection sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber + 1).Object;
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

            IAccounting calculatedAccounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            await sut.ApplyCalculationAsync(calculatedAccounting);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.ApplyCalculationAsync(It.IsAny<IAccounting>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            IPostingLineCollection result = await sut.ApplyCalculationAsync(_fixture.BuildAccountingMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}