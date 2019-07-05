using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.CountryCommandBase
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        private ICountryCommand CreateSut(string countryCode = null, string name = null, string universalName = null, string phonePrefix = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.CountryCode, countryCode ?? _fixture.Create<string>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.UniversalName, universalName ?? _fixture.Create<string>())
                .With(m => m.PhonePrefix, phonePrefix ?? _fixture.Create<string>())
                .Create();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCountry()
        {
            ICountryCommand sut = CreateSut();

            ICountry result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Country>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCountryWithCodeFromCommand()
        {
            string countryCode = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(countryCode);

            ICountry result = sut.ToDomain();

            Assert.That(result.Code, Is.EqualTo(countryCode.ToUpper()));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCountryWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(name: name);

            ICountry result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCountryWithUniversalNameFromCommand()
        {
            string universalName = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(universalName: universalName);

            ICountry result = sut.ToDomain();

            Assert.That(result.UniversalName, Is.EqualTo(universalName));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsCountryWithPhonePrefixFromCommand()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(phonePrefix: phonePrefix);

            ICountry result = sut.ToDomain();

            Assert.That(result.PhonePrefix, Is.EqualTo(phonePrefix));
        }

        private class Sut : BusinessLogic.Contacts.Commands.CountryCommandBase
        {
        }
    }
}
