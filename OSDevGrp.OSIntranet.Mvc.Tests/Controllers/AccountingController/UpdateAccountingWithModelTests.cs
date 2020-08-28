﻿using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class UpdateAccountingWithModelTests
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
        public void UpdateAccounting_WhenAccountingViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccounting(null));

            Assert.That(result.ParamName, Is.EqualTo("accountingViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            await sut.UpdateAccounting(CreateAccountingViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateAccountingCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut(false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut(false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut(false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumber()
        {
            Controller sut = CreateSut(false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.RouteValues.ContainsKey("AccountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsInvalid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumberWithValueFromAccountingViewModel()
        {
            Controller sut = CreateSut(false);

            int accountingNumber = _fixture.Create<int>();
            AccountingViewModel accountingViewModel = CreateAccountingViewModel(accountingNumber);
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(accountingViewModel);

            Assert.That(result.RouteValues["AccountingNumber"], Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingViewModel accountingViewModel = CreateAccountingViewModel(accountingNumber);
            await sut.UpdateAccounting(accountingViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountingCommand>(command => command != null && command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumber()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(CreateAccountingViewModel());

            Assert.That(result.RouteValues.ContainsKey("AccountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccounting_WhenAccountingViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumberWithValueFromAccountingViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingViewModel accountingViewModel = CreateAccountingViewModel(accountingNumber);
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccounting(accountingViewModel);

            Assert.That(result.RouteValues["AccountingNumber"], Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateAccountingCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private AccountingViewModel CreateAccountingViewModel(int? accountingNumber = null)
        {
            return _fixture.Build<AccountingViewModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .Create();
        }
    }
}