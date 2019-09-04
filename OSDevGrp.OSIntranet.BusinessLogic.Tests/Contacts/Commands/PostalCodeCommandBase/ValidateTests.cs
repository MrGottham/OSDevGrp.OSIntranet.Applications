using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.PostalCodeCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IPostalCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _contactRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            IPostalCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidator()
        {
            string countryCode = _fixture.Create<string>();
            IPostalCodeCommand sut = CreateSut(countryCode);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<string>(value => string.CompareOrdinal(value, countryCode.ToUpper()) == 0),
                    It.IsNotNull<Func<string, Task<bool>>>(),
                    It.Is<Type>(value => sut.GetType() == value),
                    It.Is<string>(value => string.CompareOrdinal(value, "CountryCode") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForCity()
        {
            string city = _fixture.Create<string>();
            ICountryIdentificationCommand sut = CreateSut(city: city);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, city) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "City") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForCity()
        {
            string city = _fixture.Create<string>();
            ICountryIdentificationCommand sut = CreateSut(city: city);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, city) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "City") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForCity()
        {
            string city = _fixture.Create<string>();
            ICountryIdentificationCommand sut = CreateSut(city: city);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, city) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "City") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForState()
        {
            string state = _fixture.Create<string>();
            ICountryIdentificationCommand sut = CreateSut(state: state);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, state) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "State") == 0),
                    It.Is<bool>(value => value)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForState()
        {
            string state = _fixture.Create<string>();
            ICountryIdentificationCommand sut = CreateSut(state: state);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, state) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "State") == 0),
                    It.Is<bool>(value => value)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IPostalCodeCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IPostalCodeCommand CreateSut(string countryCode = null, string city = null, string state = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.CountryCode, countryCode ?? _fixture.Create<string>())
                .With(m => m.City, city ?? _fixture.Create<string>())
                .With(m => m.State, state ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Commands.PostalCodeCommandBase
        {
        }
    }
}
