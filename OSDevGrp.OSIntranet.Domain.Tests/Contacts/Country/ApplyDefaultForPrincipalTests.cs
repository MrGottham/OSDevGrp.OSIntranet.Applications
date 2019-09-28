using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Domain.Tests.Contacts.Country
{
    [TestFixture]
    public class ApplyDefaultForPrincipalTests
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
        public void ApplyDefaultForPrincipal_WhenDefaultCountryCodeIsNull_AssertDefaultForPrincipalEqualToFalse()
        {
            ICountry sut = CreateSut();

            sut.ApplyDefaultForPrincipal(null);

            Assert.That(sut.DefaultForPrincipal, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyDefaultForPrincipal_WhenDefaultCountryCodeIsEmpty_AssertDefaultForPrincipalEqualToFalse()
        {
            ICountry sut = CreateSut();

            sut.ApplyDefaultForPrincipal(string.Empty);

            Assert.That(sut.DefaultForPrincipal, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyDefaultForPrincipal_WhenDefaultCountryCodeIsWhiteSpace_AssertDefaultForPrincipalEqualToFalse()
        {
            ICountry sut = CreateSut();

            sut.ApplyDefaultForPrincipal(" ");

            Assert.That(sut.DefaultForPrincipal, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyDefaultForPrincipal_WhenDefaultCountryCodeDoesNotMatchCountryCode_AssertDefaultForPrincipalEqualToFalse()
        {
            ICountry sut = CreateSut();

            sut.ApplyDefaultForPrincipal(_fixture.Create<string>());

            Assert.That(sut.DefaultForPrincipal, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyDefaultForPrincipal_WhenDefaultCountryCodeMatchCountryCode_AssertDefaultForPrincipalEqualToTrue()
        {
            string countryCode = _fixture.Create<string>();
            ICountry sut = CreateSut(countryCode);

            sut.ApplyDefaultForPrincipal(countryCode.ToUpper());

            Assert.That(sut.DefaultForPrincipal, Is.True);
        }

        private ICountry CreateSut(string code = null)
        {
            return new Domain.Contacts.Country(code ?? _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>());
        }
    }
}
