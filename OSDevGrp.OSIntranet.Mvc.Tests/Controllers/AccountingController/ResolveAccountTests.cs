using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class ResolveAccountTests
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
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), null, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), null, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), string.Empty, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), string.Empty, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), " ", DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), " ", DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 7) * -1).AddMinutes(_random.Next(-120, 120));
            await sut.ResolveAccount(accountingNumber, accountNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.Is<IGetAccountQuery>(query => query != null && query.AccountingNumber == accountingNumber && string.CompareOrdinal(query.AccountNumber, accountNumber.ToUpper()) == 0 && query.StatusDate == statusDate.Date)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenNoAccountWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenNoAccountWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsAccountViewModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result.Value, Is.TypeOf<AccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsAccountViewModelWithAccountingNumberEqualToAccountingNumberOnAccountFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IAccount account = _fixture.BuildAccountMock(accounting).Object;
            Controller sut = CreateSut(account: account);

            OkObjectResult result = (OkObjectResult)await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            AccountViewModel accountViewModel = (AccountViewModel)result.Value;

            Assert.That(accountViewModel.Accounting, Is.Not.Null);
            Assert.That(accountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveAccount_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsAccountViewModelWithAccountNumberEqualToAccountNumberOnAccountFromQueryBus()
        {
            string accountNumber = _fixture.Create<string>();
            IAccount account = _fixture.BuildAccountMock(accountNumber: accountNumber).Object;
            Controller sut = CreateSut(account: account);

            OkObjectResult result = (OkObjectResult)await sut.ResolveAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            AccountViewModel accountViewModel = (AccountViewModel)result.Value;

            Assert.That(accountViewModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        private Controller CreateSut(bool hasAccount = true, IAccount account = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.IsAny<IGetAccountQuery>()))
                .Returns(Task.FromResult(hasAccount ? account ?? _fixture.BuildAccountMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}