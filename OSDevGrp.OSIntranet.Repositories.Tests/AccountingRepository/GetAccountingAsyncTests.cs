using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetAccountingAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingAsync_WhenAccountingNumberExists_ReturnsAccounting()
        {
            IAccountingRepository sut = CreateSut();

            IAccounting result = await sut.GetAccountingAsync(WithExistingAccountingNumber(), DateTime.Today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountingAsync_WhenAccountingNumberDoesNotExist_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IAccounting result = await sut.GetAccountingAsync(WithNonExistingAccountingNumber(), DateTime.Today);

            Assert.That(result, Is.Null);
        }
    }
}