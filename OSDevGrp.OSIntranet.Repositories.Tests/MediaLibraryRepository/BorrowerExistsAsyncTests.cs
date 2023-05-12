using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class BorrowerExistsAsyncTests : MediaLibraryRepositoryTestBase
	{
		#region Private variables

		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task BorrowerExistsAsync_WhenCalledWithKnownBorrowerIdentifier_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			Guid? existingBorrowerIdentifier = WithExistingBorrowerIdentifier();
			if (existingBorrowerIdentifier.HasValue == false)
			{
				return;
			}

			bool result = await sut.BorrowerExistsAsync(existingBorrowerIdentifier.Value);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task BorrowerExistsAsync_WhenCalledWithUnknownBorrowerIdentifier_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.BorrowerExistsAsync(Guid.NewGuid());

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void BorrowerExistsAsync_WhenCalledWithFullNameWhereFullNameIsNull_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BorrowerExistsAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("fullName"));
		}

		[Test]
		[Category("UnitTest")]
		public void BorrowerExistsAsync_WhenCalledWithFullNameWhereFullNameIsEmpty_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BorrowerExistsAsync(string.Empty));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("fullName"));
		}

		[Test]
		[Category("UnitTest")]
		public void BorrowerExistsAsync_WhenCalledWithFullNameWhereFullNameIsWhiteSpace_ThrowsArgumentNullException()
		{
			IMediaLibraryRepository sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BorrowerExistsAsync(" "));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("fullName"));
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task BorrowerExistsAsync_WhenCalledWithExistingFullName_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBorrower[] knownBorrowers = (await sut.GetBorrowersAsync()).ToArray();
			if (knownBorrowers.Any() == false)
			{
				return;
			}

			IBorrower knownBorrower = knownBorrowers[_random.Next(0, knownBorrowers.Length - 1)];
			bool result = await sut.BorrowerExistsAsync(knownBorrower.FullName);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task BorrowerExistsAsync_WhenCalledWithNonExistingFullName_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.BorrowerExistsAsync(_fixture.Create<string>());

			Assert.That(result, Is.False);
		}
	}
}