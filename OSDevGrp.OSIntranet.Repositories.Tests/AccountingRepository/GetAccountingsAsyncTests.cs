using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetAccountingsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingsAsync_WhenCalled_ReturnsAccountings()
        {
            IAccountingRepository sut = CreateSut();

            IList<IAccounting> result = (await sut.GetAccountingsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}