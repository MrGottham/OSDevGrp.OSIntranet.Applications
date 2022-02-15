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
    public class ApplyCalculationAsyncWithBudgetAccountTests
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
        public void ApplyCalculationAsync_WhenCalculatedBudgetAccountIsNull_ThrowsArgumentNullException()
        {
            IPostingLine sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ApplyCalculationAsync((IBudgetAccount)null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("calculatedBudgetAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGiven_AssertBudgetInfoCollectionWasCalledOnCalculatedBudgetAccount()
        {
            IPostingLine sut = CreateSut();

            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ApplyCalculationAsync(calculatedBudgetAccountMock.Object);

            calculatedBudgetAccountMock.Verify(m => m.BudgetInfoCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGiven_AssertFindWasCalledOnBudgetInfoCollectionFromCalculatedBudgetAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            IPostingLine sut = CreateSut(postingDate);

            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = _fixture.BuildBudgetInfoCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(budgetInfoCollection: budgetInfoCollectionMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            budgetInfoCollectionMock.Verify(m => m.Find(It.Is<DateTime>(value => value == postingDate)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGiven_AssertPostingLineCollectionWasCalledOnCalculatedBudgetAccount()
        {
            IPostingLine sut = CreateSut();

            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ApplyCalculationAsync(calculatedBudgetAccountMock.Object);

            calculatedBudgetAccountMock.Verify(m => m.PostingLineCollection, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGiven_AssertCalculatePostingValueWasCalledOnPostingLineCollectionFromCalculatedBudgetAccount()
        {
            DateTime postingDate = DateTime.Today.AddDays(_random.Next(-120, 120));
            int sortOrder = _fixture.Create<int>();
            IPostingLine sut = CreateSut(postingDate, sortOrder);

            Mock<IPostingLineCollection> postingLineCollectionMock = _fixture.BuildPostingLineCollectionMock();
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollectionMock.Object).Object;
            await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            postingLineCollectionMock.Verify(m => m.CalculatePostingValue(
                    It.Is<DateTime>(value => value == new DateTime(postingDate.Year, postingDate.Month, 1)),
                    It.Is<DateTime>(value => value == postingDate),
                    It.Is<int?>(value => value == sortOrder)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasGiven_AssertBudgetInfoCollectionWasNotCalledOnCalculatedBudgetAccount()
        {
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoMock().Object;
            IPostingLine sut = CreateSut(budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate);

            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ApplyCalculationAsync(calculatedBudgetAccountMock.Object);

            calculatedBudgetAccountMock.Verify(m => m.BudgetInfoCollection, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasGiven_AssertPostingLineCollectionWasNotCalledOnCalculatedBudgetAccount()
        {
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoMock().Object;
            IPostingLine sut = CreateSut(budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate);

            Mock<IBudgetAccount> calculatedBudgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ApplyCalculationAsync(calculatedBudgetAccountMock.Object);

            calculatedBudgetAccountMock.Verify(m => m.PostingLineCollection, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLine()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereBudgetAccountIsNotNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.BudgetAccount, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalled_ReturnsSamePostingLineWhereBudgetAccountIsEqualToCalculatedBudgetAccount()
        {
            IPostingLine sut = CreateSut();

            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock().Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            Assert.That(result.BudgetAccount, Is.EqualTo(calculatedBudgetAccount));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGiven_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateNotEqualToNull()
        {
            IPostingLine sut = CreateSut();

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGivenAndCalculatedBudgetAccountDoesNotHaveBudgetInfoForPostingDate_ReturnsSamePostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToZero()
        {
            IPostingLine sut = CreateSut();

            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock(hasBudgetInfoForFind: false).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(budgetInfoCollection: budgetInfoCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            Assert.That(result.BudgetAccountValuesAtPostingDate.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGivenAndCalculatedBudgetAccountHasBudgetInfoForPostingDate_ReturnsSamePostingLineWhereBudgetOnBudgetAccountValuesAtPostingDateEqualToBudgetFromBudgetInfoForPostingDate()
        {
            IPostingLine sut = CreateSut();

            IBudgetInfo budgetInfo = _fixture.BuildBudgetInfoMock().Object;
            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock(budgetInfoForFind: budgetInfo).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(budgetInfoCollection: budgetInfoCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            Assert.That(result.BudgetAccountValuesAtPostingDate.Budget, Is.EqualTo(budgetInfo.Budget));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasNotGiven_ReturnsSamePostingLineWherePostedOnBudgetAccountValuesAtPostingDateEqualToCalculatedPostingValueFromPostingLineCollection()
        {
            IPostingLine sut = CreateSut();

            decimal calculatedPostingValue = _fixture.Create<decimal>();
            IPostingLineCollection postingLineCollection = _fixture.BuildPostingLineCollectionMock(calculatedPostingValue: calculatedPostingValue, isEmpty: true).Object;
            IBudgetAccount calculatedBudgetAccount = _fixture.BuildBudgetAccountMock(postingLineCollection: postingLineCollection).Object;
            IPostingLine result = await sut.ApplyCalculationAsync(calculatedBudgetAccount);

            Assert.That(result.BudgetAccountValuesAtPostingDate.Posted, Is.EqualTo(calculatedPostingValue));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasGiven_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateNotEqualToNull()
        {
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock().Object;
            IPostingLine sut = CreateSut(budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate);

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ApplyCalculationAsync_WhenCalledOnPostingLineWhereBudgetAccountValuesAtPostingDateWasGiven_ReturnsSamePostingLineWhereBudgetAccountValuesAtPostingDateEqualToBudgetAccountValuesAtPostingDate()
        {
            IBudgetInfoValues budgetAccountValuesAtPostingDate = _fixture.BuildBudgetInfoValuesMock().Object;
            IPostingLine sut = CreateSut(budgetAccountValuesAtPostingDate: budgetAccountValuesAtPostingDate);

            IPostingLine result = await sut.ApplyCalculationAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result.BudgetAccountValuesAtPostingDate, Is.EqualTo(budgetAccountValuesAtPostingDate));
        }

        private IPostingLine CreateSut(DateTime? postingDate = null, int? sortOrder = null, IBudgetInfoValues budgetAccountValuesAtPostingDate = null)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, Math.Min(DateTime.Today.Year, InfoBase<ICreditInfo>.MaxYear));
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, Math.Min(DateTime.Today.Month, InfoBase<ICreditInfo>.MaxMonth));
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            IAccounting accounting = _fixture.BuildAccountingMock().Object;

            return new Domain.Accounting.PostingLine(Guid.NewGuid(), postingDate ?? new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock(accounting).Object, _fixture.Create<string>(), _fixture.BuildBudgetAccountMock(accounting).Object, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, sortOrder ?? Math.Abs(_fixture.Create<int>()), null, budgetAccountValuesAtPostingDate);
        }
    }
}