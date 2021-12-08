using System;
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
        public async Task CalculateAsync_WhenStatusDateIsNewerThanContactInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromContactAccount()
        {
            DateTime statusDate = DateTime.Today;

            int year = statusDate.AddMonths(-3).Year;
            int month = statusDate.AddMonths(-3).Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IContactInfo sut = CreateSut(contactAccount, (short) year, (short) month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == new DateTime(year, month, DateTime.DaysInMonth(year, month))),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsWithinContactInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = new DateTime(today.AddMonths(-3).Year, today.AddMonths(-3).Month, 15);

            int year = statusDate.Year;
            int month = statusDate.Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IContactInfo sut = CreateSut(contactAccount, (short)year, (short)month);

            await sut.CalculateAsync(statusDate);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == statusDate),
                    It.Is<int?>(value => value.HasValue == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CalculateAsync_WhenStatusDateIsOlderThanContactInfo_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromContactAccount()
        {
            DateTime today = DateTime.Today;
            DateTime statusDate = DateTime.Today.AddMonths(-3);

            int year = today.Year;
            int month = today.Month;

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            IContactInfo sut = CreateSut(contactAccount, (short)year, (short)month);

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
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IContactInfo sut = CreateSut(contactAccount, (short)year, (short)month);

            IContactInfo result = await sut.CalculateAsync(calculationDate);

            Assert.That(result.Balance, Is.EqualTo(calculatedPostingValue));
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