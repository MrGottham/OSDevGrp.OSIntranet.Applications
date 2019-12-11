using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class GetContactGroupsAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactGroupsAsync_WhenCalled_ReturnsContactGroups()
        {
            IContactRepository sut = CreateSut();

            IList<IContactGroup> result = (await sut.GetContactGroupsAsync()).ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
