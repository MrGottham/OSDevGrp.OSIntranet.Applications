using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.MediaIdentifierHelper
{
	[TestFixture]
	public class IsExistingMediaIdentifierAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void IsExistingMediaIdentifierAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenCalled_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();

			await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(mediaIdentifier, CreateMediaLibraryRepository());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierExistsForMovie_AssertMediaExistsAsyncWasNotCalledOnMediaLibraryRepositoryForMusic()
		{
			await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(true));

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMusic>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierExistsForMovie_AssertMediaExistsAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(true));

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IBook>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierDoesNotExistForMovie_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForMusic()
		{
			Guid mediaIdentifier = Guid.NewGuid();

			await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(mediaIdentifier, CreateMediaLibraryRepository(false));

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierDoesNotExistForMovieButForMusic_AssertMediaExistsAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(false, true));

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IBook>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierDoesNotExistForMovieAndMusicButForBook_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepositoryForBook()
		{
			Guid mediaIdentifier = Guid.NewGuid();

			await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(mediaIdentifier, CreateMediaLibraryRepository(false, false));

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IBook>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierExistsForMovie_ReturnsTrue()
		{
			bool result = await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(true, false, false));

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierExistsForMusic_ReturnsTrue()
		{
			bool result = await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(false, true, false));

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierExistsForBook_ReturnsTrue()
		{
			bool result = await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(false, false, true));

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingMediaIdentifierAsync_WhenMediaIdentifierExistsForMovieMusicOrBook_ReturnsFalse()
		{
			bool result = await BusinessLogic.MediaLibrary.Logic.MediaIdentifierHelper.IsExistingMediaIdentifierAsync(Guid.NewGuid(), CreateMediaLibraryRepository(false, false, false));

			Assert.That(result, Is.False);
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository(bool? mediaIdentifierExistsAsMovie = null, bool? mediaIdentifierExistsAsMusic = null, bool? mediaIdentifierExistsAsBook = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<IMovie>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(mediaIdentifierExistsAsMovie ?? _fixture.Create<bool>()));
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<IMusic>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(mediaIdentifierExistsAsMusic ?? _fixture.Create<bool>()));
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<IBook>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(mediaIdentifierExistsAsBook ?? _fixture.Create<bool>()));

			return _mediaLibraryRepositoryMock.Object;
		}
	}
}