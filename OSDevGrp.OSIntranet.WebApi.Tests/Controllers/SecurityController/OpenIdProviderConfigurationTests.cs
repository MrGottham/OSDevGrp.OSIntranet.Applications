using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class OpenIdProviderConfigurationTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertActionContextWasCalledFourTimesOnUrlHelper()
        {
            Controller sut = CreateSut();

            await sut.OpenIdProviderConfiguration();

            _urlHelperMock.Verify(m => m.ActionContext, Times.Exactly(4));
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertActionWasCalledOnUrlHelperWithUrlActionContextWhereActionIsEqualToAuthorizeAndControllerIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            await sut.OpenIdProviderConfiguration();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value =>
                    value != null &&
                    string.IsNullOrWhiteSpace(value.Action) == false && string.CompareOrdinal(value.Action, "Authorize") == 0 &&
                    string.IsNullOrWhiteSpace(value.Controller) == false && string.CompareOrdinal(value.Controller, "Security") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertActionWasCalledOnUrlHelperWithUrlActionContextWhereActionIsEqualToAcquireTokenAndControllerIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            await sut.OpenIdProviderConfiguration();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value =>
                    value != null &&
                    string.IsNullOrWhiteSpace(value.Action) == false && string.CompareOrdinal(value.Action, "AcquireToken") == 0 &&
                    string.IsNullOrWhiteSpace(value.Controller) == false && string.CompareOrdinal(value.Controller, "Security") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertActionWasCalledOnUrlHelperWithUrlActionContextWhereActionIsEqualToJsonWebKeysAndControllerIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            await sut.OpenIdProviderConfiguration();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value =>
                    value != null &&
                    string.IsNullOrWhiteSpace(value.Action) == false && string.CompareOrdinal(value.Action, "JsonWebKeys") == 0 &&
                    string.IsNullOrWhiteSpace(value.Controller) == false && string.CompareOrdinal(value.Controller, "Security") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertActionWasCalledOnUrlHelperWithUrlActionContextWhereActionIsEqualToUserInfoAndControllerIsEqualToSecurity()
        {
            Controller sut = CreateSut();

            await sut.OpenIdProviderConfiguration();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value =>
                    value != null &&
                    string.IsNullOrWhiteSpace(value.Action) == false && string.CompareOrdinal(value.Action, "UserInfo") == 0 &&
                    string.IsNullOrWhiteSpace(value.Controller) == false && string.CompareOrdinal(value.Controller, "Security") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetOpenIdProviderConfigurationQuery()
        {
            Controller sut = CreateSut();

            await sut.OpenIdProviderConfiguration();

            _queryBusMock.Verify(m => m.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(It.IsNotNull<IGetOpenIdProviderConfigurationQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetOpenIdProviderConfigurationQueryWhereAuthorizationEndpointMatchesAuthorizationEndpointOnSecurityController()
        {
            string domainName = CreateDomainName();
            Uri requestUrl = CreateRequestUrl(domainName);
            Controller sut = CreateSut(requestUrl);

            await sut.OpenIdProviderConfiguration();

            _queryBusMock.Verify(m => m.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(It.Is<IGetOpenIdProviderConfigurationQuery>(value =>
                    value != null &&
                    value.AuthorizationEndpoint != null &&
                    string.CompareOrdinal(value.AuthorizationEndpoint.AbsoluteUri, $"https://{domainName}/security/authorize") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetOpenIdProviderConfigurationQueryWhereAuthorizationEndpointMatchesTokenEndpointOnSecurityController()
        {
            string domainName = CreateDomainName();
            Uri requestUrl = CreateRequestUrl(domainName);
            Controller sut = CreateSut(requestUrl);

            await sut.OpenIdProviderConfiguration();

            _queryBusMock.Verify(m => m.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(It.Is<IGetOpenIdProviderConfigurationQuery>(value =>
                    value != null &&
                    value.TokenEndpoint != null &&
                    string.CompareOrdinal(value.TokenEndpoint.AbsoluteUri, $"https://{domainName}/security/acquiretoken") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetOpenIdProviderConfigurationQueryWhereJsonWebKeySetEndpointMatchesJsonWebKeySetEndpointOnSecurityController()
        {
            string domainName = CreateDomainName();
            Uri requestUrl = CreateRequestUrl(domainName);
            Controller sut = CreateSut(requestUrl);

            await sut.OpenIdProviderConfiguration();

            _queryBusMock.Verify(m => m.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(It.Is<IGetOpenIdProviderConfigurationQuery>(value =>
                    value != null &&
                    value.JsonWebKeySetEndpoint != null &&
                    string.CompareOrdinal(value.JsonWebKeySetEndpoint.AbsoluteUri, $"https://{domainName}/security/jsonwebkeys") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetOpenIdProviderConfigurationQueryWhereUserInfoEndpointMatchesUserInfoEndpointOnSecurityController()
        {
            string domainName = CreateDomainName();
            Uri requestUrl = CreateRequestUrl(domainName);
            Controller sut = CreateSut(requestUrl);

            await sut.OpenIdProviderConfiguration();

            _queryBusMock.Verify(m => m.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(It.Is<IGetOpenIdProviderConfigurationQuery>(value =>
                    value != null &&
                    value.UserInfoEndpoint != null &&
                    string.CompareOrdinal(value.UserInfoEndpoint.AbsoluteUri, $"https://{domainName}/security/userinfo") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<OpenIdProviderConfigurationModel> result = await sut.OpenIdProviderConfiguration();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_ReturnsActionResultWhereResultIsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<OpenIdProviderConfigurationModel> result = await sut.OpenIdProviderConfiguration();

            Assert.That(result.Result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<OpenIdProviderConfigurationModel> result = await sut.OpenIdProviderConfiguration();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OpenIdProviderConfiguration_WhenCalled_ReturnsActionResultWhereResultIsOkObjectResultWithValueNotEqualToNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.OpenIdProviderConfiguration()).Result;

            Assert.That(result!.Value, Is.Not.Null);
        }

        private Controller CreateSut(Uri requestUrl = null, IOpenIdProviderConfiguration openIdProviderConfiguration = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetOpenIdProviderConfigurationQuery, IOpenIdProviderConfiguration>(It.IsAny<IGetOpenIdProviderConfigurationQuery>()))
                .Returns(Task.FromResult(openIdProviderConfiguration ?? _fixture.BuildOpenIdProviderConfigurationMock().Object));

            _urlHelperMock.Setup(m => m.ActionContext)
                .Returns(CreateActionContext(requestUrl));
            _urlHelperMock.Setup(m => m.Action(It.IsNotNull<UrlActionContext>()))
                .Returns<UrlActionContext>(urlActionContext => $"/{urlActionContext.Controller}/{urlActionContext.Action}".ToLower());

            return new Controller(_commandBusMock.Object, _queryBusMock.Object)
            {
                Url = _urlHelperMock.Object
            };
        }

        private ActionContext CreateActionContext(Uri requestUrl = null)
        {
            requestUrl ??= CreateRequestUrl();

            HttpContext httpContext = new DefaultHttpContext();
            httpContext.Request.IsHttps = string.CompareOrdinal(requestUrl.Scheme, "https") == 0;
            httpContext.Request.Scheme = requestUrl.Scheme;
            httpContext.Request.Host = new HostString(requestUrl.Host);
            httpContext.Request.PathBase = string.Empty;
            httpContext.Request.Path = requestUrl.AbsolutePath;

            return new ActionContext
            {
                HttpContext = httpContext
            };
        }

        private Uri CreateRequestUrl(string domainName = null)
        {
            return new Uri($"https://{domainName ?? CreateDomainName()}/.well-known/openid-configuration", UriKind.Absolute);
        }

        private string CreateDomainName()
        {
            return $"{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}.local";
        }
    }
}