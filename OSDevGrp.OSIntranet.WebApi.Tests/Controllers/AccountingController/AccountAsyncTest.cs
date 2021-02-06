using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class AccountAsyncTest
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), null));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToAccountNumber()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsEmpty_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsEmpty_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToAccountNumber()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), " "));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToAccountNumber()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenCalledWithoutStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            await sut.AccountAsync(accountingNumber, accountNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.Is<IGetAccountQuery>(value => value.AccountingNumber == accountingNumber && string.CompareOrdinal(value.AccountNumber, accountNumber.ToUpper()) == 0 && value.StatusDate == DateTime.Today)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountsAsync_WhenCalledWithStatusDate_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            DateTime statusDate = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            await sut.AccountAsync(accountingNumber, accountNumber, statusDate);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.Is<IGetAccountQuery>(value => value.AccountingNumber == accountingNumber && string.CompareOrdinal(value.AccountNumber, accountNumber.ToUpper()) == 0 && value.StatusDate == statusDate.Date)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountWasNotReturnedFromQueryBus_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut(false);

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>()));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountWasNotReturnedFromQueryBus_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueShouldBeKnown()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldBeKnown));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountWasNotReturnedFromQueryBus_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountAsync_WhenAccountWasNotReturnedFromQueryBus_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToAccountNumber()
        {
            Controller sut = CreateSut(false);

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenAccountWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<AccountModel> result = await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenAccountWasReturnedFromQueryBus_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<AccountModel> result = await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenAccountWasReturnedFromQueryBus_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<AccountModel> result = await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>())).Result;

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsAccountModel()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>())).Result;

            Assert.That(result.Value, Is.TypeOf<AccountModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AccountAsync_WhenAccountWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsAccountModelMatchingAccountFromQueryBus()
        {
            string accountNumber = _fixture.Create<string>();
            IAccount account = _fixture.BuildAccountMock(accountNumber: accountNumber).Object;
            Controller sut = CreateSut(account: account);

            OkObjectResult result = (OkObjectResult) (await sut.AccountAsync(_fixture.Create<int>(), _fixture.Create<string>())).Result;

            AccountModel accountModel = (AccountModel) result.Value;
            Assert.That(accountModel, Is.Not.Null);
            Assert.That(accountModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        private Controller CreateSut(bool hasAccount = true, IAccount account = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountQuery, IAccount>(It.IsAny<IGetAccountQuery>()))
                .Returns(Task.FromResult(hasAccount ? account ?? _fixture.BuildAccountMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}