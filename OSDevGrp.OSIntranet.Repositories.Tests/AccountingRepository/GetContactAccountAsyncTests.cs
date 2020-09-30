using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class GetContactAccountAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void GetContactAccountAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContactAccountAsync(WithExistingAccountingNumber(), null, DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetContactAccountAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContactAccountAsync(WithExistingAccountingNumber(), string.Empty, DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetContactAccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContactAccountAsync(WithExistingAccountingNumber(), " ", DateTime.Today));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactAccountAsync_WhenAccountNumberExistsWithinAccounting_ReturnsContactAccount()
        {
            IAccountingRepository sut = CreateSut();

            IContactAccount result = await sut.GetContactAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForContactAccount(), DateTime.Today);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactAccountAsync_WhenAccountNumberDoesNotExistWithinAccounting_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IContactAccount result = await sut.GetContactAccountAsync(WithExistingAccountingNumber(), WithNonExistingAccountNumber(), DateTime.Today);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactAccountAsync_WhenAccountingNumberDoesNotExist_ReturnsNull()
        {
            IAccountingRepository sut = CreateSut();

            IContactAccount result = await sut.GetContactAccountAsync(WithNonExistingAccountingNumber(), WithExistingAccountNumberForContactAccount(), DateTime.Today);

            Assert.That(result, Is.Null);
        }
    }
}