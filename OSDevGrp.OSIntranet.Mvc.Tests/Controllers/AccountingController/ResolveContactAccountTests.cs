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
    public class ResolveContactAccountTests
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
        public async Task ResolveContactAccount_WhenAccountNumberIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), null, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenAccountNumberIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), null, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenAccountNumberIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), string.Empty, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenAccountNumberIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), string.Empty, DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenAccountNumberIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), " ", DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenAccountNumberIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), " ", DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_AssertQueryAsyncWasCalledOnQueryBusWithGetContactAccountQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(0, 7) * -1).AddMinutes(_random.Next(-120, 120));
            await sut.ResolveContactAccount(accountingNumber, accountNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactAccountQuery, IContactAccount>(It.Is<IGetContactAccountQuery>(query => query != null && query.AccountingNumber == accountingNumber && string.CompareOrdinal(query.AccountNumber, accountNumber.ToUpper()) == 0 && query.StatusDate == statusDate.Date)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenNoContactAccountWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenNoContactAccountWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenContactAccountWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenContactAccountWasReturnedFromQueryBus_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenContactAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenContactAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsContactAccountViewModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult)await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            Assert.That(result.Value, Is.TypeOf<ContactAccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenContactAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsContactAccountViewModelWithAccountingNumberEqualToAccountingNumberOnContactAccountFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting).Object;
            Controller sut = CreateSut(contactAccount: contactAccount);

            OkObjectResult result = (OkObjectResult)await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel)result.Value;

            Assert.That(contactAccountViewModel.Accounting, Is.Not.Null);
            Assert.That(contactAccountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ResolveContactAccount_WhenContactAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsContactAccountViewModelWithAccountNumberEqualToAccountNumberOnContactAccountFromQueryBus()
        {
            string accountNumber = _fixture.Create<string>();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accountNumber: accountNumber).Object;
            Controller sut = CreateSut(contactAccount: contactAccount);

            OkObjectResult result = (OkObjectResult)await sut.ResolveContactAccount(_fixture.Create<int>(), _fixture.Create<string>(), DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel)result.Value;

            Assert.That(contactAccountViewModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        private Controller CreateSut(bool hasContactAccount = true, IContactAccount contactAccount = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetContactAccountQuery, IContactAccount>(It.IsAny<IGetContactAccountQuery>()))
                .Returns(Task.FromResult(hasContactAccount ? contactAccount ?? _fixture.BuildContactAccountMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}