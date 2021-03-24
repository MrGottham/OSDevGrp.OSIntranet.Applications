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
    public class UpdateAccountTests
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
        public void UpdateAccount_WhenAccountingNumberIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccount(_fixture.Create<int>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateAccount_WhenAccountingNumberIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccount(_fixture.Create<int>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateAccount_WhenAccountingNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccount(_fixture.Create<int>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumber_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            await sut.UpdateAccount(accountingNumber, accountNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.Is<IGetAccountQuery>(query => query != null && query.AccountingNumber == accountingNumber && string.CompareOrdinal(query.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumber_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForAccountGroupCollection()
        {
            Controller sut = CreateSut();

            await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForNonExistingAccount_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForNonExistingAccount_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereViewNameIsEqualToEditAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_EditAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<AccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithEditModeEqualToEdit()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountingMatchingAccountingOnAccountFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Controller sut = CreateSut(account: account);

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountNumberNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountNumber, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountNumberMatchingAccountNumberOnAccountFromQueryBus()
        {
            string accountNumber = _fixture.Create<string>();
            IAccount account = _fixture.BuildAccountMock(accountNumber: accountNumber).Object;
            Controller sut = CreateSut(account: account);

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountGroup, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupMatchingAccountGroupOnAccountFromQueryBus()
        {
            int accountGroupNumber = _fixture.Create<int>();
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(accountGroupNumber).Object;
            IAccount account = _fixture.BuildAccountMock(accountGroup: accountGroup).Object;
            Controller sut = CreateSut(account: account);

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountGroup.Number, Is.EqualTo(accountGroupNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithValuesAtStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithValuesAtEndOfLastMonthFromStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithValuesAtEndOfLastYearFromStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithCreditInfosNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.CreditInfos, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithCreditInfosContainingDataFromCreditInfoCollectionOnAccountFromQueryBus()
        {
            ICreditInfo[] creditInfos = _fixture.BuildCreditInfoCollectionMock().Object.ToArray();
            ICreditInfoCollection creditInfoCollection = _fixture.BuildCreditInfoCollectionMock(creditInfoCollection: creditInfos).Object;
            IAccount account = _fixture.BuildAccountMock(creditInfoCollection: creditInfoCollection).Object;
            Controller sut = CreateSut(account: account);

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            foreach (IGrouping<short, ICreditInfo> group in creditInfos.GroupBy(creditInfo => creditInfo.Year))
            {
                Assert.That(accountViewModel.CreditInfos.ContainsKey(group.Key), Is.True);

                CreditInfoCollectionViewModel creditInfoCollectionViewModel = accountViewModel.CreditInfos[group.Key];
                Assert.That(creditInfoCollectionViewModel, Is.Not.Null);
                Assert.That(creditInfoCollectionViewModel.All(creditInfoViewModel => group.SingleOrDefault(m => m.Month == creditInfoViewModel.Month) != null), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountViewModel.AccountGroups, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingAccount_ReturnsPartialViewResultWhereModelIsAccountViewModelWithAccountGroupsMatchingAccountGroupCollectionFromQueryBus()
        {
            IEnumerable<IAccountGroup> accountGroupCollection = _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToArray();
            Controller sut = CreateSut(accountGroupCollection: accountGroupCollection);

            PartialViewResult result = (PartialViewResult) await sut.UpdateAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountViewModel accountViewModel = (AccountViewModel) result.Model;

            Assert.That(accountGroupCollection.All(accountGroup => accountViewModel.AccountGroups.SingleOrDefault(m => m.Number == accountGroup.Number) != null), Is.True);
        }

        private Controller CreateSut(bool hasAccount = true, IAccount account = null, IEnumerable<IAccountGroup> accountGroupCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.IsAny<IGetAccountQuery>()))
                .Returns(Task.FromResult(hasAccount ? account ?? _fixture.BuildAccountMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IAccountGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(accountGroupCollection ?? _fixture.CreateMany<IAccountGroup>(_random.Next(5, 10)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}