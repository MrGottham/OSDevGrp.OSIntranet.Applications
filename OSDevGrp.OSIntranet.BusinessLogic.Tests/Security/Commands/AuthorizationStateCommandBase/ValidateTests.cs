using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.AuthorizationStateCommandBase
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
            IAuthorizationStateCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertStringWasCalledThreeTimesOnValidator()
        {
            IAuthorizationStateCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(3));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertObjectWasCalledOnValidator()
        {
            IAuthorizationStateCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithAuthorizationState()
        {
            string authorizationState = _fixture.Create<string>();
            IAuthorizationStateCommand sut = CreateSut(authorizationState: authorizationState);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, authorizationState) == 0),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationState") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinLengthWasCalledOnStringValidatorWithAuthorizationState()
        {
            string authorizationState = _fixture.Create<string>();
            IAuthorizationStateCommand sut = CreateSut(authorizationState: authorizationState);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldHaveMinLength(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, authorizationState) == 0),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationState") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldMatchPatternWasCalledOnStringValidatorWithAuthorizationState()
        {
            string authorizationState = _fixture.Create<string>();
            IAuthorizationStateCommand sut = CreateSut(authorizationState: authorizationState);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldMatchPattern(
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, authorizationState) == 0),
                    It.Is<Regex>(value => value != null && string.CompareOrdinal(value.ToString(), RegexTestHelper.Base64Pattern) == 0),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationState") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithUnprotect()
        {
            Func<byte[], byte[]> unprotect = bytes => bytes;
            IAuthorizationStateCommand sut = CreateSut(unprotect: unprotect);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Func<byte[], byte[]>>(value => value != null && value == unprotect),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Unprotect") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidatorFromArguments()
        {
            IAuthorizationStateCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IAuthorizationStateCommand CreateSut(string authorizationState = null, Func<byte[], byte[]> unprotect = null)
        {
            return new MyAuthorizationStateCommand(authorizationState ?? _fixture.Create<string>(), unprotect ?? (bytes => bytes));
        }

        private class MyAuthorizationStateCommand(string authorizationState, Func<byte[], byte[]> unprotect) : BusinessLogic.Security.Commands.AuthorizationStateCommandBase(authorizationState, unprotect);
    }
}