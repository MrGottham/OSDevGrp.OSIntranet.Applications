using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetBorrowerAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowerAsync_WhenCalledWithKnownBorrowerIdentifier_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			Guid? existingBorrowerIdentifier = WithExistingBorrowerIdentifier();
			if (existingBorrowerIdentifier.HasValue == false)
			{
				return;
			}

			IBorrower result = await sut.GetBorrowerAsync(existingBorrowerIdentifier.Value);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetBorrowerAsync_WhenCalledWithUnknownBorrowerIdentifier_ReturnsNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IBorrower result = await sut.GetBorrowerAsync(Guid.NewGuid());

			Assert.That(result, Is.Null);
		}
	}
}