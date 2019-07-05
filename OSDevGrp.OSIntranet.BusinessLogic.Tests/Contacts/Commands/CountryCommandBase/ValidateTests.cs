using System;
using System.Text.RegularExpressions;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.CountryCommandBase
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
            ICountryCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _contactRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            ICountryCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, name) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Name") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, name) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Name") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForName()
        {
            string name = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(name);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, name) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Name") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForUniversalName()
        {
            string universalName = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(universalName: universalName);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, universalName) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "UniversalName") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForUniversalName()
        {
            string universalName = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(universalName: universalName);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, universalName) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "UniversalName") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForUniversalName()
        {
            string universalName = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(universalName: universalName);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, universalName) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "UniversalName") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForPhonePrefix()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(phonePrefix: phonePrefix);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, phonePrefix) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PhonePrefix") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorForPhonePrefix()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(phonePrefix: phonePrefix);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, phonePrefix) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PhonePrefix") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidatorForPhonePrefix()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(phonePrefix: phonePrefix);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, phonePrefix) == 0),
                    It.Is<int>(value => value == 4),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PhonePrefix") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorForPhonePrefix()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountryCommand sut = CreateSut(phonePrefix: phonePrefix);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.CompareOrdinal(value, phonePrefix) == 0),
                    It.Is<Regex>(value => value != null && string.CompareOrdinal(value.ToString(), @"\+[0-9]{1,3}") == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "PhonePrefix") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            ICountryCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private ICountryCommand CreateSut(string name = null, string universalName = null, string phonePrefix = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.UniversalName, universalName ?? _fixture.Create<string>())
                .With(m => m.PhonePrefix, phonePrefix ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Commands.CountryCommandBase
        {
        }
    }
}
