using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class ApplyCalculationAsyncWithContactAccountTests
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
        public void ApplyCalculationAsync_WhenCalculatedContactAccountIsNull_ThrowsArgumentNullException()
        {
            IPostingLine sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IContactAccount)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedContactAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasNotGiven_AssertPostingLineCollectionWasCalledOnCalculatedContactAccount()
        {
            IPostingLine sut = CreateSut();

            Mock<IContactAccount> calculatedContactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ApplyCalculationAsync(calculatedContactAccountMock.Object);

            calculatedContactAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasNotGiven_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromCalculatedContactAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, sortOrder);

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedContactAccount);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasGiven_AssertPostingLineCollectionWasNotCalledOnCalculatedContactAccount()
        {
            IContactInfoValues contactAccountValuesAtPostingDate = _fixture.BuildContactInfoValuesMock().Object;
            IPostingLine sut = CreateSut(contactAccountValuesAtPostingDate: contactAccountValuesAtPostingDate);

            Mock<IContactAccount> calculatedContactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ApplyCalculationAsync(calculatedContactAccountMock.Object);

            calculatedContactAccountMock.Verify(m => m.PostingLineCollection, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereContactAccountIsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result.ContactAccount, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereContactAccountIsEqualToCalculatedContactAccount()
        {
            IPostingLine sut = CreateSut();

            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock().Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedContactAccount);

            Assert.That(result.ContactAccount, Is.EqualTo(calculatedContactAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasNotGiven_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateNotEqualToNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasNotGiven_ReturnsSamePostingLineWhereBalanceOnContactAccountValuesAtPostingDateEqualToCalculatedPostingValueFromPostingLineCollection()
        {
            IPostingLine sut = CreateSut();

            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue, isEmpty: true).Object;
            IContactAccount calculatedContactAccount = _fixture.BuildContactAccountMock(postingLineCollection: postingLineCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedContactAccount);

            Assert.That(result.ContactAccountValuesAtPostingDate.Balance, Is.EqualTo(calculatedPostingValue));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasGiven_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateNotEqualToNull()
        {
            IContactInfoValues contactAccountValuesAtPostingDate = _fixture.BuildContactInfoValuesMock().Object;
            IPostingLine sut = CreateSut(contactAccountValuesAtPostingDate: contactAccountValuesAtPostingDate);

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereContactAccountValuesAtPostingDateWasGiven_ReturnsSamePostingLineWhereContactAccountValuesAtPostingDateEqualToGivenContactAccountValuesAtPostingDate()
        {
            IContactInfoValues contactAccountValuesAtPostingDate = _fixture.BuildContactInfoValuesMock().Object;
            IPostingLine sut = CreateSut(contactAccountValuesAtPostingDate: contactAccountValuesAtPostingDate);

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result.ContactAccountValuesAtPostingDate, Is.EqualTo(contactAccountValuesAtPostingDate));
        }

        private IPostingLine CreateSut(DateTime? postingDate = null, int? sortOrder = null, IContactInfoValues contactAccountValuesAtPostingDate = null)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, Math.Min(DateTime.Today.Year, InfoBase<ICreditInfo>.MaxYear));
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, Math.Min(DateTime.Today.Month, InfoBase<ICreditInfo>.MaxMonth));
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            IAccounting accounting = _fixture.BuildAccountingMock().Object;

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), postingDate ?? new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock(accounting).Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), _fixture.BuildContactAccountMock(accounting).Object, sortOrder ?? Math.Abs(_fixture.Create<int>()), null, null, contactAccountValuesAtPostingDate);
        }
    }
}