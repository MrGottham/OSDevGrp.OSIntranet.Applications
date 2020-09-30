using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class BudgetAccountExistsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void BudgetAccountExistsAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BudgetAccountExistsAsync(WithExistingAccountingNumber(), null));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void BudgetAccountExistsAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BudgetAccountExistsAsync(WithExistingAccountingNumber(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void BudgetAccountExistsAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.BudgetAccountExistsAsync(WithExistingAccountingNumber(), " "));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task BudgetAccountExistsAsync_WhenAccountNumberExistsWithinAccounting_ReturnsTrue()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.BudgetAccountExistsAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount());

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task BudgetAccountExistsAsync_WhenAccountNumberDoesNotExistWithinAccounting_ReturnsFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.BudgetAccountExistsAsync(WithExistingAccountingNumber(), WithNonExistingAccountNumber());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task BudgetAccountExistsAsync_WhenAccountingNumberDoesNotExist_ReturnsFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.BudgetAccountExistsAsync(WithNonExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount());

            Assert.That(result, Is.False);
        }
    }
}