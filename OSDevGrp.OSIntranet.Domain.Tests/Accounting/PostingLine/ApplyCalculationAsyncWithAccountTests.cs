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
    public class ApplyCalculationAsyncWithAccountTests
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
        public void ApplyCalculationAsync_WhenCalculatedAccountIsNull_ThrowsArgumentNullException()
        {
            IPostingLine sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IAccount)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGiven_AssertCreditInfoCollectionWasCalledOnCalculatedAccount()
        {
            IPostingLine sut = CreateSut();

            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            await sut.ApplyCalculationAsync(calculatedAccountMock.Object);

            calculatedAccountMock.Verify(m => m.CreditInfoCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGiven_AssertFindWasCalledOnCreditInfoCollectionFromCalculatedAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IPostingLine sut = CreateSut(postingDate);

            Mock<ICreditInfoCollection> creditInfoCollectionMock = _fixture.BuildCreditInfoCollectionMock();
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollectionMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            creditInfoCollectionMock.Verify(m => m.Find(It.Is<DateTime>(value => value == postingDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGiven_AssertPostingLineCollectionWasCalledOnCalculatedAccount()
        {
            IPostingLine sut = CreateSut();

            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            await sut.ApplyCalculationAsync(calculatedAccountMock.Object);

            calculatedAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGiven_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromCalculatedAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, sortOrder);

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IAccount calculatedAccount = _fixture.BuildAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedAccount);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == DateTime.MinValue),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasGiven_AssertCreditInfoCollectionWasNotCalledOnCalculatedAccount()
        {
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock().Object;
            IPostingLine sut = CreateSut(accountValuesAtPostingDate: accountValuesAtPostingDate);

            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            await sut.ApplyCalculationAsync(calculatedAccountMock.Object);

            calculatedAccountMock.Verify(m => m.CreditInfoCollection, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasGiven_AssertPostingLineCollectionWasNotCalledOnCalculatedAccount()
        {
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock().Object;
            IPostingLine sut = CreateSut(accountValuesAtPostingDate: accountValuesAtPostingDate);

            Mock<IAccount> calculatedAccountMock = _fixture.BuildAccountMock();
            await sut.ApplyCalculationAsync(calculatedAccountMock.Object);

            calculatedAccountMock.Verify(m => m.PostingLineCollection, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereAccountIsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result.Account, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereAccountIsEqualToCalculatedAccount()
        {
            IPostingLine sut = CreateSut();

            IAccount calculatedAccount = _fixture.BuildAccountMock().Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedAccount);

            Assert.That(result.Account, Is.EqualTo(calculatedAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGiven_ReturnsSamePostingLineWhereAccountValuesAtPostingDateNotEqualToNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result.AccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGivenAndCalculatedAccountDoesNotHaveCreditInfoForPostingDate_ReturnsSamePostingLineWhereCreditOnAccountValuesAtPostingDateEqualToZero()
        {
            IPostingLine sut = CreateSut();

            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock(hasCreditInfoForFind: false).Object;
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedAccount);

            Assert.That(result.AccountValuesAtPostingDate.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGivenAndCalculatedAccountHasCreditInfoForPostingDate_ReturnsSamePostingLineWhereCreditOnAccountValuesAtPostingDateEqualToCreditFromCreditInfoForPostingDate()
        {
            IPostingLine sut = CreateSut();

            ICreditInfo creditInfo = _fixture.BuildCreditInfoMock().Object;
            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock(creditInfoForFind: creditInfo).Object;
            IAccount calculatedAccount = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedAccount);

            Assert.That(result.AccountValuesAtPostingDate.Credit, Is.EqualTo(creditInfo.Credit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasNotGiven_ReturnsSamePostingLineWhereBalanceOnAccountValuesAtPostingDateEqualToCalculatedPostingValueFromPostingLineCollection()
        {
            IPostingLine sut = CreateSut();

            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue, isEmpty: true).Object;
            IAccount calculatedAccount = _fixture.BuildAccountMock(postingLineCollection: postingLineCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedAccount);

            Assert.That(result.AccountValuesAtPostingDate.Balance, Is.EqualTo(calculatedPostingValue));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasGiven_ReturnsSamePostingLineWhereAccountValuesAtPostingDateNotEqualToNull()
        {
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock().Object;
            IPostingLine sut = CreateSut(accountValuesAtPostingDate: accountValuesAtPostingDate);

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result.AccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereAccountValuesAtPostingDateWasGiven_ReturnsSamePostingLineWhereAccountValuesAtPostingDateEqualToGivenAccountValuesAtPostingDate()
        {
            ICreditInfoValues accountValuesAtPostingDate = _fixture.BuildCreditInfoValuesMock().Object;
            IPostingLine sut = CreateSut(accountValuesAtPostingDate: accountValuesAtPostingDate);

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result.AccountValuesAtPostingDate, Is.EqualTo(accountValuesAtPostingDate));
        }

        private IPostingLine CreateSut(DateTime? postingDate = null, int? sortOrder = null, ICreditInfoValues accountValuesAtPostingDate = null)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, Math.Min(DateTime.Today.Year, InfoBase<ICreditInfo>.MaxYear));
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, Math.Min(DateTime.Today.Month, InfoBase<ICreditInfo>.MaxMonth));
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), postingDate ?? new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock().Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, sortOrder ?? Math.Abs(_fixture.Create<int>()), accountValuesAtPostingDate);
        }
    }
}