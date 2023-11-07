using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Queries.MediaLibraryFilterQueryBase
{
	[TestFixture]
	public class ValidateTests
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
		public void Validate_WhenFilterIsNullEmptyOrWhiteSpaceAndFilterShouldBeUpperCase_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator(string filter)
		{
			int minLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, true, minLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == minLength),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenFilterIsNotNullEmptyOrWhiteSpaceAndFilterShouldBeUpperCase_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator()
		{
			string filter = _fixture.Create<string>();
			int minLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, true, minLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == filter.ToUpper()),
					It.Is<int>(value => value == minLength),
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
		public void Validate_WhenFilterIsNullEmptyOrWhiteSpaceAndFilterShouldNotBeUpperCase_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator(string filter)
		{
			int minLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, false, minLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == minLength),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenFilterIsNotNullEmptyOrWhiteSpaceAndFilterShouldNotBeUpperCase_AssertShouldHaveMinLengthWasCalledOnStringValidatorFromValidator()
		{
			string filter = _fixture.Create<string>();
			int minLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, false, minLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(value => value == filter),
					It.Is<int>(value => value == minLength),
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
		public void Validate_WhenFilterIsNullEmptyOrWhiteSpaceAndFilterShouldBeUpperCase_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator(string filter)
		{
			int maxLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, true, maxLength: maxLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == maxLength),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenFilterIsNotNullEmptyOrWhiteSpaceAndFilterShouldBeUpperCase_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator()
		{
			string filter = _fixture.Create<string>();
			int maxLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, true, maxLength: maxLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == filter.ToUpper()),
					It.Is<int>(value => value == maxLength),
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
		public void Validate_WhenFilterIsNullEmptyOrWhiteSpaceAndFilterShouldNotBeUpperCase_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator(string filter)
		{
			int maxLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, false, maxLength: maxLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == null),
					It.Is<int>(value => value == maxLength),
					It.Is<Type>(value => value != null && value == sut.GetType()),
					It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Filter") == 0),
					It.Is<bool>(value => value)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenFilterIsNotNullEmptyOrWhiteSpaceAndFilterShouldNotBeUpperCase_AssertShouldHaveMaxLengthWasCalledOnStringValidatorFromValidator()
		{
			string filter = _fixture.Create<string>();
			int maxLength = _fixture.Create<int>();
			IMediaLibraryFilterQuery sut = CreateSut(filter, false, maxLength: maxLength);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _mediaLibraryRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(value => value == filter),
					It.Is<int>(value => value == maxLength),
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

		private IMediaLibraryFilterQuery CreateSut(string filter = null, bool? asUpperCase = null, int? minLength = null, int? maxLength = null)
		{
			return new MyMediaLibraryFilterQuery(filter, asUpperCase ?? _random.Next(100) > 50, minLength ?? _fixture.Create<int>(), maxLength ?? _fixture.Create<int>());
		}

		private class MyMediaLibraryFilterQuery : BusinessLogic.MediaLibrary.Queries.MediaLibraryFilterQueryBase
		{
			#region Constructor

			public MyMediaLibraryFilterQuery(string filter, bool asUpperCase, int minLength, int maxLength) 
				: base(filter, asUpperCase)
			{
				MinLength = minLength;
				MaxLength = maxLength;
			}

			#endregion

			#region Properties

			protected override int MinLength { get; }

			protected override int MaxLength { get; }

			#endregion
		}
	}
}