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
    public class UpdateBudgetAccountTests
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
        public void UpdateBudgetAccount_WhenAccountingNumberIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBudgetAccount(_fixture.Create<int>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateBudgetAccount_WhenAccountingNumberIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBudgetAccount(_fixture.Create<int>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateBudgetAccount_WhenAccountingNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBudgetAccount(_fixture.Create<int>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumber_AssertQueryAsyncWasCalledOnQueryBusWithGetBudgetAccountQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            await sut.UpdateBudgetAccount(accountingNumber, accountNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetBudgetAccountQuery, IBudgetAccount>(It.Is<IGetBudgetAccountQuery>(query => query != null && query.AccountingNumber == accountingNumber && string.CompareOrdinal(query.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumber_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForBudgetAccountGroupCollection()
        {
            Controller sut = CreateSut();

            await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForNonExistingBudgetAccount_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForNonExistingBudgetAccount_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereViewNameIsEqualToEditBudgetAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_EditBudgetAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<BudgetAccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithEditModeEqualToEdit()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountingMatchingAccountingOnBudgetAccountFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accounting).Object;
            Controller sut = CreateSut(budgetAccount: budgetAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountNumberNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.AccountNumber, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithAccountNumberMatchingAccountNumberOnBudgetAccountFromQueryBus()
        {
            string accountNumber = _fixture.Create<string>();
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(accountNumber: accountNumber).Object;
            Controller sut = CreateSut(budgetAccount: budgetAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetAccountGroup, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupMatchingBudgetAccountGroupOnBudgetAccountFromQueryBus()
        {
            int budgetAccountGroupNumber = _fixture.Create<int>();
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(budgetAccountGroupNumber).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(budgetAccountGroup: budgetAccountGroup).Object;
            Controller sut = CreateSut(budgetAccount: budgetAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetAccountGroup.Number, Is.EqualTo(budgetAccountGroupNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForMonthOfStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForLastMonthOfStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForLastMonthOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForYearToDateOfStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForYearToDateOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithValuesForLastYearOfStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.ValuesForLastYearOfStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetInfosNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetInfos, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetInfosContainingDataFromBudgetInfoCollectionOnBudgetAccountFromQueryBus()
        {
            IBudgetInfo[] budgetInfos = _fixture.BuildBudgetInfoCollectionMock().Object.ToArray();
            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock(budgetInfoCollection: budgetInfos).Object;
            IBudgetAccount budgetAccount = _fixture.BuildBudgetAccountMock(budgetInfoCollection: budgetInfoCollection).Object;
            Controller sut = CreateSut(budgetAccount: budgetAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            foreach (IGrouping<short, IBudgetInfo> group in budgetInfos.GroupBy(budgetInfo => budgetInfo.Year))
            {
                Assert.That(budgetAccountViewModel.BudgetInfos.ContainsKey(group.Key), Is.True);

                BudgetInfoCollectionViewModel budgetInfoCollectionViewModel = budgetAccountViewModel.BudgetInfos[group.Key];
                Assert.That(budgetInfoCollectionViewModel, Is.Not.Null);
                Assert.That(budgetInfoCollectionViewModel.All(budgetInfoViewModel => group.SingleOrDefault(m => m.Month == budgetInfoViewModel.Month) != null), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.PostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel) result.Model;

            Assert.That(budgetAccountViewModel.BudgetAccountGroups, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingBudgetAccount_ReturnsPartialViewResultWhereModelIsBudgetAccountViewModelWithBudgetAccountGroupsMatchingBudgetAccountGroupCollectionFromQueryBus()
        {
            IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToArray();
            Controller sut = CreateSut(budgetAccountGroupCollection: budgetAccountGroupCollection);

            PartialViewResult result = (PartialViewResult)await sut.UpdateBudgetAccount(_fixture.Create<int>(), _fixture.Create<string>());

            BudgetAccountViewModel budgetAccountViewModel = (BudgetAccountViewModel)result.Model;

            Assert.That(budgetAccountGroupCollection.All(budgetAccountGroup => budgetAccountViewModel.BudgetAccountGroups.SingleOrDefault(m => m.Number == budgetAccountGroup.Number) != null), Is.True);
        }

        private Controller CreateSut(bool hasBudgetAccount = true, IBudgetAccount budgetAccount = null, IEnumerable<IBudgetAccountGroup> budgetAccountGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetBudgetAccountQuery, IBudgetAccount>(It.IsAny<IGetBudgetAccountQuery>()))
                .Returns(Task.FromResult(hasBudgetAccount ? budgetAccount ?? _fixture.BuildBudgetAccountMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IBudgetAccountGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(budgetAccountGroupCollection ?? _fixture.CreateMany<IBudgetAccountGroup>(_random.Next(5, 10)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}