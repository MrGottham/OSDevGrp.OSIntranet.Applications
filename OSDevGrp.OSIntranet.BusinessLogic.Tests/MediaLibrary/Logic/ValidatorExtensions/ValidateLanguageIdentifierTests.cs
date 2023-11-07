using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateLanguageIdentifierTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateLanguageIdentifier(null, _fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLanguageIdentifier(_fixture.Create<int>(), null, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasGiven_AssertIntegerWasCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Integer, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasGiven_AssertShouldBeBetweenWasCalledOnIntegerValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			int value = _fixture.Create<int>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateLanguageIdentifier(value, _commonRepositoryMock.Object, validatingType, validatingField);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == value),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 99),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasGiven_AssertObjectWasCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasGiven_AssertShouldBeKnownValueWasCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			int value = _fixture.Create<int>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateLanguageIdentifier(value, _commonRepositoryMock.Object, validatingType, validatingField);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<int>(v => v == value),
					It.IsNotNull<Func<int, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasNotGiven_AssertIntegerWasNotCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateLanguageIdentifier(null, _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Integer, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasNotGiven_AssertShouldBeBetweenWasNotCalledOnIntegerValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateLanguageIdentifier(null, _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

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
		public void ValidateLanguageIdentifier_WhenValueWasNotGiven_AssertObjectWasNotCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateLanguageIdentifier(null, _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasNotGiven_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateLanguageIdentifier(null, _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<int>(),
					It.IsAny<Func<int, Task<bool>>>(),
					It.IsAny<Type>(),
					It.IsAny<string>(),
					It.IsAny<bool>()),
				Times.Never());
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasGiven_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasGiven_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLanguageIdentifier(_fixture.Create<int>(), _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasNotGiven_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLanguageIdentifier(null, _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateLanguageIdentifier_WhenValueWasNotGiven_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateLanguageIdentifier(null, _commonRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}