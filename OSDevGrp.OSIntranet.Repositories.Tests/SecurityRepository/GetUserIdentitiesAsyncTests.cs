using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.SecurityRepository
{
    [TestFixture]
    public class GetUserIdentitiesAsyncTests : SecurityRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetUserIdentitiesAsync_WhenCalled_ReturnsUserIdentities()
        {
            ISecurityRepository sut = CreateSut();

            IList<IUserIdentity> result = (await sut.GetUserIdentitiesAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
