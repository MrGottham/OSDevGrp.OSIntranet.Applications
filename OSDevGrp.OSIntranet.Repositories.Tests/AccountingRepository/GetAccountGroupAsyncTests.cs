using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetAccountGroupAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountGroupAsync_WhenCalled_ReturnsAccountGroup()
        {
            IAccountingRepository sut = CreateSut();

            IAccountGroup result = await sut.GetAccountGroupAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}