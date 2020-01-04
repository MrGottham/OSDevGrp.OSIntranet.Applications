using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class DeleteBudgetAccountGroupTests
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
        public async Task DeleteBudgetAccountGroup_WhenCalledWithNumber_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.DeleteBudgetAccountGroup(number);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteBudgetAccountGroupCommand>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteBudgetAccountGroup_WhenCalledWithNumber_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteBudgetAccountGroup(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteBudgetAccountGroup_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteBudgetAccountGroup(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteBudgetAccountGroup_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereActionNameIsEqualToBudgetAccountGroups()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteBudgetAccountGroup(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("BudgetAccountGroups"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteAccountGroupCommand>()))
                .Returns(Task.Run(() => { }));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}