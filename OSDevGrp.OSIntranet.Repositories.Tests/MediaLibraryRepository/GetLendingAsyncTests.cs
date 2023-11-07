using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetLendingAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingAsync_WhenCalledWithKnownLendingIdentifier_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			Guid? existingLendingIdentifier = WithExistingLendingIdentifier();
			if (existingLendingIdentifier.HasValue == false)
			{
				return;
			}

			ILending result = await sut.GetLendingAsync(existingLendingIdentifier.Value);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetLendingAsync_WhenCalledWithUnknownLendingIdentifier_ReturnsNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			ILending result = await sut.GetLendingAsync(Guid.NewGuid());

			Assert.That(result, Is.Null);
		}
	}
}