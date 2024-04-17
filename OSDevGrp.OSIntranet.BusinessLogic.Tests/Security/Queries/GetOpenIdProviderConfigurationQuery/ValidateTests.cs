using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Queries.GetOpenIdProviderConfigurationQuery
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
            IGetOpenIdProviderConfigurationQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertObjectWasCalledElevenOnValidator()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut();

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ValidatorMock.Verify(m => m.Object, Times.Exactly(11));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorForAuthorizationEndpoint()
        {
            Uri authorizationEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(authorizationEndpoint: authorizationEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Uri>(value => value != null && value == authorizationEndpoint),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationEndpoint") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForAuthorizationEndpoint()
        {
            Uri authorizationEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(authorizationEndpoint: authorizationEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == authorizationEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationEndpoint") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorForTokenEndpoint()
        {
            Uri tokenEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(tokenEndpoint: tokenEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Uri>(value => value != null && value == tokenEndpoint),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "TokenEndpoint") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForTokenEndpoint()
        {
            Uri tokenEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(tokenEndpoint: tokenEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == tokenEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "TokenEndpoint") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullWasCalledOnObjectValidatorForJsonWebKeySetEndpoint()
        {
            Uri jsonWebKeySetEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(jsonWebKeySetEndpoint: jsonWebKeySetEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldNotBeNull(
                    It.Is<Uri>(value => value != null && value == jsonWebKeySetEndpoint),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "JsonWebKeySetEndpoint") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeKnownValueWasCalledOnObjectValidatorForJsonWebKeySetEndpoint()
        {
            Uri jsonWebKeySetEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(jsonWebKeySetEndpoint: jsonWebKeySetEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == jsonWebKeySetEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "JsonWebKeySetEndpoint") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenUserInfoEndpointIsSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForUserInfoEndpoint()
        {
            Uri userInfoEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasUserInfoEndpoint: true, userInfoEndpoint: userInfoEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == userInfoEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "UserInfoEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenUserInfoEndpointIsNotSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForUserInfoEndpoint()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasUserInfoEndpoint: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value == null),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "UserInfoEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenRegistrationEndpointIsSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForRegistrationEndpoint()
        {
            Uri registrationEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasRegistrationEndpoint: true, registrationEndpoint: registrationEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == registrationEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RegistrationEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenRegistrationEndpointIsNotSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForRegistrationEndpoint()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasRegistrationEndpoint: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value == null),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RegistrationEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenServiceDocumentationEndpointIsSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForServiceDocumentationEndpoint()
        {
            Uri serviceDocumentationEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasServiceDocumentationEndpoint: true, serviceDocumentationEndpoint: serviceDocumentationEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == serviceDocumentationEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ServiceDocumentationEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenServiceDocumentationEndpointIsNotSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForServiceDocumentationEndpoint()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasServiceDocumentationEndpoint: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value == null),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "ServiceDocumentationEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenRegistrationPolicyEndpointEndpointIsSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForRegistrationPolicyEndpoint()
        {
            Uri registrationPolicyEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasRegistrationPolicyEndpoint: true, registrationPolicyEndpoint: registrationPolicyEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == registrationPolicyEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RegistrationPolicyEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenRegistrationPolicyEndpointIsNotSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForRegistrationPolicyEndpoint()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasRegistrationPolicyEndpoint: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value == null),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RegistrationPolicyEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenRegistrationTermsOfServiceEndpointIsSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForRegistrationTermsOfServiceEndpoint()
        {
            Uri registrationTermsOfServiceEndpoint = CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasRegistrationTermsOfServiceEndpoint: true, registrationTermsOfServiceEndpoint: registrationTermsOfServiceEndpoint);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value != null && value == registrationTermsOfServiceEndpoint),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RegistrationTermsOfServiceEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenRegistrationTermsOfServiceEndpointIsNotSet_AssertShouldBeKnownValueWasCalledOnObjectValidatorForRegistrationTermsOfServiceEndpoint()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut(hasRegistrationTermsOfServiceEndpoint: false);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeKnownValue(
                    It.Is<Uri>(value => value == null),
                    It.Is<Func<Uri, Task<bool>>>(value => value != null),
                    It.Is<Type>(value => value != null && value == sut.GetType()),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "RegistrationTermsOfServiceEndpoint") == 0),
                    It.Is<bool>(value => value)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsNotNull()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidatorFromArgument()
        {
            IGetOpenIdProviderConfigurationQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGetOpenIdProviderConfigurationQuery CreateSut(Uri authorizationEndpoint = null, Uri tokenEndpoint = null, Uri jsonWebKeySetEndpoint = null, bool? hasUserInfoEndpoint = null, Uri userInfoEndpoint = null, bool? hasRegistrationEndpoint = null, Uri registrationEndpoint = null, bool? hasServiceDocumentationEndpoint = null, Uri serviceDocumentationEndpoint = null, bool? hasRegistrationPolicyEndpoint = null, Uri registrationPolicyEndpoint = null, bool? hasRegistrationTermsOfServiceEndpoint = null, Uri registrationTermsOfServiceEndpoint = null)
        {
            return new BusinessLogic.Security.Queries.GetOpenIdProviderConfigurationQuery(
                authorizationEndpoint ?? CreateEndpoint(),
                tokenEndpoint ?? CreateEndpoint(),
                jsonWebKeySetEndpoint ?? CreateEndpoint(),
                hasUserInfoEndpoint ?? _random.Next(100) > 50 ? userInfoEndpoint ?? CreateEndpoint() : null,
                hasRegistrationEndpoint ?? _random.Next(100) > 50 ? registrationEndpoint ?? CreateEndpoint() : null,
                hasServiceDocumentationEndpoint ?? _random.Next(100) > 50 ? serviceDocumentationEndpoint ?? CreateEndpoint() : null,
                hasRegistrationPolicyEndpoint ?? _random.Next(100) > 50 ? registrationPolicyEndpoint ?? CreateEndpoint() : null,
                hasRegistrationTermsOfServiceEndpoint ?? _random.Next(100) > 50 ? registrationTermsOfServiceEndpoint ?? CreateEndpoint() : null);
        }

        private Uri CreateEndpoint()
        {
            return new Uri($"https://{_fixture.Create<string>().Replace("/", string.Empty)}.local/{_fixture.Create<string>().Replace("/", string.Empty)}");
        }
    }
}