using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLineCollection
{
    [TestFixture]
    public class BetweenTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IPostingLine>(builder => builder.FromFactory(() => _fixture.BuildPostingLineMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalled_AssertPostingDateWasCalledOnEachPostingLine()
        {
            IPostingLineCollection sut = CreateSut();

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1)),
                _fixture.BuildPostingLineMock(DateTime.Today.AddDays(_random.Next(0, 30) * -1))
            };
            sut.Add(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object));

            sut.Between(DateTime.Today.AddDays(-30), DateTime.Today);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalled_ReturnsNotNull()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray());

            IPostingLineCollection result = sut.Between(DateTime.Today.AddDays(_random.Next(1, 7) * -1), DateTime.Today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalled_ReturnsPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IPostingLine>(_random.Next(10, 25)).ToArray());

            IPostingLineCollection result = sut.Between(DateTime.Today.AddDays(_random.Next(1, 7) * -1), DateTime.Today);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenPostingLineCollectionOnlyContainsPostingLinesBeforeToFromDate_ReturnsEmptyPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime fromDate = DateTime.Today.AddDays(_random.Next(1, 7) * -1);
            IEnumerable<IPostingLine> postingLines = new List<IPostingLine>
            {
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object,
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object,
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object,
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object,
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object,
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object,
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object
            };
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Between(fromDate, DateTime.Today);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenPostingLineCollectionOnlyContainsPostingLinesAfterToToDate_ReturnsEmptyPostingLineCollection()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime toDate = DateTime.Today.AddDays(_random.Next(7, 14) * -1);
            IEnumerable<IPostingLine> postingLines = new List<IPostingLine>
            {
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object,
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object,
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object,
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object,
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object,
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object,
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object
            };
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Between(toDate.AddDays(-7), toDate);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenPostingLineCollectionContainsPostingLinesBetweenFromDateAndToDate_ReturnsPostingLineCollectionWithPostingLinesBetweenFromDateAndToDate()
        {
            IPostingLineCollection sut = CreateSut();

            DateTime fromDate = DateTime.Today.AddDays(_random.Next(14, 21) * -1);
            DateTime toDate = fromDate.AddDays(7);
            IPostingLine postingLineBeforeFromDate1 = _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object;
            IPostingLine postingLineBeforeFromDate2 = _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object;
            IPostingLine postingLineBeforeFromDate3 = _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 30) * -1)).Object;
            IPostingLine postingLineBeforeOnFromDate = _fixture.BuildPostingLineMock(fromDate).Object;
            IPostingLine postingLineBetweenFromDateAndToDate = _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, toDate.Subtract(fromDate).Days))).Object;
            IPostingLine postingLineBeforeOnToDate = _fixture.BuildPostingLineMock(toDate).Object;
            IPostingLine postingLineAfterToDate1 = _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object;
            IPostingLine postingLineAfterToDate2 = _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object;
            IPostingLine postingLineAfterToDate3 = _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, DateTime.Today.Subtract(toDate).Days))).Object;

            IEnumerable<IPostingLine> postingLines = new List<IPostingLine>
            {
                postingLineBeforeFromDate1,
                postingLineBeforeFromDate2,
                postingLineBeforeFromDate3,
                postingLineBeforeOnFromDate,
                postingLineBetweenFromDateAndToDate,
                postingLineBeforeOnToDate,
                postingLineAfterToDate1,
                postingLineAfterToDate2,
                postingLineAfterToDate3
            };
            sut.Add(postingLines);

            IPostingLineCollection result = sut.Between(toDate.AddDays(-7), toDate);

            Assert.That(result.Contains(postingLineBeforeFromDate1), Is.False);
            Assert.That(result.Contains(postingLineBeforeFromDate2), Is.False);
            Assert.That(result.Contains(postingLineBeforeFromDate3), Is.False);
            Assert.That(result.Contains(postingLineBeforeOnFromDate), Is.True);
            Assert.That(result.Contains(postingLineBetweenFromDateAndToDate), Is.True);
            Assert.That(result.Contains(postingLineBeforeOnToDate), Is.True);
            Assert.That(result.Contains(postingLineAfterToDate1), Is.False);
            Assert.That(result.Contains(postingLineAfterToDate1), Is.False);
            Assert.That(result.Contains(postingLineAfterToDate1), Is.False);
        }

        private IPostingLineCollection CreateSut()
        {
            return new Domain.Accounting.PostingLineCollection();
        }
    }
}