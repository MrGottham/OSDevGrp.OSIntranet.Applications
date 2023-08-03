using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.BookDataCommandBase
{
	[TestFixture]
	public class ValidateTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			IBookDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			IBookDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBookDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBookDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertIsMediaLibraryModifierWasCalledOnClaimResolver()
		{
			IBookDataCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_claimResolverMock.Verify(m => m.IsMediaLibraryModifier(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithTitle()
		{
			string title = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(title);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title.ToUpper()) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithTitle()
		{
			string title = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(title);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title.ToUpper()) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithTitle()
		{
			string title = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(title);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title.ToUpper()) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSubtitle()
		{
			string subtitle = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(subtitle: subtitle);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle.ToUpper()) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSubtitle()
		{
			string subtitle = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(subtitle: subtitle);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle.ToUpper()) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDescription()
		{
			string description = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(description: description);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDescription()
		{
			string description = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(description: description);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 512),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDetails()
		{
			string details = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(details: details);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDetails()
		{
			string details = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(details: details);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithBookGenreIdentifier()
		{
			int bookGenreIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(bookGenreIdentifier: bookGenreIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == bookGenreIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BookGenreIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithBookGenreIdentifier()
		{
			int bookGenreIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(bookGenreIdentifier: bookGenreIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == bookGenreIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BookGenreIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenWrittenLanguageIdentifierWasSetOnBookDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithWrittenLanguageIdentifier()
		{
			int writtenLanguageIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: true, writtenLanguageIdentifier: writtenLanguageIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == writtenLanguageIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenWrittenLanguageIdentifierWasSetOnBookDataCommand_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithWrittenLanguageIdentifier()
		{
			int writtenLanguageIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: true, writtenLanguageIdentifier: writtenLanguageIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == writtenLanguageIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenWrittenLanguageIdentifierWasNotSetOnBookDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithWrittenLanguageIdentifier()
		{
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenWrittenLanguageIdentifierWasNotSetOnBookDataCommand_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorWithWrittenLanguageIdentifier()
		{
			IBookDataCommand sut = CreateSut(hasWrittenLanguageIdentifier: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<int>(),
					It.IsAny<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "WrittenLanguageIdentifier") == 0),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMediaTypeIdentifier()
		{
			int mediaTypeIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(mediaTypeIdentifier: mediaTypeIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaTypeIdentifier()
		{
			int mediaTypeIdentifier = _fixture.Create<int>();
			IBookDataCommand sut = CreateSut(mediaTypeIdentifier: mediaTypeIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithInternationalStandardBookNumber()
		{
			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(internationalStandardBookNumber: internationalStandardBookNumber);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, internationalStandardBookNumber) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "InternationalStandardBookNumber") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithInternationalStandardBookNumber()
		{
			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(internationalStandardBookNumber: internationalStandardBookNumber);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, internationalStandardBookNumber) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "InternationalStandardBookNumber") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithInternationalStandardBookNumber()
		{
			string internationalStandardBookNumber = _fixture.Create<string>();
			IBookDataCommand sut = CreateSut(internationalStandardBookNumber: internationalStandardBookNumber);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, internationalStandardBookNumber) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.InternationalStandardBookNumberPattern) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "InternationalStandardBookNumber") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenPublishedWasSetOnBookDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithPublished()
		{
			short published = _fixture.Create<short>();
			IBookDataCommand sut = CreateSut(hasPublished: true, published: published);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == published),
					It.Is<int>(v => v == 1000),
					It.Is<int>(v => v == 9999),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenPublishedWasNotSetOnBookDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithPublished()
		{
			IBookDataCommand sut = CreateSut(hasPublished: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			string url = $"https://localhost/api/book/{_fixture.Create<string>()}";
			IBookDataCommand sut = CreateSut(url: url);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			string url = $"https://localhost/api/book/{_fixture.Create<string>()}";
			IBookDataCommand sut = CreateSut(url: url);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			string url = $"https://localhost/api/book/{_fixture.Create<string>()}";
			IBookDataCommand sut = CreateSut(url: url);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IBookDataCommand sut = CreateSut(image: image);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IBookDataCommand sut = CreateSut(image: image);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithAuthors()
		{
			Guid[] authors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IBookDataCommand sut = CreateSut(authors: authors);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v.All(mediaPersonalityIdentifier => authors.Contains(mediaPersonalityIdentifier))),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Authors") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithAuthors()
		{
			Guid[] authors = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			IBookDataCommand sut = CreateSut(authors: authors);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			foreach (Guid mediaPersonalityIdentifier in authors)
			{
				_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
						It.Is<Guid>(v => v == mediaPersonalityIdentifier),
						It.IsNotNull<Func<Guid, Task<bool>>>(),
						It.Is<Type>(v => v != null && v == sut.GetType()),
						It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Authors") == 0),
						It.Is<bool>(v => v == false)),
					Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IBookDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArgument()
		{
			IBookDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IBookDataCommand CreateSut(string title = null, string subtitle = null, string description = null, string details = null, int? bookGenreIdentifier = null, bool hasWrittenLanguageIdentifier = true, int? writtenLanguageIdentifier = null, int? mediaTypeIdentifier = null, string internationalStandardBookNumber = null, bool hasPublished = true, short? published = null, string url = null, byte[] image = null, IEnumerable<Guid> authors = null)
		{
			_claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
				.Returns(_fixture.Create<bool>());

			return new MyBookDataCommand(Guid.NewGuid(), title ?? _fixture.Create<string>(), subtitle ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), description ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), details ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), bookGenreIdentifier ?? _fixture.Create<int>(), hasWrittenLanguageIdentifier ? writtenLanguageIdentifier ?? _fixture.Create<int>() : null, mediaTypeIdentifier ?? _fixture.Create<int>(), internationalStandardBookNumber ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), hasPublished ? published ?? _fixture.Create<short>() : null, url ?? (_random.Next(100) > 50 ? $"https://localhost/api/book/{_fixture.Create<string>()}" : null), image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()), authors ?? _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray());
		}

		private class MyBookDataCommand : BusinessLogic.MediaLibrary.Commands.BookDataCommandBase
		{
			#region Constructor

			public MyBookDataCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int bookGenreIdentifier, int? writtenLanguageIdentifier, int mediaTypeIdentifier, string internationalStandardBookNumber, short? published, string url, byte[] image, IEnumerable<Guid> authors) 
				: base(mediaIdentifier, title, subtitle, description, details, bookGenreIdentifier, writtenLanguageIdentifier, mediaTypeIdentifier, internationalStandardBookNumber, published, url, image, authors)
			{
			}

			#endregion
		}
	}
}