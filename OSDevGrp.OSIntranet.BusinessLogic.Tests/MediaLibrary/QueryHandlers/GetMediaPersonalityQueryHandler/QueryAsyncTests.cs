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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.QueryHandlers.GetMediaPersonalityQueryHandler
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
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("query"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetMediaPersonalityQuery()
		{
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut();

			Mock<IGetMediaPersonalityQuery> getMediaPersonalityQueryMock = CreateGetMediaPersonalityQueryMock();
			await sut.QueryAsync(getMediaPersonalityQueryMock.Object);

			getMediaPersonalityQueryMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value == _validatorMock.Object),
					It.Is<IMediaLibraryRepository>(value => value == _mediaLibraryRepositoryMock.Object)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertMediaPersonalityIdentifierWasCalledOnGetMediaPersonalityQuery()
		{
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut();

			Mock<IGetMediaPersonalityQuery> getMediaPersonalityQueryMock = CreateGetMediaPersonalityQueryMock();
			await sut.QueryAsync(getMediaPersonalityQueryMock.Object);

			getMediaPersonalityQueryMock.Verify(m => m.MediaPersonalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenCalled_AssertGetMediaPersonalityAsyncWasCalledOnMediaLibraryRepository()
		{
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			IGetMediaPersonalityQuery getMediaPersonalityQuery = CreateGetMediaPersonalityQuery(mediaIdentifier);
			await sut.QueryAsync(getMediaPersonalityQuery);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaPersonalityAsync(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityHasBeenReturnedFromMediaLibraryRepository_ReturnsNotNull()
		{
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut();

			IMediaPersonality result = await sut.QueryAsync(CreateGetMediaPersonalityQuery());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityHasBeenReturnedFromMediaLibraryRepository_ReturnsMediaPersonalityFromMediaLibraryRepository()
		{
			IMediaPersonality mediaPersonality = _fixture.BuildMediaPersonalityMock().Object;
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut(mediaPersonality: mediaPersonality);

			IMediaPersonality result = await sut.QueryAsync(CreateGetMediaPersonalityQuery());

			Assert.That(result, Is.EqualTo(mediaPersonality));
		}

		[Test]
		[Category("UnitTest")]
		public async Task QueryAsync_WhenMediaPersonalityHasNotBeenReturnedFromMediaLibraryRepository_ReturnsNull()
		{
			IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> sut = CreateSut(false);

			IMediaPersonality result = await sut.QueryAsync(CreateGetMediaPersonalityQuery());

			Assert.That(result, Is.Null);
		}

		private IQueryHandler<IGetMediaPersonalityQuery, IMediaPersonality> CreateSut(bool hasMediaPersonality = true, IMediaPersonality mediaPersonality = null)
		{
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaPersonalityAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(hasMediaPersonality ? mediaPersonality ?? _fixture.BuildMediaPersonalityMock().Object : null));

			return new BusinessLogic.MediaLibrary.QueryHandlers.GetMediaPersonalityQueryHandler(_validatorMock.Object, _mediaLibraryRepositoryMock.Object);
		}

		private static IGetMediaPersonalityQuery CreateGetMediaPersonalityQuery(Guid? mediaPersonalityIdentifier = null)
		{
			return CreateGetMediaPersonalityQueryMock(mediaPersonalityIdentifier).Object;
		}

		private static Mock<IGetMediaPersonalityQuery> CreateGetMediaPersonalityQueryMock(Guid? mediaPersonalityIdentifier = null)
		{
			Mock<IGetMediaPersonalityQuery> getMediaPersonalityQueryMock = new Mock<IGetMediaPersonalityQuery>();
			getMediaPersonalityQueryMock.Setup(m => m.MediaPersonalityIdentifier)
				.Returns(mediaPersonalityIdentifier ?? Guid.NewGuid());
			return getMediaPersonalityQueryMock;
		}
	}
}