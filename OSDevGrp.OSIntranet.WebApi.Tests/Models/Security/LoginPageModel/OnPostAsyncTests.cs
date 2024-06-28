using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using OSDevGrp.OSIntranet.WebApi.Security;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Security.LoginPageModel
{
    [TestFixture]
    public class OnPostAsyncTests
    {
        #region Private variables

        private Fixture _fixture;
        private Mock<IUrlHelper> _urlHelperMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _urlHelperMock = new Mock<IUrlHelper>();
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNull_AssertActionContextWasNotCalledOnUrlHelper()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnPostAsync(null);

            _urlHelperMock.Verify(m => m.ActionContext, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNull_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnPostAsync(null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNull_ReturnsBadRequestObjectResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnPostAsync(null);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnPostAsync(null);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNull, "externalLoginProviderModel"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNotValid_AssertActionContextWasNotCalledOnUrlHelper()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut(false);

            await sut.OnPostAsync(CreateExternalLoginProviderModel());

            _urlHelperMock.Verify(m => m.ActionContext, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNotValid_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut(false);

            IActionResult result = await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNotValid_ReturnsBadRequestObjectResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut(false);

            IActionResult result = await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsNotValid_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut(false);

            IActionResult result = await sut.OnPostAsync(CreateExternalLoginProviderModel());

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.SubmittedMessageInvalid, "externalLoginProviderModel"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_AssertActionContextWasCalledOnUrlHelper()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnPostAsync(CreateExternalLoginProviderModel());

            _urlHelperMock.Verify(m => m.ActionContext, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_AssertActionWasCalledOnUrlHelperWithUrlActionContextWhereActionIsEqualToAuthorizeCallbackAndControllerIsEqualToSecurity()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnPostAsync(CreateExternalLoginProviderModel());

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value =>
                    value != null &&
                    string.IsNullOrWhiteSpace(value.Action) == false && string.CompareOrdinal(value.Action, "AuthorizeCallback") == 0 &&
                    string.IsNullOrWhiteSpace(value.Controller) == false && string.CompareOrdinal(value.Controller, "Security") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereAuthenticationSchemesIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.AuthenticationSchemes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereAuthenticationSchemesIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.AuthenticationSchemes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereAuthenticationSchemesContainsAuthenticationSchemeFromExternalLoginProviderModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            string authenticationScheme = _fixture.Create<string>();
            ExternalLoginProviderModel externalLoginProviderModel = CreateExternalLoginProviderModel(authenticationScheme: authenticationScheme);
            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(externalLoginProviderModel);

            Assert.That(result.AuthenticationSchemes.Contains(authenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWherePropertiesIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereRedirectUriInPropertiesIsEqualToSecurityAuthorizeCallback()
        {
            string domainName = CreateDomainName();
            Uri requestUrl = CreateRequestUrl(domainName);
            WebApi.Models.Security.LoginPageModel sut = CreateSut(requestUrl: requestUrl);

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.EqualTo($"https://{domainName}/security/authorizecallback"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereItemsInPropertiesIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.Items, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereItemsInPropertiesIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.Items, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereItemsInPropertiesContainsAuthorizationStateKey()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.Items.ContainsKey(KeyNames.AuthorizationStateKey), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereItemsInPropertiesContainsAuthorizationStateKeyWithValueNotEqualToNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.Items[KeyNames.AuthorizationStateKey], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereItemsInPropertiesContainsAuthorizationStateKeyWithNonEmptyValue()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(CreateExternalLoginProviderModel());

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.Items[KeyNames.AuthorizationStateKey], Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnPostAsync_WhenExternalLoginProviderModelIsValid_ReturnsChallengeResultWhereItemsInPropertiesContainsAuthorizationStateKeyWithValueEqualToAuthenticationStateFromExternalLoginProviderModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            string authenticationState = _fixture.Create<string>();
            ExternalLoginProviderModel externalLoginProviderModel = CreateExternalLoginProviderModel(authenticationState: authenticationState);
            ChallengeResult result = (ChallengeResult) await sut.OnPostAsync(externalLoginProviderModel);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.Items[KeyNames.AuthorizationStateKey], Is.EqualTo(authenticationState));
        }

        private WebApi.Models.Security.LoginPageModel CreateSut(bool modelIsValid = true, Uri requestUrl = null)
        {
            _urlHelperMock.Setup(m => m.ActionContext)
                .Returns(CreateActionContext(requestUrl));
            _urlHelperMock.Setup(m => m.Action(It.IsNotNull<UrlActionContext>()))
                .Returns<UrlActionContext>(urlActionContext => $"/{urlActionContext.Controller}/{urlActionContext.Action}".ToLower());

            WebApi.Models.Security.LoginPageModel loginPageModel = new WebApi.Models.Security.LoginPageModel
            {
                Url = _urlHelperMock.Object
            };
            if (modelIsValid == false)
            {
                loginPageModel.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return loginPageModel;
        }

        private ExternalLoginProviderModel CreateExternalLoginProviderModel(string authenticationScheme = null, string authenticationState = null)
        {
            return new ExternalLoginProviderModel
            {
                AuthenticationScheme = authenticationScheme ?? _fixture.Create<string>(),
                AuthenticationState = authenticationState ?? _fixture.Create<string>()
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
            return new Uri($"https://{domainName ?? CreateDomainName()}/{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}", UriKind.Absolute);
        }

        private string CreateDomainName()
        {
            return $"{_fixture.Create<string>().Replace("/", string.Empty).ToLower()}.local";
        }
    }
}