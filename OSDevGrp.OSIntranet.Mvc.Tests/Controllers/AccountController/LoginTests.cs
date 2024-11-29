using AutoFixture;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Tests.Helpers;
using System;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class LoginTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
		private Mock<IUrlHelper> _urlHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNull_AssertContentWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(null, withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNull_AssertActionWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(null, withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNull_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(null, withReturnUrl ? _fixture.CreateEndpointString() : null);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNull_ReturnsNotNull(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(null, withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNull_ReturnsBadRequestResult(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(null, withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsEmpty_AssertContentWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(string.Empty, withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsEmpty_AssertActionWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(string.Empty, withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsEmpty_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(string.Empty, withReturnUrl ? _fixture.CreateEndpointString() : null);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsEmpty_ReturnsNotNull(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(string.Empty, withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsEmpty_ReturnsBadRequestResult(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(string.Empty, withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsWhiteSpace_AssertContentWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(" ", withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsWhiteSpace_AssertActionWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(" ", withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsWhiteSpace_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(" ", withReturnUrl ? _fixture.CreateEndpointString() : null);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsWhiteSpace_ReturnsNotNull(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(" ", withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsWhiteSpace_ReturnsBadRequestResult(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(" ", withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNotSupported_AssertContentWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(_fixture.Create<string>(), withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNotSupported_AssertActionWasNotCalledOnUrlHelper(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(_fixture.Create<string>(), withReturnUrl ? _fixture.CreateEndpointString() : null);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNotSupported_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            sut.Login(_fixture.Create<string>(), withReturnUrl ? _fixture.CreateEndpointString() : null);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNotSupported_ReturnsNotNull(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(_fixture.Create<string>(), withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Login_WhenSchemeIsNotSupported_ReturnsBadRequestResult(bool withReturnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(_fixture.Create<string>(), withReturnUrl ? _fixture.CreateEndpointString() : null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNull_AssertContentWasCalledOnUrlHelperWithRelativeUrlToIndexOnHomeController(string scheme)
        {
            string pathBase = "home/index";
            Controller sut = CreateSut(pathBase: pathBase);

            sut.Login(scheme);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, $"~/{pathBase}") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNull_AssertActionWasCalledTwiceOnUrlHelper(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme);

            _urlHelperMock.Verify(m => m.Action(It.IsNotNull<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNull_AssertActionWasCalledOnUrlHelperWithControllerEqualToHomeAndActionEqualToIndex(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Home", "Index", null))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNull_AssertActionWasCalledOnUrlHelperWithControllerEqualToAccountAndActionEqualToLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: returnUrl);

            sut.Login(scheme);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Account", "LoginCallback", values => string.IsNullOrWhiteSpace(values) == false && string.CompareOrdinal(values, $"{{ returnUrl = {returnUrl} }}") == 0))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNull_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlToIndexOnHomeController(string scheme)
        {
            string absolutePath = _fixture.CreateEndpointString(path: "home/index");
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.Login(scheme);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, absolutePath) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResult(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.AuthenticationSchemes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.AuthenticationSchemes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesContainsOneAuthenticationScheme(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.AuthenticationSchemes.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeFromArgument(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.AuthenticationSchemes.Contains(scheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWherePropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.Properties, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsEqualToUrlForLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString(path: "account/login/callback");
            Controller sut = CreateSut(absolutePath: returnUrl);

            ChallengeResult result = (ChallengeResult) sut.Login(scheme);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.EqualTo(returnUrl));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullButCalculatedReturnUrlIsNonTrusted_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNullButCalculatedReturnUrlIsNonTrusted_ReturnsBadRequestResult(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmpty_AssertContentWasCalledOnUrlHelperWithRelativeUrlToIndexOnHomeController(string scheme)
        {
            string pathBase = "home/index";
            Controller sut = CreateSut(pathBase: pathBase);

            sut.Login(scheme, string.Empty);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, $"~/{pathBase}") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmpty_AssertActionWasCalledTwiceOnUrlHelper(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.IsNotNull<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmpty_AssertActionWasCalledOnUrlHelperWithControllerEqualToHomeAndActionEqualToIndex(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Home", "Index", null))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmpty_AssertActionWasCalledOnUrlHelperWithControllerEqualToAccountAndActionEqualToLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: returnUrl);

            sut.Login(scheme, string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Account", "LoginCallback", values => string.IsNullOrWhiteSpace(values) == false && string.CompareOrdinal(values, $"{{ returnUrl = {returnUrl} }}") == 0))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmpty_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlToIndexOnHomeController(string scheme)
        {
            string absolutePath = _fixture.CreateEndpointString(path: "home/index");
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.Login(scheme, string.Empty);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, absolutePath) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResult(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, string.Empty);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.AuthenticationSchemes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.AuthenticationSchemes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesContainsOneAuthenticationScheme(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.AuthenticationSchemes.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeFromArgument(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.AuthenticationSchemes.Contains(scheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWherePropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.Properties, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsEqualToUrlForLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString(path: "account/login/callback");
            Controller sut = CreateSut(absolutePath: returnUrl);

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, string.Empty);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.EqualTo(returnUrl));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyButCalculatedReturnUrlIsNonTrusted_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsEmptyButCalculatedReturnUrlIsNonTrusted_ReturnsBadRequestResult(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpace_AssertContentWasCalledOnUrlHelperWithRelativeUrlToIndexOnHomeController(string scheme)
        {
            string pathBase = "home/index";
            Controller sut = CreateSut(pathBase: pathBase);

            sut.Login(scheme, " ");

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, $"~/{pathBase}") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpace_AssertActionWasCalledTwiceOnUrlHelper(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, " ");

            _urlHelperMock.Verify(m => m.Action(It.IsNotNull<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpace_AssertActionWasCalledOnUrlHelperWithControllerEqualToHomeAndActionEqualToIndex(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, " ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Home", "Index", null))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpace_AssertActionWasCalledOnUrlHelperWithControllerEqualToAccountAndActionEqualToLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: returnUrl);

            sut.Login(scheme, " ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Account", "LoginCallback", values => string.IsNullOrWhiteSpace(values) == false && string.CompareOrdinal(values, $"{{ returnUrl = {returnUrl} }}") == 0))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpace_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlToIndexOnHomeController(string scheme)
        {
            string absolutePath = _fixture.CreateEndpointString(path: "home/index");
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.Login(scheme, " ");

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, absolutePath) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResult(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, " ");

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.AuthenticationSchemes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.AuthenticationSchemes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesContainsOneAuthenticationScheme(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.AuthenticationSchemes.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeFromArgument(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.AuthenticationSchemes.Contains(scheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWherePropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.Properties, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceAndCalculatedReturnUrlIsTrusted_ReturnsChallengeResultWhereRedirectUriInPropertiesIsEqualToUrlForLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString(path: "account/login/callback");
            Controller sut = CreateSut(absolutePath: returnUrl);

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, " ");

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.EqualTo(returnUrl));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceButCalculatedReturnUrlIsNonTrusted_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, " ");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsWhiteSpaceButCalculatedReturnUrlIsNonTrusted_ReturnsBadRequestResult(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, " ");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me", "user/me", "~/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me", "user/me", "~/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me", "user/me", "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me", "user/me", "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me", "user/me", "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me", "user/me", "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsRelativeUri_AssertContentWasCalledOnUrlHelperWithRelativeUrlForReturnUrl(string scheme, string returnUrl, string pathBase, string expected)
        {
            Controller sut = CreateSut(pathBase: pathBase);

            sut.Login(scheme, returnUrl);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, expected) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsRelativeUri_AssertActionWasCalledOnUrlHelperWithControllerEqualToAccountAndActionEqualToLoginCallback(string scheme, string returnUrl)
        {
            string absoluteReturnUrl = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: absoluteReturnUrl);

            sut.Login(scheme, returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Account", "LoginCallback", values => string.IsNullOrWhiteSpace(values) == false && string.CompareOrdinal(values, $"{{ returnUrl = {absoluteReturnUrl} }}") == 0))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsRelativeUri_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlForReturnUrl(string scheme, string returnUrl)
        {
            string absolutePath = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.Login(scheme, returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, absolutePath) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsNotNull(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResult(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, returnUrl);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereAuthenticationSchemesIsNotNull(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.AuthenticationSchemes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereAuthenticationSchemesIsNotEmpty(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.AuthenticationSchemes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereAuthenticationSchemesContainsOneAuthenticationScheme(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.AuthenticationSchemes.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeFromArgument(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.AuthenticationSchemes.Contains(scheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWherePropertiesIsNotNull(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.Properties, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotNull(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotEmpty(string scheme, string returnUrl)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedRelativeUri_ReturnsChallengeResultWhereRedirectUriInPropertiesIsEqualToUrlForLoginCallback(string scheme, string returnUrl)
        {
            string absoluteReturnUrl = _fixture.CreateEndpointString(path: "account/login/callback");
            Controller sut = CreateSut(absolutePath: absoluteReturnUrl);

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, returnUrl);

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.EqualTo(absoluteReturnUrl));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonTrustedRelativeUri_ReturnsNotNull(string scheme, string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme, "~/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "/users/me")]
        [TestCase(GoogleDefaults.AuthenticationScheme, "~/users/me")]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonTrustedRelativeUri_ReturnsBadRequestResult(string scheme, string returnUrl)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsAbsoluteUri_AssertContentWasNotCalledOnUrlHelper(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsAbsoluteUri_AssertActionWasCalledOnUrlHelperWithControllerEqualToAccountAndActionEqualToLoginCallback(string scheme)
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString(domainName: "localhost");
            sut.Login(scheme, returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && Matches(value, "Account", "LoginCallback", values => string.IsNullOrWhiteSpace(values) == false && string.CompareOrdinal(values, $"{{ returnUrl = {returnUrl} }}") == 0))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsAbsoluteUri_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrlForReturnUrl(string scheme)
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString(domainName: "localhost");
            sut.Login(scheme, returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, returnUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResult(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereAuthenticationSchemesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.AuthenticationSchemes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereAuthenticationSchemesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.AuthenticationSchemes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereAuthenticationSchemesContainsOneAuthenticationScheme(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.AuthenticationSchemes.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeFromArgument(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.AuthenticationSchemes.Contains(scheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWherePropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Properties, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereRedirectUriInPropertiesIsNotEmpty(string scheme)
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsTrustedAbsoluteUri_ReturnsChallengeResultWhereRedirectUriInPropertiesIsEqualToUrlForLoginCallback(string scheme)
        {
            string returnUrl = _fixture.CreateEndpointString(path: "account/login/callback");
            Controller sut = CreateSut(absolutePath: returnUrl);

            ChallengeResult result = (ChallengeResult) sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result.Properties, Is.Not.Null);
            Assert.That(result.Properties.RedirectUri, Is.EqualTo(returnUrl));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonTrustedAbsoluteUri_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonTrustedAbsoluteUri_ReturnsBadRequestResult(string scheme)
        {
            Controller sut = CreateSut(isTrustedDomain: false);

            IActionResult result = sut.Login(scheme, _fixture.CreateEndpointString(domainName: "localhost"));

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonRelativeNorAbsoluteUri_AssertContentWasNotCalledOnUrlHelper(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, GetNonAbsoluteNorRelativeUrl());

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonRelativeNorAbsoluteUri_AssertActionWasNotCalledOnUrlHelper(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, GetNonAbsoluteNorRelativeUrl());

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonRelativeNorAbsoluteUri_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver(string scheme)
        {
            Controller sut = CreateSut();

            sut.Login(scheme, GetNonAbsoluteNorRelativeUrl());

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonRelativeNorAbsoluteUri_ReturnsNotNull(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, GetNonAbsoluteNorRelativeUrl());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(MicrosoftAccountDefaults.AuthenticationScheme)]
        [TestCase(GoogleDefaults.AuthenticationScheme)]
        public void Login_WhenSchemeIsSupportedAndReturnUrlIsNonRelativeNorAbsoluteUri_ReturnsBadRequestResult(string scheme)
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(scheme, GetNonAbsoluteNorRelativeUrl());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        private Controller CreateSut(bool isTrustedDomain = true, string pathBase = null, string absolutePath = null)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);

            _urlHelperMock.Setup(_fixture, pathBase: pathBase, absolutePath: absolutePath);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _trustedDomainResolverMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object)
            {
                Url = _urlHelperMock.Object
            };
        }

        private string GetNonAbsoluteNorRelativeUrl()
        {
            return $"https://localhost:xyz/{_fixture.Create<string>()}";
        }

        private static bool Matches(UrlActionContext urlActionContext, string expectedController, string expectedAction, Func<string, bool> valuesMatcher = null)
        {
            NullGuard.NotNull(urlActionContext, nameof(urlActionContext))
                .NotNullOrWhiteSpace(expectedController, nameof(expectedController))
                .NotNullOrWhiteSpace(expectedAction, nameof(expectedAction));

            return string.IsNullOrWhiteSpace(urlActionContext.Controller) == false &&
                   string.CompareOrdinal(urlActionContext.Controller, expectedController) == 0 &&
                   string.IsNullOrWhiteSpace(urlActionContext.Action) == false &&
                   string.CompareOrdinal(urlActionContext.Action, expectedAction) == 0 &&
                   (valuesMatcher == null || (urlActionContext.Values != null && valuesMatcher(urlActionContext.Values.ToString())));
        }
    }
}