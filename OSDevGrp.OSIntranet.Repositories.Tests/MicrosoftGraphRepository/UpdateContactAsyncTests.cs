using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MicrosoftGraphRepository
{
    [TestFixture]
    public class UpdateContactAsyncTests : MicrosoftGraphRepositoryTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAsync_WhenRefreshableTokenIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAsync(null, _fixture.BuildContactMock().Object));

            Assert.That(result.ParamName, Is.EqualTo("refreshableToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAsync_WhenContactIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAsync(_fixture.BuildRefreshableTokenMock().Object, null));

            Assert.That(result.ParamName, Is.EqualTo("contact"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which communicate with Microsoft Graph should only be executed when we have an access token")]
        public async Task UpdateContactAsync_WhenCalled_CreatesContact()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            IName name = new PersonName("Albert", "Johnson");
            IContact contact = new Contact(name);

            IContact createdContact = await sut.CreateContactAsync(CreateToken(), contact);
            try
            {
                createdContact.Address.StreetLine1 = "1234, Johnson Street";
                createdContact.Address.StreetLine2 = "Johnson Town";
                createdContact.Address.PostalCode = "12345";
                createdContact.Address.City = "Petersburg";
                createdContact.Address.State = "TX";
                createdContact.Birthday = new DateTime(1970, 7, 15);
                createdContact.HomePhone = "+1 123 4444 5555";
                createdContact.MailAddress = "albert.johnson@gmail.com";
                IContact updatedContact = await sut.UpdateContactAsync(CreateToken(), createdContact);

                Assert.That(updatedContact, Is.Not.Null);

                updatedContact.Address.StreetLine1 = null;
                updatedContact.Address.StreetLine2 = null;
                updatedContact.Address.PostalCode = null;
                updatedContact.Address.City = null;
                updatedContact.Address.State = null;
                updatedContact.Birthday = null;
                updatedContact.HomePhone = null;
                updatedContact.MailAddress = null;
                updatedContact = await sut.UpdateContactAsync(CreateToken(), updatedContact);

                Assert.That(updatedContact, Is.Not.Null);
            }
            finally
            {
                await sut.DeleteContactAsync(CreateToken(), createdContact.ExternalIdentifier);
            }
        }
    }
}
