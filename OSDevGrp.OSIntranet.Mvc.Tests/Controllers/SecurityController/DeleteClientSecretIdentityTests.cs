using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class DeleteClientSecretIdentityTests
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
        public async Task DeleteClientSecretIdentity_WhenCalledWithNumber_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            int identifier = _fixture.Create<int>();
            await sut.DeleteClientSecretIdentity(identifier);

            _commandBusMock.Verify(m => m.PublishAsync<IDeleteClientSecretIdentityCommand>(It.Is<IDeleteClientSecretIdentityCommand>(value => value.Identifier == identifier)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteClientSecretIdentity_WhenCalledWithNumber_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteClientSecretIdentity_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereContollerNameIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Security"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteClientSecretIdentity_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereActionNameIsEqualToClientSecretIdentities()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteClientSecretIdentity(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("ClientSecretIdentities"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteClientSecretIdentityCommand>()))
                .Returns(Task.Run(() => { }));

            return new Mvc.Controllers.SecurityController(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
