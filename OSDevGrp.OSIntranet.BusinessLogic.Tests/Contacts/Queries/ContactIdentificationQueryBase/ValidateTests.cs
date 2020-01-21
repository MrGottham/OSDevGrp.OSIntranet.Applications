using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Queries.ContactIdentificationQueryBase
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
            IContactIdentificationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForExternalIdentifier()
        {
            string externalIdentifier = _fixture.Create<string>();
            IContactIdentificationQuery sut = CreateSut(externalIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, externalIdentifier) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "ExternalIdentifier") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IContactIdentificationQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IContactIdentificationQuery CreateSut(string externalIdentifier = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.ExternalIdentifier, externalIdentifier ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Queries.ContactIdentificationQueryBase
        {
        }
    }
}