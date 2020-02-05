using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.AddressCommand
{
    [TestFixture]
    public class IsEmptyTests
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
        public void IsEmpty_WhenCommandDoesNotHaveValueInAnyProperties_ReturnsTrue()
        {
            IAddressCommand sut = CreateSut();

            bool result = sut.IsEmpty();

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenCommandHasValueInStreetLine1_ReturnsFalse()
        {
            IAddressCommand sut = CreateSut(true);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenCommandHasValueInStreetLine2_ReturnsFalse()
        {
            IAddressCommand sut = CreateSut(hasStreetLine2: true);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenCommandHasValueInPostalCode_ReturnsFalse()
        {
            IAddressCommand sut = CreateSut(hasPostalCode: true);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenCommandHasValueInCity_ReturnsFalse()
        {
            IAddressCommand sut = CreateSut(hasCity: true);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenCommandHasValueInState_ReturnsFalse()
        {
            IAddressCommand sut = CreateSut(hasState: true);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void IsEmpty_WhenCommandHasValueInCountry_ReturnsFalse()
        {
            IAddressCommand sut = CreateSut(hasCountry: true);

            bool result = sut.IsEmpty();

            Assert.That(result, Is.False);
        }

        private IAddressCommand CreateSut(bool hasStreetLine1 = false, bool hasStreetLine2 = false, bool hasPostalCode = false, bool hasCity = false, bool hasState = false, bool hasCountry = false)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.AddressCommand>()
                .With(m => m.StreetLine1, hasStreetLine1 ? _fixture.Create<string>() : null)
                .With(m => m.StreetLine2, hasStreetLine2 ? _fixture.Create<string>() : null)
                .With(m => m.PostalCode, hasPostalCode ? _fixture.Create<string>() : null)
                .With(m => m.City, hasCity ? _fixture.Create<string>() : null)
                .With(m => m.State, hasState ? _fixture.Create<string>() : null)
                .With(m => m.Country, hasCountry ? _fixture.Create<string>() : null)
                .Create();
        }
    }
}