using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class UpdateClientSecretIdentityAsyncTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void UpdateClientSecretIdentityAsync_WhenUserIdentityIsNull_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateClientSecretIdentityAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("clientSecretIdentity"));
        }
    }
}
