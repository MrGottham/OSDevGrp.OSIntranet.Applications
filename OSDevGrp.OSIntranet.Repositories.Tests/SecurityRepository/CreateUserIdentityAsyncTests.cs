using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class CreateUserIdentityAsyncTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateUserIdentityAsync_WhenUserIdentityIsNull_ThrowsArgumentNullException()
        {
            ISecurityRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateUserIdentityAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("userIdentity"));
        }
    }
}
