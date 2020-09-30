using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class DeleteContactAccountAsyncTests : AccountingRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void DeleteContactAccountAsync_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAccountAsync(WithExistingAccountingNumber(), null));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactAccountAsync_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAccountAsync(WithExistingAccountingNumber(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactAccountAsync_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAccountAsync(WithExistingAccountingNumber(), " "));

            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
        }

        [Test]
        [Category("IntegrationTest")] 
        [Ignore("Test which will delete a contact account (should only be used for debugging)")]
        public async Task DeleteContactAccountAsync_WhenCalled_DeletesContactAccount()
        {
            IAccountingRepository sut = CreateSut();

            IContactAccount result = await sut.DeleteContactAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForContactAccount());

            Assert.That(result, Is.Null);
        }
    }
}