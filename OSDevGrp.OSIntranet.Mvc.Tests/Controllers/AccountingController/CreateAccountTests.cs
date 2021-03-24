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
    public class CreateAccountTests
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
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumber_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountingQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.CreateAccount(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(query => query != null && query.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumber_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForAccountGroupCollection()
        {
            Controller sut = CreateSut();

            await sut.CreateAccount(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForNonExistingAccounting_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForNonExistingAccounting_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereViewNameIsEqualToEditAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_EditAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<AccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithEditModeEqualToCreate()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountingMatchingAccountingFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            Controller sut = CreateSut(accounting: accounting);

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountNumberEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountGroup, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithValuesAtStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.ValuesAtStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithValuesAtEndOfLastMonthFromStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.ValuesAtEndOfLastMonthFromStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithValuesAtEndOfLastYearFromStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.ValuesAtEndOfLastYearFromStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithCreditInfosNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.CreditInfos, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithCreditInfosContainingDataForCurrentYear()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            short currentYear = (short) DateTime.Today.Year;
            short currentMonth = (short) DateTime.Today.Month;

            Assert.That(accountViewModel.CreditInfos.ContainsKey(currentYear), Is.True);

            CreditInfoCollectionViewModel creditInfoCollectionViewModel = accountViewModel.CreditInfos[currentYear];
            Assert.That(creditInfoCollectionViewModel, Is.Not.Null);
            Assert.That(creditInfoCollectionViewModel.Count, Is.EqualTo(12 - currentMonth + 1));
            for (short month = currentMonth; month <= 12; month++)
            {
                CreditInfoViewModel creditInfoViewModel = creditInfoCollectionViewModel.Single(m => m.Year == currentYear && m.Month == month);
                Assert.That(creditInfoViewModel.EditMode, Is.EqualTo(EditMode.Create));
                Assert.That(creditInfoViewModel.Credit, Is.EqualTo(0M));
                Assert.That(creditInfoViewModel.Balance, Is.EqualTo(0M));
                Assert.That(creditInfoViewModel.Available, Is.EqualTo(0M));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithCreditInfosContainingDataForNextYear()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            short nextYear = (short) (DateTime.Today.Year + 1);

            Assert.That(accountViewModel.CreditInfos.ContainsKey(nextYear), Is.True);

            CreditInfoCollectionViewModel creditInfoCollectionViewModel = accountViewModel.CreditInfos[nextYear];
            Assert.That(creditInfoCollectionViewModel, Is.Not.Null);
            Assert.That(creditInfoCollectionViewModel.Count, Is.EqualTo(12));
            for (short month = 1; month <= 12; month++)
            {
                CreditInfoViewModel creditInfoViewModel = creditInfoCollectionViewModel.Single(m => m.Year == nextYear && m.Month == month);
                Assert.That(creditInfoViewModel.EditMode, Is.EqualTo(EditMode.Create));
                Assert.That(creditInfoViewModel.Credit, Is.EqualTo(0M));
                Assert.That(creditInfoViewModel.Balance, Is.EqualTo(0M));
                Assert.That(creditInfoViewModel.Available, Is.EqualTo(0M));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountGroups, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupsMatchingAccountGroupCollectionFromQueryBus()
        {
            IEnumerable<IAccountGroup> accountGroupCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToArray();
            Controller sut = CreateSut(accountGroupCollection: accountGroupCollection);

            PartialViewResult result = (PartialViewResult) await sut.CreateAccount(_fixture.Create<int>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountGroupCollection.All(accountGroup => accountViewModel.AccountGroups.SingleOrDefault(m => m.Number == accountGroup.Number) != null), Is.True);
        }

        private Controller CreateSut(bool hasAccounting = true, IAccounting accounting = null, IEnumerable<IAccountGroup> accountGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(accountGroupCollection ?? _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}