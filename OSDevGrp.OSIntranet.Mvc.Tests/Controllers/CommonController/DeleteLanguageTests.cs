using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.CommonController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.CommonController
{
	[TestFixture]
	public class DeleteLanguageTests
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
		public async Task DeleteLanguage_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithDeleteLanguageCommandWhereNumberIsEqualToNumberFromArgument()
		{
			Controller sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.DeleteLanguage(number);

			_commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteLanguageCommand>(value => value != null && value.Number == number)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsNotNull()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResult()
		{
			Controller sut = CreateSut();

			IActionResult result = await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result, Is.TypeOf<RedirectToActionResult>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result.ControllerName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result.ControllerName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResultWhereControllerNameIsEqualToCommon()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result.ControllerName, Is.EqualTo("Common"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result.ActionName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsNotEmpty()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result.ActionName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task DeleteLanguage_WhenCalled_ReturnsRedirectToActionResultWhereActionNameIsEqualToLanguages()
		{
			Controller sut = CreateSut();

			RedirectToActionResult result = (RedirectToActionResult)await sut.DeleteLanguage(_fixture.Create<int>());

			Assert.That(result.ActionName, Is.EqualTo("Languages"));
		}

		private Controller CreateSut()
		{
			_commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteLanguageCommand>()))
				.Returns(Task.CompletedTask);

			return new Controller(_commandBusMock.Object, _queryBusMock.Object);
		}
	}
}