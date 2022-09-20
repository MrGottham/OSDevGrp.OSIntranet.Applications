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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.BudgetAccountToCsvConverter
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
        public void ConvertAsync_WhenBudgetAccountIsNull_ThrowsArgumentNullException()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("budgetAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertAccountNumberWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertAccountNameWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.AccountName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertDescriptionWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.Description, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertNoteWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.Note, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertBudgetAccountGroupWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.BudgetAccountGroup, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertNumberWasCalledOnBudgetAccountGroupFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(budgetAccountGroup: budgetAccountGroupMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            budgetAccountGroupMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertNameWasCalledOnBudgetAccountGroupFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccountGroup> budgetAccountGroupMock = _fixture.BuildBudgetAccountGroupMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(budgetAccountGroup: budgetAccountGroupMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            budgetAccountGroupMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertValuesForMonthOfStatusDateWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertBudgetWasCalledOnValuesForMonthOfStatusFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForMonthOfStatusDate: valuesForMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForMonthOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertPostedWasCalledOnValuesForMonthOfStatusFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForMonthOfStatusDate: valuesForMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForMonthOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertValuesForLastMonthOfStatusDateWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.ValuesForLastMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertBudgetWasCalledOnValuesForLastMonthOfStatusDateFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForLastMonthOfStatusDate: valuesForLastMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForLastMonthOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertPostedWasCalledOnValuesForLastMonthOfStatusDateFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastMonthOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForLastMonthOfStatusDate: valuesForLastMonthOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForLastMonthOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertValuesForYearToDateOfStatusDateWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.ValuesForYearToDateOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertBudgetWasCalledOnValuesForYearToDateOfStatusDateFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForYearToDateOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForYearToDateOfStatusDate: valuesForYearToDateOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForYearToDateOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertPostedWasCalledOnValuesForYearToDateOfStatusDateFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForYearToDateOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForYearToDateOfStatusDate: valuesForYearToDateOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForYearToDateOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertValuesForLastYearOfStatusDateWasCalledOnBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetAccount> budgetAccountMock = _fixture.BuildBudgetAccountMock();
            await sut.ConvertAsync(budgetAccountMock.Object);

            budgetAccountMock.Verify(m => m.ValuesForLastYearOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertBudgetWasCalledOnValuesForLastYearOfStatusDateFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastYearOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForLastYearOfStatusDate: valuesForLastYearOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForLastYearOfStatusDateMock.Verify(m => m.Budget, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_AssertPostedWasCalledOnValuesForLastYearOfStatusDateFromBudgetAccount()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            Mock<IBudgetInfoValues> valuesForLastYearOfStatusDateMock = _fixture.BuildBudgetInfoValuesMock();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(valuesForLastYearOfStatusDate: valuesForLastYearOfStatusDateMock.Object).Object;
            await sut.ConvertAsync(budgetAccount);

            valuesForLastYearOfStatusDateMock.Verify(m => m.Posted, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_ReturnsNotNull()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_ReturnsNonEmptyCollection()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildBudgetAccountMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenBudgetAccountIsNotNull_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromGetColumnNamesAsync()
        {
            IBudgetAccountToCsvConverter sut = CreateSut();

            string[] result = (await sut.ConvertAsync(_fixture.BuildBudgetAccountMock().Object)).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.GetColumnNamesAsync()).Count()));
        }

        private IBudgetAccountToCsvConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.BudgetAccountToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}