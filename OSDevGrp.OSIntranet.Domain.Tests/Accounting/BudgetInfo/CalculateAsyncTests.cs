using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetInfo
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
        public async Task CalculateAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnBudgetAccount()
        {
            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            IBudgetInfo sut = CreateSut(budgetAccountMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            budgetAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostingDateWasCalledOnEachPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock(),
                _fixture.BuildPostingLineMock()
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanBudgetInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, (int) statusDate.Subtract(toDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, (int) statusDate.Subtract(toDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(toDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanBudgetInfo_AssertPostingValueWasCalledOnEachPostingLineWhichAreWithinBudgetInfoInPostingLineCollectionOnBudgetAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(toDate.Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanBudgetInfo_AssertPostingValueNotWasCalledOnAnyOlderPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinBudgetInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate.AddDays(_random.Next(1, (int) toDate.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(_random.Next(1, (int) toDate.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinBudgetInfo_AssertPostingValueWasCalledOnEachPostingLineWhichAreWithinBudgetInfoInPostingLineCollectionOnBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate.Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinBudgetInfo_AssertPostingValueWasNotCalledOnAnyOlderPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanBudgetInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate.AddDays(_random.Next(1, (int) today.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(_random.Next(1, (int) today.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanBudgetInfo_AssertPostingValueWasNotCalledOnAnyPostingLineWhichAreWithinBudgetInfoInPostingLineCollectionOnBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(today.Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, Math.Max((int) today.Subtract(fromDate).TotalDays, 1))).Date),
                _fixture.BuildPostingLineMock(fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanBudgetInfo_AssertPostingValueWasNotCalledOnAnyOlderPostingLineInPostingLineCollectionOnBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnBudgetAccountOnlyContainsPostingLinesNewerThanStatusDate_AssertCalculatedPostedIsEqualToZero()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(toDate.Date),
                _fixture.BuildPostingLineMock(statusDate.AddDays(7)),
                _fixture.BuildPostingLineMock(statusDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

            IBudgetInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Posted, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnBudgetAccountOnlyContainsPostingLinesBetweenStatusDateAndFromDate_AssertCalculatedPostedIsEqualToSumOfPostingValueFromPostingLines()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(statusDate.Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short)year, (short)month);

            IBudgetInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Posted, Is.EqualTo(postingLineMockCollection.Sum(postingLineMock => postingLineMock.Object.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnAccountOnlyContainsPostingLinesOlderThanStatus_AssertCalculatedPostedIsEqualToZero()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short)year, (short)month);

            IBudgetInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Posted, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostedIsCalculated()
        {
            DateTime today = DateTime.Today;
            DateTime calculationDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(1);

            int year = calculationDate.Year;
            int month = calculationDate.Month;

            DateTime fromDate = new DateTime(year, month, 1);
            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(toDate.AddDays(1).Date),
                _fixture.BuildPostingLineMock(toDate.Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(fromDate.Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };

            foreach (DateTime statusDate in postingLineMockCollection.Select(postingLineMock => postingLineMock.Object.PostingDate).OrderByDescending(value => value.Date))
            {
                IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
                IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
                IBudgetInfo sut = CreateSut(budgetAccount, (short) year, (short) month);

                IBudgetInfo result = await sut.CalculateAsync(statusDate);

                if (statusDate.Date < fromDate.Date)
                {
                    Assert.That(result.Posted, Is.EqualTo(0M));
                    continue;
                }

                if (statusDate.Date >= fromDate.Date && statusDate.Date <= toDate.Date)
                {
                    Assert.That(result.Posted, Is.EqualTo(postingLineMockCollection.Where(postingLineMock => postingLineMock.Object.PostingDate.Date >= fromDate.Date && postingLineMock.Object.PostingDate.Date <= statusDate.Date).Sum(postingLineMock => postingLineMock.Object.PostingValue)));
                    continue;
                }

                Assert.That(result.Posted, Is.EqualTo(postingLineMockCollection.Where(postingLineMock => postingLineMock.Object.PostingDate.Date >= fromDate.Date && postingLineMock.Object.PostingDate.Date <= toDate.Date).Sum(postingLineMock => postingLineMock.Object.PostingValue)));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameBudgetInfo()
        {
            IBudgetInfo sut = CreateSut();

            IBudgetInfo result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameBudgetInfoWhereStatusDateEqualDateFromCall()
        {
            IBudgetInfo sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IBudgetInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        private IBudgetInfo CreateSut(IBudgetAccount budgetAccount = null, short? year = null, short? month = null)
        {
            return new Domain.Accounting.BudgetInfo(
                budgetAccount ?? _fixture.BuildBudgetAccountMock().Object,
                year ?? (short) _random.Next(InfoBase<IBudgetInfo>.MinYear, InfoBase<IBudgetInfo>.MaxYear),
                month ?? (short) _random.Next(InfoBase<IBudgetInfo>.MinMonth, InfoBase<IBudgetInfo>.MaxMonth),
                Math.Abs(_fixture.Create<decimal>()),
                Math.Abs(_fixture.Create<decimal>()));
        }
    }
}