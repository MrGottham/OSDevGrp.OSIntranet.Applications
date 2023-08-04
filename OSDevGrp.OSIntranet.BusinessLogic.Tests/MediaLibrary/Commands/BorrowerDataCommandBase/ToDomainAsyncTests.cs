using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.MediaLibrary.Commands.BorrowerDataCommandBase
{
	[TestFixture]
	public class ToDomainAsyncTests
	{
		#region Private variables

		private Mock<IMediaLibraryRepository> _mediaLibraryRepositoryMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_mediaLibraryRepositoryMock = new Mock<IMediaLibraryRepository>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenMediaLibraryRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBorrowerDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(null, _commonRepositoryMock.Object));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("mediaLibraryRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public void ToDomainAsync_WhenCommonRepositoryIsNull_ThrowsArgumentNullException()
		{
			IBorrowerDataCommand sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("commonRepository"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut();

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrower()
		{
			IBorrowerDataCommand sut = CreateSut();

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result, Is.TypeOf<Borrower>());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereBorrowerIdentifierIsEqualToBorrowerIdentifierFromBorrowerDataCommand()
		{
			Guid borrowerIdentifier = Guid.NewGuid();
			IBorrowerDataCommand sut = CreateSut(borrowerIdentifier);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.BorrowerIdentifier, Is.EqualTo(borrowerIdentifier));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereFullNameIsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut();

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.FullName, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereFullNameIsNotEmpty()
		{
			IBorrowerDataCommand sut = CreateSut();

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.FullName, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereFullNameIsEqualToFullNameFromBorrowerDataCommand()
		{
			string fullName = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(fullName: fullName);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.FullName, Is.EqualTo(fullName));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMailAddressIsSetOnBorrowerDataCommand_ReturnsBorrowerWhereMailAddressIsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut(hasMailAddress: true);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MailAddress, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMailAddressIsSetOnBorrowerDataCommand_ReturnsBorrowerWhereMailAddressIsNotEmpty()
		{
			IBorrowerDataCommand sut = CreateSut(hasMailAddress: true);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MailAddress, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMailAddressIsSetOnBorrowerDataCommand_ReturnsBorrowerWhereMailAddressIsEqualToMailAddressFromBorrowerDataCommand()
		{
			string mailAddress = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(hasMailAddress: true, mailAddress: mailAddress);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MailAddress, Is.EqualTo(mailAddress));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenMailAddressIsNotSetOnBorrowerDataCommand_ReturnsBorrowerWhereMailAddressIsNull()
		{
			IBorrowerDataCommand sut = CreateSut(hasMailAddress: false);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.MailAddress, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPrimaryPhoneIsSetOnBorrowerDataCommand_ReturnsBorrowerWherePrimaryPhoneIsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut(hasPrimaryPhone: true);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.PrimaryPhone, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPrimaryPhoneIsSetOnBorrowerDataCommand_ReturnsBorrowerWherePrimaryPhoneIsNotEmpty()
		{
			IBorrowerDataCommand sut = CreateSut(hasPrimaryPhone: true);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.PrimaryPhone, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPrimaryPhoneIsSetOnBorrowerDataCommand_ReturnsBorrowerWherePrimaryPhoneIsEqualToPrimaryPhoneFromBorrowerDataCommand()
		{
			string primaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(hasPrimaryPhone: true, primaryPhone: primaryPhone);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.PrimaryPhone, Is.EqualTo(primaryPhone));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenPrimaryPhoneIsNotSetOnBorrowerDataCommand_ReturnsBorrowerWherePrimaryPhoneIsNull()
		{
			IBorrowerDataCommand sut = CreateSut(hasPrimaryPhone: false);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.PrimaryPhone, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSecondaryPhoneIsSetOnBorrowerDataCommand_ReturnsBorrowerWhereSecondaryPhoneIsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut(hasSecondaryPhone: true);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SecondaryPhone, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSecondaryPhoneIsSetOnBorrowerDataCommand_ReturnsBorrowerWhereSecondaryPhoneIsNotEmpty()
		{
			IBorrowerDataCommand sut = CreateSut(hasSecondaryPhone: true);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SecondaryPhone, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSecondaryPhoneIsSetOnBorrowerDataCommand_ReturnsBorrowerWhereSecondaryPhoneIsEqualToSecondaryPhoneFromBorrowerDataCommand()
		{
			string secondaryPhone = _fixture.Create<string>();
			IBorrowerDataCommand sut = CreateSut(hasSecondaryPhone: true, secondaryPhone: secondaryPhone);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SecondaryPhone, Is.EqualTo(secondaryPhone));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenSecondaryPhoneIsNotSetOnBorrowerDataCommand_ReturnsBorrowerWhereSecondaryPhoneIsNull()
		{
			IBorrowerDataCommand sut = CreateSut(hasSecondaryPhone: false);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.SecondaryPhone, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereLendingLimitIsEqualToLendingLimitFromBorrowerDataCommand()
		{
			int lendingLimit = _fixture.Create<int>();
			IBorrowerDataCommand sut = CreateSut(lendingLimit: lendingLimit);

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.LendingLimit, Is.EqualTo(lendingLimit));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereLendingsIsNotNull()
		{
			IBorrowerDataCommand sut = CreateSut();

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ToDomainAsync_WhenCalled_ReturnsBorrowerWhereLendingsIsEmpty()
		{
			IBorrowerDataCommand sut = CreateSut();

			IBorrower result = await sut.ToDomainAsync(_mediaLibraryRepositoryMock.Object, _commonRepositoryMock.Object);

			Assert.That(result.Lendings, Is.Empty);
		}

		private IBorrowerDataCommand CreateSut(Guid? borrowerIdentifier = null, string fullName = null, bool hasMailAddress = true, string mailAddress = null, bool hasPrimaryPhone = true, string primaryPhone = null, bool hasSecondaryPhone = true, string secondaryPhone = null, int? lendingLimit = null)
		{
			return new MyBorrowerDataCommandBase(
				borrowerIdentifier ?? Guid.NewGuid(), 
				fullName ?? _fixture.Create<string>(), 
				hasMailAddress ? mailAddress ?? _fixture.Create<string>() : null, 
				hasPrimaryPhone ? primaryPhone ?? _fixture.Create<string>() : null,
				hasSecondaryPhone ? secondaryPhone ?? _fixture.Create<string>() : null, 
				lendingLimit ?? _fixture.Create<int>());
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