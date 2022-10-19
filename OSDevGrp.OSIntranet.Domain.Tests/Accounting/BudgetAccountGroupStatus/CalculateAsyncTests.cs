using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    public class CalculateAsyncTests
    {
        #region Properties

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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.TypeOf<Domain.Accounting.BudgetAccountGroupStatus>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsBudgetAccountGroupStatusWhereStatusDateEqualToStatusDateFromArgument()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IBudgetAccountGroupStatus result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalculatedBudgetAccountCollectionWasReturned_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionIsNotNull()
        {
            IBudgetAccountCollection calculatedBudgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollection).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalculatedBudgetAccountCollectionWasReturned_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionEqualToCalculatedBudgetAccountCollection()
        {
            IBudgetAccountCollection calculatedBudgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollection).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountCollection, Is.EqualTo(calculatedBudgetAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenNoCalculatedBudgetAccountCollectionWasReturned_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionIsNotNull()
        {
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(hasCalculatedBudgetAccountCollection: false).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenNoCalculatedBudgetAccountCollectionWasReturned_ReturnsBudgetAccountGroupStatusWhereBudgetAccountCollectionEqualToBudgetAccountCollectionFromConstructor()
        {
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(hasCalculatedBudgetAccountCollection: false).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountCollection, Is.EqualTo(budgetAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IBudgetAccountGroupStatus result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IBudgetAccountGroupStatus result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.TypeOf<Domain.Accounting.BudgetAccountGroupStatus>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IBudgetAccountGroupStatus result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculateAsync_WhenCalledWithStatusDateAndBudgetAccountCollection_ThrowNotSupportedException()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.ThrowsAsync<NotSupportedException>(async () => await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1), _fixture.BuildBudgetAccountCollectionMock().Object));
        }

        private IBudgetAccountGroupStatus CreateSut(IBudgetAccountCollection budgetAccountCollection = null)
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}