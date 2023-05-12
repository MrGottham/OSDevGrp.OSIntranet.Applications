using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class LendingExistsAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task LendingExistsAsync_WhenCalledWithKnownLendingIdentifier_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			Guid? existingLendingIdentifier = WithExistingLendingIdentifier();
			if (existingLendingIdentifier.HasValue == false)
			{
				return;
			}

			bool result = await sut.LendingExistsAsync(existingLendingIdentifier.Value);

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task LendingExistsAsync_WhenCalledWithUnknownLendingIdentifier_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.LendingExistsAsync(Guid.NewGuid());

			Assert.That(result, Is.False);
		}
	}
}