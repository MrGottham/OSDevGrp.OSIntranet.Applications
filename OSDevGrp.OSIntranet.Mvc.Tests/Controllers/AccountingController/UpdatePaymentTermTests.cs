using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class UpdatePaymentTermTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithNumber_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdatePaymentTerm(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPaymentTermQuery, IPaymentTerm>(It.Is<IGetPaymentTermQuery>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithNumber_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdatePaymentTerm(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithNumber_ReturnsViewResultWhereViewNameIsEqualToUpdatePaymentTerm()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdatePaymentTerm(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdatePaymentTerm"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithNumber_ReturnsViewResultWhereModelIsPaymentTermViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock(number, name).Object;
            Controller sut = CreateSut(paymentTerm);

            ViewResult result = (ViewResult) await sut.UpdatePaymentTerm(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<PaymentTermViewModel>());

            PaymentTermViewModel paymentTermViewModel = (PaymentTermViewModel) result.Model;

            Assert.That(paymentTermViewModel, Is.Not.Null);
            Assert.That(paymentTermViewModel.Number, Is.EqualTo(number));
            Assert.That(paymentTermViewModel.Name, Is.EqualTo(name));
            Assert.That(paymentTermViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdatePaymentTerm_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdatePaymentTerm(null));

            Assert.That(result.ParamName, Is.EqualTo("paymentTermViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PaymentTermViewModel model = CreateModel();
            await sut.UpdatePaymentTerm(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdatePaymentTermCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PaymentTermViewModel model = CreateModel();
            IActionResult result = await sut.UpdatePaymentTerm(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdatePaymentTerm()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PaymentTermViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdatePaymentTerm(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdatePaymentTerm"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            PaymentTermViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdatePaymentTerm(model);

            Assert.That(result.Model, Is.TypeOf<PaymentTermViewModel>());

            PaymentTermViewModel paymentTermViewModel = (PaymentTermViewModel) result.Model;

            Assert.That(paymentTermViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            await sut.UpdatePaymentTerm(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdatePaymentTermCommand>(command => command.Number == model.Number && string.CompareOrdinal(command.Name, model.Name) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            IActionResult result = await sut.UpdatePaymentTerm(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdatePaymentTerm(model);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdatePaymentTerm_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToPaymentTerms()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdatePaymentTerm(model);

            Assert.That(result.ActionName, Is.EqualTo("PaymentTerms"));
        }

        private Controller CreateSut(IPaymentTerm paymentTerm = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetPaymentTermQuery, IPaymentTerm>(It.IsAny<IGetPaymentTermQuery>()))
                .Returns(Task.Run(() => paymentTerm ?? _fixture.BuildPaymentTermMock().Object));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdatePaymentTermCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private PaymentTermViewModel CreateModel()
        {
            return _fixture.Create<PaymentTermViewModel>();
        }
    }
}
