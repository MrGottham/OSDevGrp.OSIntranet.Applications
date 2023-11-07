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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMusicCollectionQueryHandler
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
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMusicCollectionQuery()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			Mock<IGetMusicCollectionQuery> getMusicCollectionQueryMock = CreateGetMusicCollectionQueryMock();
			await sut.QueryAsync(getMusicCollectionQueryMock.Object);

			getMusicCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertFilterWasCalledOnGetMusicCollectionQuery()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			Mock<IGetMusicCollectionQuery> getMusicCollectionQueryMock = CreateGetMusicCollectionQueryMock();
			await sut.QueryAsync(getMusicCollectionQueryMock.Object);

			getMusicCollectionQueryMock.Verify(m => m.Filter, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public async Task QueryAsync_WhenFilterHasNotBeenSetOnGetMusicCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository(string filter)
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			IGetMusicCollectionQuery getMusicCollectionQuery = CreateGetMusicCollectionQuery(filter);
			await sut.QueryAsync(getMusicCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync<IMusic>(It.Is<string>(value => string.IsNullOrWhiteSpace(value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenFilterHasBeenSetOnGetMusicCollectionQuery_AssertGetMediasAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			string filter = _fixture.Create<string>();
			IGetMusicCollectionQuery getMusicCollectionQuery = CreateGetMusicCollectionQuery(filter);
			await sut.QueryAsync(getMusicCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediasAsync<IMusic>(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, filter) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			IEnumerable<IMusic> result = await sut.QueryAsync(CreateGetMusicCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut();

			IEnumerable<IMusic> result = await sut.QueryAsync(CreateGetMusicCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsMusicCollectionFromMediaLibraryRepository()
		{
			IEnumerable<IMusic> musicCollection = new[]
			{
				_fixture.BuildMusicMock().Object,
				_fixture.BuildMusicMock().Object,
				_fixture.BuildMusicMock().Object,
				_fixture.BuildMusicMock().Object,
				_fixture.BuildMusicMock().Object
			};
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut(musicCollection: musicCollection);

			IEnumerable<IMusic> result = await sut.QueryAsync(CreateGetMusicCollectionQuery());

			Assert.That(result, Is.EqualTo(musicCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut(false);

			IEnumerable<IMusic> result = await sut.QueryAsync(CreateGetMusicCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMusicCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> sut = CreateSut(false);

			IEnumerable<IMusic> result = await sut.QueryAsync(CreateGetMusicCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetMusicCollectionQuery, IEnumerable<IMusic>> CreateSut(bool hasMusicCollection = true, IEnumerable<IMusic> musicCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediasAsync<IMusic>(It.IsAny<string>()))
				.Returns(Task.FromResult(hasMusicCollection ? musicCollection ?? new[] {_fixture.BuildMusicMock().Object, _fixture.BuildMusicMock().Object, _fixture.BuildMusicMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMusicCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMusicCollectionQuery CreateGetMusicCollectionQuery(string filter = null)
		{
			return CreateGetMusicCollectionQueryMock(filter).Object;
		}

		private static Mock<IGetMusicCollectionQuery> CreateGetMusicCollectionQueryMock(string filter = null)
		{
			Mock<IGetMusicCollectionQuery> getMusicCollectionQueryMock = new Mock<IGetMusicCollectionQuery>();
			getMusicCollectionQueryMock.Setup(m => m.Filter)
				.Returns(string.IsNullOrWhiteSpace(filter) == false ? filter : null);
			return getMusicCollectionQueryMock;
		}
	}
}