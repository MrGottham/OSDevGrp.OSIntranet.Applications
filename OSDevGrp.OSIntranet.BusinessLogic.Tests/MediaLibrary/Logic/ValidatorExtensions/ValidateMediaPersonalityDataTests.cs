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
	public class ValidateMediaPersonalityDataTests
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
		public void ValidateMediaPersonalityData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateMediaPersonalityData(null, CreateMediaPersonalityDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenMediaPersonalityDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityData(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaPersonalityData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityData(CreateMediaPersonalityDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityData(CreateMediaPersonalityDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertGivenNameWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.GivenName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithGivenName()
		{
			IValidator sut = CreateSut();

			string givenName = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(givenName: givenName);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, givenName) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "GivenName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithGivenName()
		{
			IValidator sut = CreateSut();

			string givenName = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(givenName: givenName);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, givenName) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "GivenName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertMiddleNameWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.MiddleName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithMiddleName()
		{
			IValidator sut = CreateSut();

			string middleName = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(middleName: middleName);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, middleName) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MiddleName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithMiddleName()
		{
			IValidator sut = CreateSut();

			string middleName = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(middleName: middleName);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, middleName) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MiddleName") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertSurnameWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.Surname, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithSurname()
		{
			IValidator sut = CreateSut();

			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(surname: surname);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, surname) == 0),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Surname") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSurname()
		{
			IValidator sut = CreateSut();

			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(surname: surname);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, surname) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Surname") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSurname()
		{
			IValidator sut = CreateSut();

			string surname = _fixture.Create<string>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(surname: surname);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, surname) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Surname") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertNationalityIdentifierWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.NationalityIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithNationalityIdentifier()
		{
			IValidator sut = CreateSut();

			int nationalityIdentifier = _fixture.Create<int>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(nationalityIdentifier: nationalityIdentifier);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == nationalityIdentifier),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "NationalityIdentifier") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithNationalityIdentifier()
		{
			IValidator sut = CreateSut();

			int nationalityIdentifier = _fixture.Create<int>();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(nationalityIdentifier: nationalityIdentifier);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == nationalityIdentifier),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "NationalityIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsSetOnMediaPersonalityDataCommand_AssertBirthDateWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock(hasBirthDate: true);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.BirthDate, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithBirthDate()
		{
			IValidator sut = CreateSut();

			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasBirthDate: true, birthDate: birthDate);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == birthDate),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateMediaPersonalityData_WhenBirthDateIsSetOnMediaPersonalityDataCommand_AssertShouldBeEarlierThanOffsetDateWasCalledOnDateTimeValidatorWithBirthDate(bool hasDateOfDead)
		{
			IValidator sut = CreateSut();

			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasBirthDate: true, birthDate: birthDate, hasDateOfDead: hasDateOfDead, dateOfDead: dateOfDead);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.Is<DateTime>(v => v.Date == birthDate),
					It.Is<DateTime>(v => v.Date == (hasDateOfDead ? dateOfDead : DateTime.Today)),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsNotSetOnMediaPersonalityDataCommand_AssertBirthDateWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock(hasBirthDate: false);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.BirthDate, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsNotSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorWithBirthDate()
		{
			IValidator sut = CreateSut();

			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasBirthDate: false);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenBirthDateIsNotSetOnMediaPersonalityDataCommand_AssertShouldBeEarlierThanOffsetDateWasNotCalledOnDateTimeValidatorWithBirthDate()
		{
			IValidator sut = CreateSut();

			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasBirthDate: false);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BirthDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_AssertDateOfDeadWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock(hasDateOfDead: true);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.DateOfDead, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithDateOfDead()
		{
			IValidator sut = CreateSut();

			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasDateOfDead: true, dateOfDead: dateOfDead);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == dateOfDead),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsSetOnMediaPersonalityDataCommand_AssertShouldBeLaterThanOffsetDateWasCalledOnDateTimeValidatorWithDateOfDead(bool hasBirthdate)
		{
			IValidator sut = CreateSut();

			DateTime birthDate = DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365));
			DateTime dateOfDead = DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365));
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasBirthDate: hasBirthdate, birthDate: birthDate, hasDateOfDead: true, dateOfDead: dateOfDead);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOffsetDate(
					It.Is<DateTime>(v => v.Date == dateOfDead),
					It.Is<DateTime>(v => v.Date == (hasBirthdate ? birthDate : DateTime.MinValue.ToUniversalTime())),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsNotSetOnMediaPersonalityDataCommand_AssertDateOfDeadWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock(hasDateOfDead: false);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.DateOfDead, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsNotSetOnMediaPersonalityDataCommand_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorWithDateOfDead()
		{
			IValidator sut = CreateSut();

			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasDateOfDead: false);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenDateOfDeadIsNotSetOnMediaPersonalityDataCommand_AssertShouldBeLaterThanOffsetDateWasNotCalledOnDateTimeValidatorWithDateOfDead()
		{
			IValidator sut = CreateSut();

			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(hasDateOfDead: false);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "DateOfDead") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertUrlWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.Url, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/mediapersonality/{_fixture.Create<string>()}");
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(url: url);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

            string url = _fixture.CreateEndpointString(path: $"api/mediapersonality/{_fixture.Create<string>()}");
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(url: url);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithUrl()
		{
			IValidator sut = CreateSut();

			string url = _fixture.CreateEndpointString(path: $"api/mediapersonality/{_fixture.Create<string>()}");
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(url: url);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, url) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.UrlRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Url") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertImageWasCalledOnMediaPersonalityDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = CreateMediaPersonalityDataCommandMock();
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			mediaPersonalityDataCommandMock.Verify(m => m.Image, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(image: image);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorWithImage()
		{
			IValidator sut = CreateSut();

			byte[] image = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			IMediaPersonalityDataCommand mediaPersonalityDataCommand = CreateMediaPersonalityDataCommand(image: image);
			sut.ValidateMediaPersonalityData(mediaPersonalityDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(image)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == mediaPersonalityDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "Image") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityData(CreateMediaPersonalityDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityData(CreateMediaPersonalityDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private IMediaPersonalityDataCommand CreateMediaPersonalityDataCommand(string givenName = null, string middleName = null, string surname = null, int? nationalityIdentifier = null, bool hasBirthDate = true, DateTime? birthDate = null, bool hasDateOfDead = true, DateTime? dateOfDead = null, string url = null, byte[] image = null)
		{
			return CreateMediaPersonalityDataCommandMock(givenName, middleName, surname, nationalityIdentifier, hasBirthDate, birthDate, hasDateOfDead, dateOfDead, url, image).Object;
		}

		private Mock<IMediaPersonalityDataCommand> CreateMediaPersonalityDataCommandMock(string givenName = null, string middleName = null, string surname = null, int? nationalityIdentifier = null, bool hasBirthDate = true, DateTime? birthDate = null, bool hasDateOfDead = true, DateTime? dateOfDead = null, string url = null, byte[] image = null)
		{
			Mock<IMediaPersonalityDataCommand> mediaPersonalityDataCommandMock = new Mock<IMediaPersonalityDataCommand>();
			mediaPersonalityDataCommandMock.Setup(m => m.GivenName)
				.Returns(givenName ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			mediaPersonalityDataCommandMock.Setup(m => m.MiddleName)
				.Returns(middleName ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			mediaPersonalityDataCommandMock.Setup(m => m.Surname)
				.Returns(surname ?? _fixture.Create<string>());
			mediaPersonalityDataCommandMock.Setup(m => m.NationalityIdentifier)
				.Returns(nationalityIdentifier ?? _fixture.Create<int>());
			mediaPersonalityDataCommandMock.Setup(m => m.BirthDate)
				.Returns(hasBirthDate ? birthDate ?? DateTime.Today.AddYears(_random.Next(25, 75) * -1).AddDays(_random.Next(0, 365)) : null);
			mediaPersonalityDataCommandMock.Setup(m => m.DateOfDead)
				.Returns(hasDateOfDead ? dateOfDead ?? DateTime.Today.AddYears(_random.Next(5, 10) * -1).AddDays(_random.Next(0, 365)) : null);
			mediaPersonalityDataCommandMock.Setup(m => m.Url)
				.Returns(url ?? (_random.Next(100) > 50 ? _fixture.CreateEndpointString(path: $"api/mediapersonality/{_fixture.Create<string>()}") : null));
			mediaPersonalityDataCommandMock.Setup(m => m.Image)
				.Returns(image ?? (_random.Next(100) > 50 ? _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray() : Array.Empty<byte>()));
			return mediaPersonalityDataCommandMock;
		}
	}
}