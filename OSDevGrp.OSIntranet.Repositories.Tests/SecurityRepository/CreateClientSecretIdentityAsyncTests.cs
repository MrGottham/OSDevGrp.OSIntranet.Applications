using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class CreateClientSecretIdentityAsyncTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateClientSecretIdentityAsync_WhenUserIdentityIsNull_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateClientSecretIdentityAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("clientSecretIdentity"));
        }
    }
}
