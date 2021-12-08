using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
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
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime today = DateTime.Today;
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideMonthCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)),
                CreatePostingLineMockWithinMonth(today.AddYears(-1)),
                CreatePostingLineMockAtDate(today.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)),
                CreatePostingLineMockAtDate(today.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)),
                CreatePostingLineMockWithinMonth(today.AddMonths(1)),
                CreatePostingLineMockAtDate(today.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)),
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)),
                CreatePostingLineMockWithinMonth(today.AddYears(1)),
                CreatePostingLineMockAtDate(today.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinMonthCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(today),
                CreatePostingLineMockWithinMonth(today),
                CreatePostingLineMockAtDate(today),
                CreatePostingLineMockAtLastDayInMonth(today)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideMonthCollection.Concat(postingLineMockWithinMonthCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.Between(new DateTime(today.Year, today.Month, 1), today);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideMonthCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinMonthCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionDoesNotContainPostingLinesForMonth_ReturnsNotNull()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionDoesNotContainPostingLinesForMonth_ReturnsPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionDoesNotContainPostingLinesForMonth_ReturnsEmptyPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionContainsPostingLinesForMonth_ReturnsNotNull()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionContainsPostingLinesForMonth_ReturnsPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionContainsPostingLinesForMonth_ReturnsNonEmptyPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameMonthAndPostingLineCollectionContainsPostingLinesForMonth_ReturnsPostingLineCollectionContainingPostingLinesForInternal()
        {
            DateTime today = DateTime.Today;
            IList<IPostingLine> postingLineOutsideMonthCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object
            };
            if (IsLastDayInMonth(today) == false)
            {
                postingLineOutsideMonthCollection.Add(CreatePostingLineMockAtLastDayInMonth(today).Object);
            }
            IList<IPostingLine> postingLineWithinMonthCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object
            };
            if (IsLastDayInMonth(today))
            {
                postingLineWithinMonthCollection.Add(CreatePostingLineMockAtLastDayInMonth(today).Object);
            }
            IPostingLineCollection sut = CreateSut(postingLineOutsideMonthCollection.Concat(postingLineWithinMonthCollection).ToArray());

            IPostingLineCollection result = sut.Between(new DateTime(today.Year, today.Month, 1), today);

            Assert.That(postingLineOutsideMonthCollection.All(postingLine => result.Contains(postingLine) == false), Is.True);
            Assert.That(postingLineWithinMonthCollection.All(postingLine => result.Contains(postingLine)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideYearCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinYearCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear),
                CreatePostingLineMockWithinMonth(dateInYear),
                CreatePostingLineMockAtDate(dateInYear),
                CreatePostingLineMockAtLastDayInMonth(dateInYear),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(2)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideYearCollection.Concat(postingLineMockWithinYearCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideYearCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinYearCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionDoesNotContainPostingLinesForYear_ReturnsNotNull()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionDoesNotContainPostingLinesForYear_ReturnsPostingLineCollection()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionDoesNotContainPostingLinesForYear_ReturnsEmptyPostingLineCollection()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionContainsPostingLinesForYear_ReturnsNotNull()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockWithinMonth(dateInYear).Object,
                CreatePostingLineMockAtDate(dateInYear).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionContainsPostingLinesForYear_ReturnsPostingLineCollection()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockWithinMonth(dateInYear).Object,
                CreatePostingLineMockAtDate(dateInYear).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionContainsPostingLinesForYear_ReturnsNonEmptyPostingLineCollection()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockWithinMonth(dateInYear).Object,
                CreatePostingLineMockAtDate(dateInYear).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateInSameYearButDifferentMonthAndPostingLineCollectionContainsPostingLinesForYear_ReturnsPostingLineCollectionContainingPostingLinesForInternal()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IEnumerable<IPostingLine> postingLineOutsideYearCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object
            };
            IEnumerable<IPostingLine> postingLineOutsideMonthIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinMonthIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockWithinMonth(dateInYear).Object,
                CreatePostingLineMockAtDate(dateInYear).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(2)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(2)).Object
            };
            IPostingLineCollection sut = CreateSut(postingLineOutsideYearCollection.Concat(postingLineOutsideMonthIntervalCollection).Concat(postingLineWithinMonthIntervalCollection).ToArray());

            IPostingLineCollection result = sut.Between(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(postingLineOutsideYearCollection.All(postingLine => result.Contains(postingLine) == false), Is.True);
            Assert.That(postingLineOutsideMonthIntervalCollection.All(postingLine => result.Contains(postingLine) == false), Is.True);
            Assert.That(postingLineWithinMonthIntervalCollection.All(postingLine => result.Contains(postingLine)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYear_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime today = DateTime.Today;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)),
                CreatePostingLineMockWithinMonth(today.AddYears(-2)),
                CreatePostingLineMockAtDate(today.AddYears(-2)),
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)),
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)),
                CreatePostingLineMockWithinMonth(today.AddYears(-1)),
                CreatePostingLineMockAtDate(today.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)),
                CreatePostingLineMockAtDate(today.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(today),
                CreatePostingLineMockWithinMonth(today),
                CreatePostingLineMockAtDate(today),
                CreatePostingLineMockAtLastDayInMonth(today),
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)),
                CreatePostingLineMockWithinMonth(today.AddMonths(1)),
                CreatePostingLineMockAtDate(today.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)),
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)),
                CreatePostingLineMockWithinMonth(today.AddYears(1)),
                CreatePostingLineMockAtDate(today.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineDoesNotContainPostingLinesForInterval_ReturnsNotNull()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object
            );

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineDoesNotContainPostingLinesForInterval_ReturnsPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object
            );

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineDoesNotContainPostingLinesForInterval_ReturnsEmptyPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object
            );

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineContainsPostingLinesForInterval_ReturnsNotNull()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineContainsPostingLinesForInterval_ReturnsPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(result, Is.TypeOf<Domain.Accounting.PostingLineCollection>());
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineContainsPostingLinesForInterval_ReturnsNonEmptyPostingLineCollection()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object);

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Between_WhenCalledWithFromDateAndToDateNotInSameYearAndPostingLineContainsPostingLinesForInterval_ReturnsPostingLineCollectionContainingPostingLinesForInternal()
        {
            DateTime today = DateTime.Today;
            IList<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(1)).Object
            };
            if (IsLastDayInMonth(today) == false)
            {
                postingLineOutsideIntervalCollection.Add(CreatePostingLineMockAtLastDayInMonth(today).Object);
            }
            IList<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(today.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(today.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object
            };
            if (IsLastDayInMonth(today))
            {
                postingLineWithinIntervalCollection.Add(CreatePostingLineMockAtLastDayInMonth(today).Object);
            }
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            IPostingLineCollection result = sut.Between(new DateTime(today.Year - 1, 1, 1), today);

            Assert.That(postingLineOutsideIntervalCollection.All(postingLine => result.Contains(postingLine) == false), Is.True);
            Assert.That(postingLineWithinIntervalCollection.All(postingLine => result.Contains(postingLine)), Is.True);
        }

        private IPostingLineCollection CreateSut(params IPostingLine[] postingLines)
        {
            NullGuard.NotNull(postingLines, nameof(postingLines));

            return new Domain.Accounting.PostingLineCollection
            {
                postingLines
            };
        }

        private Mock<IPostingLine> CreatePostingLineMockAtFirstDayInMonth(DateTime value)
        {
            return _fixture.BuildPostingLineMock(postingDate: new DateTime(value.Year, value.Month, 1));
        }

        private Mock<IPostingLine> CreatePostingLineMockWithinMonth(DateTime value)
        {
            return _fixture.BuildPostingLineMock(postingDate: new DateTime(value.Year, value.Month, _random.Next(1, value.Day)));
        }

        private Mock<IPostingLine> CreatePostingLineMockAtDate(DateTime value)
        {
            return _fixture.BuildPostingLineMock(postingDate: value);
        }

        private Mock<IPostingLine> CreatePostingLineMockAtLastDayInMonth(DateTime value)
        {
            return _fixture.BuildPostingLineMock(postingDate: new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month)));
        }

        private static bool IsLastDayInMonth(DateTime value)
        {
            return value.Date == new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month)).Date;
        }
    }
}