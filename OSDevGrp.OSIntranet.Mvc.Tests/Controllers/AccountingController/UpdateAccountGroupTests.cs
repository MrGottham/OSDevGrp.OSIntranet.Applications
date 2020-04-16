using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
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
    public class UpdateAccountGroupTests
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
        public async Task UpdateAccountGroup_WhenCalledWithNumber_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateAccountGroup(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountGroupQuery, IAccountGroup>(It.Is<IGetAccountGroupQuery>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithNumber_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateAccountGroup(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithNumber_ReturnsViewResultWhereViewNameIsEqualToUpdateAccountGroup()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateAccountGroup(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithNumber_ReturnsViewResultWhereModelIsAccountGroupViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            Domain.Interfaces.Accounting.Enums.AccountGroupType accountGroupType = _fixture.Create<Domain.Interfaces.Accounting.Enums.AccountGroupType>();
            IAccountGroup accountGroup = _fixture.BuildAccountGroupMock(number, name, accountGroupType).Object;
            Controller sut = CreateSut(accountGroup);

            ViewResult result = (ViewResult) await sut.UpdateAccountGroup(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<AccountGroupViewModel>());

            AccountGroupViewModel accountGroupViewModel = (AccountGroupViewModel) result.Model;

            Assert.That(accountGroupViewModel, Is.Not.Null);
            Assert.That(accountGroupViewModel.Number, Is.EqualTo(number));
            Assert.That(accountGroupViewModel.Name, Is.EqualTo(name));
            Assert.That(Convert.ToString(accountGroupViewModel.AccountGroupType), Is.EqualTo(Convert.ToString(accountGroupType)));
            Assert.That(accountGroupViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateAccountGroup_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccountGroup(null));

            Assert.That(result.ParamName, Is.EqualTo("accountGroupViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            AccountGroupViewModel model = CreateModel();
            await sut.UpdateAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateAccountGroupCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            AccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.UpdateAccountGroup(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateAccountGroup()
        {
            Controller sut = CreateSut(modelIsValid: false);

            AccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateAccountGroup(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            AccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateAccountGroup(model);

            Assert.That(result.Model, Is.TypeOf<AccountGroupViewModel>());

            AccountGroupViewModel accountGroupViewModel = (AccountGroupViewModel) result.Model;

            Assert.That(accountGroupViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            await sut.UpdateAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountGroupCommand>(command => command.Number == model.Number && string.CompareOrdinal(command.Name, model.Name) == 0 && string.CompareOrdinal(Convert.ToString(command.AccountGroupType), Convert.ToString(model.AccountGroupType)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.UpdateAccountGroup(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccountGroup(model);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountGroups()
        {
            Controller sut = CreateSut();

            AccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccountGroup(model);

            Assert.That(result.ActionName, Is.EqualTo("AccountGroups"));
        }

        private Controller CreateSut(IAccountGroup accountGroup = null, bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateAccountGroupCommand>()))
                .Returns(Task.Run(() => { }));

            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountGroupQuery, IAccountGroup>(It.IsAny<IGetAccountGroupQuery>()))
                .Returns(Task.Run(() => accountGroup ?? _fixture.BuildAccountGroupMock().Object));

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