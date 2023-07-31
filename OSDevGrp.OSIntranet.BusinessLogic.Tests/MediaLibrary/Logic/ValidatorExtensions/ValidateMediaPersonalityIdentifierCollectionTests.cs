using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateMediaPersonalityIdentifierCollectionTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateMediaPersonalityIdentifierCollection(null, _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), null, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNull_AssertObjectWasCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateMediaPersonalityIdentifierCollection(null, _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNull_AssertShouldNotBeNullWasCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateMediaPersonalityIdentifierCollection(null, _mediaLibraryRepositoryMock.Object, validatingType, validatingField);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v == null),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNull_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateMediaPersonalityIdentifierCollection(null, _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<Guid>(),
					It.IsAny<Func<Guid, Task<bool>>>(),
					It.IsAny<Type>(),
					It.IsAny<string>(),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsEmpty_AssertObjectWasCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateMediaPersonalityIdentifierCollection(Array.Empty<Guid>(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsEmpty_AssertShouldNotBeNullWasCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Guid[] value = Array.Empty<Guid>();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateMediaPersonalityIdentifierCollection(value, _mediaLibraryRepositoryMock.Object, validatingType, validatingField);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v == value),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsEmpty_AssertShouldBeKnownValueWasNotCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateMediaPersonalityIdentifierCollection(Array.Empty<Guid>(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.IsAny<Guid>(),
					It.IsAny<Func<Guid, Task<bool>>>(),
					It.IsAny<Type>(),
					It.IsAny<string>(),
					It.IsAny<bool>()),
				Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNotEmpty_AssertObjectWasCalledOnValidatorMultipleTimes()
		{
			IValidator sut = CreateSut();

			Guid[] value = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			sut.ValidateMediaPersonalityIdentifierCollection(value, _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Exactly(1 + value.Length));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNotEmpty_AssertShouldNotBeNullWasCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Guid[] value = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateMediaPersonalityIdentifierCollection(value, _mediaLibraryRepositoryMock.Object, validatingType, validatingField);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
					It.Is<IEnumerable<Guid>>(v => v != null && v.All(mediaPersonalityIdentifier => value.Contains(mediaPersonalityIdentifier))),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsEmpty_AssertShouldBeKnownValueWasCalledOnObjectValidatorFromValidatorForEachMediaPersonalityIdentifier()
		{
			IValidator sut = CreateSut();

			Guid[] value = _fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateMediaPersonalityIdentifierCollection(value, _mediaLibraryRepositoryMock.Object, validatingType, validatingField);

			foreach (Guid mediaPersonalityIdentifier in value)
			{

				_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
						It.Is<Guid>(v => v == mediaPersonalityIdentifier),
						It.IsNotNull<Func<Guid, Task<bool>>>(),
						It.Is<Type>(v => v != null && v == validatingType),
						It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
						It.Is<bool>(v => v == false)),
					Times.Once);
			}
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNull_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityIdentifierCollection(null, _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNull_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityIdentifierCollection(null, _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsEmpty_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityIdentifierCollection(Array.Empty<Guid>(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsEmpty_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityIdentifierCollection(Array.Empty<Guid>(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNotEmpty_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaPersonalityIdentifierCollection_WhenValueIsNotEmpty_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaPersonalityIdentifierCollection(_fixture.CreateMany<Guid>(_random.Next(1, 7)).ToArray(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}