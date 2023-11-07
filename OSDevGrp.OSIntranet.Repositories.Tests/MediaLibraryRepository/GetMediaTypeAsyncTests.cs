using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetMediaTypeAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMediaTypeAsync_WhenCalled_ReturnsNotNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IMediaType result = await sut.GetMediaTypeAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}