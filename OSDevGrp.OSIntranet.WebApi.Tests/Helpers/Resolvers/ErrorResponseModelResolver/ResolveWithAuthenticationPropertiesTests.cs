using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.ErrorResponseModelResolver
{
    [TestFixture]
    public class ResolveWithAuthenticationPropertiesTests
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
        public void Resolve_WhenAuthenticationPropertiesIsNull_ThrowsArgumentNullException(bool withState)
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve((AuthenticationProperties) null, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("authenticationProperties"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsEmpty_ReturnsNotNull(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithEmptyItems();
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsEmpty_ReturnsExpectedErrorResponseModel(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithEmptyItems();
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndNotContainingAuthorizationStateKey_ReturnsNotNull(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: false);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndNotContainingAuthorizationStateKey_ReturnsExpectedErrorResponseModel(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: false);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndContainingAuthorizationStateKeyWithoutValue_ReturnsNotNull(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndContainingAuthorizationStateKeyWithoutValue_ReturnsExpectedErrorResponseModel(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: false);
            string state = withState ? _fixture.Create<string>() : null;
            ErrorResponseModel result = WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, state);

            result.AssertExpectedValues("access_denied", WebApi.Helpers.Resolvers.ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndContainingAuthorizationStateKeyWithValue_ThrowsNotSupportedException(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndContainingAuthorizationStateKeyWithValue_ThrowsNotSupportedExceptionWhereMessageIsNotNull(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndContainingAuthorizationStateKeyWithValue_ThrowsNotSupportedExceptionWhereMessageIsNotEmpty(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Resolve_WhenCalledWithAuthenticationPropertiesWhereItemsIsNotEmptyAndContainingAuthorizationStateKeyWithValue_ThrowsNotSupportedExceptionWhereInnerExceptionIsNull(bool withState)
        {
            AuthenticationProperties authenticationProperties = CreateAuthenticationPropertiesWithItems(hasAuthorizationStateKey: true, hasValueForAuthorizationStateKey: true);
            NotSupportedException result = Assert.Throws<NotSupportedException>(() => WebApi.Helpers.Resolvers.ErrorResponseModelResolver.Resolve(authenticationProperties, withState ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        private AuthenticationProperties CreateAuthenticationPropertiesWithEmptyItems()
        {
            return new AuthenticationProperties();
        }

        private AuthenticationProperties CreateAuthenticationPropertiesWithItems(bool hasAuthorizationStateKey = true, bool hasValueForAuthorizationStateKey = true)
        {
            IDictionary<string, string> items = new Dictionary<string, string>
            {
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()},
                {_fixture.Create<string>(), _fixture.Create<string>()}
            };

            if (hasAuthorizationStateKey)
            {
                items.Add(KeyNames.AuthorizationStateKey, hasValueForAuthorizationStateKey ? _fixture.Create<string>() : null);
            }

            return new AuthenticationProperties(items);
        }
    }
}