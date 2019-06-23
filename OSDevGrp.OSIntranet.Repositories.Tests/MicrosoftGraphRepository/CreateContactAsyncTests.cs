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
    public class CreateContactAsyncTests : MicrosoftGraphRepositoryTestBase
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
        public void CreateContactAsync_WhenRefreshableTokenIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateContactAsync(null, _fixture.BuildContactMock().Object));

            Assert.That(result.ParamName, Is.EqualTo("refreshableToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void CreateContactAsync_WhenContactIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateContactAsync(_fixture.BuildRefreshableTokenMock().Object, null));

            Assert.That(result.ParamName, Is.EqualTo("contact"));
        }

        [Test]
        [Category("IntegrationTest")]
        [Ignore("Test which communicate with Microsoft Graph should only be executed when we have an access token")]
        public async Task CreateContactAsync_WhenCalled_CreatesContact()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            IName name = new PersonName("Albert", "Johnson");
            IContact contact = new Contact(name);

            IContact result = await sut.CreateContactAsync(CreateToken(), contact);
            try
            {
                Assert.That(result, Is.Not.Null);
            }
            finally
            {
                await sut.DeleteContactAsync(CreateToken(), result.ExternalIdentifier);
            }
        }
    }
}
