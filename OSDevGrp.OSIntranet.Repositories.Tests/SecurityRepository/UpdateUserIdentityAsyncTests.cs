using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class UpdateUserIdentityAsyncTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void UpdateUserIdentityAsync_WhenUserIdentityIsNull_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateUserIdentityAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("userIdentity"));
        }
    }
}
