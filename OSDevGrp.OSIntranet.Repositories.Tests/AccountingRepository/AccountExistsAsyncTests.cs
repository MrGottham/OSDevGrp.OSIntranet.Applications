using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class AccountExistsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void AccountExistsAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AccountExistsAsync(WithExistingAccountingNumber(), null));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountExistsAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AccountExistsAsync(WithExistingAccountingNumber(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccountExistsAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AccountExistsAsync(WithExistingAccountingNumber(), " "));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task AccountExistsAsync_WhenAccountNumberExistsWithinAccounting_ReturnsTrue()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.AccountExistsAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForAccount());

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task AccountExistsAsync_WhenAccountNumberDoesNotExistWithinAccounting_ReturnsFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.AccountExistsAsync(WithExistingAccountingNumber(), WithNonExistingAccountNumber());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task AccountExistsAsync_WhenAccountingNumberDoesNotExist_ReturnsFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.AccountExistsAsync(WithNonExistingAccountingNumber(), WithExistingAccountNumberForAccount());

            Assert.That(result, Is.False);
        }
    }
}