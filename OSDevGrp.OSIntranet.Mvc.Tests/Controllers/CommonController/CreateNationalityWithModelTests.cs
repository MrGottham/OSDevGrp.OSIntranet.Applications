using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
	[TestFixture]
	public class CreateNationalityWithModelTests
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
		public async Task CreateNationality_WhenGenericCategoryViewModelIsNull_AssertPublishAsyncWasNotCalledOnCommandBusWithCreateNationalityCommand()
		{
			Controller sut = CreateSut();

			await sut.CreateNationality(null);

			_commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateNationalityCommand>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsInvalid_AssertPublishAsyncWasNotCalledOnCommandBusWithCreateNationalityCommand()
		{
			Controller sut = CreateSut(false);

			await sut.CreateNationality(CreateGenericCategoryViewModel());

			_commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateNationalityCommand>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateNationalityCommandWhereNumberIsEqualToNumberOnGenericCategoryViewModel()
		{
			Controller sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.CreateNationality(CreateGenericCategoryViewModel(number));

			_commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateNationalityCommand>(value => value != null && value.Number == number)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateNationalityCommandWhereNameIsEqualToNameOnGenericCategoryViewModel()
		{
			Controller sut = CreateSut();

			string name = _fixture.Create<string>();
			await sut.CreateNationality(CreateGenericCategoryViewModel(name: name));

			_commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateNationalityCommand>(value => value != null && string.CompareOrdinal(name, value.Name) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsNull_ReturnsNotNull()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateNationality(null);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsNull_ReturnsBadRequestResult()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateNationality(null);

			Assert.That(result, Is.TypeOf<BadRequestResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsInvalid_ReturnsNotNull()
		{
			Controller sut = CreateSut(false);

			IActionResult result = await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsInvalid_ReturnsBadRequestObjectResult()
		{
			Controller sut = CreateSut(false);

			IActionResult result = await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsInvalid_ReturnsBadRequestObjectResultWhereIsNotNull()
		{
			Controller sut = CreateSut(false);

			BadRequestObjectResult result = (BadRequestObjectResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.Value, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsInvalid_ReturnsBadRequestObjectResultWhereIsSerializableError()
		{
			Controller sut = CreateSut(false);

			BadRequestObjectResult result = (BadRequestObjectResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.Value, Is.TypeOf<SerializableError>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsNotNull()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResult()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result, Is.TypeOf<RedirectToActionResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.ControllerName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.ControllerName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToCommon()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.ControllerName, Is.EqualTo("Common"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.ActionName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.ActionName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task CreateNationality_WhenGenericCategoryViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToNationalities()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.CreateNationality(CreateGenericCategoryViewModel());

			Assert.That(result.ActionName, Is.EqualTo("Nationalities"));
		}

		private Controller CreateSut(bool modelIsValid = true)
		{
			_commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateNationalityCommand>()))
				.Returns(Task.CompletedTask);

			Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object);
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