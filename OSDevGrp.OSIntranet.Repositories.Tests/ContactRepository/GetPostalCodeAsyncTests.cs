using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class GetPostalCodeAsyncTests : ContactRepositoryTestBase
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void GetPostalCodeAsync_WhenCountryCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetPostalCodeAsync(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetPostalCodeAsync_WhenCountryCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetPostalCodeAsync(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetPostalCodeAsync_WhenCountryCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetPostalCodeAsync(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetPostalCodeAsync_WhenPostalCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetPostalCodeAsync(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetPostalCodeAsync_WhenPostalCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetPostalCodeAsync(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetPostalCodeAsync_WhenPostalCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetPostalCodeAsync(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("IntegrationTest")]
        public async Task GetPostalCodeAsync_WhenCalled_ReturnsPostalCode()
        {
            IContactRepository sut = CreateSut();

            IPostalCode result = await sut.GetPostalCodeAsync("DK", "2720");

            Assert.That(result, Is.Not.Null);
        }
    }
}
