using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
    [TestFixture]
	public class ValidateMovieDataTests
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
		public void ValidateMovieData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateMovieData(null, CreateMovieDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenMovieDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMovieData(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("movieData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMovieData(CreateMovieDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMovieData(CreateMovieDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertTitleWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Title, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(title);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(title);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(title);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertSubtitleWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Subtitle, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(subtitle: subtitle);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(subtitle: subtitle);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertDescriptionWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Description, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(description: description);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(description: description);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 512),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertDetailsWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Details, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(details: details);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(details: details);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertMovieGenreIdentifierWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.MovieGenreIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMovieGenreIdentifier()
		{
			IValidator sut = CreateSut();

			int movieGenreIdentifier = _fixture.Create<int>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(movieGenreIdentifier: movieGenreIdentifier);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == movieGenreIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MovieGenreIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMovieGenreIdentifier()
		{
			IValidator sut = CreateSut();

			int movieGenreIdentifier = _fixture.Create<int>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(movieGenreIdentifier: movieGenreIdentifier);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == movieGenreIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MovieGenreIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenSpokenLanguageIdentifierWasSetOnMovieDataCommand_AssertSpokenLanguageIdentifierWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock(hasSpokenLanguageIdentifier: true);
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.SpokenLanguageIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenSpokenLanguageIdentifierWasSetOnMovieDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithSpokenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			int spokenLanguageIdentifier = _fixture.Create<int>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasSpokenLanguageIdentifier: true, spokenLanguageIdentifier: spokenLanguageIdentifier);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == spokenLanguageIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SpokenLanguageIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenSpokenLanguageIdentifierWasSetOnMovieDataCommand_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithSpokenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			int spokenLanguageIdentifier = _fixture.Create<int>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasSpokenLanguageIdentifier: true, spokenLanguageIdentifier: spokenLanguageIdentifier);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == spokenLanguageIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SpokenLanguageIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenSpokenLanguageIdentifierWasNotSetOnMovieDataCommand_AssertSpokenLanguageIdentifierWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock(hasSpokenLanguageIdentifier: false);
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.SpokenLanguageIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenSpokenLanguageIdentifierWasNotSetOnMovieDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithSpokenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasSpokenLanguageIdentifier: false);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SpokenLanguageIdentifier") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenSpokenLanguageIdentifierWasNotSetOnMovieDataCommand_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorWithSpokenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasSpokenLanguageIdentifier: false);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<int>(),
					It.IsAny<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SpokenLanguageIdentifier") == 0),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertMediaTypeIdentifierWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.MediaTypeIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenPublishedWasSetOnMovieDataCommand_AssertPublishedWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock(hasPublished: true);
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenPublishedWasSetOnMovieDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			short published = _fixture.Create<short>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasPublished: true, published: published);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == published),
					It.Is<int>(v => v == 1000),
					It.Is<int>(v => v == 9999),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenPublishedWasNotSetOnMovieDataCommand_AssertPublishedWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock(hasPublished: false);
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenPublishedWasNotSetOnMovieDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasPublished: false);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenLengthWasSetOnMovieDataCommand_AssertLengthWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock(hasLength: true);
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Length, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenLengthWasSetOnMovieDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithLength()
		{
			IValidator sut = CreateSut();

			short length = _fixture.Create<short>();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasLength: true, length: length);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == length),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 999),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Length") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenLengthWasNotSetOnMovieDataCommand_AssertLengthWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock(hasLength: false);
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Length, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenLengthWasNotSetOnMovieDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithLength()
		{
			IValidator sut = CreateSut();

			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(hasLength: false);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Length") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertUrlWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Url, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/movie/{_fixture.Create<string>()}");
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(url: url);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/movie/{_fixture.Create<string>()}");
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(url: url);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = _fixture.CreateEndpointString(path: $"api/movie/{_fixture.Create<string>()}");
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(url: url);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertImageWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Image, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(image: image);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(image: image);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertDirectorsWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Directors, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithDirectors()
		{
			IValidator sut = CreateSut();

			Guid[] directors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(directors: directors);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v.All(mediaPersonalityIdentifier => directors.Contains(mediaPersonalityIdentifier))),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Directors") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithDirectors()
		{
			IValidator sut = CreateSut();

			Guid[] directors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(directors: directors);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in directors)
			{
				_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
						It.Is<Guid>(v => v == mediaPersonalityIdentifier),
						It.IsNotNull<Func<Guid, Task<bool>>>(),
						It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
						It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Directors") == 0),
						It.Is<bool>(v => v == false)),
					Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertActorsWasCalledOnMovieDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMovieDataCommand> movieDataCommandMock = CreateMovieDataCommandMock();
			sut.ValidateMovieData(movieDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			movieDataCommandMock.Verify(m => m.Actors, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithActors()
		{
			IValidator sut = CreateSut();

			Guid[] actors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(actors: actors);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v.All(mediaPersonalityIdentifier => actors.Contains(mediaPersonalityIdentifier))),
					It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Actors") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithActors()
		{
			IValidator sut = CreateSut();

			Guid[] actors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IMovieDataCommand movieDataCommand = CreateMovieDataCommand(actors: actors);
			sut.ValidateMovieData(movieDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in actors)
			{
				_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
						It.Is<Guid>(v => v == mediaPersonalityIdentifier),
						It.IsNotNull<Func<Guid, Task<bool>>>(),
						It.Is<Type>(v => v != null && v == movieDataCommand.GetType()),
						It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Actors") == 0),
						It.Is<bool>(v => v == false)),
					Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMovieData(CreateMovieDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMovieData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMovieData(CreateMovieDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private IMovieDataCommand CreateMovieDataCommand(string title = null, string subtitle = null, string description = null, string details = null, int? movieGenreIdentifier = null, bool hasSpokenLanguageIdentifier = true, int? spokenLanguageIdentifier = null, int? mediaTypeIdentifier = null, bool hasPublished = true, short? published = null, bool hasLength = true, short? length = null, string url = null, byte[] image = null, IEnumerable<Guid> directors = null, IEnumerable<Guid> actors = null)
		{
			return CreateMovieDataCommandMock(title, subtitle, description, details, movieGenreIdentifier, hasSpokenLanguageIdentifier, spokenLanguageIdentifier, mediaTypeIdentifier, hasPublished, published, hasLength, length, url, image, directors, actors).Object;
		}

		private Mock<IMovieDataCommand> CreateMovieDataCommandMock(string title = null, string subtitle = null, string description = null, string details = null, int? movieGenreIdentifier = null, bool hasSpokenLanguageIdentifier = true, int? spokenLanguageIdentifier = null, int? mediaTypeIdentifier = null, bool hasPublished = true, short? published = null, bool hasLength = true, short? length = null, string url = null, byte[] image = null, IEnumerable<Guid> directors = null, IEnumerable<Guid> actors = null)
		{
			Mock<IMovieDataCommand> movieDataCommandMock = new Mock<IMovieDataCommand>();
			movieDataCommandMock.Setup(m => m.Title)
				.Returns(title ?? _fixture.Create<string>());
			movieDataCommandMock.Setup(m => m.Subtitle)
				.Returns(subtitle ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			movieDataCommandMock.Setup(m => m.Description)
				.Returns(description ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			movieDataCommandMock.Setup(m => m.Details)
				.Returns(details ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			movieDataCommandMock.Setup(m => m.MovieGenreIdentifier)
				.Returns(movieGenreIdentifier ?? _fixture.Create<int>());
			movieDataCommandMock.Setup(m => m.SpokenLanguageIdentifier)
				.Returns(hasSpokenLanguageIdentifier ? spokenLanguageIdentifier ?? _fixture.Create<int>() : null);
			movieDataCommandMock.Setup(m => m.MediaTypeIdentifier)
				.Returns(mediaTypeIdentifier ?? _fixture.Create<int>());
			movieDataCommandMock.Setup(m => m.Published)
				.Returns(hasPublished ? published ?? _fixture.Create<short>() : null);
			movieDataCommandMock.Setup(m => m.Length)
				.Returns(hasLength ? length ?? _fixture.Create<short>() : null);
			movieDataCommandMock.Setup(m => m.Url)
				.Returns(url ?? (_random.Next(100) > 50 ? _fixture.CreateEndpointString(path: $"api/movie/{_fixture.Create<string>()}") : null));
			movieDataCommandMock.Setup(m => m.Image)
				.Returns(image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			movieDataCommandMock.Setup(m => m.Directors)
				.Returns(directors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray());
			movieDataCommandMock.Setup(m => m.Actors)
				.Returns(actors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray());
			return movieDataCommandMock;
		}
	}
}