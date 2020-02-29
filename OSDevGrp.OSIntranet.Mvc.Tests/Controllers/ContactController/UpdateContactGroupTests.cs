using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class UpdateContactGroupTests
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
        public async Task UpdateContactGroup_WhenCalledWithNumber_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateContactGroup(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactGroupQuery, IContactGroup>(It.Is<IGetContactGroupQuery>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithNumber_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateContactGroup(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithNumber_ReturnsViewResultWhereViewNameIsEqualToUpdateContactGroup()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateContactGroup(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateContactGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithNumber_ReturnsViewResultWhereModelIsContactGroupViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            IContactGroup contactGroup = _fixture.BuildContactGroupMock(number, name).Object;
            Controller sut = CreateSut(contactGroup);

            ViewResult result = (ViewResult) await sut.UpdateContactGroup(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<ContactGroupViewModel>());

            ContactGroupViewModel contactGroupViewModel = (ContactGroupViewModel) result.Model;

            Assert.That(contactGroupViewModel, Is.Not.Null);
            Assert.That(contactGroupViewModel.Number, Is.EqualTo(number));
            Assert.That(contactGroupViewModel.Name, Is.EqualTo(name));
            Assert.That(contactGroupViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactGroup_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactGroup(null));

            Assert.That(result.ParamName, Is.EqualTo("contactGroupViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ContactGroupViewModel model = CreateModel();
            await sut.UpdateContactGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateContactGroupCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ContactGroupViewModel model = CreateModel();
            IActionResult result = await sut.UpdateContactGroup(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateContactGroup()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ContactGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateContactGroup(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateContactGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            ContactGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateContactGroup(model);

            Assert.That(result.Model, Is.TypeOf<ContactGroupViewModel>());

            ContactGroupViewModel contactGroupViewModel = (ContactGroupViewModel) result.Model;

            Assert.That(contactGroupViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            await sut.UpdateContactGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactGroupCommand>(command =>
                    command.Number == model.Number &&
                    string.CompareOrdinal(command.Name, model.Name) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            IActionResult result = await sut.UpdateContactGroup(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactGroup(model);

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToContactGroups()
        {
            Controller sut = CreateSut();

            ContactGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactGroup(model);

            Assert.That(result.ActionName, Is.EqualTo("ContactGroups"));
        }

        private Controller CreateSut(IContactGroup contactGroup = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m =>  m.QueryAsync<IGetContactGroupQuery, IContactGroup>(It.IsAny<IGetContactGroupQuery>()))
                .Returns(Task.Run(() => contactGroup ?? _fixture.BuildContactGroupMock().Object));
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateContactGroupCommand>()))
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