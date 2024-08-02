using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class GenerateRedirectUriWithAuthorizationCodeTests : AuthorizationStateTestBase
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("code")]
        [TestCase("Code")]
        [TestCase("CODE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForCode_ThrowsIntranetSystemException(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("code")]
        [TestCase("Code")]
        [TestCase("CODE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForCode_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToUnableToGenerateRedirectUri(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateRedirectUri));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("code")]
        [TestCase("Code")]
        [TestCase("CODE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForCode_ThrowsIntranetSystemExceptionWhereMessageIsNotNull(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("code")]
        [TestCase("Code")]
        [TestCase("CODE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForCode_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("code")]
        [TestCase("Code")]
        [TestCase("CODE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForCode_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("state")]
        [TestCase("State")]
        [TestCase("STATE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForState_ThrowsIntranetSystemException(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("state")]
        [TestCase("State")]
        [TestCase("STATE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForState_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToUnableToGenerateRedirectUri(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateRedirectUri));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("state")]
        [TestCase("State")]
        [TestCase("STATE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForState_ThrowsIntranetSystemExceptionWhereMessageIsNotNull(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("state")]
        [TestCase("State")]
        [TestCase("STATE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForState_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("state")]
        [TestCase("State")]
        [TestCase("STATE")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriContainsQueryParameterForState_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull(string value)
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(CreateQueryParameter(_fixture, name: value));
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsNotInitialized_ThrowsIntranetSystemException()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsNotInitialized_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToUnableToGenerateRedirectUri()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateRedirectUri));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsNotInitialized_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsNotInitialized_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsNotInitialized_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            IAuthorizationState sut = CreateSut(hasAuthorizationCode: false);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsInitialized_AssertValueWasCalledOnAuthorizationCodeInAuthorizationState()
        {
            Mock<IAuthorizationCode> authorizationCodeMock = _fixture.BuildAuthorizationCodeMock();
            IAuthorizationState sut = CreateSut(authorizationCode: authorizationCodeMock.Object);

            sut.GenerateRedirectUriWithAuthorizationCode();

            authorizationCodeMock.Verify(m => m.Value);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsInitializedWithoutValue_ThrowsIntranetSystemException()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(hasValue: false).Object;
            IAuthorizationState sut = CreateSut(authorizationCode: authorizationCode);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsInitializedWithoutValue_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToUnableToGenerateRedirectUri()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(hasValue: false).Object;
            IAuthorizationState sut = CreateSut(authorizationCode: authorizationCode);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.UnableToGenerateRedirectUri));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsInitializedWithoutValue_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(hasValue: false).Object;
            IAuthorizationState sut = CreateSut(authorizationCode: authorizationCode);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsInitializedWithoutValue_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(hasValue: false).Object;
            IAuthorizationState sut = CreateSut(authorizationCode: authorizationCode);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenAuthorizationCodeIsInitializedWithoutValue_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(hasValue: false).Object;
            IAuthorizationState sut = CreateSut(authorizationCode: authorizationCode);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GenerateRedirectUriWithAuthorizationCode());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsNotNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWherePathIsNotNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWherePathIsNotEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWherePathIsEqualToPathFromRedirectUri()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.EqualTo(redirectUri.GetLeftPart(UriPartial.Path)));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryIsNotNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryIsNotEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCode()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("code"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueEqualToValueFromAuthorizationCode()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true, authorizationCode: authorizationCode);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.UrlDecode(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"]), Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForState()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("state"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForStateWithValueNotEqualToNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["state"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForStateWithValueNotEqualToEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["state"], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForStateWithValueEqualToExternalState()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true, externalState: externalState);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.UrlDecode(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["state"]), Is.EqualTo(externalState));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUri()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains(queryParameter.Key), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUriWithValueNotEqualToNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)[queryParameter.Key], Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUriWithValueNotEqualToEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)[queryParameter.Key], Is.Not.Empty);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUriWithMatchingValue()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true, externalState: externalState);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)[queryParameter.Key], Is.EqualTo(queryParameter.Value));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsNotNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWherePathIsNotNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWherePathIsNotEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWherePathIsEqualToPathFromRedirectUri()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.EqualTo(redirectUri.GetLeftPart(UriPartial.Path)));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryIsNotNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryIsNotEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCode()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("code"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueEqualToValueFromAuthorizationCode()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false, authorizationCode: authorizationCode);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.UrlDecode(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"]), Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryDoesNotHaveQueryParameterForState()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("state"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUri()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains(queryParameter.Key), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUriWithValueNotEqualToNull()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)[queryParameter.Key], Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUriWithValueNotEqualToEmpty()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)[queryParameter.Key], Is.Not.Empty);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriHasQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForEachQueryParameterDefinedByRedirectUriWithMatchingValue()
        {
            IReadOnlyDictionary<string, string> queryParameters = CreateQueryParameters(_fixture, _random);
            Uri redirectUri = CreateRedirectUri(_fixture, queryParameters);
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false, externalState: externalState);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            foreach (KeyValuePair<string, string> queryParameter in queryParameters)
            {
                Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)[queryParameter.Key], Is.EqualTo(queryParameter.Value));
            }
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsNotNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWherePathIsNotNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWherePathIsNotEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWherePathIsEqualToPathFromRedirectUri()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.EqualTo(redirectUri.GetLeftPart(UriPartial.Path)));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryIsNotNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryIsNotEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCode()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("code"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueEqualToValueFromAuthorizationCode()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true, authorizationCode: authorizationCode);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.UrlDecode(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"]), Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForState()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("state"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForStateWithValueNotEqualToNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["state"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForStateWithValueNotEqualToEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["state"], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasQueryParameterForStateWithValueEqualToExternalState()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            string externalState = _fixture.Create<string>();
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true, externalState: externalState);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.UrlDecode(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["state"]), Is.EqualTo(externalState));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsSet_ReturnsUriWhereQueryHasOnlyQueryParametersForCodeAndState()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: true);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Length, Is.EqualTo(2));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsNotNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWherePathIsNotNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWherePathIsNotEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWherePathIsEqualToPathFromRedirectUri()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.GetLeftPart(UriPartial.Path), Is.EqualTo(redirectUri.GetLeftPart(UriPartial.Path)));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryIsNotNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryIsNotEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(result.Query, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCode()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("code"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToNull()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueNotEqualToEmpty()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasQueryParameterForCodeWithValueEqualToValueFromAuthorizationCode()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            string value = _fixture.Create<string>();
            IAuthorizationCode authorizationCode = _fixture.BuildAuthorizationCodeMock(value: value).Object;
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false, authorizationCode: authorizationCode);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.UrlDecode(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8)["code"]), Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryDoesNotHaveQueryParameterForState()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Contains("state"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void GenerateRedirectUriWithAuthorizationCode_WhenRedirectUriDoesNotHaveAnyQueryParametersAndExternalStateIsNotSet_ReturnsUriWhereQueryHasOnlyQueryParametersForCodeOnly()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationState sut = CreateSut(redirectUri: redirectUri, hasExternalState: false);

            Uri result = sut.GenerateRedirectUriWithAuthorizationCode();

            Assert.That(HttpUtility.ParseQueryString(result.Query, Encoding.UTF8).AllKeys.Length, Is.EqualTo(1));
        }

        private IAuthorizationState CreateSut(Uri redirectUri = null, bool hasExternalState = true, string externalState = null, bool hasAuthorizationCode = true, IAuthorizationCode authorizationCode = null)
        {
            return CreateSut(_fixture, _random, redirectUri: redirectUri, hasExternalState: hasExternalState, externalState: externalState, hasAuthorizationCode: hasAuthorizationCode, authorizationCode: authorizationCode);
        }
    }
}