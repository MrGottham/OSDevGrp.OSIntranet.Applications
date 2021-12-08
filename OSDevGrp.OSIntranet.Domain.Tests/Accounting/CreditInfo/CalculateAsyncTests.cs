using System;
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
        public async Task CalculateAsync_WhenStatusDateIsNewerThanCreditInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            ICreditInfo sut = CreateSut(account, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == new DateTime(year, month, DateTime.DaysInMonth(year, month))),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinCreditInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            ICreditInfo sut = CreateSut(account, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == statusDate),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanCreditInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            ICreditInfo sut = CreateSut(account, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == statusDate),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertBalanceIsCalculated()
        {
            DateTime today = DateTime.Today;
            DateTime calculationDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(1);

            int year = calculationDate.Year;
            int month = calculationDate.Month;

            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue).Object;
            IAccount account = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            ICreditInfo sut = CreateSut(account, (short)year, (short)month);

            ICreditInfo result = await sut.CalculateAsync(calculationDate);

            Assert.That(result.Balance, Is.EqualTo(calculatedPostingValue));
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