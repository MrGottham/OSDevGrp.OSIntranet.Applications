﻿using AutoFixture;
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
	public class ValidateMediaIdentifierTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateMediaIdentifier(null, Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaIdentifier(Guid.NewGuid(), null, _fixture.Create<Type>(), _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, null, _fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingType"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), " "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validatingField"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenCalled_AssertObjectWasCalledOnValidator()
		{
			IValidator sut = CreateSut();

			sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			_validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorFromValidator()
		{
			IValidator sut = CreateSut();

			Guid value = Guid.NewGuid();
			Type validatingType = _fixture.Create<Type>();
			string validatingField = _fixture.Create<string>();
			sut.ValidateMediaIdentifier(value, _mediaLibraryRepositoryMock.Object, validatingType, validatingField);

			_validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
					It.Is<Guid>(v => v == value),
					It.IsNotNull<Func<Guid, Task<bool>>>(),
					It.Is<Type>(v => v != null && v == validatingType),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, validatingField) == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateMediaIdentifier_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateMediaIdentifier(Guid.NewGuid(), _mediaLibraryRepositoryMock.Object, _fixture.Create<Type>(), _fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}
	}
}