using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.DeleteLanguageCommand
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IDeleteLanguageCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteLanguageCommand()
		{
			ICommandHandler<IDeleteLanguageCommand> sut = CreateSut();

			Mock<IDeleteLanguageCommand> deleteLanguageCommandMock = BuildDeleteLanguageCommandMock();
			await sut.ExecuteAsync(deleteLanguageCommandMock.Object);

			deleteLanguageCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<ILanguage>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteLanguageCommand()
		{
			ICommandHandler<IDeleteLanguageCommand> sut = CreateSut();

			Mock<IDeleteLanguageCommand> deleteLanguageCommandMock = BuildDeleteLanguageCommandMock();
			await sut.ExecuteAsync(deleteLanguageCommandMock.Object);

			deleteLanguageCommandMock.Verify(m => m.Number, Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToUpdateLanguageAsyncOnCommonRepositoryWithNumberFromDeleteLanguageCommand()
		{
			ICommandHandler<IDeleteLanguageCommand> sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.ExecuteAsync(BuildDeleteLanguageCommand(number));

			_commonRepositoryMock.Verify(m => m.DeleteLanguageAsync(It.Is<int>(value => value == number)), Times.Once());
		}

		private ICommandHandler<IDeleteLanguageCommand> CreateSut()
		{
			_commonRepositoryMock.Setup(m => m.DeleteLanguageAsync(It.IsAny<int>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.Common.CommandHandlers.DeleteLanguageCommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteLanguageCommand BuildDeleteLanguageCommand(int? number = null)
		{
			return BuildDeleteLanguageCommandMock(number).Object;
		}

		private Mock<IDeleteLanguageCommand> BuildDeleteLanguageCommandMock(int? number = null)
		{
			Mock<IDeleteLanguageCommand> deleteLanguageCommandMock = new Mock<IDeleteLanguageCommand>();
			deleteLanguageCommandMock.Setup(m => m.Number)
				.Returns(number ?? _fixture.Create<int>());
			return deleteLanguageCommandMock;
		}
	}
}