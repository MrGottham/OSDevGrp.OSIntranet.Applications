using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetMusicGenresAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMusicGenresAsync_WhenCalled_ReturnsNotNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IMusicGenre> result = await sut.GetMusicGenresAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetMusicGenresAsync_WhenCalled_ReturnsEmptyCollectionOfMusicGenres()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IMusicGenre> result = await sut.GetMusicGenresAsync();

            Assert.That(result, Is.Empty);
        }
    }
}