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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.MediaPersonalityDataCommandBase
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
			IMediaPersonalityDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithGivenName()
		{
			string givenName = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(givenName: givenName);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, givenName) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "GivenName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithGivenName()
		{
			string givenName = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(givenName: givenName);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, givenName) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "GivenName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithMiddleName()
		{
			string middleName = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(middleName: middleName);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, middleName) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MiddleName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithMiddleName()
		{
			string middleName = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(middleName: middleName);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, middleName) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MiddleName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithSurname()
		{
			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(surname: surname);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, surname) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Surname") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSurname()
		{
			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(surname: surname);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, surname) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Surname") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSurname()
		{
			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand sut = CreateSut(surname: surname);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, surname) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Surname") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithNationalityIdentifier()
		{
			int nationalityIdentifier = _fixture.Create<int>();
			IMediaPersonalityDataCommand sut = CreateSut(nationalityIdentifier: nationalityIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == nationalityIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "NationalityIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithNationalityIdentifier()
		{
			int nationalityIdentifier = _fixture.Create<int>();
			IMediaPersonalityDataCommand sut = CreateSut(nationalityIdentifier: nationalityIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == nationalityIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "NationalityIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithBirthDate()
		{
			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: true, birthDate: birthDate);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == birthDate),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateMediaPersonalityData_WhenBirthDateIsSetOnMediaPersonalityDataCommand_AssertShouldBeEarlierThanOffsetDateWasCalledOnDateTimeValidatorWithBirthDate(bool hasDateOfDead)
		{
			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: true, birthDate: birthDate, hasDateOfDead: hasDateOfDead, dateOfDead: dateOfDead);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.Is<DateTime>(v => v.Date == birthDate),
					It.Is<DateTime>(v => v.Date == (hasDateOfDead ? dateOfDead : DateTime.Today)),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsNotSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorWithBirthDate()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsNotSetOnMediaPersonalityDataCommand_AssertShouldBeEarlierThanOffsetDateWasNotCalledOnDateTimeValidatorWithBirthDate()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithDateOfDead()
		{
			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand sut = CreateSut(hasDateOfDead: true, dateOfDead: dateOfDead);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == dateOfDead),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_AssertShouldBeLaterThanOffsetDateWasCalledOnDateTimeValidatorWithDateOfDead(bool hasBirthdate)
		{
			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand sut = CreateSut(hasBirthDate: hasBirthdate, birthDate: birthDate, hasDateOfDead: true, dateOfDead: dateOfDead);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOffsetDate(
					It.Is<DateTime>(v => v.Date == dateOfDead),
					It.Is<DateTime>(v => v.Date == (hasBirthdate ? birthDate : DateTime.MinValue.ToUniversalTime())),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsNotSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorWithDateOfDead()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasDateOfDead: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsNotSetOnMediaPersonalityDataCommand_AssertShouldBeLaterThanOffsetDateWasNotCalledOnDateTimeValidatorWithDateOfDead()
		{
			IMediaPersonalityDataCommand sut = CreateSut(hasDateOfDead: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			string url = $"https://localhost/api/mediapersonality/{_fixture.Create<string>()}";
			IMediaPersonalityDataCommand sut = CreateSut(url: url);

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
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			string url = $"https://localhost/api/mediapersonality/{_fixture.Create<string>()}";
			IMediaPersonalityDataCommand sut = CreateSut(url: url);

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
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			string url = $"https://localhost/api/mediapersonality/{_fixture.Create<string>()}";
			IMediaPersonalityDataCommand sut = CreateSut(url: url);

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
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaPersonalityDataCommand sut = CreateSut(image: image);

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
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaPersonalityDataCommand sut = CreateSut(image: image);

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
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArgument()
		{
			IMediaPersonalityDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IMediaPersonalityDataCommand CreateSut(string givenName = null, string middleName = null, string surname = null, int? nationalityIdentifier = null, bool hasBirthDate = true, DateTime? birthDate = null, bool hasDateOfDead = true, DateTime? dateOfDead = null, string url = null, byte[] image = null)
		{
			_claimResolverMock.Setup(m => m.IsMediaLibraryModifier())
				.Returns(_fixture.Create<bool>());

			return new MyMediaPersonalityDataCommand(Guid.NewGuid(), givenName ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), middleName ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), surname ?? _fixture.Create<string>(), nationalityIdentifier ?? _fixture.Create<int>(), hasBirthDate ? birthDate ?? DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365)) : null, hasDateOfDead ? dateOfDead ?? DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365)) : null, url ?? (_random.Next(100) > 50 ? $"https://localhost/api/mediapersonality/{_fixture.Create<string>()}" : null), image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
		}

		private class MyMediaPersonalityDataCommand : BusinessLogic.MediaLibrary.Commands.MediaPersonalityDataCommandBase
		{
			#region Constructor

			public MyMediaPersonalityDataCommand(Guid mediaIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image) 
				: base(mediaIdentifier, givenName, middleName, surname, nationalityIdentifier, birthDate, dateOfDead, url, image)
			{
			}

			#endregion
		}
	}
}