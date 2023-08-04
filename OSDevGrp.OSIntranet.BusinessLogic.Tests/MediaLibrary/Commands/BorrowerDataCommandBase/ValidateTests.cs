using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.BorrowerDataCommandBase
{
	[TestFixture]
	public class ValidateTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IClaimResolver> _claimResolverMock;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_claimResolverMock = new Mock<IClaimResolver>();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			IBorrowerDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenClaimResolverIsNull_ThrowsArgumentNullException()
		{
			IBorrowerDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("claimResolver"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBorrowerDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBorrowerDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertIsMediaLibraryLenderWasCalledOnClaimResolver()
		{
			IBorrowerDataCommand sut = CreateSut();

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_claimResolverMock.Verify(m => m.IsMediaLibraryLender(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithFullName()
		{
			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(fullName: fullName);
			
			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, fullName) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "FullName") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithFullName()
		{
			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(fullName: fullName);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, fullName) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "FullName") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithFullName()
		{
			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(fullName: fullName);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, fullName) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "FullName") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithMailAddress()
		{
			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(mailAddress: mailAddress);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, mailAddress) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MailAddress") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithMailAddress()
		{
			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(mailAddress: mailAddress);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, mailAddress) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MailAddress") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithMailAddress()
		{
			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(mailAddress: mailAddress);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, mailAddress) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.MailAddressRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MailAddress") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithPrimaryPhone()
		{
			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(primaryPhone: primaryPhone);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, primaryPhone) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "PrimaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithPrimaryPhone()
		{
			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(primaryPhone: primaryPhone);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, primaryPhone) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "PrimaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithPrimaryPhone()
		{
			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(primaryPhone: primaryPhone);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, primaryPhone) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "PrimaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSecondaryPhone()
		{
			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, secondaryPhone) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SecondaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSecondaryPhone()
		{
			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, secondaryPhone) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SecondaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithSecondaryPhone()
		{
			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(secondaryPhone: secondaryPhone);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, secondaryPhone) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SecondaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithLendingLimit()
		{
			int lendingLimit = _fixture.Create<int>();
			IBorrowerDataCommand sut = CreateSut(lendingLimit: lendingLimit);

			sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == lendingLimit),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 365),
					It.Is<Type>(v => v != null && v == sut.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "LendingLimit") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Validate_WhenCalled_ReturnsValidatorFromArguments()
		{
			IBorrowerDataCommand sut = CreateSut();

			IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _claimResolverMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(_validatorMockContext.ValidatorMock.Object));
		}

		private IBorrowerDataCommand CreateSut(string fullName = null, string mailAddress = null, string primaryPhone = null, string secondaryPhone = null, int? lendingLimit = null)
		{
			_claimResolverMock.Setup(m => m.IsMediaLibraryLender())
				.Returns(_fixture.Create<bool>());

			return new MyBorrowerDataCommandBase(Guid.NewGuid(), fullName ?? _fixture.Create<string>(), mailAddress ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), primaryPhone ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), secondaryPhone ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null), lendingLimit ?? _fixture.Create<int>());
		}

		private class MyBorrowerDataCommandBase : BusinessLogic.MediaLibrary.Commands.BorrowerDataCommandBase
		{
			#region Constructor

			public MyBorrowerDataCommandBase(Guid borrowerIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit) 
				: base(borrowerIdentifier, fullName, mailAddress, primaryPhone, secondaryPhone, lendingLimit)
			{
			}

			#endregion
		}
	}
}