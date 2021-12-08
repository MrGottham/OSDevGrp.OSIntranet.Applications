using System;
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
        public async Task CalculateAsync_WhenStatusDateIsNewerThanBudgetInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromBudgetAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(year, month, 1)),
                    It.Is<DateTime>(value => value == new DateTime(year, month, DateTime.DaysInMonth(year, month))),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinBudgetInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(year, month, 1)),
                    It.Is<DateTime>(value => value == statusDate),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanBudgetInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromBudgetAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(year, month, 1)),
                    It.Is<DateTime>(value => value == statusDate),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenCalled_AssertPostedIsCalculated()
        {
            DateTime today = DateTime.Today;
            DateTime calculationDate = DateTime.Today.AddMonths(_random.Next(1, 5) * -1).AddDays(today.Day * -1).AddDays(1);

            int year = calculationDate.Year;
            int month = calculationDate.Month;

            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IBudgetInfo sut = CreateSut(budgetAccount, (short)year, (short)month);

            IBudgetInfo result = await sut.CalculateAsync(calculationDate);

            Assert.That(result.Posted, Is.EqualTo(calculatedPostingValue));
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