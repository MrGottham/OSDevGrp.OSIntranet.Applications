using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class AccountingExistsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task AccountingExistsAsync_WhenCalledForExistingAccountingNumber_ReturnTrue()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.AccountingExistsAsync(WithExistingAccountingNumber());

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task AccountingExistsAsync_WhenCalledForNonExistingAccountingNumber_ReturnFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.AccountingExistsAsync(WithNonExistingAccountingNumber());

            Assert.That(result, Is.False);
        }
    }
}