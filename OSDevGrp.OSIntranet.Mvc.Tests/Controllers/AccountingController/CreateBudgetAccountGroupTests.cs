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
    public class CreateBudgetAccountGroupTests
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
        public void CreateBudgetAccountGroup_WhenCalledWithoutModel_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.CreateBudgetAccountGroup();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateBudgetAccountGroup_WhenCalledWithoutModel_ReturnsViewResultWhereViewNameIsEqualToCreateBudgetAccountGroup()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateBudgetAccountGroup();

            Assert.That(result.ViewName, Is.EqualTo("CreateBudgetAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateBudgetAccountGroup_WhenCalledWithoutModel_ReturnsViewResultWhereModelIsBudgetAccountGroupViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.CreateBudgetAccountGroup();

            Assert.That(result.Model, Is.TypeOf<BudgetAccountGroupViewModel>());

            BudgetAccountGroupViewModel budgetAccountGroupViewModel = (BudgetAccountGroupViewModel) result.Model;

            Assert.That(budgetAccountGroupViewModel, Is.Not.Null);
            Assert.That(budgetAccountGroupViewModel.Number, Is.EqualTo(default(int)));
            Assert.That(budgetAccountGroupViewModel.Name, Is.Null);
            Assert.That(budgetAccountGroupViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateBudgetAccountGroup_WhenCalledWithModelWhereModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateBudgetAccountGroup(null));

            Assert.That(result.ParamName, Is.EqualTo("budgetAccountGroupViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithInvalidModel_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            BudgetAccountGroupViewModel model = CreateModel();
            await sut.CreateBudgetAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateBudgetAccountGroupCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResult()
        {
            Controller sut = CreateSut(false);

            BudgetAccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.CreateBudgetAccountGroup(model);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereViewNameIsEqualToCreateBudgetAccountGroup()
        {
            Controller sut = CreateSut(false);

            BudgetAccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateBudgetAccountGroup(model);

            Assert.That(result.ViewName, Is.EqualTo("CreateBudgetAccountGroup"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithInvalidModel_ReturnsViewResultWhereModelIsEqualToInvalidModel()
        {
            Controller sut = CreateSut(false);

            BudgetAccountGroupViewModel model = CreateModel();
            ViewResult result = (ViewResult) await sut.CreateBudgetAccountGroup(model);

            Assert.That(result.Model, Is.TypeOf<BudgetAccountGroupViewModel>());

            BudgetAccountGroupViewModel budgetAccountGroupViewModel = (BudgetAccountGroupViewModel) result.Model;

            Assert.That(budgetAccountGroupViewModel, Is.EqualTo(model));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithValidModel_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            await sut.CreateBudgetAccountGroup(model);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountGroupCommand>(command => command.Number == model.Number && string.CompareOrdinal(command.Name, model.Name) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            IActionResult result = await sut.CreateBudgetAccountGroup(model);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccountGroup(model);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccountGroup_WhenCalledWithValidModel_ReturnsRedirectToActionResultWhereActionNameIsEqualToBudgetAccountGroups()
        {
            Controller sut = CreateSut();

            BudgetAccountGroupViewModel model = CreateModel();
            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccountGroup(model);

            Assert.That(result.ActionName, Is.EqualTo("BudgetAccountGroups"));
        }

        private Controller CreateSut(bool modelIsValid = true)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateBudgetAccountGroupCommand>()))
                .Returns(Task.Run(() => { }));

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
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