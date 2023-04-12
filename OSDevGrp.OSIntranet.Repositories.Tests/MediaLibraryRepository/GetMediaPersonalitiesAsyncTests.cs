using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
	[TestFixture]
	public class GetMediaPersonalitiesAsyncTests : MediaLibraryRepositoryTestBase
	{
		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenCalled_ReturnsNotNull()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task GetMediaPersonalitiesAsync_WhenCalled_ReturnsNonEmptyCollectionOfMediaPersonalities()
		{
			IMediaLibraryRepository sut = CreateSut();

			IEnumerable<IMediaPersonality> result = await sut.GetMediaPersonalitiesAsync();

			Assert.That(result, Is.Not.Empty);
		}
	}
}