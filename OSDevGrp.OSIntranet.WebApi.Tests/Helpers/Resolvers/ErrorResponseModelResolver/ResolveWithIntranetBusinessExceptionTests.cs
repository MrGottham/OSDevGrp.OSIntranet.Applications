using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithIntranetBusinessExceptionTests
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
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenIntranetBusinessExceptionIsNull_ThrowsArgumentNullException(bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve((IntranetBusinessException) null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("intranetBusinessException"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveJwtBearerTokenForAuthenticatedClient_ReturnsNotNull(bool withState)
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveJwtBearerTokenForAuthenticatedClient_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient, message: message);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException: intranetBusinessException, state);

            result.AssertExpectedValues("unauthorized_client", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToMissingNecessaryPermission_ReturnsNotNull(bool withState)
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.MissingNecessaryPermission);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToMissingNecessaryPermission_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.MissingNecessaryPermission, message: message);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException: intranetBusinessException, state);

            result.AssertExpectedValues("unauthorized_client", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToAuthorizeUser_ReturnsNotNull(bool withState)
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableToAuthorizeUser_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableToAuthorizeUser, message: message);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException: intranetBusinessException, state);

            result.AssertExpectedValues("access_denied", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableAuthenticateClient_ReturnsNotNull(bool withState)
        {
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsEqualToUnableAuthenticateClient_ReturnsExpectedErrorResponseModel(bool withState)
        {
            string message = _fixture.Create<string>();
            IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode: ErrorCode.UnableAuthenticateClient, message: message);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException: intranetBusinessException, state);

            result.AssertExpectedValues("invalid_client", message, null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsNotSupported_ThrowsNotSupportedException(bool withState)
        {
            foreach (ErrorCode errorCode in GetNoneSupportedErrorCodes())
            {
                IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode);

                NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null));

                Assert.That(result, Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsNotSupported_ThrowsNotSupportedExceptionWhereMessageIsNotNull(bool withState)
        {
            foreach (ErrorCode errorCode in GetNoneSupportedErrorCodes())
            {
                IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode);

                NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null));

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Message, Is.Not.Null);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsNotSupported_ThrowsNotSupportedExceptionWhereMessageIsNotEmpty(bool withState)
        {
            foreach (ErrorCode errorCode in GetNoneSupportedErrorCodes())
            {
                IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode);

                NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null));

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Message, Is.Not.Empty);
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsNotSupported_ThrowsNotSupportedExceptionWhereMessageTellsThatIntranetBusinessExceptionIsUnsupported(bool withState)
        {
            foreach (ErrorCode errorCode in GetNoneSupportedErrorCodes())
            {
                IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode);

                NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null));

                Assert.That(result, Is.Not.Null);
                Assert.That(result.Message, Is.EqualTo($"Unsupported {intranetBusinessException.GetType().Name}: {errorCode}"));
            }
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithIntranetBusinessExceptionWhereErrorCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsNull(bool withState)
        {
            foreach (ErrorCode errorCode in GetNoneSupportedErrorCodes())
            {
                IntranetBusinessException intranetBusinessException = CreateIntranetBusinessException(errorCode);

                NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(intranetBusinessException, withState ? _fixture.Create<string>() : null));

                Assert.That(result, Is.Not.Null);
                Assert.That(result.InnerException, Is.Null);
            }
        }

        private IntranetBusinessException CreateIntranetBusinessException(ErrorCode? errorCode = null, string message = null)
        {
            return new IntranetBusinessException(errorCode ?? _fixture.Create<ErrorCode>(), message ?? _fixture.Create<string>());
        }

        private static IEnumerable<ErrorCode> GetNoneSupportedErrorCodes()
        {
            ErrorCode[] supportedErrorCodes = GetSupportedErrorCodes().ToArray();

            return Enum.GetValues<ErrorCode>()
                .Where(errorCode => supportedErrorCodes.Contains(errorCode) == false)
                .ToArray();
        }

        private static IEnumerable<ErrorCode> GetSupportedErrorCodes()
        {
            yield return ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedClient;
            yield return ErrorCode.MissingNecessaryPermission;
            yield return ErrorCode.UnableToAuthorizeUser;
            yield return ErrorCode.UnableAuthenticateClient;
        }
    }
}