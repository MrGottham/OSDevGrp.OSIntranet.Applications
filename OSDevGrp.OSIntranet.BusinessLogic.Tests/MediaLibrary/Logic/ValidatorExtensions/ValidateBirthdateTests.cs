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
	public class ValidateBirthdateTests
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
		public void ValidateBirthdate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateBirthdate(null, DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsSet_AssertDateTimeWasCalledOnValidatorTwoTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.DateTime, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsSet_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			DateTime value = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateBirthdate(value, DateTime.Today.AddDays(_random.Next(0, 7)), validatingType, validatingField);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == value),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsSet_AssertShouldBeEarlierThanOffsetDateWasCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			DateTime value = DateTime.Today.AddDays(_random.Next(0, 365) * -1);
			DateTime maxValue = DateTime.Today.AddDays(_random.Next(0, 7));
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateBirthdate(value, maxValue, validatingType, validatingField);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.Is<DateTime>(v => v.Date == value),
					It.Is<DateTime>(v => v.Date == maxValue),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsNotSet_AssertDateTimeWasNotCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateBirthdate(null, DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.DateTime, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsNotSet_AssertShouldBePastDateOrTodayWasNotCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateBirthdate(null, DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.IsAny<DateTime>(),
					It.IsAny<Type>(),
					It.IsAny<string>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsNotSet_AssertShouldBeEarlierThanOffsetDateWasNotCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateBirthdate(null, DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.IsAny<DateTime>(),
					It.IsAny<DateTime>(),
					It.IsAny<Type>(),
					It.IsAny<string>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsSet_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsSet_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBirthdate(DateTime.Today.AddDays(_random.Next(0, 365) * -1), DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsNotSet_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBirthdate(null, DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBirthdate_WhenValueIsNotSet_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBirthdate(null, DateTime.Today.AddDays(_random.Next(0, 7)), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}