using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateInternationalStandardBookNumberTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_fixture = new Fixture();
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase(null)]
		[TestCase("ISBN 978-0-596-52068-7")]
		[TestCase("ISBN-13: 978-0-596-52068-7")]
		[TestCase("978 0 596 52068 7")]
		[TestCase("9780596520687")]
		[TestCase("ISBN-10 0-596-52068-9")]
		[TestCase("0-596-52068-9")]
		[TestCase("87-02-04879-5")]
		[TestCase("ISBN 87-02-04879-5")]
		[TestCase("978-87-400-2445-6")]
		[TestCase("ISBN 978-87-400-2445-6")]
		[TestCase("978-87-400-2445-6")]
		[TestCase("ISBN 978-87-400-2445-6")]
		[TestCase("978-87-02-12882-6")]
		[TestCase("ISBN 978-87-02-12882-6")]
		public void ValidateInternationalStandardBookNumber_WhenValueMatchesInternationalStandardBookNumber_ReturnsStringValidator(string value)
		{
			IServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddBusinessLogicValidators();

			using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

			IValidator validator = serviceProvider.GetRequiredService<IValidator>();

			IValidator result = validator.ValidateInternationalStandardBookNumber(value, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.TypeOf<StringValidator>());
		}

		[Test]
		[Category("IntegrationTest")]
		[TestCase("", ErrorCode.ValueShouldHaveMinLength)]
		[TestCase(" ", ErrorCode.ValueShouldMatchPattern)]
		[TestCase("  ", ErrorCode.ValueShouldMatchPattern)]
		[TestCase("   ", ErrorCode.ValueShouldMatchPattern)]
		[TestCase("XYZ", ErrorCode.ValueShouldMatchPattern)]
		public void ValidateInternationalStandardBookNumber_WhenValueDoesNotMatchInternationalStandardBookNumber_ReturnsStringValidator(string value, ErrorCode expectedErrorCode)
		{
			IServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddBusinessLogicValidators();

			using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

			IValidator validator = serviceProvider.GetRequiredService<IValidator>();

			IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => validator.ValidateInternationalStandardBookNumber(value, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ErrorCode, Is.EqualTo(expectedErrorCode));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateInternationalStandardBookNumber(null, _fixture.Create<string>(), _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithValue_AssertStringWasCalledOnValidatorThreeTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(3));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithValue_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			string value = _fixture.Create<string>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateInternationalStandardBookNumber(value, validatingType, validatingField);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, value) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithValue_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			string value = _fixture.Create<string>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateInternationalStandardBookNumber(value, validatingType, validatingField);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, value) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithValue_AssertShouldMatchPatternWasCalledOnStringValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			string value = _fixture.Create<string>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateInternationalStandardBookNumber(value, validatingType, validatingField);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, value) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.InternationalStandardBookNumberPattern) == 0),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithoutValue_AssertStringWasCalledOnValidatorThreeTimes(string value)
		{
			IValidator sut = CreateSut();

			sut.ValidateInternationalStandardBookNumber(value, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(3));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithoutValue_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator(string value)
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateInternationalStandardBookNumber(value, validatingType, validatingField);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v)),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithoutValue_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator(string value)
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateInternationalStandardBookNumber(value, validatingType, validatingField);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v)),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public void ValidateInternationalStandardBookNumber_WhenCalledWithoutValue_AssertShouldMatchPatternWasCalledOnStringValidatorFromValidator(string value)
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateInternationalStandardBookNumber(value, validatingType, validatingField);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) && string.CompareOrdinal(v, value) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.InternationalStandardBookNumberPattern) == 0),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateInternationalStandardBookNumber_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateInternationalStandardBookNumber(_fixture.Create<string>(), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}