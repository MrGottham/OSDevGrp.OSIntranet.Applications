using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetBudgetAccountGroupAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountGroupAsync_WhenCalled_ReturnsBudgetAccountGroup()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccountGroup result = await sut.GetBudgetAccountGroupAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}