using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMusicQueryHandler
{
	[TestFixture]
	public class QueryAsyncTests
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
		public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
		{
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMusicQuery()
		{
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut();

			Mock<IGetMusicQuery> getMusicQueryMock = CreateGetMusicQueryMock();
			await sut.QueryAsync(getMusicQueryMock.Object);

			getMusicQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertMediaIdentifierWasCalledOnGetMusicQuery()
		{
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut();

			Mock<IGetMusicQuery> getMusicQueryMock = CreateGetMusicQueryMock();
			await sut.QueryAsync(getMusicQueryMock.Object);

			getMusicQueryMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetMediaAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IGetMusicQuery getMusicQuery = CreateGetMusicQuery(mediaIdentifier);
			await sut.QueryAsync(getMusicQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut();

			IMusic result = await sut.QueryAsync(CreateGetMusicQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicHasBeenReturnedFromMediaLibraryRepository_ReturnsMusicFromMediaLibraryRepository()
		{
			IMusic music = _fixture.BuildMusicMock().Object;
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut(music: music);

			IMusic result = await sut.QueryAsync(CreateGetMusicQuery());

			Assert.That(result, Is.EqualTo(music));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNull()
		{
			IQueryHandler<IGetMusicQuery, IMusic> sut = CreateSut(false);

			IMusic result = await sut.QueryAsync(CreateGetMusicQuery());

			Assert.That(result, Is.Null);
		}

		private IQueryHandler<IGetMusicQuery, IMusic> CreateSut(bool hasMusic = true, IMusic music = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(hasMusic ? music ?? _fixture.BuildMusicMock().Object : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMusicQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMusicQuery CreateGetMusicQuery(Guid? mediaIdentifier = null)
		{
			return CreateGetMusicQueryMock(mediaIdentifier).Object;
		}

		private static Mock<IGetMusicQuery> CreateGetMusicQueryMock(Guid? mediaIdentifier = null)
		{
			Mock<IGetMusicQuery> getMusicQueryMock = new Mock<IGetMusicQuery>();
			getMusicQueryMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			return getMusicQueryMock;
		}
	}
}