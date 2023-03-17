using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.MediaLibraryController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.MediaLibraryController
{
	[TestFixture]
    public class DeleteBookGenreTests
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
		public async Task DeleteBookGenre_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithDeleteBookGenreCommandWhereNumberIsEqualToNumberFromArgument()
		{
			Controller sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.DeleteBookGenre(number);

			_commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteBookGenreCommand>(value => value != null && value.Number == number)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsNotNull()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResult()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result, Is.TypeOf<RedirectToActionResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result.ControllerName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result.ControllerName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsEqualToMediaLibrary()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result.ControllerName, Is.EqualTo("MediaLibrary"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result.ActionName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result.ActionName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteBookGenre_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsEqualToBookGenres()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteBookGenre(_fixture.Create<int>());

			Assert.That(result.ActionName, Is.EqualTo("BookGenres"));
		}

		private Controller CreateSut()
        {
	        _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteBookGenreCommand>()))
		        .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
	}
}