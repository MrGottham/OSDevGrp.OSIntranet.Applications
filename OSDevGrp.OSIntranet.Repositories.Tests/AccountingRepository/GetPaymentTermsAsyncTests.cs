using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetPaymentTermsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetPaymentTermsAsync_WhenCalled_ReturnsPaymentTerms()
        {
            IAccountingRepository sut = CreateSut();

            IList<IPaymentTerm> result = (await sut.GetPaymentTermsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
