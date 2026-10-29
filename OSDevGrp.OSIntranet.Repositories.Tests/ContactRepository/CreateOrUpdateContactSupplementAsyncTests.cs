using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class CreateOrUpdateContactSupplementAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task CreateOrUpdateContactSupplementAsync_WhenContactIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateOrUpdateContactSupplementAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("contact"));
        }
    }
}
