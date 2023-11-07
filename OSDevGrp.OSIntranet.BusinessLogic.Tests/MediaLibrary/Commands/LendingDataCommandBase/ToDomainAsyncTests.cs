using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.LendingDataCommandBase
{
	[TestFixture]
	public class ToDomainAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ILendingDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			ILendingDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_AssertGetBorrowerAsyncWasCalledOnMediaLibraryRepository()
		{
			Guid borrowerIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(borrowerIdentifier: borrowerIdentifier);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetBorrowerAsync(It.Is<Guid>(value => value == borrowerIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMovie_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier, isMovie: true, isMusic: false, isBook: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMovie_AssertGetMediaAsyncWasNotCalledOnMediaLibraryRepositoryForMusic()
		{
			ILendingDataCommand sut = CreateSut(isMovie: true, isMusic: false, isBook: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMovie_AssertGetMediaAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			ILendingDataCommand sut = CreateSut(isMovie: true, isMusic: false, isBook: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMusic_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier, isMovie: false, isMusic: true, isBook: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMusic_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMusic()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier, isMovie: false, isMusic: true, isBook: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMusic_AssertGetMediaAsyncWasNotCalledOnMediaLibraryRepositoryForBook()
		{
			ILendingDataCommand sut = CreateSut(isMovie: false, isMusic: true, isBook: false);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMovie()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier, isMovie: false, isMusic: false, isBook: true);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMovie>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForMusic()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier, isMovie: false, isMusic: false, isBook: true);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IMusic>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsBook_AssertGetMediaAsyncWasCalledOnMediaLibraryRepositoryForBook()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier, isMovie: false, isMusic: false, isBook: true);

			await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_mediaLibraryRepositoryMock.Verify(m => m.GetMediaAsync<IBook>(It.Is<Guid>(value => value == mediaIdentifier)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsNotNull()
		{
			ILendingDataCommand sut = CreateSut();

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsLending()
		{
			ILendingDataCommand sut = CreateSut();

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.TypeOf<Lending>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsLendingWhereLendingIdentifierIsEqualToLendingIdentifierFromLendingDataCommand()
		{
			Guid lendingIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(lendingIdentifier);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.LendingIdentifier, Is.EqualTo(lendingIdentifier));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsLendingWhereBorrowerIsNotNull()
		{
			IBorrower borrower = _fixture.BuildBorrowerMock().Object;
			ILendingDataCommand sut = CreateSut(borrower: borrower);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Borrower, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsLendingWhereBorrowerIsEqualToMatchingBorrowerFromMediaLibraryRepository()
		{
			IBorrower borrower = _fixture.BuildBorrowerMock().Object;
			ILendingDataCommand sut = CreateSut(borrower: borrower);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Borrower, Is.EqualTo(borrower));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMovie_ReturnsLendingWhereMediaIsNotNull()
		{
			ILendingDataCommand sut = CreateSut(isMovie: true, isMusic: false, isBook: false);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Media, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMovie_ReturnsLendingWhereMediaIsEqualToMatchingMovieFromMediaLibraryRepository()
		{
			IMovie movie = _fixture.BuildMovieMock().Object;
			ILendingDataCommand sut = CreateSut(isMovie: true, movie: movie, isMusic: false, isBook: false);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Media, Is.EqualTo(movie));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMusic_ReturnsLendingWhereMediaIsNotNull()
		{
			ILendingDataCommand sut = CreateSut(isMovie: false, isMusic: true, isBook: false);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Media, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsMusic_ReturnsLendingWhereMediaIsEqualToMatchingMusicFromMediaLibraryRepository()
		{
			IMusic music = _fixture.BuildMusicMock().Object;
			ILendingDataCommand sut = CreateSut(isMovie: false, isMusic: true, music: music, isBook: false);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Media, Is.EqualTo(music));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsBook_ReturnsLendingWhereMediaIsNotNull()
		{
			ILendingDataCommand sut = CreateSut(isMovie: false, isMusic: false, isBook: true);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Media, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMediaIdentifierIsBook_ReturnsLendingWhereMediaIsEqualToMatchingBookFromMediaLibraryRepository()
		{
			IBook book = _fixture.BuildBookMock().Object;
			ILendingDataCommand sut = CreateSut(isMovie: false, isMusic: false, isBook: true, book: book);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Media, Is.EqualTo(book));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsLendingWhereLendingDateIsEqualToLendingDateFromLendingDataCommand()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.LendingDate, Is.EqualTo(lendingDate));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsLendingWhereRecallDateIsEqualToRecallDateFromLendingDataCommand()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			DateTime recallDate = lendingDate.AddDays(_random.Next(7, 14));
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate, recallDate: recallDate);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.RecallDate, Is.EqualTo(recallDate));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenReturnedDateIsSetOnLendingDataCommand_ReturnsLendingWhereReturnedDateIsNotNull()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate, hasReturnedDate: true);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.ReturnedDate, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenReturnedDateIsSetOnLendingDataCommand_ReturnsLendingWhereReturnedDateIsEqualToReturnedDateFromLendingDataCommand()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			DateTime returnedDate = lendingDate.AddDays(_random.Next(7, 14));
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate, hasReturnedDate: true, returnedDate: returnedDate);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.ReturnedDate, Is.EqualTo(returnedDate));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenReturnedDateIsNotSetOnLendingDataCommand_ReturnsLendingWhereReturnedDateIsNull()
		{
			ILendingDataCommand sut = CreateSut(hasReturnedDate: false);

			ILending result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.ReturnedDate, Is.Null);
		}

		private ILendingDataCommand CreateSut(Guid? lendingIdentifier = null, Guid? borrowerIdentifier = null, IBorrower borrower = null, Guid? mediaIdentifier = null, bool isMovie = true, IMovie movie = null, bool isMusic = false, IMusic music = null, bool isBook = false, IBook book = null, DateTime? lendingDate = null, DateTime? recallDate = null, bool? hasReturnedDate = null, DateTime? returnedDate = null)
		{
			lendingDate ??= DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			recallDate ??= lendingDate.Value.AddDays(_random.Next(7, 14));
			returnedDate = hasReturnedDate ?? _random.Next(100) > 50
				? returnedDate ?? lendingDate.Value.AddDays(_random.Next(7, 14))
				: null;

			_mediaLibraryRepositoryMock.Setup(m => m.GetBorrowerAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(borrower ?? _fixture.BuildBorrowerMock().Object));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMovie>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(isMovie ? movie ?? _fixture.BuildMovieMock().Object : null));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IMusic>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(isMusic ? music ?? _fixture.BuildMusicMock().Object : null));
			_mediaLibraryRepositoryMock.Setup(m => m.GetMediaAsync<IBook>(It.IsAny<Guid>()))
				.Returns(Task.FromResult(isBook ? book ?? _fixture.BuildBookMock().Object : null));

			return new MyLendingDataCommandBase(lendingIdentifier ?? Guid.NewGuid(), borrowerIdentifier ?? Guid.NewGuid(), mediaIdentifier ?? Guid.NewGuid(), lendingDate.Value.Date, recallDate.Value.Date, returnedDate?.Date);
		}

		private class MyLendingDataCommandBase : BusinessLogic.MediaLibrary.Commands.LendingDataCommandBase
		{
			#region Constructor

			public MyLendingDataCommandBase(Guid lendingIdentifier, Guid borrowerIdentifier, Guid mediaIdentifier, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate)
				: base(lendingIdentifier, borrowerIdentifier, mediaIdentifier, lendingDate, recallDate, returnedDate)
			{
			}

			#endregion
		}
	}
}