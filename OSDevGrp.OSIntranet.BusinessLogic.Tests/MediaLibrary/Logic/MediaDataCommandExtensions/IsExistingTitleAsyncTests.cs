using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.MediaDataCommandExtensions
{
	[TestFixture]
	public class IsExistingTitleAsyncTests
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
		public void IsExistingTitleAsync_WhenMediaDataCommandIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(null, CreateMediaLibraryRepository<IMedia>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaDataCommand"));
		}

		[Test]
		[Category("UnitTest")]
		public void IsExistingTitleAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(CreateMediaDataCommand<IMedia>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingTitleAsync_WhenCalled_AssertTitleWasCalledOnMediaDataCommand()
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock<IMedia>();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommandMock.Object, CreateMediaLibraryRepository<IMedia>());

			mediaDataCommandMock.Verify(m => m.Title, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingTitleAsync_WhenCalled_AssertSubtitleWasCalledOnMediaDataCommand()
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock<IMedia>();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommandMock.Object, CreateMediaLibraryRepository<IMedia>());

			mediaDataCommandMock.Verify(m => m.Subtitle, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingTitleAsync_WhenCalled_AssertMediaTypeIdentifierWasCalledOnMediaDataCommand()
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock<IMedia>();

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommandMock.Object, CreateMediaLibraryRepository<IMedia>());

			mediaDataCommandMock.Verify(m => m.MediaTypeIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingTitleAsync_WhenCalled_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepository(bool hasSubtitle)
		{
			string title = _fixture.Create<string>().ToUpper();
			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(title, hasSubtitle, mediaTypeIdentifier: mediaTypeIdentifier);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, CreateMediaLibraryRepository<IMedia>());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMedia>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, title) == 0),
					It.IsAny<string>(),
					It.Is<int>(value => value == mediaTypeIdentifier)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingTitleAsync_WhenSubtitleIsSetOnMediaDataCommand_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepository()
		{
			string subtitle = _fixture.Create<string>().ToUpper();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(hasSubtitle: true, subtitle: subtitle);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, CreateMediaLibraryRepository<IMedia>());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMedia>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, subtitle) == 0),
					It.IsAny<int>()),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task IsExistingTitleAsync_WhenSubtitleIsNotSetOnMediaDataCommand_AssertMediaExistsAsyncWasCalledOnMediaLibraryRepository()
		{
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand<IMedia>(hasSubtitle: false);

			await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(mediaDataCommand, CreateMediaLibraryRepository<IMedia>());

			_mediaLibraryRepositoryMock.Verify(m => m.MediaExistsAsync<IMedia>(
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false),
					It.Is<string>(value => value == null),
					It.IsAny<int>()),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public async Task IsExistingTitleAsync_WhenCalled_ReturnResultFromMediaExistsAsyncOnMediaLibraryRepository(bool mediaExists)
		{
			IMediaLibraryRepository mediaLibraryRepository = CreateMediaLibraryRepository<IMedia>(mediaExists);

			bool result = await BusinessLogic.MediaLibrary.Logic.MediaDataCommandExtensions.IsExistingTitleAsync<IMediaDataCommand<IMedia>, IMedia>(CreateMediaDataCommand<IMedia>(), mediaLibraryRepository);

			Assert.That(result, Is.EqualTo(mediaExists));
		}

		private IMediaDataCommand<TMedia> CreateMediaDataCommand<TMedia>(string title = null, bool hasSubtitle = false, string subtitle = null, int? mediaTypeIdentifier = null) where TMedia : IMedia
		{
			return CreateMediaDataCommandMock<TMedia>(title, hasSubtitle, subtitle, mediaTypeIdentifier).Object;
		}

		private Mock<IMediaDataCommand<TMedia>> CreateMediaDataCommandMock<TMedia>(string title = null, bool hasSubtitle = false, string subtitle = null, int? mediaTypeIdentifier = null) where TMedia : IMedia
		{
			Mock<IMediaDataCommand<TMedia>> mediaDataCommandMock = new Mock<IMediaDataCommand<TMedia>>();
			mediaDataCommandMock.Setup(m => m.Title)
				.Returns(title ?? _fixture.Create<string>().ToUpper());
			mediaDataCommandMock.Setup(m => m.Subtitle)
				.Returns(hasSubtitle ? subtitle ?? _fixture.Create<string>().ToUpper() : null);
			mediaDataCommandMock.Setup(m => m.MediaTypeIdentifier)
				.Returns(mediaTypeIdentifier ?? _fixture.Create<int>());
			return mediaDataCommandMock;
		}

		private IMediaLibraryRepository CreateMediaLibraryRepository<TMedia>(bool? mediaExists = null) where TMedia : class, IMedia
		{
			_mediaLibraryRepositoryMock.Setup(m => m.MediaExistsAsync<TMedia>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
				.Returns(Task.FromResult(mediaExists ?? _fixture.Create<bool>()));

			return _mediaLibraryRepositoryMock.Object;
		}
	}
}