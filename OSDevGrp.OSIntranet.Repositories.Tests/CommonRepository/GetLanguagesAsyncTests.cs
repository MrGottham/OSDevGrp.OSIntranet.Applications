using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class GetLanguagesAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetLanguagesAsync_WhenCalled_ReturnsNotNull()
        {
            ICommonRepository sut = CreateSut();

            IEnumerable<ILanguage> result = await sut.GetLanguagesAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetLanguagesAsync_WhenCalled_ReturnsNonEmptyCollectionOfLanguages()
        {
            ICommonRepository sut = CreateSut();

            IEnumerable<ILanguage> result = await sut.GetLanguagesAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}