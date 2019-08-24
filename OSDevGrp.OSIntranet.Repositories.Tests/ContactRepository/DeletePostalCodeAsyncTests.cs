using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ContactRepository
{
    [TestFixture]
    public class DeletePostalCodeAsyncTests : ContactRepositoryTestBase
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
        public void DeletePostalCodeAsync_WhenCountryCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCodeAsync(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCodeAsync_WhenCountryCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCodeAsync(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCodeAsync_WhenCountryCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCodeAsync(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCodeAsync_WhenPostalCodeIsNull_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCodeAsync(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCodeAsync_WhenPostalCodeIsEmpty_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCodeAsync(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeletePostalCodeAsync_WhenPostalCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            IContactRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeletePostalCodeAsync(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("postalCode"));
        }
    }
}
