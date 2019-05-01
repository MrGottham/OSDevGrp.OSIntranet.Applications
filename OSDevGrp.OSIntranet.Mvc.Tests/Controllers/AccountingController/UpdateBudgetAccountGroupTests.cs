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
    public class UpdateBudgetAccountGroupTests
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
        public async Task UpdateBudgetAccountGroup_WhenCalledWithNumber_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.UpdateBudgetAccountGroup(number);

            _queryBusMock.Verify(m => m.QueryAsync<IGetBudgetAccountGroupQuery, IBudgetAccountGroup>(It.Is<IGetBudgetAccountGroupQuery>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithNumber_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateBudgetAccountGroup(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithNumber_ReturnsViewResultWhereViewNameIsEqualToUpdateBudgetAccountGroup()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.UpdateBudgetAccountGroup(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("UpdateBudgetAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithNumber_ReturnsViewResultWhereModelIsBudgetAccountGroupViewModel()
        {
            int number = _fixture.Create<int>();
            string name = _fixture.Create<string>();
            IBudgetAccountGroup budgetAccountGroup = _fixture.BuildBudgetAccountGroupMock(number, name).Object;
            Controller sut = CreateSut(budgetAccountGroup);

            ViewResult result = (ViewResult) await sut.UpdateBudgetAccountGroup(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<BudgetAccountGroupViewModel>());

            BudgetAccountGroupViewModel budgetAccountGroupViewModel = (BudgetAccountGroupViewModel) result.Model;

            Assert.That(budgetAccountGroupViewModel, Is.Not.Null);
            Assert.That(budgetAccountGroupViewModel.Number, Is.EqualTo(number));
            Assert.That(budgetAccountGroupViewModel.Name, Is.EqualTo(name));
            Assert.That(budgetAccountGroupViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateBudgetAccountGroup_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateBudgetAccountGroup(null));

            Assert.That(result.ParamName, Is.EqualTo("budgetAccountGroupViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            BudgetAccountGroupViewModel model = CreateModel();
            await sut.UpdateBudgetAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync<IUpdateBudgetAccountGroupCommand>(It.IsAny<IUpdateBudgetAccountGroupCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            BudgetAccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.UpdateBudgetAccountGroup(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToUpdateBudgetAccountGroup()
        {
            Controller sut = CreateSut(modelIsValid: false);

            BudgetAccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateBudgetAccountGroup(model);

            Assert.That(result.ViewName, Is.EqualTo("UpdateBudgetAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            BudgetAccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.UpdateBudgetAccountGroup(model);

            Assert.That(result.Model, Is.TypeOf<BudgetAccountGroupViewModel>());

            BudgetAccountGroupViewModel budgetAccountGroupViewModel = (BudgetAccountGroupViewModel) result.Model;

            Assert.That(budgetAccountGroupViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            await sut.UpdateBudgetAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync<IUpdateBudgetAccountGroupCommand>(It.Is<IUpdateBudgetAccountGroupCommand>(command => command.Number == model.Number && string.Compare(command.Name, model.Name, false) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.UpdateBudgetAccountGroup(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereContollerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateBudgetAccountGroup(model);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateBudgetAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToBudgetAccountGroups()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateBudgetAccountGroup(model);

            Assert.That(result.ActionName, Is.EqualTo("BudgetAccountGroups"));
        }

        private Controller CreateSut(IBudgetAccountGroup budgetAccountGroup = null, bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync<IUpdateBudgetAccountGroupCommand>(It.IsAny<IUpdateBudgetAccountGroupCommand>()))
                .Returns(Task.Run(() => { }));

            _queryBusMock.Setup(m => m.QueryAsync<IGetBudgetAccountGroupQuery, IBudgetAccountGroup>(It.IsAny<IGetBudgetAccountGroupQuery>()))
                .Returns(Task.Run(() => budgetAccountGroup ?? _fixture.BuildBudgetAccountGroupMock().Object));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private BudgetAccountGroupViewModel CreateModel()
        {
            return _fixture.Create<BudgetAccountGroupViewModel>();
        }
    }
}