using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
    [TestFixture]
    public class UpdateLetterHeadTest
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
        public async Task UpdateLetterHead_WhenCalledWithNumber_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateLetterHead(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetLetterHeadQuery, ILetterHead>(It.Is<IGetLetterHeadQuery>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithNumber_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateLetterHead(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithNumber_ReturnsViewResultWhereViewNameIsEqualToUpdateLetterHead()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateLetterHead(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateLetterHead"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithNumber_ReturnsViewResultWhereModelIsAccountGroupViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            string line1 = _fixture.Create<string>();
            string line2 = _fixture.Create<string>();
            string line3 = _fixture.Create<string>();
            string line4 = _fixture.Create<string>();
            string line5 = _fixture.Create<string>();
            string line6 = _fixture.Create<string>();
            string line7 = _fixture.Create<string>();
            string companyIdentificationNumber = _fixture.Create<string>();
            ILetterHead letterHead = _fixture.BuildLetterHeadMock(number, name, line1, line2, line3, line4, line5, line6, line7, companyIdentificationNumber).Object;
            Controller sut = CreateSut(letterHead);

            ViewResult result = (ViewResult) await sut.UpdateLetterHead(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<LetterHeadViewModel>());

            LetterHeadViewModel letterHeadViewModel = (LetterHeadViewModel) result.Model;

            Assert.That(letterHeadViewModel, Is.Not.Null);
            Assert.That(letterHeadViewModel.Number, Is.EqualTo(number));
            Assert.That(letterHeadViewModel.Name, Is.EqualTo(name));
            Assert.That(letterHeadViewModel.Line1, Is.EqualTo(line1));
            Assert.That(letterHeadViewModel.Line2, Is.EqualTo(line2));
            Assert.That(letterHeadViewModel.Line3, Is.EqualTo(line3));
            Assert.That(letterHeadViewModel.Line4, Is.EqualTo(line4));
            Assert.That(letterHeadViewModel.Line5, Is.EqualTo(line5));
            Assert.That(letterHeadViewModel.Line6, Is.EqualTo(line6));
            Assert.That(letterHeadViewModel.Line7, Is.EqualTo(line7));
            Assert.That(letterHeadViewModel.CompanyIdentificationNumber, Is.EqualTo(companyIdentificationNumber));
            Assert.That(letterHeadViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateLetterHead_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateLetterHead(null));

            Assert.That(result.ParamName, Is.EqualTo("letterHeadViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            LetterHeadViewModel model = CreateModel();
            await sut.UpdateLetterHead(model);

            _commandBusMock.Verify(m => m.PublishAsync<IUpdateLetterHeadCommand>(It.IsAny<IUpdateLetterHeadCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            LetterHeadViewModel model = CreateModel();
            IActionResult result = await sut.UpdateLetterHead(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateLetterHead()
        {
            Controller sut = CreateSut(modelIsValid: false);

            LetterHeadViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateLetterHead(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateLetterHead"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            LetterHeadViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateLetterHead(model);

            Assert.That(result.Model, Is.TypeOf<LetterHeadViewModel>());

            LetterHeadViewModel letterHeadViewModel = (LetterHeadViewModel) result.Model;

            Assert.That(letterHeadViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            await sut.UpdateLetterHead(model);

            _commandBusMock.Verify(m => m.PublishAsync<IUpdateLetterHeadCommand>(It.Is<IUpdateLetterHeadCommand>(command => 
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
        public async Task UpdateLetterHead_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            IActionResult result = await sut.UpdateLetterHead(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToCommon()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateLetterHead(model);

            Assert.That(result.ControllerName, Is.EqualTo("Common"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateLetterHead_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToLetterHeads()
        {
            Controller sut = CreateSut();

            LetterHeadViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateLetterHead(model);

            Assert.That(result.ActionName, Is.EqualTo("LetterHeads"));
        }

        private Controller CreateSut(ILetterHead letterHead = null, bool modelIsValid = true)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetLetterHeadQuery, ILetterHead>(It.IsAny<IGetLetterHeadQuery>()))
                .Returns(Task.Run(() => letterHead ?? _fixture.BuildLetterHeadMock().Object));

            _commandBusMock.Setup(m => m.PublishAsync<IUpdateLetterHeadCommand>(It.IsAny<IUpdateLetterHeadCommand>()))
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