using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.GenerateIdTokenCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IGenerateIdTokenCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertStringWasCalledThreeTimesOnValidator()
        {
            IGenerateIdTokenCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(3));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertObjectWasCalledTwoTimesOnValidator()
        {
            IGenerateIdTokenCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertDateTimeWasOnceOnValidator()
        {
            IGenerateIdTokenCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.DateTime, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithAuthorizationState()
        {
            string authorizationState = _fixture.Create<string>();
            IGenerateIdTokenCommand sut = CreateSut(authorizationState: authorizationState);

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
            IGenerateIdTokenCommand sut = CreateSut(authorizationState: authorizationState);

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
            IGenerateIdTokenCommand sut = CreateSut(authorizationState: authorizationState);

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
            IGenerateIdTokenCommand sut = CreateSut(unprotect: unprotect);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Func<byte[], byte[]>>(value => value != null && value == unprotect),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Unprotect") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithClaimsPrincipal()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal();
            IGenerateIdTokenCommand sut = CreateSut(claimsPrincipal: claimsPrincipal);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<ClaimsPrincipal>(value => value != null && value == claimsPrincipal),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ClaimsPrincipal") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IGenerateIdTokenCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAuthenticationTimeIsUtcTime_AssertShouldBePastDateTimeWasCalledOnDateTimeValidatorWithUnprotect()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.UtcNow;
            IGenerateIdTokenCommand sut = CreateSut(authenticationTime: authenticationTime);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateTime(
                    It.Is<DateTime>(value => value == authenticationTime.UtcDateTime),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthenticationTime") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenAuthenticationTimeIsLocalTime_AssertShouldBePastDateTimeWasCalledOnDateTimeValidatorWithUnprotect()
        {
            DateTimeOffset authenticationTime = DateTimeOffset.Now;
            IGenerateIdTokenCommand sut = CreateSut(authenticationTime: authenticationTime);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.DateTimeValidatorMock.Verify(m => m.ShouldBePastDateTime(
                    It.Is<DateTime>(value => value == authenticationTime.UtcDateTime),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthenticationTime") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidatorFromArguments()
        {
            IGenerateIdTokenCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGenerateIdTokenCommand CreateSut(ClaimsPrincipal claimsPrincipal = null, DateTimeOffset? authenticationTime = null, string authorizationState = null, Func<byte[], byte[]> unprotect = null)
        {
            return new BusinessLogic.Security.Commands.GenerateIdTokenCommand(claimsPrincipal ?? CreateClaimsPrincipal(), authenticationTime ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(300) * -1), authorizationState ?? _fixture.Create<string>(), unprotect ?? (bytes => bytes));
        }

        private static ClaimsPrincipal CreateClaimsPrincipal()
        {
            return new ClaimsPrincipal();
        }
    }
}