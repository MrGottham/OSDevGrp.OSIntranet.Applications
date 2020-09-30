using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class DeleteBudgetAccountAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void DeleteBudgetAccountAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteBudgetAccountAsync(WithExistingAccountingNumber(), null));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteBudgetAccountAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteBudgetAccountAsync(WithExistingAccountingNumber(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteBudgetAccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteBudgetAccountAsync(WithExistingAccountingNumber(), " "));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will delete a budget account (should only be used for debugging)")]
        public async Task DeleteBudgetAccountAsync_WhenCalled_DeletesBudgetAccount()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccount result = await sut.DeleteBudgetAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount());

            Assert.That(result, Is.Null);
        }
    }
}