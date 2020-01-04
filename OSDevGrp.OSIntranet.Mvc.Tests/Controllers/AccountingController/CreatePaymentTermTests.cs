using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class CreatePaymentTermTests
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
        public void CreatePaymentTerm_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreatePaymentTerm();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePaymentTerm_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreatePaymentTerm()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreatePaymentTerm();

            Assert.That(result.ViewName, Is.EqualTo("CreatePaymentTerm"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePaymentTerm_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsPaymentTermViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreatePaymentTerm();

            Assert.That(result.Model, Is.TypeOf<PaymentTermViewModel>());

            PaymentTermViewModel paymentTermViewModel = (PaymentTermViewModel) result.Model;

            Assert.That(paymentTermViewModel, Is.Not.Null);
            Assert.That(paymentTermViewModel.Number, Is.EqualTo(default(int)));
            Assert.That(paymentTermViewModel.Name, Is.Null);
            Assert.That(paymentTermViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreatePaymentTerm_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreatePaymentTerm(null));

            Assert.That(result.ParamName, Is.EqualTo("paymentTermViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            PaymentTermViewModel model = CreateModel();
            await sut.CreatePaymentTerm(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreatePaymentTermCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(false);

            PaymentTermViewModel model = CreateModel();
            IActionResult result = await sut.CreatePaymentTerm(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreatePaymentTerm()
        {
            Controller sut = CreateSut(false);

            PaymentTermViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreatePaymentTerm(model);

            Assert.That(result.ViewName, Is.EqualTo("CreatePaymentTerm"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(false);

            PaymentTermViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreatePaymentTerm(model);

            Assert.That(result.Model, Is.TypeOf<PaymentTermViewModel>());

            PaymentTermViewModel paymentTermViewModel = (PaymentTermViewModel) result.Model;

            Assert.That(paymentTermViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            await sut.CreatePaymentTerm(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreatePaymentTermCommand>(command =>
                    command.Number == model.Number &&
                    string.CompareOrdinal(command.Name, model.Name) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            IActionResult result = await sut.CreatePaymentTerm(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreatePaymentTerm(model);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreatePaymentTerm_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToPaymentTerms()
        {
            Controller sut = CreateSut();

            PaymentTermViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreatePaymentTerm(model);

            Assert.That(result.ActionName, Is.EqualTo("PaymentTerms"));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreatePaymentTermCommand>()))
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
