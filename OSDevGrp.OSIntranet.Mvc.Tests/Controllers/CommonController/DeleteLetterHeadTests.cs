using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
    [TestFixture]
    public class DeleteLetterHeadTests
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
        public async Task DeleteLetterHead_WhenCalledWithNumber_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            int number = _fixture.Create<int>();
            await sut.DeleteLetterHead(number);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteLetterHeadCommand>(value => value.Number == number)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteLetterHead_WhenCalledWithNumber_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteLetterHead(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteLetterHead_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereControllerNameIsEqualToCommon()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteLetterHead(_fixture.Create<int>());

            Assert.That(result.ControllerName, Is.EqualTo("Common"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteLetterHead_WhenCalledWithNumber_ReturnsRedirectToActionResultWhereActionNameIsEqualToLetterHeads()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteLetterHead(_fixture.Create<int>());

            Assert.That(result.ActionName, Is.EqualTo("LetterHeads"));
        }

        private Controller CreateSut()
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteLetterHeadCommand>()))
                .Returns(Task.Run(() => { }));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}