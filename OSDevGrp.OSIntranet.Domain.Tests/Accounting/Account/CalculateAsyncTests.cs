using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Account
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnCreditInfoCollection()
        {
            Mock<ICreditInfoCollection> creditInfoCollectionMock = _fixture.BuildCreditInfoCollectionMock();
            IAccount sut = CreateSut(creditInfoCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            creditInfoCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount sut = CreateSut(postingLineCollection: postingLineCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertApplyCalculationAsyncWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount sut = CreateSut(postingLineCollection: postingLineCollectionMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            postingLineCollectionMock.Verify(m => m.ApplyCalculationAsync(It.Is<IAccount>(value => value != null && value == sut)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccount()
        {
            IAccount sut = CreateSut();

            IAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountWhereCreditInfoCollectionIsEqualToCalculatedCreditInfoCollection()
        {
            ICreditInfoCollection calculatedCreditInfoCollection = _fixture.BuildCreditInfoCollectionMock().Object;
            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock(calculatedCreditInfoCollection: calculatedCreditInfoCollection).Object;
            IAccount sut = CreateSut(creditInfoCollection);

            IAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.CreditInfoCollection, Is.EqualTo(calculatedCreditInfoCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountWherePostingLineCollectionIsEqualToCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock().Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection).Object;
            IAccount sut = CreateSut(postingLineCollection: postingLineCollection);

            IAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.PostingLineCollection, Is.EqualTo(calculatedPostingLineCollection));
        }

        private IAccount CreateSut(ICreditInfoCollection creditInfoCollection = null, IPostingLineCollection postingLineCollection = null)
        {
            return new Domain.Accounting.Account(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildAccountGroupMock().Object, creditInfoCollection ?? _fixture.BuildCreditInfoCollectionMock().Object, postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}