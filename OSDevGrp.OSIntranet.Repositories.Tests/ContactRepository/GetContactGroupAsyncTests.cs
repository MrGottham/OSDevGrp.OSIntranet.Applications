using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class GetContactGroupAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("IntegrationTest")]
        public async Task GetContactGroupAsync_WhenCalled_ReturnsContactGroup()
        {
            IContactRepository sut = CreateSut();

            IContactGroup result = await sut.GetContactGroupAsync(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}
