using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class DeleteContactGroupTests
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
        public async Task DeleteContactGroup_WhenCalledWithNumber_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.DeleteContactGroup(number);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteContactGroupCommand>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContactGroup_WhenCalledWithNumber_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteContactGroup(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContactGroup_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteContactGroup(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContactGroup_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereActionNameIsEqualToContactGroups()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result =  (RedirectToActionResult) await sut.DeleteContactGroup(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("ContactGroups"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteContactGroupCommand>()))
                .Returns(Task.Run(() => { }));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}
