using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class GetLetterHeadAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetLetterHeadAsync_WhenCalled_ReturnsLetterHead()
        {
            ICommonRepository sut = CreateSut();

            ILetterHead result = await sut.GetLetterHeadAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}