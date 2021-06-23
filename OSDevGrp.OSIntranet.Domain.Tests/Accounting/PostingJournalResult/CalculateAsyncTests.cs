using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingJournalResult
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
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock(isEmpty: true);
            IPostingJournalResult sut = CreateSut(postingLineCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertCalculateAsyncWasCalledOnPostingWarningCalculatorWithCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(isEmpty: true).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection, isEmpty: true).Object;
            Mock<IPostingWarningCalculator> postingWarningCalculatorMock = _fixture.BuildPostingWarningCalculatorMock(isEmpty: true);
            IPostingJournalResult sut = CreateSut(postingLineCollection, postingWarningCalculatorMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.CalculateAsync(statusDate);

            postingWarningCalculatorMock.Verify(m => m.CalculateAsync(It.Is<IPostingLineCollection>(value => value == calculatedPostingLineCollection)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingJournalResult()
        {
            IPostingJournalResult sut = CreateSut();

            IPostingJournalResult result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingJournalResultWherePostingLineCollectionIsEqualToCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(isEmpty: true).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection, isEmpty: true).Object;
            IPostingJournalResult sut = CreateSut(postingLineCollection);

            IPostingJournalResult result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.PostingLineCollection, Is.SameAs(calculatedPostingLineCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSamePostingJournalResultWherePostingWarningCollectionIsEqualToPostingWarningCollectionFromPostingWarningCalculator()
        {
            IPostingWarningCollection postingWarningCollection = _fixture.BuildPostingWarningCollectionMock().Object;
            IPostingWarningCalculator postingWarningCalculator = _fixture.BuildPostingWarningCalculatorMock(postingWarningCollection).Object;
            IPostingJournalResult sut = CreateSut(postingWarningCalculator: postingWarningCalculator);

            IPostingJournalResult result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result.PostingWarningCollection, Is.SameAs(postingWarningCollection));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnPostingLineCollection()
        {
            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock(isEmpty: true);
            IPostingJournalResult sut = CreateSut(postingLineCollectionMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculateAsync(It.Is<DateTime>(value => value == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalledMultipleTimesWithSameStatusDate_AssertCalculateAsyncWasCalledOnlyOnceOnPostingWarningCalculatorWithCalculatedPostingLineCollection()
        {
            IPostingLineCollection calculatedPostingLineCollection = _fixture.BuildPostingLineCollectionMock(isEmpty: true).Object;
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingLineCollection: calculatedPostingLineCollection, isEmpty: true).Object;
            Mock<IPostingWarningCalculator> postingWarningCalculatorMock = _fixture.BuildPostingWarningCalculatorMock(isEmpty: true);
            IPostingJournalResult sut = CreateSut(postingLineCollection, postingWarningCalculatorMock.Object);

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await (await (await sut.CalculateAsync(statusDate)).CalculateAsync(statusDate)).CalculateAsync(statusDate);

            postingWarningCalculatorMock.Verify(m => m.CalculateAsync(It.Is<IPostingLineCollection>(value => value == calculatedPostingLineCollection)), Times.Once);
        }

        private IPostingJournalResult CreateSut(IPostingLineCollection postingLineCollection = null, IPostingWarningCalculator postingWarningCalculator = null)
        {
            return new Domain.Accounting.PostingJournalResult(postingLineCollection ?? _fixture.BuildPostingLineCollectionMock(isEmpty: true).Object, postingWarningCalculator ?? _fixture.BuildPostingWarningCalculatorMock(isEmpty: true).Object);
        }
    }
}