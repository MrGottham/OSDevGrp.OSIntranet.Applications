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
	public class ValidateLendingDateTests
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
		public void ValidateLendingDate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateLendingDate(null, DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenCalled_AssertDateTimeWasCalledOnValidatorTwoTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.DateTime, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenCalled_AssertShouldBePastDateOrTodayWasCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			DateTime value = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateLendingDate(value, DateTime.Today.AddDays(_random.Next(7, 14)), validatingType, validatingField);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateOrToday(
					It.Is<DateTime>(v => v.Date == value),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenCalled_AssertShouldBeEarlierThanOffsetDateWasCalledOnDateTimeValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			DateTime value = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
			DateTime recallDate = DateTime.Today.AddDays(_random.Next(7, 14));
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateLendingDate(value, recallDate, validatingType, validatingField);

			_validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBeEarlierThanOffsetDate(
					It.Is<DateTime>(v => v.Date == value),
					It.Is<DateTime>(v => v.Date == recallDate),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLendingDate_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLendingDate(DateTime.Today.AddDays(_random.Next(0, 7) * -1), DateTime.Today.AddDays(_random.Next(7, 14)), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}