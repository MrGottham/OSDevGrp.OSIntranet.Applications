using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.OpenIdProviderConfigurationBuilder
{
    [TestFixture]
    public class BuildTests : OpenIdProviderConfigurationBuilderTestBase
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
        public void Build_WhenCalled_ReturnsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfiguration()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result, Is.TypeOf<Domain.Security.OpenIdProviderConfiguration>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIssuerIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.Issuer, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIssuerIsEqualToIssuerFromConstructor()
        {
            Uri issuer = CreateIssuer(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, issuer: issuer);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.Issuer, Is.EqualTo(issuer));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereAuthorizationEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.AuthorizationEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereAuthorizationEndpointIsEqualToAuthorizationEndpointFromConstructor()
        {
            Uri authorizationEndpoint = CreateAuthorizationEndpoint(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, authorizationEndpoint: authorizationEndpoint);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.AuthorizationEndpoint, Is.EqualTo(authorizationEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.TokenEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointIsEqualToTokenEndpointFromConstructor()
        {
            Uri tokenEndpoint = CreateTokenEndpoint(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, tokenEndpoint: tokenEndpoint);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.TokenEndpoint, Is.EqualTo(tokenEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEndpointCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEndpointIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.UserInfoEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoEndpoint(CreateUserInfoEndpoint(_fixture)).Build();

            Assert.That(result.UserInfoEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEndpointIsEqualToUserInfoEndpointFromWithUserInfoEndpoint()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            Uri userInfoEndpoint = CreateUserInfoEndpoint(_fixture);
            IOpenIdProviderConfiguration result = sut.WithUserInfoEndpoint(userInfoEndpoint).Build();

            Assert.That(result.UserInfoEndpoint, Is.EqualTo(userInfoEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereJsonWebKeySetEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.JsonWebKeySetEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereJsonWebKeySetEndpointIsEqualToJsonWebKeySetEndpointFromConstructor()
        {
            Uri jsonWebKeySetEndpoint = CreateJsonWebKeySetEndpoint(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, jsonWebKeySetEndpoint: jsonWebKeySetEndpoint);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.JsonWebKeySetEndpoint, Is.EqualTo(jsonWebKeySetEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationEndpointCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationEndpointIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RegistrationEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRegistrationEndpoint(CreateRegistrationEndpoint(_fixture)).Build();

            Assert.That(result.RegistrationEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationEndpointsEqualToRegistrationEndpointFromWithRegistrationEndpoint()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            Uri registrationEndpoint = CreateRegistrationEndpoint(_fixture);
            IOpenIdProviderConfiguration result = sut.WithRegistrationEndpoint(registrationEndpoint).Build();

            Assert.That(result.RegistrationEndpoint, Is.EqualTo(registrationEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithScopesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ScopesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithScopesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithScopesSupported(CreateScopesSupported(_fixture)).Build();

            Assert.That(result.ScopesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithScopesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithScopesSupported(CreateScopesSupported(_fixture)).Build();

            Assert.That(result.ScopesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithScopesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedEqualToScopesSupportedFromWithScopesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] scopesSupported = CreateScopesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithScopesSupported(scopesSupported).Build();

            Assert.That(result.ScopesSupported, Is.EqualTo(scopesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseTypesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ResponseTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseTypesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ResponseTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseTypesSupportedEqualToResponseTypesSupportedFromConstructor()
        {
            string[] responseTypesSupported = CreateResponseTypesSupported(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, responseTypesSupported: responseTypesSupported);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ResponseTypesSupported, Is.EqualTo(responseTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithResponseModesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ResponseModesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithResponseModesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithResponseModesSupported(CreateResponseModesSupported(_fixture)).Build();

            Assert.That(result.ResponseModesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithResponseModesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithResponseModesSupported(CreateResponseModesSupported(_fixture)).Build();

            Assert.That(result.ResponseModesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithResponseModesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedEqualToResponseModesSupportedFromWithResponseModesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] responseModesSupported = CreateResponseModesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithResponseModesSupported(responseModesSupported).Build();

            Assert.That(result.ResponseModesSupported, Is.EqualTo(responseModesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithGrantTypesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.GrantTypesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithGrantTypesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithGrantTypesSupported(CreateGrantTypesSupported(_fixture)).Build();

            Assert.That(result.GrantTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithGrantTypesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithGrantTypesSupported(CreateGrantTypesSupported(_fixture)).Build();

            Assert.That(result.GrantTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithGrantTypesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedEqualToGrantTypesSupportedFromWithGrantTypesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] grantTypesSupported = CreateGrantTypesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithGrantTypesSupported(grantTypesSupported).Build();

            Assert.That(result.GrantTypesSupported, Is.EqualTo(grantTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferencesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereAuthenticationContextClassReferencesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.AuthenticationContextClassReferencesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferencesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereAuthenticationContextClassReferencesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithAuthenticationContextClassReferencesSupported(CreateAuthenticationContextClassReferencesSupported(_fixture)).Build();

            Assert.That(result.AuthenticationContextClassReferencesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferencesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereAuthenticationContextClassReferencesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithAuthenticationContextClassReferencesSupported(CreateAuthenticationContextClassReferencesSupported(_fixture)).Build();

            Assert.That(result.AuthenticationContextClassReferencesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithAuthenticationContextClassReferencesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereAuthenticationContextClassReferencesSupportedEqualToAuthenticationContextClassReferencesSupportedFromWithAuthenticationContextClassReferencesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] authenticationContextClassReferencesSupported = CreateAuthenticationContextClassReferencesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithAuthenticationContextClassReferencesSupported(authenticationContextClassReferencesSupported).Build();

            Assert.That(result.AuthenticationContextClassReferencesSupported, Is.EqualTo(authenticationContextClassReferencesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereSubjectTypesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.SubjectTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereSubjectTypesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.SubjectTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereSubjectTypesSupportedEqualToSubjectTypesSupportedFromConstructor()
        {
            string[] subjectTypesSupported = CreateSubjectTypesSupported(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, subjectTypesSupported: subjectTypesSupported);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.SubjectTypesSupported, Is.EqualTo(subjectTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenSigningAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.IdTokenSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenSigningAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.IdTokenSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenSigningAlgValuesSupportedEqualToIdTokenSigningAlgValuesSupportedFromConstructor()
        {
            string[] idTokenSigningAlgValuesSupported = CreateIdTokenSigningAlgValuesSupported(_fixture);
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture, idTokenSigningAlgValuesSupported: idTokenSigningAlgValuesSupported);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.IdTokenSigningAlgValuesSupported, Is.EqualTo(idTokenSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionAlgValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionAlgValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.IdTokenEncryptionAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithIdTokenEncryptionAlgValuesSupported(CreateIdTokenEncryptionAlgValuesSupported(_fixture)).Build();

            Assert.That(result.IdTokenEncryptionAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithIdTokenEncryptionAlgValuesSupported(CreateIdTokenEncryptionAlgValuesSupported(_fixture)).Build();

            Assert.That(result.IdTokenEncryptionAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionAlgValuesSupportedEqualToIdTokenEncryptionAlgValuesSupportedFromWithIdTokenEncryptionAlgValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] idTokenEncryptionAlgValuesSupported = CreateIdTokenEncryptionAlgValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithIdTokenEncryptionAlgValuesSupported(idTokenEncryptionAlgValuesSupported).Build();

            Assert.That(result.IdTokenEncryptionAlgValuesSupported, Is.EqualTo(idTokenEncryptionAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionEncValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionEncValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.IdTokenEncryptionEncValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionEncValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithIdTokenEncryptionEncValuesSupported(CreateIdTokenEncryptionEncValuesSupported(_fixture)).Build();

            Assert.That(result.IdTokenEncryptionEncValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionEncValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithIdTokenEncryptionEncValuesSupported(CreateIdTokenEncryptionEncValuesSupported(_fixture)).Build();

            Assert.That(result.IdTokenEncryptionEncValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithIdTokenEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionEncValuesSupportedEqualToIdTokenEncryptionEncValuesSupportedFromWithIdTokenEncryptionEncValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] idTokenEncryptionEncValuesSupported = CreateIdTokenEncryptionEncValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithIdTokenEncryptionEncValuesSupported(idTokenEncryptionEncValuesSupported).Build();

            Assert.That(result.IdTokenEncryptionEncValuesSupported, Is.EqualTo(idTokenEncryptionEncValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoSigningAlgValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoSigningAlgValuesSupported(CreateUserInfoSigningAlgValuesSupported(_fixture)).Build();

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoSigningAlgValuesSupported(CreateUserInfoSigningAlgValuesSupported(_fixture)).Build();

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedEqualToUserInfoSigningAlgValuesSupportedFromWithUserInfoSigningAlgValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] userInfoSigningAlgValuesSupported = CreateUserInfoSigningAlgValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithUserInfoSigningAlgValuesSupported(userInfoSigningAlgValuesSupported).Build();

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.EqualTo(userInfoSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionAlgValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionAlgValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.UserInfoEncryptionAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoEncryptionAlgValuesSupported(CreateUserInfoEncryptionAlgValuesSupported(_fixture)).Build();

            Assert.That(result.UserInfoEncryptionAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoEncryptionAlgValuesSupported(CreateUserInfoEncryptionAlgValuesSupported(_fixture)).Build();

            Assert.That(result.UserInfoEncryptionAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionAlgValuesSupportedEqualToUserInfoEncryptionAlgValuesSupportedFromWithUserInfoEncryptionAlgValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] userInfoEncryptionAlgValuesSupported = CreateUserInfoEncryptionAlgValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithUserInfoEncryptionAlgValuesSupported(userInfoEncryptionAlgValuesSupported).Build();

            Assert.That(result.UserInfoEncryptionAlgValuesSupported, Is.EqualTo(userInfoEncryptionAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionEncValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionEncValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.UserInfoEncryptionEncValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionEncValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoEncryptionEncValuesSupported(CreateUserInfoEncryptionEncValuesSupported(_fixture)).Build();

            Assert.That(result.UserInfoEncryptionEncValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionEncValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUserInfoEncryptionEncValuesSupported(CreateUserInfoEncryptionEncValuesSupported(_fixture)).Build();

            Assert.That(result.UserInfoEncryptionEncValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUserInfoEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionEncValuesSupportedEqualToUserInfoEncryptionEncValuesSupportedFromWithUserInfoEncryptionEncValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] userInfoEncryptionEncValuesSupported = CreateUserInfoEncryptionEncValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithUserInfoEncryptionEncValuesSupported(userInfoEncryptionEncValuesSupported).Build();

            Assert.That(result.UserInfoEncryptionEncValuesSupported, Is.EqualTo(userInfoEncryptionEncValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectSigningAlgValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestObjectSigningAlgValuesSupported(CreateRequestObjectSigningAlgValuesSupported(_fixture)).Build();

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestObjectSigningAlgValuesSupported(CreateRequestObjectSigningAlgValuesSupported(_fixture)).Build();

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedEqualToRequestObjectSigningAlgValuesSupportedFromWithRequestObjectSigningAlgValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] requestObjectSigningAlgValuesSupported = CreateRequestObjectSigningAlgValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithRequestObjectSigningAlgValuesSupported(requestObjectSigningAlgValuesSupported).Build();

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.EqualTo(requestObjectSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionAlgValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionAlgValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RequestObjectEncryptionAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestObjectEncryptionAlgValuesSupported(CreateRequestObjectEncryptionAlgValuesSupported(_fixture)).Build();

            Assert.That(result.RequestObjectEncryptionAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestObjectEncryptionAlgValuesSupported(CreateRequestObjectEncryptionAlgValuesSupported(_fixture)).Build();

            Assert.That(result.RequestObjectEncryptionAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionAlgValuesSupportedEqualToRequestObjectEncryptionAlgValuesSupportedFromWithRequestObjectEncryptionAlgValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] requestObjectEncryptionAlgValuesSupported = CreateRequestObjectEncryptionAlgValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithRequestObjectEncryptionAlgValuesSupported(requestObjectEncryptionAlgValuesSupported).Build();

            Assert.That(result.RequestObjectEncryptionAlgValuesSupported, Is.EqualTo(requestObjectEncryptionAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionEncValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionEncValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RequestObjectEncryptionEncValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionEncValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestObjectEncryptionEncValuesSupported(CreateRequestObjectEncryptionEncValuesSupported(_fixture)).Build();

            Assert.That(result.RequestObjectEncryptionEncValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionEncValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestObjectEncryptionEncValuesSupported(CreateRequestObjectEncryptionEncValuesSupported(_fixture)).Build();

            Assert.That(result.RequestObjectEncryptionEncValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestObjectEncryptionEncValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionEncValuesSupportedEqualToRequestObjectEncryptionEncValuesSupportedFromWithRequestObjectEncryptionEncValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] requestObjectEncryptionEncValuesSupported = CreateRequestObjectEncryptionEncValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithRequestObjectEncryptionEncValuesSupported(requestObjectEncryptionEncValuesSupported).Build();

            Assert.That(result.RequestObjectEncryptionEncValuesSupported, Is.EqualTo(requestObjectEncryptionEncValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationMethodsSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationMethodsSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithTokenEndpointAuthenticationMethodsSupported(CreateTokenEndpointAuthenticationMethodsSupported(_fixture)).Build();

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationMethodsSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithTokenEndpointAuthenticationMethodsSupported(CreateTokenEndpointAuthenticationMethodsSupported(_fixture)).Build();

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationMethodsSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedEqualToTokenEndpointAuthenticationMethodsSupportedFromWithTokenEndpointAuthenticationMethodsSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] tokenEndpointAuthenticationMethodsSupported = CreateTokenEndpointAuthenticationMethodsSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithTokenEndpointAuthenticationMethodsSupported(tokenEndpointAuthenticationMethodsSupported).Build();

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.EqualTo(tokenEndpointAuthenticationMethodsSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationSigningAlgValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationSigningAlgValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.TokenEndpointAuthenticationSigningAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationSigningAlgValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithTokenEndpointAuthenticationSigningAlgValuesSupported(CreateTokenEndpointAuthenticationSigningAlgValuesSupported(_fixture)).Build();

            Assert.That(result.TokenEndpointAuthenticationSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationSigningAlgValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithTokenEndpointAuthenticationSigningAlgValuesSupported(CreateTokenEndpointAuthenticationSigningAlgValuesSupported(_fixture)).Build();

            Assert.That(result.TokenEndpointAuthenticationSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithTokenEndpointAuthenticationSigningAlgValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationSigningAlgValuesSupportedEqualToTokenEndpointAuthenticationSigningAlgValuesSupportedFromWithTokenEndpointAuthenticationSigningAlgValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] tokenEndpointAuthenticationSigningAlgValuesSupported = CreateTokenEndpointAuthenticationSigningAlgValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithTokenEndpointAuthenticationSigningAlgValuesSupported(tokenEndpointAuthenticationSigningAlgValuesSupported).Build();

            Assert.That(result.TokenEndpointAuthenticationSigningAlgValuesSupported, Is.EqualTo(tokenEndpointAuthenticationSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithDisplayValuesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.DisplayValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithDisplayValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithDisplayValuesSupported(CreateDisplayValuesSupported(_fixture)).Build();

            Assert.That(result.DisplayValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithDisplayValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithDisplayValuesSupported(CreateDisplayValuesSupported(_fixture)).Build();

            Assert.That(result.DisplayValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithDisplayValuesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedEqualToDisplayValuesSupportedFromWithDisplayValuesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] displayValuesSupported = CreateDisplayValuesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithDisplayValuesSupported(displayValuesSupported).Build();

            Assert.That(result.DisplayValuesSupported, Is.EqualTo(displayValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimTypesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ClaimTypesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimTypesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimTypesSupported(CreateClaimTypesSupported(_fixture)).Build();

            Assert.That(result.ClaimTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimTypesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimTypesSupported(CreateClaimTypesSupported(_fixture)).Build();

            Assert.That(result.ClaimTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimTypesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedEqualToClaimTypesSupportedFromWithClaimTypesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] claimTypesSupported = CreateClaimTypesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithClaimTypesSupported(claimTypesSupported).Build();

            Assert.That(result.ClaimTypesSupported, Is.EqualTo(claimTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ClaimsSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimsSupported(CreateClaimsSupported(_fixture)).Build();

            Assert.That(result.ClaimsSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimsSupported(CreateClaimsSupported(_fixture)).Build();

            Assert.That(result.ClaimsSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedEqualToClaimsSupportedFromWithClaimsSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] claimsSupported = CreateClaimsSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithClaimsSupported(claimsSupported).Build();

            Assert.That(result.ClaimsSupported, Is.EqualTo(claimsSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithServiceDocumentationEndpointCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereServiceDocumentationEndpointIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ServiceDocumentationEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithServiceDocumentationEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereServiceDocumentationEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithServiceDocumentationEndpoint(CreateServiceDocumentationEndpoint(_fixture)).Build();

            Assert.That(result.ServiceDocumentationEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithServiceDocumentationEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereServiceDocumentationEndpointsEqualToServiceDocumentationEndpointFromWithServiceDocumentationEndpoint()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            Uri serviceDocumentationEndpoint = CreateServiceDocumentationEndpoint(_fixture);
            IOpenIdProviderConfiguration result = sut.WithServiceDocumentationEndpoint(serviceDocumentationEndpoint).Build();

            Assert.That(result.ServiceDocumentationEndpoint, Is.EqualTo(serviceDocumentationEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsLocalesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ClaimsLocalesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsLocalesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimsLocalesSupported(CreateClaimsLocalesSupported(_fixture)).Build();

            Assert.That(result.ClaimsLocalesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsLocalesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimsLocalesSupported(CreateClaimsLocalesSupported(_fixture)).Build();

            Assert.That(result.ClaimsLocalesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsLocalesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedEqualToClaimsLocalesSupportedFromWithClaimsLocalesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] claimsLocalesSupported = CreateClaimsLocalesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithClaimsLocalesSupported(claimsLocalesSupported).Build();

            Assert.That(result.ClaimsLocalesSupported, Is.EqualTo(claimsLocalesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUiLocalesSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.UiLocalesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUiLocalesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUiLocalesSupported(CreateUiLocalesSupported(_fixture)).Build();

            Assert.That(result.UiLocalesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUiLocalesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedIsNotEmpty()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithUiLocalesSupported(CreateUiLocalesSupported(_fixture)).Build();

            Assert.That(result.UiLocalesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithUiLocalesSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedEqualToUiLocalesSupportedFromWithUiLocalesSupported()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            string[] uiLocalesSupported = CreateUiLocalesSupported(_fixture);
            IOpenIdProviderConfiguration result = sut.WithUiLocalesSupported(uiLocalesSupported).Build();

            Assert.That(result.UiLocalesSupported, Is.EqualTo(uiLocalesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsParameterSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsParameterSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.ClaimsParameterSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithClaimsParameterSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsParameterSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimsParameterSupported(CreateClaimsParameterSupported(_fixture)).Build();

            Assert.That(result.ClaimsParameterSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithClaimsParameterSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsParameterSupportedEqualToClaimsParameterSupportedFromWithClaimsParameterSupported(bool claimsParameterSupported)
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithClaimsParameterSupported(claimsParameterSupported).Build();

            Assert.That(result.ClaimsParameterSupported, Is.EqualTo(claimsParameterSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestParameterSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestParameterSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RequestParameterSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestParameterSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestParameterSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestParameterSupported(CreateRequestParameterSupported(_fixture)).Build();

            Assert.That(result.RequestParameterSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithRequestParameterSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestParameterSupportedEqualToRequestParameterSupportedFromWithRequestParameterSupported(bool requestParameterSupported)
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestParameterSupported(requestParameterSupported).Build();

            Assert.That(result.RequestParameterSupported, Is.EqualTo(requestParameterSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestUriParameterSupportedCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestUriParameterSupportedIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RequestUriParameterSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequestUriParameterSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestUriParameterSupportedIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestUriParameterSupported(CreateRequestUriParameterSupported(_fixture)).Build();

            Assert.That(result.RequestUriParameterSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithRequestUriParameterSupportedCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequestUriParameterSupportedEqualToRequestUriParameterSupportedFromWithRequestUriParameterSupported(bool requestUriParameterSupported)
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequestUriParameterSupported(requestUriParameterSupported).Build();

            Assert.That(result.RequestUriParameterSupported, Is.EqualTo(requestUriParameterSupported));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequireRequestUriRegistrationCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequireRequestUriRegistrationIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RequireRequestUriRegistration, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRequireRequestUriRegistrationCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequireRequestUriRegistrationIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequireRequestUriRegistration(CreateRequireRequestUriRegistration(_fixture)).Build();

            Assert.That(result.RequireRequestUriRegistration, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenWithRequireRequestUriRegistrationCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRequireRequestUriRegistrationEqualToRequireRequestUriRegistrationFromWithRequireRequestUriRegistration(bool requireRequestUriRegistration)
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRequireRequestUriRegistration(requireRequestUriRegistration).Build();

            Assert.That(result.RequireRequestUriRegistration, Is.EqualTo(requireRequestUriRegistration));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationPolicyEndpointCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationPolicyEndpointIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RegistrationPolicyEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationPolicyEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationPolicyEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRegistrationPolicyEndpoint(CreateRegistrationPolicyEndpoint(_fixture)).Build();

            Assert.That(result.RegistrationPolicyEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationPolicyEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationPolicyEndpointsEqualToRegistrationPolicyEndpointFromWithRegistrationPolicyEndpoint()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            Uri registrationPolicyEndpoint = CreateRegistrationPolicyEndpoint(_fixture);
            IOpenIdProviderConfiguration result = sut.WithRegistrationPolicyEndpoint(registrationPolicyEndpoint).Build();

            Assert.That(result.RegistrationPolicyEndpoint, Is.EqualTo(registrationPolicyEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationTermsOfServiceEndpointCalledHasNotBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationTermsOfServiceEndpointIsNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.Build();

            Assert.That(result.RegistrationTermsOfServiceEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationTermsOfServiceEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationTermsOfServiceEndpointIsNotNull()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            IOpenIdProviderConfiguration result = sut.WithRegistrationTermsOfServiceEndpoint(CreateRegistrationTermsOfServiceEndpoint(_fixture)).Build();

            Assert.That(result.RegistrationTermsOfServiceEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithRegistrationTermsOfServiceEndpointCalledHasBeenCalled_ReturnsOpenIdProviderConfigurationWhereRegistrationTermsOfServiceEndpointsEqualToRegistrationTermsOfServiceEndpointFromWithRegistrationTermsOfServiceEndpoint()
        {
            IOpenIdProviderConfigurationBuilder sut = CreateSut(_fixture);

            Uri registrationTermsOfServiceEndpoint = CreateRegistrationTermsOfServiceEndpoint(_fixture);
            IOpenIdProviderConfiguration result = sut.WithRegistrationTermsOfServiceEndpoint(registrationTermsOfServiceEndpoint).Build();

            Assert.That(result.RegistrationTermsOfServiceEndpoint, Is.EqualTo(registrationTermsOfServiceEndpoint));
        }
    }
}