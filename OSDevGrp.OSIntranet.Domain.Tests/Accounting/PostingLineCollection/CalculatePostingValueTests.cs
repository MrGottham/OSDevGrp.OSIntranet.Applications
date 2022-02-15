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
    public class CalculatePostingValueTests
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
        public void CalculatePostingValue_WhenFromDateIsGreaterThanToDate_AssertPostingDateWasNotCalledOnAnyPostingLine()
        {
            DateTime today = DateTime.Today;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(today),
                CreatePostingLineMockWithinMonth(today),
                CreatePostingLineMockAtDate(today),
                CreatePostingLineMockAtLastDayInMonth(today)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(today.AddDays(_random.Next(1, 7)), today);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateIsGreaterThanToDate_AssertPostingValueWasNotCalledOnAnyPostingLine()
        {
            DateTime today = DateTime.Today;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(today),
                CreatePostingLineMockWithinMonth(today),
                CreatePostingLineMockAtDate(today),
                CreatePostingLineMockAtLastDayInMonth(today)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(today.AddDays(_random.Next(1, 7)), today);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateIsGreaterThanToDate_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime today = DateTime.Today;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(today),
                CreatePostingLineMockWithinMonth(today),
                CreatePostingLineMockAtDate(today),
                CreatePostingLineMockAtLastDayInMonth(today)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(today.AddDays(_random.Next(1, 7)), today);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateIsGreaterThanToDate_ReturnsZero()
        {
            DateTime today = DateTime.Today;
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(today).Object,
                CreatePostingLineMockWithinMonth(today).Object,
                CreatePostingLineMockAtDate(today).Object,
                CreatePostingLineMockAtLastDayInMonth(today).Object);

            decimal result = sut.CalculatePostingValue(today.AddDays(_random.Next(1, 7)), today);

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalNotFullMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime dateInMonth = DateTime.Today.AddDays(IsLastDayInMonth(DateTime.Today) ? -1 : 0);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideMonthCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(1)),
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(1)),
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinMonthCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth),
                CreatePostingLineMockWithinMonth(dateInMonth),
                CreatePostingLineMockAtDate(dateInMonth),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideMonthCollection.Concat(postingLineMockWithinMonthCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInMonth.Year, dateInMonth.Month, 1), dateInMonth);

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
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalNotFullMonth_AssertPostingValueWasCalledOnEachPostingLine()
        {
            DateTime dateInMonth = DateTime.Today.AddDays(IsLastDayInMonth(DateTime.Today) ? -1 : 0);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth),
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(1)),
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(1)),
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth),
                CreatePostingLineMockWithinMonth(dateInMonth),
                CreatePostingLineMockAtDate(dateInMonth),
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideIntervalCollection.Concat(postingLineMockWithinIntervalCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInMonth.Year, dateInMonth.Month, 1), dateInMonth);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalNotFullMonth_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime dateInMonth = DateTime.Today.AddDays(IsLastDayInMonth(DateTime.Today) ? -1 : 0);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth),
                CreatePostingLineMockWithinMonth(dateInMonth),
                CreatePostingLineMockAtDate(dateInMonth),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth),
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(1)),
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(1)),
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInMonth.Year, dateInMonth.Month, 1), dateInMonth);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalNotFullMonthAndPostingLineCollectionDoesNotContainPostingLinesForInterval_ReturnsZero()
        {
            DateTime dateInMonth = DateTime.Today.AddDays(IsLastDayInMonth(DateTime.Today) ? -1 : 0);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInMonth).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(1)).Object);

            decimal result = sut.CalculatePostingValue(new DateTime(dateInMonth.Year, dateInMonth.Month, 1), dateInMonth);

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalNotFullMonthAndPostingLineCollectionContainsPostingLinesForInterval_ReturnsCalculatedPostingValue()
        {
            DateTime dateInMonth = DateTime.Today.AddDays(IsLastDayInMonth(DateTime.Today) ? -1 : 0);
            IEnumerable<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInMonth).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInMonth.AddMonths(1)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInMonth).Object,
                CreatePostingLineMockWithinMonth(dateInMonth).Object,
                CreatePostingLineMockAtDate(dateInMonth).Object
            };
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            decimal result = sut.CalculatePostingValue(new DateTime(dateInMonth.Year, dateInMonth.Month, 1), dateInMonth);

            Assert.That(result, Is.EqualTo(postingLineWithinIntervalCollection.Sum(m => m.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalIsFullMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime today = DateTime.Today;
            DateTime lastDateInMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth),
                CreatePostingLineMockWithinMonth(lastDateInMonth),
                CreatePostingLineMockAtDate(lastDateInMonth),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth),
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1), lastDateInMonth);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalIsFullMonth_AssertPostingValueWasCalledOnEachPostingLine()
        {
            DateTime today = DateTime.Today;
            DateTime lastDateInMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth),
                CreatePostingLineMockWithinMonth(lastDateInMonth),
                CreatePostingLineMockAtDate(lastDateInMonth),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth),
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1), lastDateInMonth);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalIsFullMonth_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime today = DateTime.Today;
            DateTime lastDateInMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth),
                CreatePostingLineMockWithinMonth(lastDateInMonth),
                CreatePostingLineMockAtDate(lastDateInMonth),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth),
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1), lastDateInMonth);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalIsFullMonthAndPostingLineCollectionDoesNotContainPostingLinesForInterval_ReturnsZero()
        {
            DateTime today = DateTime.Today;
            DateTime lastDateInMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(1)).Object);

            decimal result = sut.CalculatePostingValue(new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1), lastDateInMonth);

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateIsSameMonthAndIntervalIsFullMonthAndPostingLineCollectionContainsPostingLinesForInterval_ReturnsCalculatedPostingValue()
        {
            DateTime today = DateTime.Today;
            DateTime lastDateInMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            IEnumerable<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(lastDateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(lastDateInMonth.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth.AddMonths(1)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(lastDateInMonth).Object,
                CreatePostingLineMockWithinMonth(lastDateInMonth).Object,
                CreatePostingLineMockAtDate(lastDateInMonth).Object,
                CreatePostingLineMockAtLastDayInMonth(lastDateInMonth).Object
            };
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            decimal result = sut.CalculatePostingValue(new DateTime(lastDateInMonth.Year, lastDateInMonth.Month, 1), lastDateInMonth);

            Assert.That(result, Is.EqualTo(postingLineWithinIntervalCollection.Sum(m => m.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateIsFirstDayInMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
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
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateIsFirstDayInMonth_AssertPostingValueWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
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
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateIsFirstDayInMonth_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
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
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateIsFirstDayInMonthAndPostingLineCollectionDoesNotContainPostingLinesForInterval_ReturnsZero()
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
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            decimal result = sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateIsFirstDayInMonthAndPostingLineCollectionContainsPostingLinesForInterval_ReturnsCalculatedPostingValue()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 1);
            IEnumerable<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
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
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            decimal result = sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.EqualTo(postingLineWithinIntervalCollection.Sum(m => m.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateNotFirstDayInMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 7);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideFirstMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)),
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
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinFirstMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear),
                CreatePostingLineMockWithinMonth(dateInYear),
                CreatePostingLineMockAtDate(dateInYear),
                CreatePostingLineMockAtLastDayInMonth(dateInYear),
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideFirstMonthOfIntervalCollection.Concat(postingLineMockWithinFirstMonthOfIntervalCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideFirstMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinFirstMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateNotFirstDayInMonth_AssertPostingValueWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 7);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideFirstMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)),
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
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinFirstMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtDate(dateInYear),
                CreatePostingLineMockAtLastDayInMonth(dateInYear),
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideFirstMonthOfIntervalCollection.Concat(postingLineMockWithinFirstMonthOfIntervalCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideFirstMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinFirstMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateNotFirstDayInMonth_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 7);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
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
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateNotFirstDayInMonthAndPostingLineCollectionDoesNotContainPostingLinesForInterval_ReturnsZero()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 7);
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
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            decimal result = sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndFromDateNotFirstDayInMonthAndPostingLineCollectionContainsPostingLinesForInterval_ReturnsCalculatedPostingValue()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 7);
            IEnumerable<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
            {
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
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            decimal result = sut.CalculatePostingValue(dateInYear, new DateTime(dateInYear.Year, dateInYear.AddMonths(2).Month, DateTime.DaysInMonth(dateInYear.Year, dateInYear.AddMonths(2).Month)));

            Assert.That(result, Is.EqualTo(postingLineWithinIntervalCollection.Sum(m => m.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateIsLastDayInMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 31);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)),
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
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateIsLastDayInMonth_AssertPostingValueWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 31);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)),
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
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(1));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateIsLastDayInMonth_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 31);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)),
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
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateIsLastDayInMonthAndPostingLineCollectionDoesNotContainPostingLinesForInterval_ReturnsZero()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 31);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            decimal result = sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateIsLastDayInMonthAndPostingLineCollectionContainsPostingLinesForInterval_ReturnsCalculatedPostingValue()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 31);
            IEnumerable<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockWithinMonth(dateInYear).Object,
                CreatePostingLineMockAtDate(dateInYear).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear).Object
            };
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            decimal result = sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            Assert.That(result, Is.EqualTo(postingLineWithinIntervalCollection.Sum(m => m.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateNotLastDayInMonth_AssertPostingDateWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 15);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideLastMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinLastMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear),
                CreatePostingLineMockWithinMonth(dateInYear),
                CreatePostingLineMockAtDate(dateInYear),
                CreatePostingLineMockAtLastDayInMonth(dateInYear)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideLastMonthOfIntervalCollection.Concat(postingLineMockWithinLastMonthOfIntervalCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideLastMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinLastMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateNotLastDayInMonth_AssertPostingValueWasCalledOnEachPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 15);
            IEnumerable<Mock<IPostingLine>> postingLineMockOutsideLastMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IEnumerable<Mock<IPostingLine>> postingLineMockWithinLastMonthOfIntervalCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear),
                CreatePostingLineMockAtDate(dateInYear)
            };
            IPostingLineCollection sut = CreateSut(postingLineMockOutsideLastMonthOfIntervalCollection.Concat(postingLineMockWithinLastMonthOfIntervalCollection).Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockOutsideLastMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(1));
            }

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockWithinLastMonthOfIntervalCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Exactly(2));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateNotLastDayInMonth_AssertSortOrderWasNotCalledOnAnyPostingLine()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 15);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)),
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)),
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
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)),
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)),
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1))
            };
            IPostingLineCollection sut = CreateSut(postingLineMockCollection.Select(postingLineMock => postingLineMock.Object).ToArray());

            sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.SortOrder, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateNotLastDayInMonthAndPostingLineCollectionDoesNotContainPostingLinesForInterval_ReturnsZero()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 15);
            IPostingLineCollection sut = CreateSut(
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object);

            decimal result = sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            Assert.That(result, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void CalculatePostingValue_WhenFromDateAndToDateNoteSameMonthAndToDateNotLastDayInMonthAndPostingLineCollectionContainsPostingLinesForInterval_ReturnsCalculatedPostingValue()
        {
            DateTime dateInYear = new DateTime(DateTime.Today.Year, 7, 15);
            IEnumerable<IPostingLine> postingLineOutsideIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-3)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddYears(1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddYears(1)).Object
            };
            IEnumerable<IPostingLine> postingLineWithinIntervalCollection = new List<IPostingLine>
            {
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-2)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockWithinMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtDate(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtLastDayInMonth(dateInYear.AddMonths(-1)).Object,
                CreatePostingLineMockAtFirstDayInMonth(dateInYear).Object,
                CreatePostingLineMockAtDate(dateInYear).Object
            };
            IPostingLineCollection sut = CreateSut(postingLineOutsideIntervalCollection.Concat(postingLineWithinIntervalCollection).ToArray());

            decimal result = sut.CalculatePostingValue(new DateTime(dateInYear.AddMonths(-2).Year, dateInYear.AddMonths(-2).Month, 1), dateInYear);

            Assert.That(result, Is.EqualTo(postingLineWithinIntervalCollection.Sum(m => m.PostingValue)));
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
            return value.Date == new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }
    }
}