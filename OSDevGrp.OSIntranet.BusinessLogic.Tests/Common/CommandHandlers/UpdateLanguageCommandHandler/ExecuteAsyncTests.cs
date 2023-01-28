using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.UpdateLanguageCommandHandler
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
			ICommandHandler<IUpdateLanguageCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateLanguageCommand()
		{
			ICommandHandler<IUpdateLanguageCommand> sut = CreateSut();

			Mock<IUpdateLanguageCommand> updateLanguageCommandMock = BuildUpdateLanguageCommandMock();
			await sut.ExecuteAsync(updateLanguageCommandMock.Object);

			updateLanguageCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<ILanguage>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateLanguageCommand()
		{
			ICommandHandler<IUpdateLanguageCommand> sut = CreateSut();

			Mock<IUpdateLanguageCommand> updateLanguageCommandMock = BuildUpdateLanguageCommandMock();
			await sut.ExecuteAsync(updateLanguageCommandMock.Object);

			updateLanguageCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToUpdateLanguageAsyncOnCommonRepositoryWithLanguageFromUpdateLanguageCommand()
		{
			ICommandHandler<IUpdateLanguageCommand> sut = CreateSut();

			ILanguage language = _fixture.BuildLanguageMock().Object;
			await sut.ExecuteAsync(BuildUpdateLanguageCommand(language));

			_commonRepositoryMock.Verify(m => m.UpdateLanguageAsync(It.Is<ILanguage>(value => value != null && value == language)), Times.Once());
		}

		private ICommandHandler<IUpdateLanguageCommand> CreateSut()
		{
			_commonRepositoryMock.Setup(m => m.UpdateLanguageAsync(It.IsAny<ILanguage>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.Common.CommandHandlers.UpdateLanguageCommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
		}

		private IUpdateLanguageCommand BuildUpdateLanguageCommand(ILanguage language = null)
		{
			return BuildUpdateLanguageCommandMock(language).Object;
		}

		private Mock<IUpdateLanguageCommand> BuildUpdateLanguageCommandMock(ILanguage language = null)
		{
			Mock<IUpdateLanguageCommand> updateLanguageCommandMock = new Mock<IUpdateLanguageCommand>();
			updateLanguageCommandMock.Setup(m => m.ToDomain())
				.Returns(language ?? _fixture.BuildLanguageMock().Object);
			return updateLanguageCommandMock;
		}
	}
}