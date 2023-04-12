using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class MediaPersonalityExistsAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithKnownMediaPersonalityIdentifier_ReturnsTrue()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaPersonalityExistsAsync(WithExistingMediaPersonalityIdentifier());

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task MediaPersonalityExistsAsync_WhenCalledWithUnknownMediaPersonalityIdentifier_ReturnsFalse()
		{
			IMediaLibraryRepository sut = CreateSut();

			bool result = await sut.MediaPersonalityExistsAsync(Guid.NewGuid());

			Assert.That(result, Is.False);
		}
	}
}