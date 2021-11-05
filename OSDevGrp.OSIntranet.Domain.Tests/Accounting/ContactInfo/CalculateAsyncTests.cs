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

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactInfo
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
        public async Task CalculateAsync_WhenCalled_AssertPostingLineCollectionWasCalledOnContactAccount()
        {
            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            IContactInfo sut = CreateSut(contactAccountMock.Object);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            contactAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostingDateWasCalledOnEachPostingLineInPostingLineCollectionOnContactAccount()
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
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount);

            await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingDate, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanContactInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnContactAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, (int) statusDate.Subtract(toDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, (int) statusDate.Subtract(toDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanContactInfo_AssertPostingValueWasCalledOnEachPostingLineWhichAreWithinContactInfoInPostingLineCollectionOnContactAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: toDate.Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsNewerThanContactInfo_AssertPostingValueWasCalledOnEachOlderPostingLineInPostingLineCollectionOnContactAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinContactInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, (int) toDate.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, (int) toDate.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinContactInfo_AssertPostingValueWasCalledOnEachPostingLineWhichAreWithinContactInfoInPostingLineCollectionOnContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: statusDate.Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinContactInfo_AssertPostingValueWasCalledOnEachOlderPostingLineInPostingLineCollectionOnContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanContactInfo_AssertPostingValueWasNotCalledOnAnyNewerPostingLineInPostingLineCollectionOnContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, (int) today.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, (int) today.Subtract(statusDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanContactInfo_AssertPostingValueWasNotCalledOnAnyPostingLineWhichAreWithinContactInfoInPostingLineCollectionOnContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            DateTime fromDate = new DateTime(year, month, 1).Date;
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: today.Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, Math.Max((int) today.Subtract(fromDate).TotalDays, 1))).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanContactInfo_AssertPostingValueWasCalledOnEachOlderPostingLineInPostingLineCollectionOnContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            foreach (Mock<IPostingLine> postingLineMock in postingLineMockCollection)
            {
                postingLineMock.Verify(m => m.PostingValue, Times.Once);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnContactAccountOnlyContainsPostingLinesNewerThanStatusDate_AssertCalculatedBalanceIsEqualToZero()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.Date),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(7)),
                _fixture.BuildPostingLineMock(postingDate: statusDate.AddDays(1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            IContactInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Balance, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnContactAccountOnlyContainsPostingLinesBetweenStatusDateAndFromDate_AssertCalculatedBalanceIsEqualToSumOfPostingValueFromPostingLines()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: statusDate.Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) statusDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            IContactInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.Balance, Is.EqualTo(postingLineMockCollection.Sum(postingLineMock => postingLineMock.Object.PostingValue)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenPostingLineCollectionOnContactAccountOnlyContainsPostingLinesOlderThanStatus_AssertCalculatedBalanceIsEqualToSumOfPostingValueFromPostingLines()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            DateTime fromDate = new DateTime(year, month, 1);
            IEnumerable<Mock<IPostingLine>> postingLineMockCollection = new List<Mock<IPostingLine>>
            {
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            IContactInfo result = await sut.CalculateAsync(statusDate);

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
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(_random.Next(1, 365)).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.AddDays(1).Date),
                _fixture.BuildPostingLineMock(postingDate: toDate.Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, (int) toDate.Subtract(fromDate).TotalDays)).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(-1).Date),
                _fixture.BuildPostingLineMock(postingDate: fromDate.AddDays(_random.Next(1, 365) * -1).Date)
            };

            foreach (DateTime statusDate in postingLineMockCollection.Select(postingLineMock => postingLineMock.Object.PostingDate).OrderByDescending(value => value.Date))
            {
                IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(postingLineCollection: postingLineMockCollection.Select(m => m.Object).ToArray()).Object;
                IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
                IContactInfo sut = CreateSut(contactAccount, (short)year, (short)month);

                IContactInfo result = await sut.CalculateAsync(statusDate);

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
        public async Task CalculateAsync_WhenCalled_ReturnsSameContactInfo()
        {
            IContactInfo sut = CreateSut();

            IContactInfo result = await sut.CalculateAsync(DateTime.Now.AddDays(_random.Next(1, 365) * -1));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_ReturnsSameContactInfoWhereStatusDateEqualDateFromCall()
        {
            IContactInfo sut = CreateSut();

            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            IContactInfo result = await sut.CalculateAsync(statusDate);

            Assert.That(result.StatusDate, Is.EqualTo(statusDate.Date));
        }

        private IContactInfo CreateSut(IContactAccount contactAccount = null, short? year = null, short? month = null)
        {
            return new Domain.Accounting.ContactInfo(
                contactAccount ?? _fixture.BuildContactAccountMock().Object,
                year ?? (short) _random.Next(InfoBase<IContactInfo>.MinYear, InfoBase<IContactInfo>.MaxYear),
                month ?? (short) _random.Next(InfoBase<IContactInfo>.MinMonth, InfoBase<IContactInfo>.MaxMonth));
        }
    }
}