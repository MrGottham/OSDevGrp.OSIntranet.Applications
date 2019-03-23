using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetBudgetAccountGroupsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountGroupsAsync_WhenCalled_ReturnsBudgetAccountGroups()
        {
            IAccountingRepository sut = CreateSut();

            IList<IBudgetAccountGroup> result = (await sut.GetBudgetAccountGroupsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}