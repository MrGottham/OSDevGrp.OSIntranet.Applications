using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetBookGenreAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMusicGenreAsync_WhenCalled_ReturnsNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IBookGenre result = await sut.GetBookGenreAsync(1);

            Assert.That(result, Is.Null);
        }
    }
}