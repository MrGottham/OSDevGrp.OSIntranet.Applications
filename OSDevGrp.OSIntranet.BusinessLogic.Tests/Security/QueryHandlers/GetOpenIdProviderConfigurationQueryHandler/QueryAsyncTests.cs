using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.QueryHandlers.GetOpenIdProviderConfigurationQueryHandler
{
    [TestFixture]
    public class QueryAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IOptions<TokenGeneratorOptions>> _tokenGeneratorOptionsMock;
        private Mock<IOpenIdProviderConfigurationStaticValuesProvider> _openIdProviderConfigurationStaticValuesProviderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _tokenGeneratorOptionsMock = new Mock<IOptions<TokenGeneratorOptions>>();
            _openIdProviderConfigurationStaticValuesProviderMock = new Mock<IOpenIdProviderConfigurationStaticValuesProvider>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void QueryAsync_WhenQueryIsNull_ThrowsArgumentNullException()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.QueryAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("query"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValidateWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertAuthorizationEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.AuthorizationEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertTokenEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.TokenEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertJsonWebKeySetEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.JsonWebKeySetEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertUserInfoEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.UserInfoEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRegistrationEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.RegistrationEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertServiceDocumentationEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.ServiceDocumentationEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRegistrationPolicyEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.RegistrationPolicyEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRegistrationTermsOfServiceEndpointWasCalledOnGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = CreateGetOpenIdProviderConfigurationQueryMock();
            await sut.QueryAsync(getOpenIdProviderConfigurationQueryMock.Object);

            getOpenIdProviderConfigurationQueryMock.Verify(m => m.RegistrationTermsOfServiceEndpoint, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertValueWasCalledOnTokenGeneratorOptions()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _tokenGeneratorOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertScopesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ScopesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertResponseTypesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ResponseTypesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertSubjectTypesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.SubjectTypesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertIdTokenSigningAlgValuesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.IdTokenSigningAlgValuesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertResponseModesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ResponseModesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertGrantTypesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.GrantTypesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertUserInfoSigningAlgValuesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.UserInfoSigningAlgValuesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRequestObjectSigningAlgValuesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.RequestObjectSigningAlgValuesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertTokenEndpointAuthenticationMethodsSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.TokenEndpointAuthenticationMethodsSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertDisplayValuesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.DisplayValuesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertClaimTypesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ClaimTypesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertClaimsSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ClaimsSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertClaimsLocalesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ClaimsLocalesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertUiLocalesSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.UiLocalesSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertClaimsParameterSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.ClaimsParameterSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRequestParameterSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.RequestParameterSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AsserRequestUriParameterSupportedWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.RequestUriParameterSupported, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_AssertRequireRequestUriRegistrationWasCalledOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            _openIdProviderConfigurationStaticValuesProviderMock.Verify(m => m.RequireRequestUriRegistration, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIssuerIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.Issuer, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIssuerIsEqualToIssuerFromTokenGeneratorOptions()
        {
            string issuer = CreateIssuer();
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions(issuer);
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(tokenGeneratorOptions);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.Issuer.AbsoluteUri.TrimEnd('/'), Is.EqualTo(issuer));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereAuthorizationEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.AuthorizationEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereAuthorizationEndpointIsEqualToAuthorizationEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri authorizationEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(authorizationEndpoint: authorizationEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.AuthorizationEndpoint, Is.EqualTo(authorizationEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.TokenEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointIsEqualToTokenEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri tokenEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(tokenEndpoint: tokenEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.TokenEndpoint, Is.EqualTo(tokenEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasUserInfoEndpoint_ReturnsOpenIdProviderConfigurationWhereUserInfoEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasUserInfoEndpoint: true);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.UserInfoEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasUserInfoEndpoint_ReturnsOpenIdProviderConfigurationWhereUserInfoEndpointIsEqualToUserInfoEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri userInfoEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasUserInfoEndpoint: true, userInfoEndpoint: userInfoEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.UserInfoEndpoint, Is.EqualTo(userInfoEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryDoesNotHaveUserInfoEndpoint_ReturnsOpenIdProviderConfigurationWhereUserInfoEndpointIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasUserInfoEndpoint: false);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.UserInfoEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereJsonWebKeySetEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.JsonWebKeySetEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereJsonWebKeySetEndpointIsEqualToJsonWebKeySetEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri jsonWebKeySetEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(jsonWebKeySetEndpoint: jsonWebKeySetEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.JsonWebKeySetEndpoint, Is.EqualTo(jsonWebKeySetEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasRegistrationEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationEndpoint: true);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasRegistrationEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationEndpointIsEqualToRegistrationEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri registrationEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationEndpoint: true, registrationEndpoint: registrationEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationEndpoint, Is.EqualTo(registrationEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryDoesNotHaveRegistrationEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationEndpointIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationEndpoint: false);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ScopesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ScopesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereScopesSupportedContainingNameForEachSupportedScopeGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            IScope[] scopesSupported =
            [
                _fixture.BuildScopeMock().Object,
                _fixture.BuildScopeMock().Object,
                _fixture.BuildScopeMock().Object
            ];
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(scopesSupported: scopesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ScopesSupported, Is.EqualTo(scopesSupported.Select(m => m.Name).ToArray()));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseTypesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ResponseTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseTypesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ResponseTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseTypesSupportedIsEqualToResponseTypesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] responseTypesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(responseTypesSupported: responseTypesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ResponseTypesSupported, Is.EqualTo(responseTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ResponseModesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ResponseModesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereResponseModesSupportedIsEqualToResponseModesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] responseModesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(responseModesSupported: responseModesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ResponseModesSupported, Is.EqualTo(responseModesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.GrantTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.GrantTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereGrantTypesSupportedIsEqualToGrantTypesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] grantTypesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(grantTypesSupported: grantTypesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.GrantTypesSupported, Is.EqualTo(grantTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereAuthenticationContextClassReferencesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.AuthenticationContextClassReferencesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereSubjectTypesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.SubjectTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereSubjectTypesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.SubjectTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereSubjectTypesSupportedIsEqualToSubjectTypesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] subjectTypesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(subjectTypesSupported: subjectTypesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.SubjectTypesSupported, Is.EqualTo(subjectTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenSigningAlgValuesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.IdTokenSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenSigningAlgValuesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.IdTokenSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenSigningAlgValuesSupportedIsEqualToIdTokenSigningAlgValuesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] idTokenSigningAlgValuesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(idTokenSigningAlgValuesSupported: idTokenSigningAlgValuesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.IdTokenSigningAlgValuesSupported, Is.EqualTo(idTokenSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionAlgValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.IdTokenEncryptionAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereIdTokenEncryptionEncValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.IdTokenEncryptionEncValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoSigningAlgValuesSupportedIsEqualToUserInfoSigningAlgValuesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] userInfoSigningAlgValuesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(userInfoSigningAlgValuesSupported: userInfoSigningAlgValuesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UserInfoSigningAlgValuesSupported, Is.EqualTo(userInfoSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionAlgValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UserInfoEncryptionAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUserInfoEncryptionEncValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UserInfoEncryptionEncValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectSigningAlgValuesSupportedIsEqualToRequestObjectSigningAlgValuesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] requestObjectSigningAlgValuesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(requestObjectSigningAlgValuesSupported: requestObjectSigningAlgValuesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestObjectSigningAlgValuesSupported, Is.EqualTo(requestObjectSigningAlgValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionAlgValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestObjectEncryptionAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestObjectEncryptionEncValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestObjectEncryptionEncValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationMethodsSupportedIsEqualToTokenEndpointAuthenticationMethodsSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] tokenEndpointAuthenticationMethodsSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(tokenEndpointAuthenticationMethodsSupported: tokenEndpointAuthenticationMethodsSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.TokenEndpointAuthenticationMethodsSupported, Is.EqualTo(tokenEndpointAuthenticationMethodsSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereTokenEndpointAuthenticationSigningAlgValuesSupportedIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.TokenEndpointAuthenticationSigningAlgValuesSupported, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.DisplayValuesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.DisplayValuesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereDisplayValuesSupportedIsEqualToDisplayValuesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] displayValuesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(displayValuesSupported: displayValuesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.DisplayValuesSupported, Is.EqualTo(displayValuesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimTypesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimTypesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimTypesSupportedIsEqualToClaimTypesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] claimTypesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(claimTypesSupported: claimTypesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimTypesSupported, Is.EqualTo(claimTypesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsSupportedIsEqualToClaimsSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] claimsSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(claimsSupported: claimsSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsSupported, Is.EqualTo(claimsSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasServiceDocumentationEndpoint_ReturnsOpenIdProviderConfigurationWhereServiceDocumentationEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasServiceDocumentationEndpoint: true);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.ServiceDocumentationEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasServiceDocumentationEndpoint_ReturnsOpenIdProviderConfigurationWhereServiceDocumentationEndpointIsEqualToServiceDocumentationEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri serviceDocumentationEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasServiceDocumentationEndpoint: true, serviceDocumentationEndpoint: serviceDocumentationEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.ServiceDocumentationEndpoint, Is.EqualTo(serviceDocumentationEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryDoesNotHaveServiceDocumentationEndpoint_ReturnsOpenIdProviderConfigurationWhereServiceDocumentationEndpointIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasServiceDocumentationEndpoint: false);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.ServiceDocumentationEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsLocalesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsLocalesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsLocalesSupportedIsEqualToClaimsLocalesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] claimsLocalesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(claimsLocalesSupported: claimsLocalesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsLocalesSupported, Is.EqualTo(claimsLocalesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UiLocalesSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedIsNotEmpty()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UiLocalesSupported, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereUiLocalesSupportedIsEqualToUiLocalesSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider()
        {
            string[] uiLocalesSupported = _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(uiLocalesSupported: uiLocalesSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.UiLocalesSupported, Is.EqualTo(uiLocalesSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsParameterSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsParameterSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereClaimsParameterSupportedIsEqualToClaimsParameterSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider(bool claimsParameterSupported)
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(claimsParameterSupported: claimsParameterSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.ClaimsParameterSupported, Is.EqualTo(claimsParameterSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestParameterSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestParameterSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestParameterSupportedIsEqualToRequestParameterSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider(bool requestParameterSupported)
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(requestParameterSupported: requestParameterSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestParameterSupported, Is.EqualTo(requestParameterSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestUriParameterSupportedIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestUriParameterSupported, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequestUriParameterSupportedIsEqualToRequestUriParameterSupportedGivenByOnOpenIdProviderConfigurationStaticValuesProvider(bool requestUriParameterSupported)
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(requestUriParameterSupported: requestUriParameterSupported);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequestUriParameterSupported, Is.EqualTo(requestUriParameterSupported));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequireRequestUriRegistrationIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequireRequestUriRegistration, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public async Task QueryAsync_WhenCalled_ReturnsOpenIdProviderConfigurationWhereRequireRequestUriRegistrationIsEqualToRequireRequestUriRegistrationGivenByOnOpenIdProviderConfigurationStaticValuesProvider(bool requireRequestUriRegistration)
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut(requireRequestUriRegistration: requireRequestUriRegistration);

            IOpenIdProviderConfiguration result = await sut.QueryAsync(CreateGetOpenIdProviderConfigurationQuery());

            Assert.That(result.RequireRequestUriRegistration, Is.EqualTo(requireRequestUriRegistration));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasRegistrationPolicyEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationPolicyEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationPolicyEndpoint: true);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationPolicyEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasRegistrationPolicyEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationPolicyEndpointIsEqualToRegistrationPolicyEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri registrationPolicyEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationPolicyEndpoint: true, registrationPolicyEndpoint: registrationPolicyEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationPolicyEndpoint, Is.EqualTo(registrationPolicyEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryDoesNotHaveRegistrationPolicyEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationPolicyEndpointIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationPolicyEndpoint: false);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationPolicyEndpoint, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasRegistrationTermsOfServiceEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationTermsOfServiceEndpointIsNotNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationTermsOfServiceEndpoint: true);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationTermsOfServiceEndpoint, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryHasRegistrationTermsOfServiceEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationTermsOfServiceEndpointIsEqualToRegistrationTermsOfServiceEndpointFromGetOpenIdProviderConfigurationQuery()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            Uri registrationTermsOfServiceEndpoint = _fixture.CreateEndpoint();
            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationTermsOfServiceEndpoint: true, registrationTermsOfServiceEndpoint: registrationTermsOfServiceEndpoint);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationTermsOfServiceEndpoint, Is.EqualTo(registrationTermsOfServiceEndpoint));
        }

        [Test]
        [Category("UnitTest")]
        public async Task QueryAsync_WhenGetOpenIdProviderConfigurationQueryDoesNotHaveRegistrationTermsOfServiceEndpoint_ReturnsOpenIdProviderConfigurationWhereRegistrationTermsOfServiceEndpointIsNull()
        {
            IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> sut = CreateSut();

            IGetOpenIdProviderConfigurationQuery getOpenIdProviderConfiguration = CreateGetOpenIdProviderConfigurationQuery(hasRegistrationTermsOfServiceEndpoint: false);
            IOpenIdProviderConfiguration result = await sut.QueryAsync(getOpenIdProviderConfiguration);

            Assert.That(result.RegistrationTermsOfServiceEndpoint, Is.Null);
        }

        private IQueryHandler<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration> CreateSut(TokenGeneratorOptions tokenGeneratorOptions = null, IEnumerable<IScope> scopesSupported = null, IEnumerable<string> responseTypesSupported = null, IEnumerable<string> responseModesSupported = null, IEnumerable<string> grantTypesSupported = null, IEnumerable<string> subjectTypesSupported = null, IEnumerable<string> idTokenSigningAlgValuesSupported = null, IEnumerable<string> userInfoSigningAlgValuesSupported = null, IEnumerable<string> requestObjectSigningAlgValuesSupported = null, IEnumerable<string> tokenEndpointAuthenticationMethodsSupported = null, IEnumerable<string> displayValuesSupported = null, IEnumerable<string> claimTypesSupported = null, IEnumerable<string> claimsSupported = null, IEnumerable<string> claimsLocalesSupported = null, IEnumerable<string> uiLocalesSupported = null, bool? claimsParameterSupported = null, bool? requestParameterSupported = null, bool? requestUriParameterSupported = null, bool? requireRequestUriRegistration = null)
        {
            scopesSupported ??=
            [
                _fixture.BuildScopeMock().Object,
                _fixture.BuildScopeMock().Object,
                _fixture.BuildScopeMock().Object
            ];
            responseTypesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            responseModesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            grantTypesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            subjectTypesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            idTokenSigningAlgValuesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            userInfoSigningAlgValuesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            requestObjectSigningAlgValuesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            tokenEndpointAuthenticationMethodsSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            displayValuesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            claimTypesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            claimsSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            claimsLocalesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();
            uiLocalesSupported ??= _fixture.CreateMany<string>(_random.Next(1, 5)).ToArray();

            _tokenGeneratorOptionsMock.Setup(m => m.Value)
                .Returns(tokenGeneratorOptions ?? CreateTokenGeneratorOptions());

            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ScopesSupported)
                .Returns(new ReadOnlyDictionary<string, IScope>(scopesSupported.ToDictionary(scopeSupported => scopeSupported.Name, scopeSupported => scopeSupported)));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ResponseTypesSupported)
                .Returns(new ReadOnlyCollection<string>(responseTypesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ResponseModesSupported)
                .Returns(new ReadOnlyCollection<string>(responseModesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.GrantTypesSupported)
                .Returns(new ReadOnlyCollection<string>(grantTypesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.SubjectTypesSupported)
                .Returns(new ReadOnlyCollection<string>(subjectTypesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.IdTokenSigningAlgValuesSupported)
                .Returns(new ReadOnlyCollection<string>(idTokenSigningAlgValuesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.UserInfoSigningAlgValuesSupported)
                .Returns(new ReadOnlyCollection<string>(userInfoSigningAlgValuesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.RequestObjectSigningAlgValuesSupported)
                .Returns(new ReadOnlyCollection<string>(requestObjectSigningAlgValuesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.TokenEndpointAuthenticationMethodsSupported)
                .Returns(new ReadOnlyCollection<string>(tokenEndpointAuthenticationMethodsSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.DisplayValuesSupported)
                .Returns(new ReadOnlyCollection<string>(displayValuesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ClaimTypesSupported)
                .Returns(new ReadOnlyCollection<string>(claimTypesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ClaimsSupported)
                .Returns(new ReadOnlyCollection<string>(claimsSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ClaimsLocalesSupported)
                .Returns(new ReadOnlyCollection<string>(claimsLocalesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.UiLocalesSupported)
                .Returns(new ReadOnlyCollection<string>(uiLocalesSupported.ToList()));
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.ClaimsParameterSupported)
                .Returns(claimsParameterSupported ?? _fixture.Create<bool>());
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.RequestParameterSupported)
                .Returns(requestParameterSupported ?? _fixture.Create<bool>());
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.RequestUriParameterSupported)
                .Returns(requestUriParameterSupported ?? _fixture.Create<bool>());
            _openIdProviderConfigurationStaticValuesProviderMock.Setup(m => m.RequireRequestUriRegistration)
                .Returns(requireRequestUriRegistration ?? _fixture.Create<bool>());

            return new BusinessLogic.Security.QueryHandlers.GetOpenIdProviderConfigurationQueryHandler(_validatorMock.Object, _tokenGeneratorOptionsMock.Object, _openIdProviderConfigurationStaticValuesProviderMock.Object);
        }

        private TokenGeneratorOptions CreateTokenGeneratorOptions(string issuer = null)
        {
            return new TokenGeneratorOptions
            {
                Issuer = issuer ?? CreateIssuer()
            };
        }

        private IGetOpenIdProviderConfigurationQuery CreateGetOpenIdProviderConfigurationQuery(Uri authorizationEndpoint = null, Uri tokenEndpoint = null, Uri jsonWebKeySetEndpoint = null, bool? hasUserInfoEndpoint = null, Uri userInfoEndpoint = null, bool? hasRegistrationEndpoint = null, Uri registrationEndpoint = null, bool? hasServiceDocumentationEndpoint = null, Uri serviceDocumentationEndpoint = null, bool? hasRegistrationPolicyEndpoint = null, Uri registrationPolicyEndpoint = null, bool? hasRegistrationTermsOfServiceEndpoint = null, Uri registrationTermsOfServiceEndpoint = null)
        {
            return CreateGetOpenIdProviderConfigurationQueryMock(authorizationEndpoint, tokenEndpoint, jsonWebKeySetEndpoint, hasUserInfoEndpoint, userInfoEndpoint, hasRegistrationEndpoint, registrationEndpoint, hasServiceDocumentationEndpoint, serviceDocumentationEndpoint, hasRegistrationPolicyEndpoint, registrationPolicyEndpoint, hasRegistrationTermsOfServiceEndpoint, registrationTermsOfServiceEndpoint).Object;
        }

        private Mock<IGetOpenIdProviderConfigurationQuery> CreateGetOpenIdProviderConfigurationQueryMock(Uri authorizationEndpoint = null, Uri tokenEndpoint = null, Uri jsonWebKeySetEndpoint = null, bool? hasUserInfoEndpoint = null, Uri userInfoEndpoint = null, bool? hasRegistrationEndpoint = null, Uri registrationEndpoint = null, bool? hasServiceDocumentationEndpoint = null, Uri serviceDocumentationEndpoint = null, bool? hasRegistrationPolicyEndpoint = null, Uri registrationPolicyEndpoint = null, bool? hasRegistrationTermsOfServiceEndpoint = null, Uri registrationTermsOfServiceEndpoint = null)
        {
            Mock<IGetOpenIdProviderConfigurationQuery> getOpenIdProviderConfigurationQueryMock = new Mock<IGetOpenIdProviderConfigurationQuery>();
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.AuthorizationEndpoint)
                .Returns(authorizationEndpoint ?? _fixture.CreateEndpoint());
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.TokenEndpoint)
                .Returns(tokenEndpoint ?? _fixture.CreateEndpoint());
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.JsonWebKeySetEndpoint)
                .Returns(jsonWebKeySetEndpoint ?? _fixture.CreateEndpoint());
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.UserInfoEndpoint)
                .Returns(hasUserInfoEndpoint ?? _random.Next(100) > 50 ? userInfoEndpoint ?? _fixture.CreateEndpoint() : null);
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.RegistrationEndpoint)
                .Returns(hasRegistrationEndpoint ?? _random.Next(100) > 50 ? registrationEndpoint ?? _fixture.CreateEndpoint() : null);
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.ServiceDocumentationEndpoint)
                .Returns(hasServiceDocumentationEndpoint ?? _random.Next(100) > 50 ? serviceDocumentationEndpoint ?? _fixture.CreateEndpoint() : null);
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.RegistrationPolicyEndpoint)
                .Returns(hasRegistrationPolicyEndpoint ?? _random.Next(100) > 50 ? registrationPolicyEndpoint ?? _fixture.CreateEndpoint() : null);
            getOpenIdProviderConfigurationQueryMock.Setup(m => m.RegistrationTermsOfServiceEndpoint)
                .Returns(hasRegistrationTermsOfServiceEndpoint ?? _random.Next(100) > 50 ? registrationTermsOfServiceEndpoint ?? _fixture.CreateEndpoint() : null);
            return getOpenIdProviderConfigurationQueryMock;
        }

        private string CreateIssuer()
        {
            return _fixture.CreateEndpointString(withoutPathAndQuery: true);
        }
    }
}