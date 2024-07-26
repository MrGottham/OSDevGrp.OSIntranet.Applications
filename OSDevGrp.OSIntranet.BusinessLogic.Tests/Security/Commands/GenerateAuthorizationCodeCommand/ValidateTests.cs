using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Commands.GenerateAuthorizationCodeCommand
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
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertStringWasCalledThreeTimesOnValidator()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.String, Times.Exactly(3));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertObjectWasCalledTwoTimesOnValidator()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertEnumerableWasCalledTwoTimesOnValidator()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Enumerable, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorWithAuthorizationState()
        {
            string authorizationState = _fixture.Create<string>();
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationState: authorizationState);

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
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationState: authorizationState);

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
            IGenerateAuthorizationCodeCommand sut = CreateSut(authorizationState: authorizationState);

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
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithClaims()
        {
            IReadOnlyCollection<Claim> claims = CreateClaims();
            IGenerateAuthorizationCodeCommand sut = CreateSut(claims: claims);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Claim[]>(value => value != null && claims.All(value.Contains)),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Claims") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldContainItemsWasCalledOnEnumerableValidatorWithClaims()
        {
            IReadOnlyCollection<Claim> claims = CreateClaims();
            IGenerateAuthorizationCodeCommand sut = CreateSut(claims: claims);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldContainItems(
                    It.Is<Claim[]>(value => value != null && claims.All(value.Contains)),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Claims") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldHaveMinItemsWasCalledOnEnumerableValidatorWithClaims()
        {
            IReadOnlyCollection<Claim> claims = CreateClaims();
            IGenerateAuthorizationCodeCommand sut = CreateSut(claims: claims);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.EnumerableValidatorMock.Verify(m => m.ShouldHaveMinItems(
                    It.Is<Claim[]>(value => value != null && claims.All(value.Contains)),
                    It.Is<int>(value => value == 1),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "Claims") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorWithUnprotect()
        {
            Func<byte[], byte[]> unprotect = bytes => bytes;
            IGenerateAuthorizationCodeCommand sut = CreateSut(unprotect: unprotect);

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
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidatorFromArguments()
        {
            IGenerateAuthorizationCodeCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGenerateAuthorizationCodeCommand CreateSut(string authorizationState = null, IReadOnlyCollection<Claim> claims = null, Func<byte[], byte[]> unprotect = null)
        {
            return new BusinessLogic.Security.Commands.GenerateAuthorizationCodeCommand(authorizationState ?? _fixture.Create<string>(), claims ?? CreateClaims(), unprotect ?? (bytes => bytes));
        }

        private IReadOnlyCollection<Claim> CreateClaims()
        {
            return new Claim[]
            {
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>())
            };
        }
    }
}