using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.MediaLibraryController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.MediaLibraryController
{
	[TestFixture]
    public class CreateBookGenreWithModelTests
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
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithCreateBookGenreCommand()
		{
			Controller sut = CreateSut();

			await sut.CreateBookGenre(null);

			_commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateBookGenreCommand>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsInvalid_AssertPublishAsyncWasNotCalledOnCommandBusWithCreateBookGenreCommand()
		{
			Controller sut = CreateSut(false);

			await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			_commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateBookGenreCommand>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBookGenreCommandWhereNumberIsEqualToNumberOnGenericCategoryViewModel()
		{
			Controller sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.CreateBookGenre(CreateGenericCategoryViewModel(number));

			_commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBookGenreCommand>(value => value != null && value.Number == number)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBookGenreCommandWhereNameIsEqualToNameOnGenericCategoryViewModel()
		{
			Controller sut = CreateSut();

			string name = _fixture.Create<string>();
			await sut.CreateBookGenre(CreateGenericCategoryViewModel(name: name));

			_commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBookGenreCommand>(value => value != null && string.CompareOrdinal(name, value.Name) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsNull_ReturnsNotNull()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateBookGenre(null);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsNull_ReturnsBadRequestResult()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateBookGenre(null);

			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsInvalid_ReturnsNotNull()
		{
			Controller sut = CreateSut(false);

			IActionResult result = await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsInvalid_ReturnsBadRequestObjectResult()
		{
			Controller sut = CreateSut(false);

			IActionResult result = await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsInvalid_ReturnsBadRequestObjectResultWhereIsNotNull()
		{
			Controller sut = CreateSut(false);

			BadRequestObjectResult result = (BadRequestObjectResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.Value, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsInvalid_ReturnsBadRequestObjectResultWhereIsSerializableError()
		{
			Controller sut = CreateSut(false);

			BadRequestObjectResult result = (BadRequestObjectResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.Value, Is.TypeOf<SerializableError>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsNotNull()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResult()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result, Is.TypeOf<RedirectToActionResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.ControllerName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.ControllerName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToMediaLibrary()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.ControllerName, Is.EqualTo("MediaLibrary"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.ActionName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.ActionName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateBookGenre_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToBookGenres()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateBookGenre(CreateGenericCategoryViewModel());

			Assert.That(result.ActionName, Is.EqualTo("BookGenres"));
		}

		private Controller CreateSut(bool modelIsValid = true)
        {
	        _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateBookGenreCommand>()))
		        .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
	            controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private GenericCategoryViewModel CreateGenericCategoryViewModel(int? number = null, string name = null)
        {
	        return _fixture.Build<GenericCategoryViewModel>()
		        .With(m => m.Number, number ?? _fixture.Create<int>())
		        .With(m => m.Name, name ?? _fixture.Create<string>())
		        .Create();
        }
	}
}