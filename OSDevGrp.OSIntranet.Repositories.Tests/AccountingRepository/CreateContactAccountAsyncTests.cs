using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class CreateContactAccountAsyncTests : AccountingRepositoryTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void CreateContactAccountAsync_WhenContactAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateContactAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("contactAccount"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will create a contact account (should only be used for debugging)")]
        public async Task CreateContactAccountAsync_WhenCalled_CreatesContactAccount()
        {
            IAccountingRepository sut = CreateSut();

            IAccounting accounting = await sut.GetAccountingAsync(WithExistingAccountingNumber(), DateTime.Today);
            IPaymentTerm[] paymentTermCollection = (await sut.GetPaymentTermsAsync()).ToArray();

            IContactAccount contactAccount = new ContactAccount(accounting, WithExistingAccountNumberForContactAccount(), _fixture.Create<string>(), paymentTermCollection[_random.Next(0, paymentTermCollection.Length - 1)])
            {
                Description = _fixture.Create<string>(),
                PrimaryPhone = _fixture.Create<string>()
            };
            IContactAccount result = await sut.CreateContactAccountAsync(contactAccount);

            Assert.That(result, Is.Not.Null);
        }
    }
}