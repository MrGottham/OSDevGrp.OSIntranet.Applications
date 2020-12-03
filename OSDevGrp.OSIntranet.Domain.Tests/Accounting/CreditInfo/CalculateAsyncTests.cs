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

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.CreditInfo
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
        public async Task CalculateAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnAccount()
        {
            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            ICreditInfo sut = CreateSut(accountMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            accountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostingDateWasCalledOnEachPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanCreditInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanCreditInfo_AssertPostingValueWasCalledOnEachPostingLineWhichAreWithinCreditInfoInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanCreditInfo_AssertPostingValueWasCalledOnEachOlderPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinCreditInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinCreditInfo_AssertPostingValueWasCalledOnEachPostingLineWhichAreWithinCreditInfoInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinCreditInfo_AssertPostingValueWasCalledOnEachOlderPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanCreditInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanCreditInfo_AssertPostingValueWasNotCalledOnAnyPostingLineWhichAreWithinCreditInfoInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanCreditInfo_AssertPostingValueWasCalledOnEachOlderPostingLineInPostingLineCollectionOnAccount()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnAccountOnlyContainsPostingLinesNewerThanStatusDate_AssertCalculatedBalanceIsEqualToZero()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            ICreditInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnAccountOnlyContainsPostingLinesBetweenStatusDateAndFromDate_AssertCalculatedBalanceIsEqualToSumOfPostingValueFromPostingLines()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            ICreditInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Balance, Is.EqualTo(postingLineMockCollection.Sum(postingLineMock => postingLineMock.Object.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnAccountOnlyContainsPostingLinesOlderThanStatus_AssertCalculatedBalanceIsEqualToSumOfPostingValueFromPostingLines()
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
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short) year, (short) month);

            ICreditInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Balance, Is.EqualTo(postingLineMockCollection.Sum(postingLineMock => postingLineMock.Object.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceIsCalculated()
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
                IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
                ICreditInfo sut = CreateSut(account, (short) year, (short) month);

                ICreditInfo result = await sut.CalculateAsync(statusDate);

                if (statusDate.Date < fromDate.Date)
                {
                    Assert.That(result.Balance, Is.EqualTo(postingLineMockCollection.Where(postingLineMock => postingLineMock.Object.PostingDate.Date <= statusDate.Date).Sum(postingLineMock => postingLineMock.Object.PostingValue)));
                    continue;
                }

                if (statusDate.Date >= fromDate.Date && statusDate.Date <= toDate.Date)
                {
                    Assert.That(result.Balance, Is.EqualTo(postingLineMockCollection.Where(postingLineMock => postingLineMock.Object.PostingDate.Date <= statusDate.Date).Sum(postingLineMock => postingLineMock.Object.PostingValue)));
                    continue;
                }

                Assert.That(result.Balance, Is.EqualTo(postingLineMockCollection.Where(postingLineMock => postingLineMock.Object.PostingDate.Date <= toDate.Date).Sum(postingLineMock => postingLineMock.Object.PostingValue)));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCreditInfo()
        {
            ICreditInfo sut = CreateSut();

            ICreditInfo result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameCreditInfoWhereStatusDateEqualDateFromCall()
        {
            ICreditInfo sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            ICreditInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        private ICreditInfo CreateSut(IAccount account = null, short? year = null, short? month = null)
        {
            return new Domain.Accounting.CreditInfo(
                account ?? _fixture.BuildAccountMock().Object,
                year ?? (short) _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear),
                month ?? (short) _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth),
                Math.Abs(_fixture.Create<decimal>()));
        }
    }
}