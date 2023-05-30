using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.CreateLanguageCommandHandler
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
			ICommandHandler<ICreateLanguageCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateLanguageCommand()
		{
			ICommandHandler<ICreateLanguageCommand> sut = CreateSut();

			Mock<ICreateLanguageCommand> createLanguageCommandMock = BuildCreateLanguageCommandMock();
			await sut.ExecuteAsync(createLanguageCommandMock.Object);

			createLanguageCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<ILanguage>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateLanguageCommand()
		{
			ICommandHandler<ICreateLanguageCommand> sut = CreateSut();

			Mock<ICreateLanguageCommand> createLanguageCommandMock = BuildCreateLanguageCommandMock();
			await sut.ExecuteAsync(createLanguageCommandMock.Object);

			createLanguageCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertCreateLanguageAsyncWasCalledOnCommonRepositoryWithLanguageFromCreateLanguageCommand()
		{
			ICommandHandler<ICreateLanguageCommand> sut = CreateSut();

			ILanguage language = _fixture.BuildLanguageMock().Object;
			await sut.ExecuteAsync(BuildCreateLanguageCommand(language));

			_commonRepositoryMock.Verify(m => m.CreateLanguageAsync(It.Is<ILanguage>(value => value != null && value == language)), Times.Once());
		}

		private ICommandHandler<ICreateLanguageCommand> CreateSut()
		{
			_commonRepositoryMock.Setup(m => m.CreateLanguageAsync(It.IsAny<ILanguage>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.Common.CommandHandlers.CreateLanguageCommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateLanguageCommand BuildCreateLanguageCommand(ILanguage language = null)
		{
			return BuildCreateLanguageCommandMock(language).Object;
		}

		private Mock<ICreateLanguageCommand> BuildCreateLanguageCommandMock(ILanguage language = null)
		{
			Mock<ICreateLanguageCommand> createLanguageCommandMock = new Mock<ICreateLanguageCommand>();
			createLanguageCommandMock.Setup(m => m.ToDomain())
				.Returns(language ?? _fixture.BuildLanguageMock().Object);
			return createLanguageCommandMock;
		}
	}
}