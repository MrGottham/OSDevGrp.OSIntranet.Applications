using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateReturnedDateTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenValidatorIsNull_ThrowsArgumentNullException(bool hasValue)
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateReturnedDate(null, hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * - 1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenValidatingTypeIsNull_ThrowsArgumentNullException(bool hasValue)
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateReturnedDate(hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * -1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenValidatingFieldIsNull_ThrowsArgumentNullException(bool hasValue)
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateReturnedDate(hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * -1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException(bool hasValue)
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateReturnedDate(hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * -1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException(bool hasValue)
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateReturnedDate(hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * -1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateReturnedDate_WhenValueIsSet_AssertDateTimeWasCalledOnValidatorTwoTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateReturnedDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.DateTime, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateReturnedDate_WhenValueIsSet_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			DateTime value = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateReturnedDate(value, DateTime.Today.AddDays(_random.Next(7, 14) * -1), validatingType, validatingField);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == value),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateReturnedDate_WhenValueIsSet_AssertShouldBeLaterThanOrEqualToOffsetDateWasCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			DateTime value = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime lendingDate = DateTime.Today.AddDays(_random.Next(7, 14) * -1);
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateReturnedDate(value, lendingDate, validatingType, validatingField);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOrEqualToOffsetDate(
					It.Is<DateTime>(v => v.Date == value),
					It.Is<DateTime>(v => v.Date == lendingDate),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateReturnedDate_WhenValueIsNotSet_AssertDateTimeWasNotCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateReturnedDate(null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.DateTime, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateReturnedDate_WhenValueIsNotSet_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateReturnedDate(null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.IsAny<Type>(),
					It.IsAny<string>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateReturnedDate_WhenValueIsNotSet_AssertShouldBeLaterThanOrEqualToOffsetDateWasNotCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateReturnedDate(null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeLaterThanOrEqualToOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.IsAny<Type>(),
					It.IsAny<string>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenCalled_ReturnsNotNull(bool hasValue)
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateReturnedDate(hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * -1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void ValidateReturnedDate_WhenCalled_ReturnsSameValidator(bool hasValue)
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateReturnedDate(hasValue ? DateTime.Today.AddDays(_random.Next(0, 7) * -1) : null, DateTime.Today.AddDays(_random.Next(7, 14) * -1), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}