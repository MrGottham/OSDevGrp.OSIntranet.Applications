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
	public class ValidateBookDataTests
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
		public void ValidateBookData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateBookData(null, CreateBookDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenBookDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBookData(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("bookData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBookData(CreateBookDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBookData(CreateBookDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertTitleWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Title, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(title);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(title);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(title);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertSubtitleWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Subtitle, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(subtitle: subtitle);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(subtitle: subtitle);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertDescriptionWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Description, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(description: description);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(description: description);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 512),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertDetailsWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Details, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(details: details);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(details: details);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertBookGenreIdentifierWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.BookGenreIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithBookGenreIdentifier()
		{
			IValidator sut = CreateSut();

			int bookGenreIdentifier = _fixture.Create<int>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(bookGenreIdentifier: bookGenreIdentifier);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == bookGenreIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BookGenreIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithBookGenreIdentifier()
		{
			IValidator sut = CreateSut();

			int bookGenreIdentifier = _fixture.Create<int>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(bookGenreIdentifier: bookGenreIdentifier);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == bookGenreIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BookGenreIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenWrittenLanguageIdentifierWasSetOnBookDataCommand_AssertWrittenLanguageIdentifierWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock(hasWrittenLanguageIdentifier: true);
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.WrittenLanguageIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenWrittenLanguageIdentifierWasSetOnBookDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithWrittenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			int writtenLanguageIdentifier = _fixture.Create<int>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(hasWrittenLanguageIdentifier: true, writtenLanguageIdentifier: writtenLanguageIdentifier);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == writtenLanguageIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenWrittenLanguageIdentifierWasSetOnBookDataCommand_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithWrittenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			int writtenLanguageIdentifier = _fixture.Create<int>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(hasWrittenLanguageIdentifier: true, writtenLanguageIdentifier: writtenLanguageIdentifier);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == writtenLanguageIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenWrittenLanguageIdentifierWasNotSetOnBookDataCommand_AssertWrittenLanguageIdentifierWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock(hasWrittenLanguageIdentifier: false);
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.WrittenLanguageIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenWrittenLanguageIdentifierWasNotSetOnBookDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithWrittenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			IBookDataCommand bookDataCommand = CreateBookDataCommand(hasWrittenLanguageIdentifier: false);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenWrittenLanguageIdentifierWasNotSetOnBookDataCommand_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorWithWrittenLanguageIdentifier()
		{
			IValidator sut = CreateSut();

			IBookDataCommand bookDataCommand = CreateBookDataCommand(hasWrittenLanguageIdentifier: false);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<int>(),
					It.IsAny<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertMediaTypeIdentifierWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.MediaTypeIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertInternationalStandardBookNumberWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.InternationalStandardBookNumber, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithInternationalStandardBookNumber()
		{
			IValidator sut = CreateSut();

			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(internationalStandardBookNumber: internationalStandardBookNumber);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, internationalStandardBookNumber) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "InternationalStandardBookNumber") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithInternationalStandardBookNumber()
		{
			IValidator sut = CreateSut();

			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(internationalStandardBookNumber: internationalStandardBookNumber);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, internationalStandardBookNumber) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "InternationalStandardBookNumber") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithInternationalStandardBookNumber()
		{
			IValidator sut = CreateSut();

			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(internationalStandardBookNumber: internationalStandardBookNumber);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, internationalStandardBookNumber) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.InternationalStandardBookNumberPattern) == 0),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "InternationalStandardBookNumber") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenPublishedWasSetOnBookDataCommand_AssertPublishedWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock(hasPublished: true);
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenPublishedWasSetOnBookDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			short published = _fixture.Create<short>();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(hasPublished: true, published: published);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == published),
					It.Is<int>(v => v == 1000),
					It.Is<int>(v => v == 9999),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenPublishedWasNotSetOnBookDataCommand_AssertPublishedWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock(hasPublished: false);
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenPublishedWasNotSetOnBookDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			IBookDataCommand bookDataCommand = CreateBookDataCommand(hasPublished: false);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertUrlWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Url, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/book/{_fixture.Create<string>()}");
			IBookDataCommand bookDataCommand = CreateBookDataCommand(url: url);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/book/{_fixture.Create<string>()}");
			IBookDataCommand bookDataCommand = CreateBookDataCommand(url: url);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/book/{_fixture.Create<string>()}");
            IBookDataCommand bookDataCommand = CreateBookDataCommand(url: url);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertImageWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Image, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(image: image);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(image: image);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertAuthorsWasCalledOnBookDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBookDataCommand> bookDataCommandMock = CreateBookDataCommandMock();
			sut.ValidateBookData(bookDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			bookDataCommandMock.Verify(m => m.Authors, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithAuthors()
		{
			IValidator sut = CreateSut();

			Guid[] authors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(authors: authors);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v.All(mediaPersonalityIdentifier => authors.Contains(mediaPersonalityIdentifier))),
					It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Authors") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAuthors()
		{
			IValidator sut = CreateSut();

			Guid[] authors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IBookDataCommand bookDataCommand = CreateBookDataCommand(authors: authors);
			sut.ValidateBookData(bookDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in authors)
			{
				_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
						It.Is<Guid>(v => v == mediaPersonalityIdentifier),
						It.IsNotNull<Func<Guid, Task<bool>>>(),
						It.Is<Type>(v => v != null && v == bookDataCommand.GetType()),
						It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Authors") == 0),
						It.Is<bool>(v => v == false)),
					Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBookData(CreateBookDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBookData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBookData(CreateBookDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private IBookDataCommand CreateBookDataCommand(string title = null, string subtitle = null, string description = null, string details = null, int? bookGenreIdentifier = null, bool hasWrittenLanguageIdentifier = true, int? writtenLanguageIdentifier = null, int? mediaTypeIdentifier = null, string internationalStandardBookNumber = null, bool hasPublished = true, short? published = null, string url = null, byte[] image = null, IEnumerable<Guid> authors = null)
		{
			return CreateBookDataCommandMock(title, subtitle, description, details, bookGenreIdentifier, hasWrittenLanguageIdentifier, writtenLanguageIdentifier, mediaTypeIdentifier, internationalStandardBookNumber, hasPublished, published, url, image, authors).Object;
		}

		private Mock<IBookDataCommand> CreateBookDataCommandMock(string title = null, string subtitle = null, string description = null, string details = null, int? bookGenreIdentifier = null, bool hasWrittenLanguageIdentifier = true, int? writtenLanguageIdentifier = null, int? mediaTypeIdentifier = null, string internationalStandardBookNumber = null, bool hasPublished = true, short? published = null, string url = null, byte[] image = null, IEnumerable<Guid> authors = null)
		{
			Mock<IBookDataCommand> bookDataCommandMock = new Mock<IBookDataCommand>();
			bookDataCommandMock.Setup(m => m.Title)
				.Returns(title ?? _fixture.Create<string>());
			bookDataCommandMock.Setup(m => m.Subtitle)
				.Returns(subtitle ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			bookDataCommandMock.Setup(m => m.Description)
				.Returns(description ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			bookDataCommandMock.Setup(m => m.Details)
				.Returns(details ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			bookDataCommandMock.Setup(m => m.BookGenreIdentifier)
				.Returns(bookGenreIdentifier ?? _fixture.Create<int>());
			bookDataCommandMock.Setup(m => m.WrittenLanguageIdentifier)
				.Returns(hasWrittenLanguageIdentifier ? writtenLanguageIdentifier ?? _fixture.Create<int>() : null);
			bookDataCommandMock.Setup(m => m.MediaTypeIdentifier)
				.Returns(mediaTypeIdentifier ?? _fixture.Create<int>());
			bookDataCommandMock.Setup(m => m.InternationalStandardBookNumber)
				.Returns(internationalStandardBookNumber ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			bookDataCommandMock.Setup(m => m.Published)
				.Returns(hasPublished ? published ?? _fixture.Create<short>() : null);
			bookDataCommandMock.Setup(m => m.Url)
				.Returns(url ?? (_random.Next(100) > 50 ? _fixture.CreateEndpointString(path: $"api/book/{_fixture.Create<string>()}") : null));
			bookDataCommandMock.Setup(m => m.Image)
				.Returns(image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			bookDataCommandMock.Setup(m => m.Authors)
				.Returns(authors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray());
			return bookDataCommandMock;
		}
	}
}