using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateMediaDataTests
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
		public void ValidateMediaData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateMediaData(null, CreateMediaDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenMediaDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaData<IMedia>(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaData(CreateMediaDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaData(CreateMediaDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertTitleWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Title, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(title);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(title);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithTitle()
		{
			IValidator sut = CreateSut();

			string title = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(title);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, title) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Title") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertSubtitleWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Subtitle, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(subtitle: subtitle);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSubtitle()
		{
			IValidator sut = CreateSut();

			string subtitle = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(subtitle: subtitle);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, subtitle) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Subtitle") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertDescriptionWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Description, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(description: description);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDescription()
		{
			IValidator sut = CreateSut();

			string description = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(description: description);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, description) == 0),
					It.Is<int>(v => v == 512),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Description") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertDetailsWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Details, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(details: details);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithDetails()
		{
			IValidator sut = CreateSut();

			string details = _fixture.Create<string>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(details: details);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, details) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Details") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertMediaTypeIdentifierWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.MediaTypeIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaTypeIdentifier()
		{
			IValidator sut = CreateSut();

			int mediaTypeIdentifier = _fixture.Create<int>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(mediaTypeIdentifier: mediaTypeIdentifier);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == mediaTypeIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaTypeIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenPublishedWasSetOnMediaDataCommand_AssertPublishedWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock(hasPublished: true);
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenPublishedWasSetOnMediaDataCommand_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			short published = _fixture.Create<short>();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(hasPublished: true, published: published);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == published),
					It.Is<int>(v => v == 1000),
					It.Is<int>(v => v == 9999),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenPublishedWasNotSetOnMediaDataCommand_AssertPublishedWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock(hasPublished: false);
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Published, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenPublishedWasNotSetOnMediaDataCommand_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorWithPublished()
		{
			IValidator sut = CreateSut();

			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(hasPublished: false);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Published") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertUrlWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Url, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = $"https://localhost/api/media/{_fixture.Create<string>()}";
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(url: url);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = $"https://localhost/api/media/{_fixture.Create<string>()}";
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(url: url);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = $"https://localhost/api/media/{_fixture.Create<string>()}";
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(url: url);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertImageWasCalledOnMediaDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = CreateMediaDataCommandMock();
			sut.ValidateMediaData(mediaDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaDataCommandMock.Verify(m => m.Image, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(image: image);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaDataCommand<IMedia> mediaDataCommand = CreateMediaDataCommand(image: image);
			sut.ValidateMediaData(mediaDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == mediaDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaData(CreateMediaDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaData(CreateMediaDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private IMediaDataCommand<IMedia> CreateMediaDataCommand(string title = null, string subtitle = null, string description = null, string details = null, int? mediaTypeIdentifier = null, bool hasPublished = true, short? published = null, string url = null, byte[] image = null)
		{
			return CreateMediaDataCommandMock(title, subtitle, description, details, mediaTypeIdentifier, hasPublished, published, url, image).Object;
		}

		private Mock<IMediaDataCommand<IMedia>> CreateMediaDataCommandMock(string title = null, string subtitle = null, string description = null, string details = null, int? mediaTypeIdentifier = null, bool hasPublished = true, short? published = null, string url = null, byte[] image = null)
		{
			Mock<IMediaDataCommand<IMedia>> mediaDataCommandMock = new Mock<IMediaDataCommand<IMedia>>();
			mediaDataCommandMock.Setup(m => m.Title)
				.Returns(title ?? _fixture.Create<string>());
			mediaDataCommandMock.Setup(m => m.Subtitle)
				.Returns(subtitle ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			mediaDataCommandMock.Setup(m => m.Description)
				.Returns(description ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			mediaDataCommandMock.Setup(m => m.Details)
				.Returns(details ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			mediaDataCommandMock.Setup(m => m.MediaTypeIdentifier)
				.Returns(mediaTypeIdentifier ?? _fixture.Create<int>());
			mediaDataCommandMock.Setup(m => m.Published)
				.Returns(hasPublished ? published ?? _fixture.Create<short>() : null);
			mediaDataCommandMock.Setup(m => m.Url)
				.Returns(url ?? (_random.Next(100) > 50 ? $"https://localhost/api/media/{_fixture.Create<string>()}" : null));
			mediaDataCommandMock.Setup(m => m.Image)
				.Returns(image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			return mediaDataCommandMock;
		}
	}
}