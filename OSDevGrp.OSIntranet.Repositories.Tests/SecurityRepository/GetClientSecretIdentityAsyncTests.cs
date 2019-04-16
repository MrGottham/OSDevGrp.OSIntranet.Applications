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
    public class GetClientSecretIdentityAsyncTests : SecurityRepositoryTestBase
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
        [Category("IntegrationTest")]
        public async Task GetClientSecretIdentityAsync_WhenCalledWithClientSecretIdentityIdentifier_ReturnsClientSecretIdentity()
        {
            ISecurityRepository sut = CreateSut();

            IList<IClientSecretIdentity> clientSecretIdentityCollection = (await sut.GetClientSecretIdentitiesAsync()).ToList();
            IClientSecretIdentity result = await sut.GetClientSecretIdentityAsync(clientSecretIdentityCollection[_random.Next(0, clientSecretIdentityCollection.Count - 1)].Identifier);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetClientSecretIdentityAsync_WhenClientIdIsNull_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetClientSecretIdentityAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClientSecretIdentityAsync_WhenClientIdIsEmpty_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetClientSecretIdentityAsync(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetClientSecretIdentityAsync_WhenClientIdIsWhiteSpace_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetClientSecretIdentityAsync(" "));

            Assert.That(result.ParamName, Is.EqualTo("clientId"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetClientSecretIdentityAsync_WhenCalledWithClientId_ReturnsClientSecretIdentity()
        {
            ISecurityRepository sut = CreateSut();

            IList<IClientSecretIdentity> clientSecretIdentityCollection = (await sut.GetClientSecretIdentitiesAsync()).ToList();
            IClientSecretIdentity result = await sut.GetClientSecretIdentityAsync(clientSecretIdentityCollection[_random.Next(0, clientSecretIdentityCollection.Count - 1)].ClientId);

            Assert.That(result, Is.Not.Null);
        }
    }
}
