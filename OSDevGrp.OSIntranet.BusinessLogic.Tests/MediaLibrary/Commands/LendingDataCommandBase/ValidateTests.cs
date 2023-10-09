using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.LendingDataCommandBase
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
			ILendingDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			ILendingDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			ILendingDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			ILendingDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertIsMediaLibraryLenderWasCalledOnClaimResolver()
		{
			ILendingDataCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_claimResolverMock.Verify(m => m.IsMediaLibraryLender(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithBorrowerIdentifier()
		{
			Guid borrowerIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(borrowerIdentifier: borrowerIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(v => v == borrowerIdentifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "BorrowerIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorWithMediaIdentifier()
		{
			Guid mediaIdentifier = Guid.NewGuid();
			ILendingDataCommand sut = CreateSut(mediaIdentifier: mediaIdentifier);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(v => v == mediaIdentifier),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MediaIdentifier") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithLendingDate()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "LendingDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeEarlierThanOffsetDateWasCalledOnDateTimeValidatorWithLendingDate()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime recallDate = DateTime.Today.AddDays(_random.Next(7, 14));
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate, recallDate: recallDate);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<DateTime>(v => v.Date == recallDate),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "LendingDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeLaterThanOffsetDateWasCalledOnDateTimeValidatorWithRecallDate()
		{
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime recallDate = DateTime.Today.AddDays(_random.Next(7, 14));
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate, recallDate: recallDate);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOffsetDate(
					It.Is<DateTime>(v => v.Date == recallDate),
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "RecallDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenReturnedDateIsSetOnLendingDataCommand_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorWithReturnedDate()
		{
			DateTime returnedDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			ILendingDataCommand sut = CreateSut(hasReturnedDate: true, returnedDate: returnedDate);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == returnedDate),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenReturnedDateIsSetOnLendingDataCommand_AssertShouldBeLaterThanOrEqualToOffsetDateWasCalledOnDateTimeValidatorWithReturnedDate()
		{
			DateTime returnedDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(7, 14) * -1);
			ILendingDataCommand sut = CreateSut(lendingDate: lendingDate, hasReturnedDate: true, returnedDate: returnedDate);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOrEqualToOffsetDate(
					It.Is<DateTime>(v => v.Date == returnedDate),
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenReturnedDateIsNotSetOnLendingDataCommand_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorWithReturnedDate()
		{
			ILendingDataCommand sut = CreateSut(hasReturnedDate: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenReturnedDateIsNotSetOnLendingDataCommand_AssertShouldBeLaterThanOrEqualToOffsetDateWasNotCalledOnDateTimeValidatorWithReturnedDate()
		{
			ILendingDataCommand sut = CreateSut(hasReturnedDate: false);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOrEqualToOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "ReturnedDate") == 0)),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			ILendingDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArguments()
		{
			ILendingDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private ILendingDataCommand CreateSut(Guid? borrowerIdentifier = null, Guid? mediaIdentifier = null, DateTime? lendingDate = null, DateTime? recallDate = null, bool? hasReturnedDate = null, DateTime? returnedDate = null)
		{
			lendingDate ??= DateTime.Today.AddDays(_random.Next(30, 365) * -1);
			recallDate ??= lendingDate.Value.AddDays(_random.Next(7, 14));
			returnedDate = hasReturnedDate ?? _random.Next(100) > 50
				? returnedDate ?? lendingDate.Value.AddDays(_random.Next(7, 14))
				: null;

			_claimResolverMock.Setup(m => m.IsMediaLibraryLender())
				.Returns(_fixture.Create<bool>());

			return new MyLendingDataCommandBase(Guid.NewGuid(), borrowerIdentifier ?? Guid.NewGuid(), mediaIdentifier ?? Guid.NewGuid(), lendingDate.Value.Date, recallDate.Value.Date, returnedDate?.Date);
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