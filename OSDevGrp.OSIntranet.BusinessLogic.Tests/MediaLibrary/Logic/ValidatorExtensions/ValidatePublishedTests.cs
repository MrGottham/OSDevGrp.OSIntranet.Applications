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
	public class ValidatePublishedTests
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
		public void ValidatePublished_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidatePublished(null, _random.Next(100) > 50 ? _fixture.Create<short>() : null, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidatePublished(_random.Next(100) > 50 ? _fixture.Create<short>() : null, null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidatePublished(_random.Next(100) > 50 ? _fixture.Create<short>() : null, _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidatePublished(_random.Next(100) > 50 ? _fixture.Create<short>() : null, _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidatePublished(_random.Next(100) > 50 ? _fixture.Create<short>() : null, _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasGiven_AssertIntegerWasCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidatePublished(_fixture.Create<short>(), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Integer, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasGiven_AssertShouldBeBetweenWasCalledOnIntegerValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			short value = _fixture.Create<short>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidatePublished(value, validatingType, validatingField);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == value),
					It.Is<int>(v => v == 1000),
					It.Is<int>(v => v == 9999),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasNotGiven_AssertIntegerWasNotCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidatePublished(null, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Integer, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasNotGiven_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidatePublished(null, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<int>(),
					It.IsAny<Type>(),
					It.IsAny<string>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasGiven_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidatePublished(_fixture.Create<short>(), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasGiven_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidatePublished(_fixture.Create<short>(), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasNotGiven_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidatePublished(null, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidatePublished_WhenValueWasNotGiven_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidatePublished(null, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}