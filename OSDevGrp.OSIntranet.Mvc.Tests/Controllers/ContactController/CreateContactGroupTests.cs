using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class CreateContactGroupTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void CreateContactGroup_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateContactGroup();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateContactGroup_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateContactGroup()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateContactGroup();

            Assert.That(result.ViewName, Is.EqualTo("CreateContactGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateContactGroup_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsContactGroupViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateContactGroup();

            Assert.That(result.Model, Is.TypeOf<ContactGroupViewModel>());

            ContactGroupViewModel contactGroupViewModel = (ContactGroupViewModel) result.Model;

            Assert.That(contactGroupViewModel, Is.Not.Null);
            Assert.That(contactGroupViewModel.Number, Is.EqualTo(default(int)));
            Assert.That(contactGroupViewModel.Name, Is.Null);
            Assert.That(contactGroupViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateContactGroup_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateContactGroup(null));

            Assert.That(result.ParamName, Is.EqualTo("contactGroupViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            ContactGroupViewModel model = CreateModel();
            await sut.CreateContactGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateContactGroupCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(false);

            ContactGroupViewModel model = CreateModel();
            IActionResult result = await sut.CreateContactGroup(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateContactGroup()
        {
            Controller sut = CreateSut(false);

            ContactGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateContactGroup(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateContactGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(false);

            ContactGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateContactGroup(model);

            Assert.That(result.Model, Is.TypeOf<ContactGroupViewModel>());

            ContactGroupViewModel contactGroupViewModel = (ContactGroupViewModel) result.Model;

            Assert.That(contactGroupViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            await sut.CreateContactGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateContactGroupCommand>(command =>
                    command.Number == model.Number &&
                    string.CompareOrdinal(command.Name, model.Name) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            IActionResult result = await sut.CreateContactGroup(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContactGroup(model);

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToCountries()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContactGroup(model);

            Assert.That(result.ActionName, Is.EqualTo("ContactGroups"));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateContactGroupCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _tokenHelperFactoryMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private ContactGroupViewModel CreateModel()
        {
            return _fixture.Create<ContactGroupViewModel>();
        }
    }
}