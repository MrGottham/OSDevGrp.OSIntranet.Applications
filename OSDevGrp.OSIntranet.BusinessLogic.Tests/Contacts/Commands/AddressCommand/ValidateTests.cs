using System;
using System.Text.RegularExpressions;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.AddressCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IAddressCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("validator"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMinLengthWasCalledOnStringValidatorForStreetLine1()
        {
            string streetLine1 = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(streetLine1: streetLine1);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, streetLine1) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "StreetLine1") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMinLengthWasCalledOnStringValidatorForStreetLine2()
        {
            string streetLine2 = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(streetLine2: streetLine2);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, streetLine2) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "StreetLine2") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMinLengthWasCalledOnStringValidatorForPostalCode()
        {
            string postalCode = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(postalCode: postalCode);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, postalCode) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PostalCode") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForPostalCode()
        {
            string postalCode = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(postalCode: postalCode);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, postalCode) == 0),
                    It.Is<int>(value => value == 16),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PostalCode") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldMatchPatternWasCalledOnStringValidatorForPostalCode()
        {
            string postalCode = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(postalCode: postalCode);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, postalCode) == 0),
                    It.Is<Regex>(value => string.CompareOrdinal(value.ToString(), RegexTestHelper.PostalCodeRegexPattern) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "PostalCode") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMinLengthWasCalledOnStringValidatorForCity()
        {
            string city = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(city: city);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, city) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "City") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMinLengthWasCalledOnStringValidatorForState()
        {
            string state = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(state: state);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, state) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "State") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_AssertShouldHaveMinLengthWasCalledOnStringValidatorForCountry()
        {
            string country = _fixture.Create<string>();
            IAddressCommand sut = CreateSut(country: country);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, country) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "Country") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorForStreetLine1()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "StreetLine1") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorForStreetLine2()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "StreetLine2") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorForPostalCode()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "PostalCode") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMaxLengthWasNotCalledOnStringValidatorForPostalCode()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "PostalCode") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldMatchPatternWasNotCalledOnStringValidatorForPostalCode()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.IsAny<string>(),
                    It.IsAny<Regex>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "PostalCode") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorForCity()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "City") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorForState()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "State") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_AssertShouldHaveMinLengthWasNotCalledOnStringValidatorForCountry()
        {
            IAddressCommand sut = CreateSut(true);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<Type>(),
                    It.Is<string>(value => string.CompareOrdinal(value, "Country") == 0),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_ReturnsNotNull()
        {
            IAddressCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithNonEmptyAddress_ReturnsValidator()
        {
            IAddressCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_ReturnsNotNull()
        {
            IAddressCommand sut = CreateSut(true);

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalledWithEmptyAddress_ReturnsValidator()
        {
            IAddressCommand sut = CreateSut(true);

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IAddressCommand CreateSut(bool isEmpty = false, string streetLine1 = null, string streetLine2 = null, string postalCode = null, string city = null, string state = null, string country = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.AddressCommand>()
                .With(m => m.StreetLine1, isEmpty ? null : streetLine1 ?? _fixture.Create<string>())
                .With(m => m.StreetLine2, isEmpty ? null : streetLine2 ?? _fixture.Create<string>())
                .With(m => m.PostalCode, isEmpty ? null : postalCode ?? _fixture.Create<string>())
                .With(m => m.City, isEmpty ? null : city ?? _fixture.Create<string>())
                .With(m => m.State, isEmpty ? null : state ?? _fixture.Create<string>())
                .With(m => m.Country, isEmpty ? null : country ?? _fixture.Create<string>())
                .Create();
        }
    }
}