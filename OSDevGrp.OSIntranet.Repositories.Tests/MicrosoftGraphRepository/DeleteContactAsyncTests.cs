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
    public class DeleteContactAsyncTests : MicrosoftGraphRepositoryTestBase
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
        public void DeleteContactAsync_WhenRefreshableTokenIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAsync(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("refreshableToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactAsync_WhenIdentifierIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAsync(_fixture.BuildRefreshableTokenMock().Object, null));

            Assert.That(result.ParamName, Is.EqualTo("identifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactAsync_WhenIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAsync(_fixture.BuildRefreshableTokenMock().Object, string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("identifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContactAsync_WhenIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContactAsync(_fixture.BuildRefreshableTokenMock().Object, " "));

            Assert.That(result.ParamName, Is.EqualTo("identifier"));
        }

        [Test]
        [Category("IntegrationTest")] 
        [Ignore("Test which communicate with Microsoft Graph should only be executed when we have an access token")]
        public async Task DeleteContactAsync_WhenCalled_DeletesContact()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            IName name = new PersonName("Albert", "Johnson");
            IContact contact = new Contact(name);

            IContact createdContact = await sut.CreateContactAsync(CreateToken(), contact);

            await sut.DeleteContactAsync(CreateToken(), createdContact.ExternalIdentifier);
        }
    }
}
