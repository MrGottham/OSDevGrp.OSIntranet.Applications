using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.PostalCodeCommandBase
{
    [TestFixture]
    public class ToDomainTests
    {
        #region Private variables

        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            IPostalCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ToDomain(null));
            
            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_AssertGetCountryAsyncWasCalledOnContactRepository()
        {
            string countryCode = _fixture.Create<string>();
            IPostalCodeCommand sut = CreateSut(countryCode);

            sut.ToDomain(_contactRepositoryMock.Object);

            _contactRepositoryMock.Verify(m => m.GetCountryAsync(It.Is<string>(value => string.CompareOrdinal(value, countryCode.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCode()
        {
            IPostalCodeCommand sut = CreateSut();

            IPostalCode result = sut.ToDomain(_contactRepositoryMock.Object);

            Assert.That(result, Is.TypeOf<PostalCode>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithCountryFromContactRepository()
        {
            ICountry country = _fixture.BuildCountryMock().Object;
            IPostalCodeCommand sut = CreateSut(country: country);

            ICountry result = sut.ToDomain(_contactRepositoryMock.Object).Country;

            Assert.That(result, Is.EqualTo(country));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithCodeFromCommand()
        {
            string postalCode = _fixture.Create<string>();
            IPostalCodeCommand sut = CreateSut(postalCode: postalCode);

            string result = sut.ToDomain(_contactRepositoryMock.Object).Code;

            Assert.That(result, Is.EqualTo(postalCode));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithCityFromCommand()
        {
            string city = _fixture.Create<string>();
            IPostalCodeCommand sut = CreateSut(city: city);

            string result = sut.ToDomain(_contactRepositoryMock.Object).City;

            Assert.That(result, Is.EqualTo(city));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithStateFromCommandWhenStateIsNull()
        {
            IPostalCodeCommand sut = CreateSut();

            string result = sut.ToDomain(_contactRepositoryMock.Object).State;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithStateFromCommandWhenStateIsEmpty()
        {
            string state = string.Empty;
            IPostalCodeCommand sut = CreateSut(state: state);

            string result = sut.ToDomain(_contactRepositoryMock.Object).State;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithStateFromCommandWhenStateIsWhiteSpace()
        {
            string state = " ";
            IPostalCodeCommand sut = CreateSut(state: state);

            string result = sut.ToDomain(_contactRepositoryMock.Object).State;

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsPostalCodeWithStateFromCommandWhenStateIsNotNullEmptyOrWhiteSpace()
        {
            string state = _fixture.Create<string>();
            IPostalCodeCommand sut = CreateSut(state: state);

            string result = sut.ToDomain(_contactRepositoryMock.Object).State;

            Assert.That(result, Is.EqualTo(state));
        }

        private IPostalCodeCommand CreateSut(string countryCode = null, ICountry country = null, string postalCode = null, string city = null, string state = null)
        {
            _contactRepositoryMock.Setup(m => m.GetCountryAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => country ?? _fixture.BuildCountryMock().Object));

            return _fixture.Build<Sut>()
                .With(m => m.CountryCode, countryCode ?? _fixture.Create<string>())
                .With(m => m.PostalCode, postalCode ?? _fixture.Create<string>())
                .With(m => m.City, city ?? _fixture.Create<string>())
                .With(m => m.State, state)
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Commands.PostalCodeCommandBase
        {
        }
    }
}
