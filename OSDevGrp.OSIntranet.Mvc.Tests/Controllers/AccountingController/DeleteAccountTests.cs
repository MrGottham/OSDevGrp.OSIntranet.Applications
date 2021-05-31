using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class DeleteAccountTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteAccount_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteAccount(_fixture.Create<int>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteAccount_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteAccount(_fixture.Create<int>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteAccount_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteAccount(_fixture.Create<int>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithDeleteAccountCommandContainingAccountingNumberFromArguments()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.DeleteAccount(accountingNumber, _fixture.Create<string>());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteAccountCommand>(command => command != null && command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithDeleteAccountCommandContainingAccountNumberFromArguments()
        {
            Controller sut = CreateSut();

            string accountNumber = _fixture.Create<string>();
            await sut.DeleteAccount(_fixture.Create<int>(), accountNumber);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumber()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.RouteValues.ContainsKey("accountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccount_WhenCalled_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumberWithAccountingNumberFromArguments()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccount(accountingNumber, _fixture.Create<string>());

            Assert.That(result.RouteValues["accountingNumber"], Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteAccountCommand>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}