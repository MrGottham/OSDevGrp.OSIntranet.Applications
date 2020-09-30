using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.AccountingRepository
{
    [TestFixture]
    public class UpdateContactAccountAsyncTests : AccountingRepositoryTestBase
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
        public void UpdateContactAccountAsync_WhenContactAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountingRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAccountAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("contactAccount"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which will update a contact account (should only be used for debugging)")]
        public async Task UpdateContactAccountAsync_WhenCalled_UpdatesContactAccount()
        {
            IAccountingRepository sut = CreateSut();

            IPaymentTerm[] paymentTermCollection = (await sut.GetPaymentTermsAsync()).ToArray();

            IContactAccount contactAccount = await sut.GetContactAccountAsync(WithExistingAccountingNumber(), WithExistingAccountNumberForContactAccount(), DateTime.Today);
            contactAccount.Description = _fixture.Create<string>();
            contactAccount.Note = _fixture.Create<string>();
            contactAccount.PrimaryPhone = _fixture.Create<string>();
            contactAccount.SecondaryPhone = _fixture.Create<string>();
            contactAccount.MailAddress = _fixture.Create<string>();
            contactAccount.PaymentTerm = paymentTermCollection[_random.Next(0, paymentTermCollection.Length - 1)];

            IContactAccount result = await sut.UpdateContactAccountAsync(contactAccount);

            Assert.That(result, Is.Not.Null);
        }
    }
}