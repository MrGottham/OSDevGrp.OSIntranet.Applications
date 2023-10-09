using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateLendingDataTests
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
		public void ValidateLendingData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateLendingData(null, CreateLendingDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenLendingDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingData(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("lendingData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingData(CreateLendingDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingData(CreateLendingDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertBorrowerIdentifierWasCalledOnLendingDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			sut.ValidateLendingData(lendingDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			lendingDataCommandMock.Verify(m => m.BorrowerIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithBorrowerIdentifier()
		{
			IValidator sut = CreateSut();

			Guid borrowerIdentifier = Guid.NewGuid();
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(borrowerIdentifier: borrowerIdentifier);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(v => v == borrowerIdentifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BorrowerIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertMediaIdentifierWasCalledOnLendingDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			sut.ValidateLendingData(lendingDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			lendingDataCommandMock.Verify(m => m.MediaIdentifier, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaIdentifier()
		{
			IValidator sut = CreateSut();

			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(mediaIdentifier: mediaIdentifier);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(v => v == mediaIdentifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertLendingDateWasCalledOnLendingDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			sut.ValidateLendingData(lendingDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			lendingDataCommandMock.Verify(m => m.LendingDate, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithLendingDate()
		{
			IValidator sut = CreateSut();

			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(lendingDate: lendingDate);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "LendingDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertShouldBeEarlierThanOffsetDateWasCalledOnDateTimeValidatorWithLendingDate()
		{
			IValidator sut = CreateSut();

			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime recallDate = DateTime.Today.AddDays(_random.Next(7, 14));
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(lendingDate: lendingDate, recallDate: recallDate);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<DateTime>(v => v.Date == recallDate),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "LendingDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertRecallDateWasCalledOnLendingDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			sut.ValidateLendingData(lendingDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			lendingDataCommandMock.Verify(m => m.RecallDate, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertShouldBeLaterThanOffsetDateWasCalledOnDateTimeValidatorWithRecallDate()
		{
			IValidator sut = CreateSut();

			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime recallDate = DateTime.Today.AddDays(_random.Next(7, 14));
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(lendingDate: lendingDate, recallDate: recallDate);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOffsetDate(
					It.Is<DateTime>(v => v.Date == recallDate),
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "RecallDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_AssertReturnedDateWasCalledOnLendingDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<ILendingDataCommand> lendingDataCommandMock = CreateLendingDataCommandMock();
			sut.ValidateLendingData(lendingDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			lendingDataCommandMock.Verify(m => m.ReturnedDate, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenReturnedDateIsSetOnLendingDataCommand_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithReturnedDate()
		{
			IValidator sut = CreateSut();

			DateTime returnedDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(hasReturnedDate: true, returnedDate: returnedDate);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == returnedDate),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenReturnedDateIsSetOnLendingDataCommand_AssertShouldBeLaterThanOrEqualToOffsetDateWasCalledOnDateTimeValidatorWithReturnedDate()
		{
			IValidator sut = CreateSut();

			DateTime returnedDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(7, 14) * -1);
			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(lendingDate: lendingDate, hasReturnedDate: true, returnedDate: returnedDate);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOrEqualToOffsetDate(
					It.Is<DateTime>(v => v.Date == returnedDate),
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenReturnedDateIsNotSetOnLendingDataCommand_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorWithReturnedDate()
		{
			IValidator sut = CreateSut();

			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(hasReturnedDate: false);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenReturnedDateIsNotSetOnLendingDataCommand_AssertShouldBeLaterThanOrEqualToOffsetDateWasNotCalledOnDateTimeValidatorWithReturnedDate()
		{
			IValidator sut = CreateSut();

			ILendingDataCommand lendingDataCommand = CreateLendingDataCommand(hasReturnedDate: false);
			sut.ValidateLendingData(lendingDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOrEqualToOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == lendingDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLendingData(CreateLendingDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLendingData(CreateLendingDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private ILendingDataCommand CreateLendingDataCommand(Guid? borrowerIdentifier = null, Guid? mediaIdentifier = null, DateTime? lendingDate = null, DateTime? recallDate = null, bool? hasReturnedDate = null, DateTime? returnedDate = null)
		{
			return CreateLendingDataCommandMock(borrowerIdentifier, mediaIdentifier, lendingDate, recallDate, hasReturnedDate, returnedDate).Object;
		}

		private Mock<ILendingDataCommand> CreateLendingDataCommandMock(Guid? borrowerIdentifier = null, Guid? mediaIdentifier = null, DateTime? lendingDate = null, DateTime? recallDate = null, bool? hasReturnedDate = null, DateTime? returnedDate = null)
		{
			lendingDate ??= DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			recallDate ??= lendingDate.Value.AddDays(_random.Next(7, 14));
			returnedDate = hasReturnedDate ?? _random.Next(100) > 50
				? returnedDate ?? lendingDate.Value.AddDays(_random.Next(7, 14))
				: null;

			Mock<ILendingDataCommand> lendingDataCommandMock = new Mock<ILendingDataCommand>();
			lendingDataCommandMock.Setup(m => m.BorrowerIdentifier)
				.Returns(borrowerIdentifier ?? Guid.NewGuid());
			lendingDataCommandMock.Setup(m => m.MediaIdentifier)
				.Returns(mediaIdentifier ?? Guid.NewGuid());
			lendingDataCommandMock.Setup(m => m.LendingDate)
				.Returns(lendingDate.Value.Date);
			lendingDataCommandMock.Setup(m => m.RecallDate)
				.Returns(recallDate.Value.Date);
			lendingDataCommandMock.Setup(m => m.ReturnedDate)
				.Returns(returnedDate?.Date);
			return lendingDataCommandMock;
		}
	}
}