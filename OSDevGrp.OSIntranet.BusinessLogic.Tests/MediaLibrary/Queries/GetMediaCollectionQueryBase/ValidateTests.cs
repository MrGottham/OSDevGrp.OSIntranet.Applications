using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Queries.GetMediaCollectionQueryBase
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
			IMediaLibraryFilterQuery sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _mediaLibraryRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryFilterQuery sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertStringWasCalledOnValidator()
		{
			IMediaLibraryFilterQuery sut = CreateSut();

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
			IMediaLibraryFilterQuery sut = CreateSut(filter);

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
			IMediaLibraryFilterQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == 256),
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
			IMediaLibraryFilterQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == filter.ToUpper()),
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
			IMediaLibraryFilterQuery sut = CreateSut(filter);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == filter.ToUpper()),
					It.Is<int>(value => value == 256),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IMediaLibraryFilterQuery sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArguments()
		{
			IMediaLibraryFilterQuery sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IMediaLibraryFilterQuery CreateSut(string filter = null)
		{
			return new MyGetMediaCollectionQuery(filter);
		}

		private class MyGetMediaCollectionQuery : BusinessLogic.MediaLibrary.Queries.GetMediaCollectionQueryBase
		{
			#region Constructor

			public MyGetMediaCollectionQuery(string filter)
				: base(filter)
			{
			}

			#endregion
		}
	}
}