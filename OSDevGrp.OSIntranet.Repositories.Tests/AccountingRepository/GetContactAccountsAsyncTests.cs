using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetContactAccountsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactAccountsAsync_WhenAccountingNumberExists_ReturnsNonEmptyContactAccountCollection()
        {
            IAccountingRepository sut = CreateSut();

            IContactAccountCollection result = await sut.GetContactAccountsAsync(WithExistingAccountingNumber(), DateTime.Today);

            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactAccountsAsync_WhenAccountingNumberDoesNotExist_ReturnsEmptyContactAccountCollection()
        {
            IAccountingRepository sut = CreateSut();

            IContactAccountCollection result = await sut.GetContactAccountsAsync(WithNonExistingAccountingNumber(), DateTime.Today);

            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}