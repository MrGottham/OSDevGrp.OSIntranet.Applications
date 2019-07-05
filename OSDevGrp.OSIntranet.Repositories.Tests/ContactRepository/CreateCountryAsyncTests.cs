using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class CreateCountryAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void CreateCountryAsync_WhenCountryIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateCountryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("country"));
        }
    }
}
