using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetAccountAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void GetAccountAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetAccountAsync(WithExistingAccountingNumber(), null, DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccountAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetAccountAsync(WithExistingAccountingNumber(), string.Empty, DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetAccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetAccountAsync(WithExistingAccountingNumber(), " ", DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountAsync_WhenAccountNumberExistsWithinAccounting_ReturnsAccount()
        {
            IAccountingRepository sut = CreateSut();

            IAccount result = await sut.GetAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForAccount(), DateTime.Today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountAsync_WhenAccountNumberDoesNotExistWithinAccounting_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IAccount result = await sut.GetAccountAsync(WithExistingAccountingNumber(), WithNonExistingAccountNumber(), DateTime.Today);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetAccountAsync_WhenAccountingNumberDoesNotExist_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IAccount result = await sut.GetAccountAsync(WithNonExistingAccountingNumber(), WithExistingAccountNumberForAccount(), DateTime.Today);

            Assert.That(result, Is.Null);
        }
    }
}