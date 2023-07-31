using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateMusicDataTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateMusicData(null, CreateMusicDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenMusicDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMusicData(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("musicData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMusicData(CreateMusicDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMusicData(CreateMusicDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertTitleWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Title, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(title);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(title);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(title);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertSubtitleWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Subtitle, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(subtitle: subtitle);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(subtitle: subtitle);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertDescriptionWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Description, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(description: description);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(description: description);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 512),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertDetailsWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Details, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(details: details);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(details: details);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertMusicGenreIdentifierWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.MusicGenreIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMusicGenreIdentifier()
		{
			IValidator sut = CreateSut();

			int musicGenreIdentifier = _fixture.Create<int>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(musicGenreIdentifier: musicGenreIdentifier);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == musicGenreIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MusicGenreIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMusicGenreIdentifier()
		{
			IValidator sut = CreateSut();

			int musicGenreIdentifier = _fixture.Create<int>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(musicGenreIdentifier: musicGenreIdentifier);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == musicGenreIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MusicGenreIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertMediaTypeIdentifierWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.MediaTypeIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenPublishedWasSetOnMusicDataCommand_AssertPublishedWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock(hasPublished: true);
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenPublishedWasSetOnMusicDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			short published = _fixture.Create<short>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(hasPublished: true, published: published);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == published),
					It.Is<int>(v => v == 1000),
					It.Is<int>(v => v == 9999),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenPublishedWasNotSetOnMusicDataCommand_AssertPublishedWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock(hasPublished: false);
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenPublishedWasNotSetOnMusicDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(hasPublished: false);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenTracksWasSetOnMusicDataCommand_AssertTracksWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock(hasTracks: true);
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Tracks, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenTracksWasSetOnMusicDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithTracks()
		{
			IValidator sut = CreateSut();

			short tracks = _fixture.Create<short>();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(hasTracks: true, tracks: tracks);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == tracks),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 999),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Tracks") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenTracksWasNotSetOnMusicDataCommand_AssertTracksWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock(hasTracks: false);
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Tracks, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenTracksWasNotSetOnMusicDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithTracks()
		{
			IValidator sut = CreateSut();

			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(hasTracks: false);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Tracks") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertUrlWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Url, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = $"https://localhost/api/music/{_fixture.Create<string>()}";
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(url: url);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = $"https://localhost/api/music/{_fixture.Create<string>()}";
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(url: url);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = $"https://localhost/api/music/{_fixture.Create<string>()}";
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(url: url);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertImageWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Image, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(image: image);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(image: image);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertArtistsWasCalledOnMusicDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMusicDataCommand> musicDataCommandMock = CreateMusicDataCommandMock();
			sut.ValidateMusicData(musicDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			musicDataCommandMock.Verify(m => m.Artists, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithArtists()
		{
			IValidator sut = CreateSut();

			Guid[] artists = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(artists: artists);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v.All(mediaPersonalityIdentifier => artists.Contains(mediaPersonalityIdentifier))),
					It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Artists") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithArtists()
		{
			IValidator sut = CreateSut();

			Guid[] artists = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IMusicDataCommand musicDataCommand = CreateMusicDataCommand(artists: artists);
			sut.ValidateMusicData(musicDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in artists)
			{
				_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
						It.Is<Guid>(v => v == mediaPersonalityIdentifier),
						It.IsNotNull<Func<Guid, Task<bool>>>(),
						It.Is<Type>(v => v != null && v == musicDataCommand.GetType()),
						It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Artists") == 0),
						It.Is<bool>(v => v == false)),
					Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMusicData(CreateMusicDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMusicData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMusicData(CreateMusicDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private IMusicDataCommand CreateMusicDataCommand(string title = null, string subtitle = null, string description = null, string details = null, int? musicGenreIdentifier = null, int? mediaTypeIdentifier = null, bool hasPublished = true, short? published = null, bool hasTracks = true, short? tracks = null, string url = null, byte[] image = null, IEnumerable<Guid> artists = null)
		{
			return CreateMusicDataCommandMock(title, subtitle, description, details, musicGenreIdentifier, mediaTypeIdentifier, hasPublished, published, hasTracks, tracks, url, image, artists).Object;
		}

		private Mock<IMusicDataCommand> CreateMusicDataCommandMock(string title = null, string subtitle = null, string description = null, string details = null, int? musicGenreIdentifier = null, int? mediaTypeIdentifier = null, bool hasPublished = true, short? published = null, bool hasTracks = true, short? tracks = null, string url = null, byte[] image = null, IEnumerable<Guid> artists = null)
		{
			Mock<IMusicDataCommand> musicDataCommandMock = new Mock<IMusicDataCommand>();
			musicDataCommandMock.Setup(m => m.Title)
				.Returns(title ?? _fixture.Create<string>());
			musicDataCommandMock.Setup(m => m.Subtitle)
				.Returns(subtitle ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			musicDataCommandMock.Setup(m => m.Description)
				.Returns(description ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			musicDataCommandMock.Setup(m => m.Details)
				.Returns(details ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			musicDataCommandMock.Setup(m => m.MusicGenreIdentifier)
				.Returns(musicGenreIdentifier ?? _fixture.Create<int>());
			musicDataCommandMock.Setup(m => m.MediaTypeIdentifier)
				.Returns(mediaTypeIdentifier ?? _fixture.Create<int>());
			musicDataCommandMock.Setup(m => m.Published)
				.Returns(hasPublished ? published ?? _fixture.Create<short>() : null);
			musicDataCommandMock.Setup(m => m.Tracks)
				.Returns(hasTracks ? tracks ?? _fixture.Create<short>() : null);
			musicDataCommandMock.Setup(m => m.Url)
				.Returns(url ?? (_random.Next(100) > 50 ? $"https://localhost/api/music/{_fixture.Create<string>()}" : null));
			musicDataCommandMock.Setup(m => m.Image)
				.Returns(image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			musicDataCommandMock.Setup(m => m.Artists)
				.Returns(artists ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray());
			return musicDataCommandMock;
		}
	}
}