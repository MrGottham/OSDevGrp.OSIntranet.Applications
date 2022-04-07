using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ExternalDashboardRepository
{
    [TestFixture]
    public class GetNewsAsyncTests : ExternalDashboardRepositoryBase
    {
        [Test]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(25)]
        [TestCase(50)]
        [TestCase(75)]
        [Category("IntegrationTest")]
        public async Task GetNewsAsync_WhenCalled_ReturnsNews(int numberOfNews)
        {
            IExternalDashboardRepository sut = CreateSut();

            INews[] result = (await sut.GetNewsAsync(numberOfNews)).ToArray();

            Assert.That(result.Length, Is.GreaterThan(0));
            Assert.That(result.Length, Is.LessThanOrEqualTo(numberOfNews));
        }
    }
}