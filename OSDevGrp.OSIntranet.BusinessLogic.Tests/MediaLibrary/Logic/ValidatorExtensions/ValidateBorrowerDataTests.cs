using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Logic.ValidatorExtensions
{
	[TestFixture]
	public class ValidateBorrowerDataTests
	{
		#region Private variables

		private ValidatorMockContext _validatorMockContext;
		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMockContext = new ValidatorMockContext();
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenValidatorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.MediaLibrary.Logic.ValidatorExtensions.ValidateBorrowerData(null, CreateBorrowerDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("validator"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenBorrowerDataIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBorrowerData(null, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("borrowerData"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBorrowerData(CreateBorrowerDataCommand(), null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IValidator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ValidateBorrowerData(CreateBorrowerDataCommand(), _mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertFullNameWasCalledOnBorrowerDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			sut.ValidateBorrowerData(borrowerDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			borrowerDataCommandMock.Verify(m => m.FullName, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithFullName()
		{
			IValidator sut = CreateSut();

			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(fullName: fullName);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, fullName) == 0),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "FullName") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithFullName()
		{
			IValidator sut = CreateSut();

			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(fullName: fullName);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, fullName) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "FullName") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithFullName()
		{
			IValidator sut = CreateSut();

			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(fullName: fullName);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, fullName) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "FullName") == 0),
					It.Is<bool>(v => v == false)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertMailAddressWasCalledOnBorrowerDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			sut.ValidateBorrowerData(borrowerDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			borrowerDataCommandMock.Verify(m => m.MailAddress, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithMailAddress()
		{
			IValidator sut = CreateSut();

			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(mailAddress: mailAddress);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, mailAddress) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MailAddress") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithMailAddress()
		{
			IValidator sut = CreateSut();

			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(mailAddress: mailAddress);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, mailAddress) == 0),
					It.Is<int>(v => v == 256),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MailAddress") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithMailAddress()
		{
			IValidator sut = CreateSut();

			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(mailAddress: mailAddress);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, mailAddress) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.MailAddressRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "MailAddress") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertPrimaryPhoneWasCalledOnBorrowerDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			sut.ValidateBorrowerData(borrowerDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			borrowerDataCommandMock.Verify(m => m.PrimaryPhone, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithPrimaryPhone()
		{
			IValidator sut = CreateSut();

			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(primaryPhone: primaryPhone);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, primaryPhone) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "PrimaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithPrimaryPhone()
		{
			IValidator sut = CreateSut();

			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(primaryPhone: primaryPhone);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, primaryPhone) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "PrimaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithPrimaryPhone()
		{
			IValidator sut = CreateSut();

			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(primaryPhone: primaryPhone);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, primaryPhone) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "PrimaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertSecondaryPhoneWasCalledOnBorrowerDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			sut.ValidateBorrowerData(borrowerDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			borrowerDataCommandMock.Verify(m => m.SecondaryPhone, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithSecondaryPhone()
		{
			IValidator sut = CreateSut();

			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(secondaryPhone: secondaryPhone);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, secondaryPhone) == 0),
					It.Is<int>(v => v == 1),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SecondaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorWithSecondaryPhone()
		{
			IValidator sut = CreateSut();

			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(secondaryPhone: secondaryPhone);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, secondaryPhone) == 0),
					It.Is<int>(v => v == 32),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SecondaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithSecondaryPhone()
		{
			IValidator sut = CreateSut();

			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(secondaryPhone: secondaryPhone);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, secondaryPhone) == 0),
					It.Is<Regex>(v => v != null && string.CompareOrdinal(v.ToString(), RegexTestHelper.PhoneNumberRegexPattern) == 0),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "SecondaryPhone") == 0),
					It.Is<bool>(v => v)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertLendingLimitWasCalledOnBorrowerDataCommand()
		{
			IValidator sut = CreateSut();

			Mock<IBorrowerDataCommand> borrowerDataCommandMock = CreateBorrowerDataCommandMock();
			sut.ValidateBorrowerData(borrowerDataCommandMock.Object, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			borrowerDataCommandMock.Verify(m => m.LendingLimit, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_AssertShouldBeBetweenWasCalledOnIntegerValidatorWithLendingLimit()
		{
			IValidator sut = CreateSut();

			int lendingLimit = _fixture.Create<int>();
			IBorrowerDataCommand borrowerDataCommand = CreateBorrowerDataCommand(lendingLimit: lendingLimit);
			sut.ValidateBorrowerData(borrowerDataCommand, _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			_validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeBetween(
					It.Is<int>(v => v == lendingLimit),
					It.Is<int>(v => v == 1),
					It.Is<int>(v => v == 365),
					It.Is<Type>(v => v != null && v == borrowerDataCommand.GetType()),
					It.Is<string>(v => string.IsNullOrWhiteSpace(v) == false && string.CompareOrdinal(v, "LendingLimit") == 0)),
				Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_ReturnsNotNull()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBorrowerData(CreateBorrowerDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ValidateBorrowerData_WhenCalled_ReturnsSameValidator()
		{
			IValidator sut = CreateSut();

			IValidator result = sut.ValidateBorrowerData(CreateBorrowerDataCommand(), _mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.SameAs(sut));
		}

		private IValidator CreateSut()
		{
			return _validatorMockContext.ValidatorMock.Object;
		}

		private IBorrowerDataCommand CreateBorrowerDataCommand(string fullName = null, string mailAddress = null, string primaryPhone = null, string secondaryPhone = null, int? lendingLimit = null)
		{
			return CreateBorrowerDataCommandMock(fullName, mailAddress, primaryPhone, secondaryPhone, lendingLimit).Object;
		}

		private Mock<IBorrowerDataCommand> CreateBorrowerDataCommandMock(string fullName = null, string mailAddress = null, string primaryPhone = null, string secondaryPhone = null, int? lendingLimit = null)
		{
			Mock<IBorrowerDataCommand> borrowerDataCommandMock = new Mock<IBorrowerDataCommand>();
			borrowerDataCommandMock.Setup(m => m.FullName)
				.Returns(fullName ?? _fixture.Create<string>());
			borrowerDataCommandMock.Setup(m => m.MailAddress)
				.Returns(mailAddress ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			borrowerDataCommandMock.Setup(m => m.PrimaryPhone)
				.Returns(primaryPhone ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			borrowerDataCommandMock.Setup(m => m.SecondaryPhone)
				.Returns(secondaryPhone ?? (_random.Next(100) > 50 ? _fixture.Create<string>() : null));
			borrowerDataCommandMock.Setup(m => m.LendingLimit)
				.Returns(lendingLimit ?? _fixture.Create<int>());
			return borrowerDataCommandMock;
		}
	}
}