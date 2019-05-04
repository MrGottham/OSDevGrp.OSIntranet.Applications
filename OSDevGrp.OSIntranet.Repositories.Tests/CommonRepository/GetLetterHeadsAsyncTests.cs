using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class GetLetterHeadsAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetLetterHeadsAsync_WhenCalled_ReturnsLetterHeads()
        {
            ICommonRepository sut = CreateSut();

            IList<ILetterHead> result = (await sut.GetLetterHeadsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}