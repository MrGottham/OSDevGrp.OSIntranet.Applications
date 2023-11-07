using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetMediaPersonalityAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalityAsync_WhenCalledWithKnownMediaPersonalityIdentifier_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality result = await sut.GetMediaPersonalityAsync(WithExistingMediaPersonalityIdentifier());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalityAsync_WhenCalledWithUnknownMediaPersonalityIdentifier_ReturnsNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IMediaPersonality result = await sut.GetMediaPersonalityAsync(Guid.NewGuid());

			Assert.That(result, Is.Null);
		}
	}
}