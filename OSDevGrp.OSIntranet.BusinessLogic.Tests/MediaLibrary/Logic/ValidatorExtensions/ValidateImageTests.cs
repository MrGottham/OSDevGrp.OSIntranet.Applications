using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateImageTests
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
		public void ValidateImage_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateImage(null, _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalledWithValueWhichContainsBytes_AssertEnumerableWasCalledOnValidatorTwoTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Enumerable, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalledWithValueWhichContainsBytes_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			byte[] value = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateImage(value, validatingType, validatingField);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(value)) == 0),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalledWithValueWhichContainsBytes_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			byte[] value = _fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateImage(value, validatingType, validatingField);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && string.CompareOrdinal(Convert.ToBase64String(v.ToArray()), Convert.ToBase64String(value)) == 0),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalledWithValueWhichDoesNotContainBytes_AssertEnumerableWasCalledOnValidatorTwoTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateImage(Array.Empty<byte>(), _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Enumerable, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalledWithValueWhichDoesNotContainBytes_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateImage(Array.Empty<byte>(), validatingType, validatingField);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v != null && v.Any() == false),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalledWithValueWhichDoesNotContainBytes_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateImage(Array.Empty<byte>(), validatingType, validatingField);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v != null && v.Any() == false),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValueIsNull_AssertEnumerableWasCalledOnValidatorTwoTimes()
		{
			IValidator sut = CreateSut();

			sut.ValidateImage(null, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Enumerable, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValueIsNull_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateImage(null, validatingType, validatingField);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
					It.Is<IEnumerable<byte>>(v => v == null),
					It.Is<int>(v => v == 0),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenValueIsNull_AssertShouldHaveMaxItemsWasCalledOnEnumerableValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateImage(null, validatingType, validatingField);

			_validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMaxItems(
					It.Is<IEnumerable<byte>>(v => v == null),
					It.Is<int>(v => v == 32768),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateImage_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateImage(_fixture.CreateMany<byte>(_random.Next(1024, 4096)).ToArray(), _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}