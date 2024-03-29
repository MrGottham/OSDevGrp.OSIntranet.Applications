using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class DeleteAccountGroupTests
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
        public async Task DeleteAccountGroup_WhenCalledWithNumber_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.DeleteAccountGroup(number);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteAccountGroupCommand>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccountGroup_WhenCalledWithNumber_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteAccountGroup(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccountGroup_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccountGroup(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteAccountGroup_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountGroups()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteAccountGroup(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("AccountGroups"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteAccountGroupCommand>()))
                .Returns(Task.Run(() => { }));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}