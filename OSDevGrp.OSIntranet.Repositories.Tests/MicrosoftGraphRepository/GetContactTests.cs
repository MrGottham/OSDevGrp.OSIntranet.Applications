using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MicrosoftGraphRepository
{
    [TestFixture]
    public class GetContactTests : MicrosoftGraphRepositoryTestBase
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
        public void GetContact_WhenRefreshableTokenIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContact(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("refreshableToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetContact_WhenIdentifierIsNull_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContact(_fixture.BuildRefreshableTokenMock().Object, null));

            Assert.That(result.ParamName, Is.EqualTo("identifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetContact_WhenIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContact(_fixture.BuildRefreshableTokenMock().Object, string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("identifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetContact_WhenIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            IMicrosoftGraphRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetContact(_fixture.BuildRefreshableTokenMock().Object, " "));

            Assert.That(result.ParamName, Is.EqualTo("identifier"));
        }

        [Test]
        [Category("IntegrationTest")]
        [TestCase("[TBD]")]
        [Ignore("Test which communicate with Microsoft Graph should only be executed when we have an access token")]
        public async Task GetContact_WhenCalled_ReturnsContact(string identifier)
        {
            IMicrosoftGraphRepository sut = CreateSut();

            IContact result = await sut.GetContact(CreateToken(), identifier);

            Assert.That(result, Is.Not.Null);
        }
    }
}
