using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithAuthenticateResultTests
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
        public void Resolve_WhenAuthenticateResultIsNull_ThrowsArgumentNullException(bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve((AuthenticateResult) null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authenticateResult"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToNoResult_ReturnsNotNull(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.NoResult();
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToNoResult_ReturnsExpectedErrorResponseModel(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.NoResult();
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToFail_ReturnsNotNull(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Fail(new UnauthorizedAccessException());
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToFail_ReturnsExpectedErrorResponseModel(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Fail(new UnauthorizedAccessException());
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToSuccessWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsNotNull(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipalWithoutClaimsIdentity(), _fixture.Create<string>()));
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToSuccessWhereClaimsPrincipalHasNoClaimsIdentity_ReturnsExpectedErrorResponseModel(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipalWithoutClaimsIdentity(), _fixture.Create<string>()));
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToSuccessWhereClaimsPrincipalHasClaimsIdentity_ThrowsNotSupportedException(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipalWithClaimsIdentity(), _fixture.Create<string>()));
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToSuccessWhereClaimsPrincipalHasClaimsIdentity_ThrowsNotSupportedExceptionWhereMessageIsNotNull(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipalWithClaimsIdentity(), _fixture.Create<string>()));
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToSuccessWhereClaimsPrincipalHasClaimsIdentity_ThrowsNotSupportedExceptionWhereMessageIsNotEmpty(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipalWithClaimsIdentity(), _fixture.Create<string>()));
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticateResultEqualToSuccessWhereClaimsPrincipalHasClaimsIdentity_ThrowsNotSupportedExceptionWhereInnerExceptionIsNull(bool withState)
        {
            AuthenticateResult authenticateResult = AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipalWithClaimsIdentity(), _fixture.Create<string>()));
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticateResult, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        private static ClaimsPrincipal CreateClaimsPrincipalWithoutClaimsIdentity()
        {
            return new ClaimsPrincipal();
        }

        private static ClaimsPrincipal CreateClaimsPrincipalWithClaimsIdentity()
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }
    }
}