using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetMovieGenreAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMovieGenreAsync_WhenCalled_ReturnsNotNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IMovieGenre result = await sut.GetMovieGenreAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}