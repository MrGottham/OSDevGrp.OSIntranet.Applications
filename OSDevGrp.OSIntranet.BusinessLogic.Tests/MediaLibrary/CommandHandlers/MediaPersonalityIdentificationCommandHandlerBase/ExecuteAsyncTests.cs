﻿using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.CommandHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.MediaPersonalityIdentificationCommandHandlerBase
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IMediaPersonalityIdentificationCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("command"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnMediaPersonalityIdentificationCommand()
		{
			ICommandHandler<IMediaPersonalityIdentificationCommand> sut = CreateSut();

			Mock<IMediaPersonalityIdentificationCommand> mediaPersonalityIdentificationCommandMock = CreateMediaPersonalityIdentificationCommandMock();
			await sut.ExecuteAsync(mediaPersonalityIdentificationCommandMock.Object);

			mediaPersonalityIdentificationCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IClaimResolver>(value => value == _claimResolverMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object),
					It.Is<ICommonRepository>(value => value == _commonRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnMediaPersonalityIdentificationCommandHandlerBase()
		{
			ICommandHandler<IMediaPersonalityIdentificationCommand> sut = CreateSut();

			await sut.ExecuteAsync(CreateMediaPersonalityIdentificationCommand());

			Assert.That(((MyMediaPersonalityIdentificationCommandHandler) sut).ManageAsyncWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertManageAsyncWasCalledWasCalledOnMediaPersonalityIdentificationCommandHandlerBaseWithSameMediaPersonalityIdentificationCommand()
		{
			ICommandHandler<IMediaPersonalityIdentificationCommand> sut = CreateSut();

			IMediaPersonalityIdentificationCommand mediaPersonalityIdentificationCommand = CreateMediaPersonalityIdentificationCommand();
			await sut.ExecuteAsync(mediaPersonalityIdentificationCommand);

			Assert.That(((MyMediaPersonalityIdentificationCommandHandler) sut).ManageAsyncCalledWithMediaPersonalityIdentificationCommand, Is.SameAs(mediaPersonalityIdentificationCommand));
		}

		private ICommandHandler<IMediaPersonalityIdentificationCommand> CreateSut()
		{
			return new MyMediaPersonalityIdentificationCommandHandler(_validatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);
		}

		private static IMediaPersonalityIdentificationCommand CreateMediaPersonalityIdentificationCommand()
		{
			return CreateMediaPersonalityIdentificationCommandMock().Object;
		}

		private static Mock<IMediaPersonalityIdentificationCommand> CreateMediaPersonalityIdentificationCommandMock()
		{
			return new Mock<IMediaPersonalityIdentificationCommand>();
		}

		private class MyMediaPersonalityIdentificationCommandHandler : MediaPersonalityIdentificationCommandHandlerBase<IMediaPersonalityIdentificationCommand>
		{
			#region Constructor

			public MyMediaPersonalityIdentificationCommandHandler(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository) 
				: base(validator, claimResolver, mediaLibraryRepository, commonRepository)
			{
			}

			#endregion

			#region Properties

			public bool ManageAsyncWasCalled { get; private set; }

			public IMediaPersonalityIdentificationCommand ManageAsyncCalledWithMediaPersonalityIdentificationCommand { get; private set; }

			#endregion

			#region Methods

			protected override Task ManageAsync(IMediaPersonalityIdentificationCommand command)
			{
				NullGuard.NotNull(command, nameof(command));

				ManageAsyncWasCalled = true;
				ManageAsyncCalledWithMediaPersonalityIdentificationCommand = command;

				return Task.CompletedTask;
			}

			#endregion
		}
	}
}