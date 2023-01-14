using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    [TestFixture]
    public class GetMediaTypesAsyncTests : MediaLibraryRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetMediaTypesAsync_WhenCalled_ReturnsNotNull()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IMediaType> result = await sut.GetMediaTypesAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetMediaTypesAsync_WhenCalled_ReturnsEmptyCollectionOfMediaTypes()
        {
            IMediaLibraryRepository sut = CreateSut();

            IEnumerable<IMediaType> result = await sut.GetMediaTypesAsync();

            Assert.That(result, Is.Empty);
        }
    }
}