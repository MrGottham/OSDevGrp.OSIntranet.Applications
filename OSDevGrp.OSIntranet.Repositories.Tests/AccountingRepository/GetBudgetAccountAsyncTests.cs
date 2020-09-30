using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetBudgetAccountAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void GetBudgetAccountAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), null, DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetBudgetAccountAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), string.Empty, DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetBudgetAccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), " ", DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountAsync_WhenAccountNumberExistsWithinAccounting_ReturnsBudgetAccount()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccount result = await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount(), DateTime.Today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetBudgetAccountAsync_WhenAccountNumberDoesNotExistWithinAccounting_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccount result = await sut.GetBudgetAccountAsync(WithExistingAccountingNumber(), WithNonExistingAccountNumber(), DateTime.Today);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task BudgetAccountExistsAsync_WhenAccountingNumberDoesNotExist_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IBudgetAccount result = await sut.GetBudgetAccountAsync(WithNonExistingAccountingNumber(), WithExistingAccountNumberForBudgetAccount(), DateTime.Today);

            Assert.That(result, Is.Null);
        }
    }
}