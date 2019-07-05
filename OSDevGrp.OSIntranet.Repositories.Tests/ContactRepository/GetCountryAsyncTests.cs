using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class GetCountryAsyncTests : ContactRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void GetCountryAsync_WhenCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetCountryAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetCountryAsync_WhenCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetCountryAsync(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetCountryAsync_WhenCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetCountryAsync(" "));

            Assert.That(result.ParamName, Is.EqualTo("code"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetCountryAsync_WhenCalled_ReturnsCountry()
        {
            IContactRepository sut = CreateSut();

            ICountry result = await sut.GetCountryAsync("DK");

            Assert.That(result, Is.Not.Null);
        }
    }
}
