using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.BudgetAccountGroupStatusToCsvConverter
{
    [TestFixture]
    public class ConvertAsyncTests
    {
        #region Private variables

        private Mock<IStatusDateProvider> _statusDateProviderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _statusDateProviderMock = new Mock<IStatusDateProvider>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ConvertAsync_WhenBudgetAccountGroupStatusIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("budgetAccountGroupStatus"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertNumberWasCalledOnBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = _fixture.BuildBudgetAccountGroupStatusMock();
            await sut.ConvertAsync(budgetAccountGroupStatusMock.Object);

            budgetAccountGroupStatusMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertNameWasCalledOnBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = _fixture.BuildBudgetAccountGroupStatusMock();
            await sut.ConvertAsync(budgetAccountGroupStatusMock.Object);

            budgetAccountGroupStatusMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertValuesForMonthOfStatusDateWasCalledOnBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = _fixture.BuildBudgetAccountGroupStatusMock();
            await sut.ConvertAsync(budgetAccountGroupStatusMock.Object);

            budgetAccountGroupStatusMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertBudgetWasCalledOnValuesForMonthOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForMonthOfStatusDate: valuesForMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForMonthOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertPostedWasCalledOnValuesForMonthOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForMonthOfStatusDate: valuesForMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForMonthOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertValuesForLastMonthOfStatusDateWasCalledOnBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = _fixture.BuildBudgetAccountGroupStatusMock();
            await sut.ConvertAsync(budgetAccountGroupStatusMock.Object);

            budgetAccountGroupStatusMock.Verify(m => m.ValuesForLastMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertBudgetWasCalledOnValuesForLastMonthOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastMonthOfStatusDate: valuesForLastMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForLastMonthOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertPostedWasCalledOnValuesForLastMonthOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastMonthOfStatusDate: valuesForLastMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForLastMonthOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertValuesForYearToDateOfStatusDateWasCalledOnBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = _fixture.BuildBudgetAccountGroupStatusMock();
            await sut.ConvertAsync(budgetAccountGroupStatusMock.Object);

            budgetAccountGroupStatusMock.Verify(m => m.ValuesForYearToDateOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertBudgetWasCalledOnValuesForYearToDateOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForYearToDateOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForYearToDateOfStatusDate: valuesForYearToDateOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForYearToDateOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertPostedWasCalledOnValuesForYearToDateOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForYearToDateOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForYearToDateOfStatusDate: valuesForYearToDateOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForYearToDateOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertValuesForLastYearOfStatusDateWasCalledOnBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroupStatus> budgetAccountGroupStatusMock = _fixture.BuildBudgetAccountGroupStatusMock();
            await sut.ConvertAsync(budgetAccountGroupStatusMock.Object);

            budgetAccountGroupStatusMock.Verify(m => m.ValuesForLastYearOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertBudgetWasCalledOnValuesForLastYearOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastYearOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastYearOfStatusDate: valuesForLastYearOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForLastYearOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_AssertPostedWasCalledOnValuesForLastYearOfStatusDateFromBudgetAccountGroupStatus()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastYearOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccountGroupStatus budgetAccountGroupStatus = _fixture.BuildBudgetAccountGroupStatusMock(valuesForLastYearOfStatusDate: valuesForLastYearOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccountGroupStatus);

            valuesForLastYearOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_ReturnsNotNull()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildBudgetAccountGroupStatusMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_ReturnsNonEmptyCollection()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildBudgetAccountGroupStatusMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountGroupStatusIsNotNull_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromGetColumnNamesAsync()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            string[] result = (await sut.ConvertAsync(_fixture.BuildBudgetAccountGroupStatusMock().Object)).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.GetColumnNamesAsync()).Count()));
        }

        private IBudgetAccountGroupStatusToCsvConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.BudgetAccountGroupStatusToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}