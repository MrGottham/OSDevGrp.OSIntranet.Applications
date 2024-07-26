using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithClaimsIdentityTests
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
        public void Resolve_WhenClaimsIdentityIsNull_ThrowsArgumentNullException(bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve((ClaimsIdentity) null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimsIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithUnauthenticatedClaimsIdentity_ReturnsNotNull(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithUnauthenticatedClaimsIdentity_ReturnsExpectedErrorResponseModel(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: false);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsNotNull(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingNoEmailClaim_ReturnsExpectedErrorResponseModel(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: false);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsNotNull(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true,hasValueInEmailClaim: false);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingEmailClaimWithoutValue_ReturnsExpectedErrorResponseModel(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: false);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingEmailClaimWithValue_ThrowsNotSupportedException(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingEmailClaimWithValue_ThrowsNotSupportedExceptionWhereMessageIsNotNull(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingEmailClaimWithValue_ThrowsNotSupportedExceptionWhereMessageIsNotEmpty(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticatedClaimsIdentityContainingEmailClaimWithValue_ThrowsNotSupportedExceptionWhereInnerExceptionIsNull(bool withState)
        {
            ClaimsIdentity claimsIdentity = CreateClaimsIdentity(isAuthenticated: true, hasEmailClaim: true, hasValueInEmailClaim: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(claimsIdentity, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        private ClaimsIdentity CreateClaimsIdentity(bool isAuthenticated = true, bool hasEmailClaim = true, bool hasValueInEmailClaim = true)
        {
            if (isAuthenticated == false)
            {
                return new ClaimsIdentity();
            }

            IList<Claim> claims = new List<Claim>
            {
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>()),
                new(_fixture.Create<string>(), _fixture.Create<string>())
            };

            if (hasEmailClaim)
            {
                claims.Add(new Claim(ClaimTypes.Email, hasValueInEmailClaim ? _fixture.Create<string>() : string.Empty));
            }

            return new ClaimsIdentity(claims, _fixture.Create<string>());
        }
    }
}