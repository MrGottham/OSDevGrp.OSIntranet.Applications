using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class GetUserIdentityAsyncTests : SecurityRepositoryTestBase
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
        public void GetUserIdentityAsync_WhenExternalUserIdentifierIsNull_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetUserIdentityAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("externalUserIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetUserIdentityAsync_WhenExternalUserIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetUserIdentityAsync(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("externalUserIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetUserIdentityAsync_WhenExternalUserIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetUserIdentityAsync(" "));

            Assert.That(result.ParamName, Is.EqualTo("externalUserIdentifier"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetUserIdentityAsync_WhenCalled_ReturnsUserIdentity()
        {
            ISecurityRepository sut = CreateSut();

            IList<IUserIdentity> userIdentityCollection = (await sut.GetUserIdentitiesAsync()).ToList();
            IUserIdentity result = await sut.GetUserIdentityAsync(userIdentityCollection[_random.Next(0, userIdentityCollection.Count - 1)].ExternalUserIdentifier);

            Assert.That(result, Is.Not.Null);
        }
    }
}
