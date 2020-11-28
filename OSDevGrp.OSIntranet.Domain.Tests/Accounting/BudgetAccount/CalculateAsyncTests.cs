using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccount
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnBudgetInfoCollection()
        {
            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = _fixture.BuildBudgetInfoCollectionMock();
            IBudgetAccount sut = CreateSut(budgetInfoCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetInfoCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount sut = CreateSut(postingLineCollection: postingLineCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameBudgetAccount()
        {
            IBudgetAccount sut = CreateSut();

            IBudgetAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameBudgetAccountWhereBudgetInfoCollectionIsEqualToCalculatedBudgetInfoCollection()
        {
            IBudgetInfoCollection calculatedBudgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock().Object;
            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock(calculatedBudgetInfoCollection: calculatedBudgetInfoCollection).Object;
            IBudgetAccount sut = CreateSut(budgetInfoCollection);

            IBudgetAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetInfoCollection, Is.EqualTo(calculatedBudgetInfoCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameBudgetAccountWherePostingLineCollectionIsEqualToCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock().Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection).Object;
            IBudgetAccount sut = CreateSut(postingLineCollection: postingLineCollection);

            IBudgetAccount result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.PostingLineCollection, Is.EqualTo(calculatedPostingLineCollection));
        }

        private IBudgetAccount CreateSut(IBudgetInfoCollection budgetInfoCollection = null, IPostingLineCollection postingLineCollection = null)
        {
            return new Domain.Accounting.BudgetAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildBudgetAccountGroupMock().Object, budgetInfoCollection ?? _fixture.BuildBudgetInfoCollectionMock().Object, postingLineCollection ?? _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}