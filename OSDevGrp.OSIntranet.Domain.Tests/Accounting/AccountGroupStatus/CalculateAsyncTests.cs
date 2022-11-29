using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupStatus
{
    [TestFixture]
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccountGroupStatus sut = CreateSut(accountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatus()
        {
            IAccountGroupStatus sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.TypeOf<Domain.Accounting.AccountGroupStatus>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsAccountGroupStatusWhereStatusDateEqualToStatusDateFromArgument()
        {
            IAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountGroupStatus result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalculatedAccountCollectionWasReturned_ReturnsAccountGroupStatusWhereAccountCollectionIsNotNull()
        {
            IAccountCollection calculatedAccountCollection = _fixture.BuildAccountCollectionMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollection).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalculatedAccountCollectionWasReturned_ReturnsAccountGroupStatusWhereAccountCollectionEqualToCalculatedAccountCollection()
        {
            IAccountCollection calculatedAccountCollection = _fixture.BuildAccountCollectionMock().Object;
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(calculatedAccountCollection: calculatedAccountCollection).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountCollection, Is.EqualTo(calculatedAccountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenNoCalculatedAccountCollectionWasReturned_ReturnsAccountGroupStatusWhereAccountCollectionIsNotNull()
        {
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(hasCalculatedAccountCollection: false).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountCollection, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenNoCalculatedAccountCollectionWasReturned_ReturnsAccountGroupStatusWhereAccountCollectionEqualToAccountCollectionFromConstructor()
        {
            IAccountCollection accountCollection = _fixture.BuildAccountCollectionMock(hasCalculatedAccountCollection: false).Object;
            IAccountGroupStatus sut = CreateSut(accountCollection);

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.AccountCollection, Is.EqualTo(accountCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameAccountGroupStatus()
        {
            IAccountGroupStatus sut = CreateSut();

            IAccountGroupStatus result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnceOnAccountCollection()
        {
            Mock<IAccountCollection> accountCollectionMock = _fixture.BuildAccountCollectionMock();
            IAccountGroupStatus sut = CreateSut(accountCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            accountCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsNotNull()
        {
            IAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountGroupStatus result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsAccountGroupStatus()
        {
            IAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountGroupStatus result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.TypeOf<Domain.Accounting.AccountGroupStatus>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_ReturnsSameAccountGroupStatus()
        {
            IAccountGroupStatus sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IAccountGroupStatus result = await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculateAsync_WhenCalledWithStatusDateAndAccountCollection_ThrowNotSupportedException()
        {
            IAccountGroupStatus sut = CreateSut();

            Assert.ThrowsAsync<NotSupportedException>(async () => await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1), _fixture.BuildAccountCollectionMock().Object));
        }

        private IAccountGroupStatus CreateSut(IAccountCollection accountCollection = null)
        {
            return new Domain.Accounting.AccountGroupStatus(_fixture.BuildAccountGroupMock().Object, accountCollection ?? _fixture.BuildAccountCollectionMock().Object);
        }
    }
}