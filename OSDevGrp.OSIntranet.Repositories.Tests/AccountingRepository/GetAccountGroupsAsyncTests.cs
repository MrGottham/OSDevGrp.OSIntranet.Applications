using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetAccountGroupsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountGroupsAsync_WhenCalled_ReturnsAccountGroups()
        {
            IAccountingRepository sut = CreateSut();

            IList<IAccountGroup> result = (await sut.GetAccountGroupsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}