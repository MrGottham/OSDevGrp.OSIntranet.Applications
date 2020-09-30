using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetAccountsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountsAsync_WhenAccountingNumberExists_ReturnsNonEmptyAccountCollection()
        {
            IAccountingRepository sut = CreateSut();

            IAccountCollection result = await sut.GetAccountsAsync(WithExistingAccountingNumber(), DateTime.Today);

            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountsAsync_WhenAccountingNumberDoesNotExist_ReturnsEmptyAccountCollection()
        {
            IAccountingRepository sut = CreateSut();

            IAccountCollection result = await sut.GetAccountsAsync(WithNonExistingAccountingNumber(), DateTime.Today);

            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}