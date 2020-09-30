using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class ContactAccountExistsAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void ContactAccountExistsAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ContactAccountExistsAsync(WithExistingAccountingNumber(), null));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountExistsAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ContactAccountExistsAsync(WithExistingAccountingNumber(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void ContactAccountExistsAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ContactAccountExistsAsync(WithExistingAccountingNumber(), " "));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ContactAccountExistsAsync_WhenAccountNumberExistsWithinAccounting_ReturnsTrue()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.ContactAccountExistsAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForContactAccount());

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ContactAccountExistsAsync_WhenAccountNumberDoesNotExistWithinAccounting_ReturnsFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.ContactAccountExistsAsync(WithExistingAccountingNumber(), WithNonExistingAccountNumber());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task ContactAccountExistsAsync_WhenAccountingNumberDoesNotExist_ReturnsFalse()
        {
            IAccountingRepository sut = CreateSut();

            bool result = await sut.ContactAccountExistsAsync(WithNonExistingAccountingNumber(), WithExistingAccountNumberForContactAccount());

            Assert.That(result, Is.False);
        }
    }
}