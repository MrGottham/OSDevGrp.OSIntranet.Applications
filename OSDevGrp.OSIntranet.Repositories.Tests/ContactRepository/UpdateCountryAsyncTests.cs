using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class UpdateCountryAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async System.Threading.Tasks.Task UpdateCountryAsync_WhenCountryIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = (await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateCountryAsync(null)))!;

            Assert.That(result.ParamName, Is.EqualTo("country"));
        }
    }
}
