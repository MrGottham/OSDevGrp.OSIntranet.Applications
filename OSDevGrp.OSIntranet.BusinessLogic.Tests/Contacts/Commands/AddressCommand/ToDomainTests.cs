using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.AddressCommand
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

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandIsEmpty_ReturnsNull()
        {
            IAddressCommand sut = CreateSut(false, hasStreetLine2: false, hasPostalCode: false, hasCity: false, hasState: false, hasCountry: false);

            IAddress result = sut.ToDomain();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandIsNotEmpty_ReturnsAddress()
        {
            IAddressCommand sut = CreateSut();

            IAddress result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<Address>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveStreetLine1_ReturnsAddressWithoutStreetLine1()
        {
            IAddressCommand sut = CreateSut(false);

            string result = sut.ToDomain().StreetLine1;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasStreetLine1_ReturnsAddressWithStreetLine1FromCommand()
        {
            string streetLine1 = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(streetLine1: streetLine1);

            string result = sut.ToDomain().StreetLine1;

            Assert.That(result, Is.EqualTo(streetLine1));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveStreetLine2_ReturnsAddressWithoutStreetLine2()
        {
            IAddressCommand sut = CreateSut(hasStreetLine2: false);

            string result = sut.ToDomain().StreetLine2;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasStreetLine2_ReturnsAddressWithStreetLine2FromCommand()
        {
            string streetLine2 = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(streetLine2: streetLine2);

            string result = sut.ToDomain().StreetLine2;

            Assert.That(result, Is.EqualTo(streetLine2));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHavePostalCode_ReturnsAddressWithoutPostalCode()
        {
            IAddressCommand sut = CreateSut(hasPostalCode: false);

            string result = sut.ToDomain().PostalCode;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasPostalCode_ReturnsAddressWithPostalCodeFromCommand()
        {
            string postalCode = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(postalCode: postalCode);

            string result = sut.ToDomain().PostalCode;

            Assert.That(result, Is.EqualTo(postalCode));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveCity_ReturnsAddressWithoutCity()
        {
            IAddressCommand sut = CreateSut(hasCity: false);

            string result = sut.ToDomain().City;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasCity_ReturnsAddressWithCityFromCommand()
        {
            string city = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(city: city);

            string result = sut.ToDomain().City;

            Assert.That(result, Is.EqualTo(city));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveState_ReturnsAddressWithoutState()
        {
            IAddressCommand sut = CreateSut(hasState: false);

            string result = sut.ToDomain().State;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasState_ReturnsAddressWithStateFromCommand()
        {
            string state = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(state: state);

            string result = sut.ToDomain().State;

            Assert.That(result, Is.EqualTo(state));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandDoesNotHaveCountry_ReturnsAddressWithoutCountry()
        {
            IAddressCommand sut = CreateSut(hasCountry: false);

            string result = sut.ToDomain().Country;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCommandHasCountry_ReturnsAddressWithCountryFromCommand()
        {
            string country = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(country: country);

            string result = sut.ToDomain().Country;

            Assert.That(result, Is.EqualTo(country));
        }

        private IAddressCommand CreateSut(bool hasStreetLine1 = true, string streetLine1 = null, bool hasStreetLine2 = true, string streetLine2 = null, bool hasPostalCode = true, string postalCode = null, bool hasCity = true, string city = null, bool hasState = true, string state = null, bool hasCountry = true, string country = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.AddressCommand>()
                .With(m => m.StreetLine1, hasStreetLine1 ? streetLine1 ?? _fixture.Create<string>() : null)
                .With(m => m.StreetLine2, hasStreetLine2 ? streetLine2 ?? _fixture.Create<string>() : null)
                .With(m => m.PostalCode, hasPostalCode ? postalCode ?? _fixture.Create<string>() : null)
                .With(m => m.City, hasCity ? city ?? _fixture.Create<string>() : null)
                .With(m => m.State, hasState ? state ?? _fixture.Create<string>() : null)
                .With(m => m.Country, hasCountry ? country ?? _fixture.Create<string>() : null)
                .Create();
        }
    }
}