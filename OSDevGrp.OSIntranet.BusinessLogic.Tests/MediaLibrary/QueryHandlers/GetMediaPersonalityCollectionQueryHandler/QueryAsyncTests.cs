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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMediaPersonalityCollectionQueryHandler
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
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMediaPersonalityCollectionQuery()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			Mock<IGetMediaPersonalityCollectionQuery> getMediaPersonalityCollectionQueryMock = CreateGetMediaPersonalityCollectionQueryMock();
			await sut.QueryAsync(getMediaPersonalityCollectionQueryMock.Object);

			getMediaPersonalityCollectionQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertFilterWasCalledOnGetMediaPersonalityCollectionQuery()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			Mock<IGetMediaPersonalityCollectionQuery> getMediaPersonalityCollectionQueryMock = CreateGetMediaPersonalityCollectionQueryMock();
			await sut.QueryAsync(getMediaPersonalityCollectionQueryMock.Object);

			getMediaPersonalityCollectionQueryMock.Verify(m => m.Filter, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public async Task QueryAsync_WhenFilterHasNotBeenSetOnGetMediaPersonalityCollectionQuery_AssertGetMediaPersonalitiesAsyncWasCalledOnMediaLibraryRepository(string filter)
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			IGetMediaPersonalityCollectionQuery getMediaPersonalityCollectionQuery = CreateGetMediaPersonalityCollectionQuery(filter);
			await sut.QueryAsync(getMediaPersonalityCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaPersonalitiesAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value))), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenFilterHasBeenSetOnGetMediaPersonalityCollectionQuery_AssertGetMediaPersonalitiesAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			string filter = _fixture.Create<string>();
			IGetMediaPersonalityCollectionQuery getMediaPersonalityCollectionQuery = CreateGetMediaPersonalityCollectionQuery(filter);
			await sut.QueryAsync(getMediaPersonalityCollectionQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaPersonalitiesAsync(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, filter) == 0)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.QueryAsync(CreateGetMediaPersonalityCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsNotEmpty()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.QueryAsync(CreateGetMediaPersonalityCollectionQuery());

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityCollectionHasBeenReturnedFromMediaLibraryRepository_ReturnsMediaPersonalityCollectionFromMediaLibraryRepository()
		{
			IEnumerable<IMediaPersonality> mediaPersonalityCollection = new[]
			{
				_fixture.BuildMediaPersonalityMock().Object,
				_fixture.BuildMediaPersonalityMock().Object,
				_fixture.BuildMediaPersonalityMock().Object,
				_fixture.BuildMediaPersonalityMock().Object,
				_fixture.BuildMediaPersonalityMock().Object
			};
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut(mediaPersonalityCollection: mediaPersonalityCollection);

			IEnumerable<IMediaPersonality> result = await sut.QueryAsync(CreateGetMediaPersonalityCollectionQuery());

			Assert.That(result, Is.EqualTo(mediaPersonalityCollection));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut(false);

			IEnumerable<IMediaPersonality> result = await sut.QueryAsync(CreateGetMediaPersonalityCollectionQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityCollectionHasNotBeenReturnedFromMediaLibraryRepository_ReturnsEmpty()
		{
			IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> sut = CreateSut(false);

			IEnumerable<IMediaPersonality> result = await sut.QueryAsync(CreateGetMediaPersonalityCollectionQuery());

			Assert.That(result, Is.Empty);
		}

		private IQueryHandler<IGetMediaPersonalityCollectionQuery, IEnumerable<IMediaPersonality>> CreateSut(bool hasMediaPersonalityCollection = true, IEnumerable<IMediaPersonality> mediaPersonalityCollection = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaPersonalitiesAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(hasMediaPersonalityCollection ? mediaPersonalityCollection ?? new[] {_fixture.BuildMediaPersonalityMock().Object, _fixture.BuildMediaPersonalityMock().Object, _fixture.BuildMediaPersonalityMock().Object} : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMediaPersonalityCollectionQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMediaPersonalityCollectionQuery CreateGetMediaPersonalityCollectionQuery(string filter = null)
		{
			return CreateGetMediaPersonalityCollectionQueryMock(filter).Object;
		}

		private static Mock<IGetMediaPersonalityCollectionQuery> CreateGetMediaPersonalityCollectionQueryMock(string filter = null)
		{
			Mock<IGetMediaPersonalityCollectionQuery> getMediaPersonalityCollectionQueryMock = new Mock<IGetMediaPersonalityCollectionQuery>();
			getMediaPersonalityCollectionQueryMock.Setup(m => m.Filter)
				.Returns(string.IsNullOrWhiteSpace(filter) == false ? filter : null);
			return getMediaPersonalityCollectionQueryMock;
		}
	}
}