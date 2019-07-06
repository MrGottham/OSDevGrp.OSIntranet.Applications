using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
    [TestFixture]
    public class CreateLetterHeadTests
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
        public void CreateLetterHead_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateLetterHead();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateLetterHead_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateLetterHead()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateLetterHead();

            Assert.That(result.ViewName, Is.EqualTo("CreateLetterHead"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateLetterHead_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsLetterHeadViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateLetterHead();

            Assert.That(result.Model, Is.TypeOf<LetterHeadViewModel>());

            LetterHeadViewModel letterHeadViewModel = (LetterHeadViewModel) result.Model;

            Assert.That(letterHeadViewModel, Is.Not.Null);
            Assert.That(letterHeadViewModel.Number, Is.EqualTo(default(int)));
            Assert.That(letterHeadViewModel.Name, Is.Null);
            Assert.That(letterHeadViewModel.Line1, Is.Null);
            Assert.That(letterHeadViewModel.Line2, Is.Null);
            Assert.That(letterHeadViewModel.Line3, Is.Null);
            Assert.That(letterHeadViewModel.Line4, Is.Null);
            Assert.That(letterHeadViewModel.Line5, Is.Null);
            Assert.That(letterHeadViewModel.Line6, Is.Null);
            Assert.That(letterHeadViewModel.Line7, Is.Null);
            Assert.That(letterHeadViewModel.CompanyIdentificationNumber, Is.Null);
            Assert.That(letterHeadViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateLetterHead_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateLetterHead(null));

            Assert.That(result.ParamName, Is.EqualTo("letterHeadViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            LetterHeadViewModel model = CreateModel();
            await sut.CreateLetterHead(model);

            _commandBusMock.Verify(m => m.PublishAsync<ICreateLetterHeadCommand>(It.IsAny<ICreateLetterHeadCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(false);

            LetterHeadViewModel model = CreateModel();
            IActionResult result = await sut.CreateLetterHead(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateLetterHead()
        {
            Controller sut = CreateSut(false);

            LetterHeadViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateLetterHead(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateLetterHead"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(false);

            LetterHeadViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateLetterHead(model);

            Assert.That(result.Model, Is.TypeOf<LetterHeadViewModel>());

            LetterHeadViewModel letterHeadViewModel = (LetterHeadViewModel) result.Model;

            Assert.That(letterHeadViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            await sut.CreateLetterHead(model);

            _commandBusMock.Verify(m => m.PublishAsync<ICreateLetterHeadCommand>(It.Is<ICreateLetterHeadCommand>(command => 
                    command.Number == model.Number && 
                    string.CompareOrdinal(command.Name, model.Name) == 0 && 
                    string.CompareOrdinal(command.Line1, model.Line1) == 0 && 
                    string.CompareOrdinal(command.Line2, model.Line2) == 0 && 
                    string.CompareOrdinal(command.Line3, model.Line3) == 0 &&
                    string.CompareOrdinal(command.Line4, model.Line4) == 0 &&
                    string.CompareOrdinal(command.Line5, model.Line5) == 0 &&
                    string.CompareOrdinal(command.Line6, model.Line6) == 0 &&
                    string.CompareOrdinal(command.Line7, model.Line7) == 0 &&
                    string.CompareOrdinal(command.CompanyIdentificationNumber, model.CompanyIdentificationNumber) == 0)), 
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            IActionResult result = await sut.CreateLetterHead(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToCommon()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateLetterHead(model);

            Assert.That(result.ControllerName, Is.EqualTo("Common"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateLetterHead_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToLetterHeads()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateLetterHead(model);

            Assert.That(result.ActionName, Is.EqualTo("LetterHeads"));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync<ICreateLetterHeadCommand>(It.IsAny<ICreateLetterHeadCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private LetterHeadViewModel CreateModel()
        {
            return _fixture.Create<LetterHeadViewModel>();
        }
    }
}