using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetMovieGenresAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMovieGenresAsync_WhenCalled_ReturnsNotNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IMovieGenre> result = await sut.GetMovieGenresAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetMovieGenresAsync_WhenCalled_ReturnsNonEmptyCollectionOfMovieGenres()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IMovieGenre> result = await sut.GetMovieGenresAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}