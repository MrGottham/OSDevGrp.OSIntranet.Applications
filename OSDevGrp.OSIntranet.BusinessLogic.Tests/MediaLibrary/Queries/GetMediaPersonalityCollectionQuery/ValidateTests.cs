﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Queries.GetMediaPersonalityCollectionQuery
{
	[TestFixture]
	public class ValidateTests
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
		public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _mediaLibraryRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertStringWasCalledOnValidator()
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(2));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public void Validate_WhenNoFilterWasGiven_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator(string filter)
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == 1),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(null)]
		[TestCase("")]
		[TestCase(" ")]
		[TestCase("  ")]
		public void Validate_WhenNoFilterWasGiven_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator(string filter)
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == 32),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenFilterWasGiven_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator()
		{
			string filter = _fixture.Create<string>();
			IGetMediaPersonalityCollectionQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == filter),
					It.Is<int>(value => value == 1),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenFilterWasGiven_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator()
		{
			string filter = _fixture.Create<string>();
			IGetMediaPersonalityCollectionQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == filter),
					It.Is<int>(value => value == 32),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArguments()
		{
			IGetMediaPersonalityCollectionQuery sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IGetMediaPersonalityCollectionQuery CreateSut(string filter = null)
		{
			return new BusinessLogic.MediaLibrary.Queries.GetMediaPersonalityCollectionQuery(filter);
		}
	}
}