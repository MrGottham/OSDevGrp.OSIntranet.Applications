using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithSpecifiedValuesTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsNull_ThrowsArgumentNullException(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(null, withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("error"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsEmpty_ThrowsArgumentNullException(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(string.Empty, withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("error"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsWhiteSpace_ThrowsArgumentNullException(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(" ", withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("error"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsInvalid_ThrowsNotSupportedException(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(_fixture.Create<string>(), withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsInvalid_ThrowsNotSupportedExceptionWhereMessageIsNotNull(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(_fixture.Create<string>(), withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsInvalid_ThrowsNotSupportedExceptionWhereMessageIsNotEmpty(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(_fixture.Create<string>(), withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsInvalid_ThrowsNotSupportedExceptionWhereMessageIsEqualToUnsupportedError(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            string error = _fixture.Create<string>();
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(error, withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo($"Unsupported error: {error}"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Resolve_WhenErrorIsInvalid_ThrowsNotSupportedExceptionWhereInnerExceptionIsNull(bool withErrorDescription, bool withErrorUri, bool withState)
        {
            string error = _fixture.Create<string>();
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(error, withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("invalid_request", true, true, true)]
        [TestCase("invalid_request", true, true, false)]
        [TestCase("invalid_request", true, false, true)]
        [TestCase("invalid_request", true, false, false)]
        [TestCase("invalid_request", false, true, true)]
        [TestCase("invalid_request", false, true, false)]
        [TestCase("invalid_request", false, false, true)]
        [TestCase("invalid_request", false, false, false)]
        [TestCase("invalid_client", true, true, true)]
        [TestCase("invalid_client", true, true, false)]
        [TestCase("invalid_client", true, false, true)]
        [TestCase("invalid_client", true, false, false)]
        [TestCase("invalid_client", false, true, true)]
        [TestCase("invalid_client", false, true, false)]
        [TestCase("invalid_client", false, false, true)]
        [TestCase("invalid_client", false, false, false)]
        [TestCase("invalid_grant", true, true, true)]
        [TestCase("invalid_grant", true, true, false)]
        [TestCase("invalid_grant", true, false, true)]
        [TestCase("invalid_grant", true, false, false)]
        [TestCase("invalid_grant", false, true, true)]
        [TestCase("invalid_grant", false, true, false)]
        [TestCase("invalid_grant", false, false, true)]
        [TestCase("invalid_grant", false, false, false)]
        [TestCase("invalid_scope", true, true, true)]
        [TestCase("invalid_scope", true, true, false)]
        [TestCase("invalid_scope", true, false, true)]
        [TestCase("invalid_scope", true, false, false)]
        [TestCase("invalid_scope", false, true, true)]
        [TestCase("invalid_scope", false, true, false)]
        [TestCase("invalid_scope", false, false, true)]
        [TestCase("invalid_scope", false, false, false)]
        [TestCase("unauthorized_client", true, true, true)]
        [TestCase("unauthorized_client", true, true, false)]
        [TestCase("unauthorized_client", true, false, true)]
        [TestCase("unauthorized_client", true, false, false)]
        [TestCase("unauthorized_client", false, true, true)]
        [TestCase("unauthorized_client", false, true, false)]
        [TestCase("unauthorized_client", false, false, true)]
        [TestCase("unauthorized_client", false, false, false)]
        [TestCase("access_denied", true, true, true)]
        [TestCase("access_denied", true, true, false)]
        [TestCase("access_denied", true, false, true)]
        [TestCase("access_denied", true, false, false)]
        [TestCase("access_denied", false, true, true)]
        [TestCase("access_denied", false, true, false)]
        [TestCase("access_denied", false, false, true)]
        [TestCase("access_denied", false, false, false)]
        [TestCase("unsupported_response_type", true, true, true)]
        [TestCase("unsupported_response_type", true, true, false)]
        [TestCase("unsupported_response_type", true, false, true)]
        [TestCase("unsupported_response_type", true, false, false)]
        [TestCase("unsupported_response_type", false, true, true)]
        [TestCase("unsupported_response_type", false, true, false)]
        [TestCase("unsupported_response_type", false, false, true)]
        [TestCase("unsupported_response_type", false, false, false)]
        [TestCase("unsupported_grant_type", true, true, true)]
        [TestCase("unsupported_grant_type", true, true, false)]
        [TestCase("unsupported_grant_type", true, false, true)]
        [TestCase("unsupported_grant_type", true, false, false)]
        [TestCase("unsupported_grant_type", false, true, true)]
        [TestCase("unsupported_grant_type", false, true, false)]
        [TestCase("unsupported_grant_type", false, false, true)]
        [TestCase("unsupported_grant_type", false, false, false)]
        [TestCase("server_error", true, true, true)]
        [TestCase("server_error", true, true, false)]
        [TestCase("server_error", true, false, true)]
        [TestCase("server_error", true, false, false)]
        [TestCase("server_error", false, true, true)]
        [TestCase("server_error", false, true, false)]
        [TestCase("server_error", false, false, true)]
        [TestCase("server_error", false, false, false)]
        [TestCase("temporarily_unavailable", true, true, true)]
        [TestCase("temporarily_unavailable", true, true, false)]
        [TestCase("temporarily_unavailable", true, false, true)]
        [TestCase("temporarily_unavailable", true, false, false)]
        [TestCase("temporarily_unavailable", false, true, true)]
        [TestCase("temporarily_unavailable", false, true, false)]
        [TestCase("temporarily_unavailable", false, false, true)]
        [TestCase("temporarily_unavailable", false, false, false)]
        public void Resolve_WhenErrorIsValid_ReturnsNotNull(string error, bool withErrorDescription, bool withErrorUri, bool withState)
        {
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(error, withErrorDescription ? _fixture.Create<string>() : null, withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase("invalid_request", true, true, true)]
        [TestCase("invalid_request", true, true, false)]
        [TestCase("invalid_request", true, false, true)]
        [TestCase("invalid_request", true, false, false)]
        [TestCase("invalid_request", false, true, true)]
        [TestCase("invalid_request", false, true, false)]
        [TestCase("invalid_request", false, false, true)]
        [TestCase("invalid_request", false, false, false)]
        [TestCase("invalid_client", true, true, true)]
        [TestCase("invalid_client", true, true, false)]
        [TestCase("invalid_client", true, false, true)]
        [TestCase("invalid_client", true, false, false)]
        [TestCase("invalid_client", false, true, true)]
        [TestCase("invalid_client", false, true, false)]
        [TestCase("invalid_client", false, false, true)]
        [TestCase("invalid_client", false, false, false)]
        [TestCase("invalid_grant", true, true, true)]
        [TestCase("invalid_grant", true, true, false)]
        [TestCase("invalid_grant", true, false, true)]
        [TestCase("invalid_grant", true, false, false)]
        [TestCase("invalid_grant", false, true, true)]
        [TestCase("invalid_grant", false, true, false)]
        [TestCase("invalid_grant", false, false, true)]
        [TestCase("invalid_grant", false, false, false)]
        [TestCase("invalid_scope", true, true, true)]
        [TestCase("invalid_scope", true, true, false)]
        [TestCase("invalid_scope", true, false, true)]
        [TestCase("invalid_scope", true, false, false)]
        [TestCase("invalid_scope", false, true, true)]
        [TestCase("invalid_scope", false, true, false)]
        [TestCase("invalid_scope", false, false, true)]
        [TestCase("invalid_scope", false, false, false)]
        [TestCase("unauthorized_client", true, true, true)]
        [TestCase("unauthorized_client", true, true, false)]
        [TestCase("unauthorized_client", true, false, true)]
        [TestCase("unauthorized_client", true, false, false)]
        [TestCase("unauthorized_client", false, true, true)]
        [TestCase("unauthorized_client", false, true, false)]
        [TestCase("unauthorized_client", false, false, true)]
        [TestCase("unauthorized_client", false, false, false)]
        [TestCase("access_denied", true, true, true)]
        [TestCase("access_denied", true, true, false)]
        [TestCase("access_denied", true, false, true)]
        [TestCase("access_denied", true, false, false)]
        [TestCase("access_denied", false, true, true)]
        [TestCase("access_denied", false, true, false)]
        [TestCase("access_denied", false, false, true)]
        [TestCase("access_denied", false, false, false)]
        [TestCase("unsupported_response_type", true, true, true)]
        [TestCase("unsupported_response_type", true, true, false)]
        [TestCase("unsupported_response_type", true, false, true)]
        [TestCase("unsupported_response_type", true, false, false)]
        [TestCase("unsupported_response_type", false, true, true)]
        [TestCase("unsupported_response_type", false, true, false)]
        [TestCase("unsupported_response_type", false, false, true)]
        [TestCase("unsupported_response_type", false, false, false)]
        [TestCase("unsupported_grant_type", true, true, true)]
        [TestCase("unsupported_grant_type", true, true, false)]
        [TestCase("unsupported_grant_type", true, false, true)]
        [TestCase("unsupported_grant_type", true, false, false)]
        [TestCase("unsupported_grant_type", false, true, true)]
        [TestCase("unsupported_grant_type", false, true, false)]
        [TestCase("unsupported_grant_type", false, false, true)]
        [TestCase("unsupported_grant_type", false, false, false)]
        [TestCase("server_error", true, true, true)]
        [TestCase("server_error", true, true, false)]
        [TestCase("server_error", true, false, true)]
        [TestCase("server_error", true, false, false)]
        [TestCase("server_error", false, true, true)]
        [TestCase("server_error", false, true, false)]
        [TestCase("server_error", false, false, true)]
        [TestCase("server_error", false, false, false)]
        [TestCase("temporarily_unavailable", true, true, true)]
        [TestCase("temporarily_unavailable", true, true, false)]
        [TestCase("temporarily_unavailable", true, false, true)]
        [TestCase("temporarily_unavailable", true, false, false)]
        [TestCase("temporarily_unavailable", false, true, true)]
        [TestCase("temporarily_unavailable", false, true, false)]
        [TestCase("temporarily_unavailable", false, false, true)]
        [TestCase("temporarily_unavailable", false, false, false)]
        public void Resolve_WhenErrorIsValid_ReturnsExpectedErrorResponseModel(string error, bool withErrorDescription, bool withErrorUri, bool withState)
        {
            string errorDescription = withErrorDescription ? _fixture.Create<string>() : null;
            Uri errorUri = withErrorUri ? new Uri($"https://localhost/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute) : null;
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(error, errorDescription, errorUri, state);

            result.AssertExpectedValues(error, errorDescription, errorUri, state);
        }
    }
}