using AutoFixture;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Security.LoginPageModel
{
    [TestFixture]
    public class OnGetAsyncTests
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
        public async Task OnGetAsync_WhenAuthorizationStateIsNull_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsNull_ReturnsBadRequestObjectResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(null);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(null);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorizationState"), null, null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsEmpty_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsEmpty_ReturnsBadRequestObjectResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(string.Empty);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorizationState"), null, string.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsWhiteSpace_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(" ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(" ");

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenAuthorizationStateIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(" ");

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "authorizationState"), null, " ");
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectMicrosoftIsNotNullOnLoginPageModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Microsoft, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationSchemeInMicrosoftOnLoginPageModelIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Microsoft.AuthenticationScheme, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationSchemeInMicrosoftOnLoginPageModelIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Microsoft.AuthenticationScheme, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationSchemeInMicrosoftOnLoginPageModelIsEqualToDefaultAuthenticationSchemeForMicrosoft()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Microsoft.AuthenticationScheme, Is.EqualTo(MicrosoftAccountDefaults.AuthenticationScheme));
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationStateInMicrosoftOnLoginPageModelIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Microsoft.AuthenticationState, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationStateInMicrosoftOnLoginPageModelIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Microsoft.AuthenticationState, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationStateInMicrosoftOnLoginPageModelIsEqualToAuthenticationStateFromArgument()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            string authenticationState = _fixture.Create<string>();
            await sut.OnGetAsync(authenticationState);

            Assert.That(sut.Microsoft.AuthenticationState, Is.EqualTo(authenticationState));
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectGoogleIsNotNullOnLoginPageModel()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Google, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationSchemeInGoogleOnLoginPageModelIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Google.AuthenticationScheme, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationSchemeInGoogleOnLoginPageModelIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Google.AuthenticationScheme, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationSchemeInGoogleOnLoginPageModelIsEqualToDefaultAuthenticationSchemeForGoogle()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Google.AuthenticationScheme, Is.EqualTo(GoogleDefaults.AuthenticationScheme));
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationStateInGoogleOnLoginPageModelIsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Google.AuthenticationState, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationStateInGoogleOnLoginPageModelIsNotEmpty()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(sut.Google.AuthenticationState, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ExpectAuthenticationStateInGoogleOnLoginPageModelIsEqualToAuthenticationStateFromArgument()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            string authenticationState = _fixture.Create<string>();
            await sut.OnGetAsync(authenticationState);

            Assert.That(sut.Google.AuthenticationState, Is.EqualTo(authenticationState));
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ReturnsNotNull()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task OnGetAsync_WhenCalled_ReturnsPageResult()
        {
            WebApi.Models.Security.LoginPageModel sut = CreateSut();

            IActionResult result = await sut.OnGetAsync(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PageResult>());
        }

        private static WebApi.Models.Security.LoginPageModel CreateSut()
        {
            return new WebApi.Models.Security.LoginPageModel();
        }
    }
}