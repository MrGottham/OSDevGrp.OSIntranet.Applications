using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetBookGenresAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetBookGenresAsync_WhenCalled_ReturnsNotNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IBookGenre> result = await sut.GetBookGenresAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetBookGenresAsync_WhenCalled_ReturnsEmptyCollectionOfBookGenres()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IBookGenre> result = await sut.GetBookGenresAsync();

            Assert.That(result, Is.Empty);
        }
    }
}