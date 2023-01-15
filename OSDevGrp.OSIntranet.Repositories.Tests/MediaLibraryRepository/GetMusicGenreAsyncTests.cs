using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetMusicGenreAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMusicGenreAsync_WhenCalled_ReturnsNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IMusicGenre result = await sut.GetMusicGenreAsync(1);

            Assert.That(result, Is.Null);
        }
    }
}