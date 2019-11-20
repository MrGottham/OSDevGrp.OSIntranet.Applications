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
        public async Task GetAccountingAsync_WhenCalled_ReturnsAccounting()
        {
            IAccountingRepository sut = CreateSut();

            IAccounting result = await sut.GetAccountingAsync(1, DateTime.Today);

            Assert.That(result, Is.Not.Null);
        }
    }
}