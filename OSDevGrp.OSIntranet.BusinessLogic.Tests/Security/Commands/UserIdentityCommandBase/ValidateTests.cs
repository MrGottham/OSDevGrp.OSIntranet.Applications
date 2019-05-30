using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.UserIdentityCommandBase
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IUserIdentityCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _securityRepositoryMock.Object));
            
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenSecurityRepositoryIsNull_ThrowsArgumentNullException()
        {
            IUserIdentityCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));
            
            Assert.That(result.ParamName, Is.EqualTo("securityRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidator()
        {
            string externalUserIdentifier = _fixture.Create<string>();
            IUserIdentityCommand sut = CreateSut(externalUserIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, externalUserIdentifier) == 0),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "ExternalUserIdentifier", false) == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidator()
        {
            string externalUserIdentifier = _fixture.Create<string>();
            IUserIdentityCommand sut = CreateSut(externalUserIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.CompareOrdinal(value, externalUserIdentifier) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "ExternalUserIdentifier", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMaxLengthWasCalledOnStringValidator()
        {
            string externalUserIdentifier = _fixture.Create<string>();
            IUserIdentityCommand sut = CreateSut(externalUserIdentifier);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMaxLength(
                    It.Is<string>(value => string.CompareOrdinal(value, externalUserIdentifier) == 0),
                    It.Is<int>(value => value == 256),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.Compare(field, "ExternalUserIdentifier", false) == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IUserIdentityCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _securityRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IUserIdentityCommand CreateSut(string externalUserIdentifier = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.ExternalUserIdentifier, externalUserIdentifier ?? _fixture.Create<string>())
                .With(m => m.Claims, new List<Claim>())
                .Create();
        }

        private class Sut : OSDevGrp.OSIntranet.BusinessLogic.Security.Commands.UserIdentityCommandBase
        {
        }
    }
}