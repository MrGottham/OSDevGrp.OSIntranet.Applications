using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class CreateBudgetAccountTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();

            _fixture = new Fixture();
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumber_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountingQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.CreateBudgetAccount(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(query => query != null && query.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumber_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForBudgetAccountGroupCollection()
        {
            Controller sut = CreateSut();

            await sut.CreateBudgetAccount(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForNonExistingAccounting_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForNonExistingAccounting_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereViewNameIsEqualToEditBudgetAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_EditBudgetAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<BudgetAccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithEditModeEqualToCreate()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountingMatchingAccountingFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            Controller sut = CreateSut(accounting: accounting);

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountNumberEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.AccountNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetAccountGroup, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForMonthOfStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForMonthOfStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForLastMonthOfStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForLastMonthOfStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForYearToDateOfStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForYearToDateOfStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForLastYearOfStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForLastYearOfStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetInfosNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetInfos, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetInfosContainingDataForCurrentYear()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            short currentYear = (short) DateTime.Today.Year;
            short currentMonth = (short) DateTime.Today.Month;

            Assert.That(budgetAccountViewModel.BudgetInfos.ContainsKey(currentYear), Is.True);

            BudgetInfoCollectionViewModel budgetInfoCollectionViewModel = budgetAccountViewModel.BudgetInfos[currentYear];
            Assert.That(budgetInfoCollectionViewModel, Is.Not.Null);
            Assert.That(budgetInfoCollectionViewModel.Count, Is.EqualTo(12 - currentMonth + 1));
            for (short month = currentMonth; month <= 12; month++)
            {
                BudgetInfoViewModel budgetInfoViewModel = budgetInfoCollectionViewModel.Single(m => m.Year == currentYear && m.Month == month);
                Assert.That(budgetInfoViewModel.EditMode, Is.EqualTo(EditMode.Create));
                Assert.That(budgetInfoViewModel.Income, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Expenses, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Budget, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Posted, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Available, Is.EqualTo(0M));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetInfosContainingDataForNextYear()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            short nextYear = (short) (DateTime.Today.Year + 1);

            Assert.That(budgetAccountViewModel.BudgetInfos.ContainsKey(nextYear), Is.True);

            BudgetInfoCollectionViewModel budgetInfoCollectionViewModel = budgetAccountViewModel.BudgetInfos[nextYear];
            Assert.That(budgetInfoCollectionViewModel, Is.Not.Null);
            Assert.That(budgetInfoCollectionViewModel.Count, Is.EqualTo(12));
            for (short month = 1; month <= 12; month++)
            {
                BudgetInfoViewModel budgetInfoViewModel = budgetInfoCollectionViewModel.Single(m => m.Year == nextYear && m.Month == month);
                Assert.That(budgetInfoViewModel.EditMode, Is.EqualTo(EditMode.Create));
                Assert.That(budgetInfoViewModel.Income, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Expenses, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Budget, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Posted, Is.EqualTo(0M));
                Assert.That(budgetInfoViewModel.Available, Is.EqualTo(0M));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetAccountGroups, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupsMatchingBudgetAccountGroupCollectionFromQueryBus()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToArray();
            Controller sut = CreateSut(budgetAccountGroupCollection: budgetAccountGroupCollection);

            PartialViewResult result = (PartialViewResult) await sut.CreateBudgetAccount(_fixture.Create<int>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountGroupCollection.All(budgetAccountGroup => budgetAccountViewModel.BudgetAccountGroups.SingleOrDefault(m => m.Number == budgetAccountGroup.Number) != null), Is.True);
        }

        private Controller CreateSut(bool hasAccounting = true, IAccounting accounting = null, IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(budgetAccountGroupCollection ?? _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}