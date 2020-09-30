using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetBudgetAccountsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountsAsync_WhenAccountingNumberExists_ReturnsNonEmptyBudgetAccountCollection()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccountCollection result = await sut.GetBudgetAccountsAsync(WithExistingAccountingNumber(), DateTime.Today);

            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountsAsync_WhenAccountingNumberDoesNotExist_ReturnsEmptyBudgetAccountCollection()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccountCollection result = await sut.GetBudgetAccountsAsync(WithNonExistingAccountingNumber(), DateTime.Today);

            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}