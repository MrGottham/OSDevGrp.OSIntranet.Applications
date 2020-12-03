using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Accounting
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            ICalculable<IAccounting> sut = CreateSut(accountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            ICalculable<IAccounting> sut = CreateSut(budgetAccountCollection: budgetAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnContactAccountCollection()
        {
            Mock<IContactAccountCollection> contactAccountCollectionMock = _fixture.BuildContactAccountCollectionMock();
            ICalculable<IAccounting> sut = CreateSut(contactAccountCollection: contactAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            contactAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCalculable()
        {
            ICalculable<IAccounting> sut = CreateSut();

            IAccounting result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCalculableWhereStatusDateEqualDateFromCall()
        {
            ICalculable<IAccounting> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccounting result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCalculableWithCalculatedAccountCollection()
        {
            IAccountCollection calculatedAccountCollection = _fixture.BuildAccountCollectionMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollection).Object;
            ICalculable<IAccounting> sut = CreateSut(accountCollection: accountCollection);

            IAccounting result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountCollection, Is.EqualTo(calculatedAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCalculableWithCalculatedBudgetAccountCollection()
        {
            IBudgetAccountCollection calculatedBudgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(calculatedBudgetAccountCollection: calculatedBudgetAccountCollection).Object;
            ICalculable<IAccounting> sut = CreateSut(budgetAccountCollection: budgetAccountCollection);

            IAccounting result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.BudgetAccountCollection, Is.EqualTo(calculatedBudgetAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCalculableWithCalculatedContactAccountCollection()
        {
            IContactAccountCollection calculatedContactAccountCollection = _fixture.BuildContactAccountCollectionMock().Object;
            IContactAccountCollection contactAccountCollection = _fixture.BuildContactAccountCollectionMock(calculatedContactAccountCollection: calculatedContactAccountCollection).Object;
            ICalculable<IAccounting> sut = CreateSut(contactAccountCollection: contactAccountCollection);

            IAccounting result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.ContactAccountCollection, Is.EqualTo(calculatedContactAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            ICalculable<IAccounting> sut = CreateSut(accountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            ICalculable<IAccounting> sut = CreateSut(budgetAccountCollection: budgetAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            budgetAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnContactAccountCollection()
        {
            Mock<IContactAccountCollection> contactAccountCollectionMock = _fixture.BuildContactAccountCollectionMock();
            ICalculable<IAccounting> sut = CreateSut(contactAccountCollection: contactAccountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            contactAccountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameCalculable()
        {
            ICalculable<IAccounting> sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccounting result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        private ICalculable<IAccounting> CreateSut(IAccountCollection accountCollection = null, IBudgetAccountCollection budgetAccountCollection = null, IContactAccountCollection contactAccountCollection = null)
        {
            return new Domain.Accounting.Accounting(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.BuildLetterHeadMock().Object, _fixture.Create<BalanceBelowZeroType>(), _fixture.Create<int>(), accountCollection ?? _fixture.BuildAccountCollectionMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object, contactAccountCollection ?? _fixture.BuildContactAccountCollectionMock().Object);
        }
   }
}