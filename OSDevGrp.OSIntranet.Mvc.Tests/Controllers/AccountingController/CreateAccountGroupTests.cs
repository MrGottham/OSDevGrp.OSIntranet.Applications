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
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class CreateAccountGroupTests
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
        public void CreateAccountGroup_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateAccountGroup();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateAccountGroup_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateAccountGroup()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateAccountGroup();

            Assert.That(result.ViewName, Is.EqualTo("CreateAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateAccountGroup_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsAccountGroupViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateAccountGroup();

            Assert.That(result.Model, Is.TypeOf<AccountGroupViewModel>());

            AccountGroupViewModel accountGroupViewModel = (AccountGroupViewModel) result.Model;

            Assert.That(accountGroupViewModel, Is.Not.Null);
            Assert.That(accountGroupViewModel.Number, Is.EqualTo(default(int)));
            Assert.That(accountGroupViewModel.Name, Is.Null);
            Assert.That(accountGroupViewModel.AccountGroupType, Is.EqualTo(default(AccountGroupType)));
            Assert.That(accountGroupViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateAccountGroup_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateAccountGroup(null));

            Assert.That(result.ParamName, Is.EqualTo("accountGroupViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            AccountGroupViewModel model = CreateModel();
            await sut.CreateAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateAccountGroupCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(false);

            AccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.CreateAccountGroup(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateAccountGroup()
        {
            Controller sut = CreateSut(false);

            AccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateAccountGroup(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(false);

            AccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateAccountGroup(model);

            Assert.That(result.Model, Is.TypeOf<AccountGroupViewModel>());

            AccountGroupViewModel accountGroupViewModel = (AccountGroupViewModel) result.Model;

            Assert.That(accountGroupViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            await sut.CreateAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateAccountGroupCommand>(command => command.Number == model.Number && string.CompareOrdinal(command.Name, model.Name) == 0 && string.CompareOrdinal(Convert.ToString(command.AccountGroupType), Convert.ToString(model.AccountGroupType)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.CreateAccountGroup(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateAccountGroup(model);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountGroups()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateAccountGroup(model);

            Assert.That(result.ActionName, Is.EqualTo("AccountGroups"));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateAccountGroupCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private AccountGroupViewModel CreateModel()
        {
            return _fixture.Create<AccountGroupViewModel>();
        }
    }
}