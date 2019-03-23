using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class GetClientSecretIdentitiesAsyncTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetClientSecretIdentitiesAsync_WhenCalled_ReturnsClientSecretIdentities()
        {
            ISecurityRepository sut = CreateSut();

            IList<IClientSecretIdentity> result = (await sut.GetClientSecretIdentitiesAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
