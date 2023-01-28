﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.CommandHandlers.UpdateMusicGenreCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IUpdateMusicGenreCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnUpdateMusicGenreCommand()
		{
			ICommandHandler<IUpdateMusicGenreCommand> sut = CreateSut();

			Mock<IUpdateMusicGenreCommand> updateMusicGenreCommandMock = BuildUpdateMusicGenreCommandMock();
			await sut.ExecuteAsync(updateMusicGenreCommandMock.Object);

			updateMusicGenreCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<IMusicGenre>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnUpdateMusicGenreCommand()
		{
			ICommandHandler<IUpdateMusicGenreCommand> sut = CreateSut();

			Mock<IUpdateMusicGenreCommand> updateMusicGenreCommandMock = BuildUpdateMusicGenreCommandMock();
			await sut.ExecuteAsync(updateMusicGenreCommandMock.Object);

			updateMusicGenreCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertUpdateMusicGenreAsyncWasCalledOnMediaLibraryRepositoryWithMusicGenreFromUpdateMusicGenreCommand()
		{
			ICommandHandler<IUpdateMusicGenreCommand> sut = CreateSut();

			IMusicGenre musicGenre = _fixture.BuildMusicGenreMock().Object;
			await sut.ExecuteAsync(BuildUpdateMusicGenreCommand(musicGenre));

			_mediaLibraryRepositoryMock.Verify(m => m.UpdateMusicGenreAsync(It.Is<IMusicGenre>(value => value != null && value == musicGenre)), Times.Once());
		}

		private ICommandHandler<IUpdateMusicGenreCommand> CreateSut()
		{
			_mediaLibraryRepositoryMock.Setup(m => m.UpdateMusicGenreAsync(It.IsAny<IMusicGenre>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.MediaLibrary.CommandHandlers.UpdateMusicGenreCommandHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private IUpdateMusicGenreCommand BuildUpdateMusicGenreCommand(IMusicGenre musicGenre = null)
		{
			return BuildUpdateMusicGenreCommandMock(musicGenre).Object;
		}

		private Mock<IUpdateMusicGenreCommand> BuildUpdateMusicGenreCommandMock(IMusicGenre musicGenre = null)
		{
			Mock<IUpdateMusicGenreCommand> updateMusicGenreCommandMock = new Mock<IUpdateMusicGenreCommand>();
			updateMusicGenreCommandMock.Setup(m => m.ToDomain())
				.Returns(musicGenre ?? _fixture.BuildMusicGenreMock().Object);
			return updateMusicGenreCommandMock;
		}
	}
}