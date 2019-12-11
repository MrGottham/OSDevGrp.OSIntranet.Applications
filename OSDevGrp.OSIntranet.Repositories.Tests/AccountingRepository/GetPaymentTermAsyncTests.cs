using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetPaymentTermAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetPaymentTermAsync_WhenCalled_ReturnsPaymentTerm()
        {
            IAccountingRepository sut = CreateSut();

            IPaymentTerm result = await sut.GetPaymentTermAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}
