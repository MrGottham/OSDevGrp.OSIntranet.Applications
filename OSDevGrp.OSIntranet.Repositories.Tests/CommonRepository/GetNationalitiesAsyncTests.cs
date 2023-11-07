using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class GetNationalitiesAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetNationalitiesAsync_WhenCalled_ReturnsNotNull()
        {
            ICommonRepository sut = CreateSut();

            IEnumerable<INationality> result = await sut.GetNationalitiesAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetNationalitiesAsync_WhenCalled_ReturnsNonEmptyCollectionOfNationalities()
        {
            ICommonRepository sut = CreateSut();

            IEnumerable<INationality> result = await sut.GetNationalitiesAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}