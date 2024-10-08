using AutoFixture;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
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
    public class LoginWithGoogleAccountTests
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
        public void LoginWithGoogleAccount_WhenReturnUrlIsNull_AssertActionWasCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNull_AssertIsTrustedDomainWasCalledOnTrustedDomainResolver()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.LoginWithGoogleAccount();

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsNonTrustedDomain_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            sut.LoginWithGoogleAccount();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsNonTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        IActionResult result = sut.LoginWithGoogleAccount();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsNonTrustedDomain_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.LoginWithGoogleAccount();

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualTAccount()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.LoginWithGoogleAccount();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithGoogleAccount();

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount();

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmpty_AssertActionWasCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmpty_AssertIsTrustedDomainWasCalledOnTrustedDomainResolver()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.LoginWithGoogleAccount(string.Empty);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsNonTrustedDomain_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            sut.LoginWithGoogleAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsNonTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        IActionResult result = sut.LoginWithGoogleAccount(string.Empty);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsNonTrustedDomain_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.LoginWithGoogleAccount(string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.LoginWithGoogleAccount(string.Empty);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithGoogleAccount(string.Empty);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(string.Empty);

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpace_AssertActionWasCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpace_AssertIsTrustedDomainWasCalledOnTrustedDomainResolver()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.LoginWithGoogleAccount(" ");

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsNonTrustedDomain_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            sut.LoginWithGoogleAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsNonTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        IActionResult result = sut.LoginWithGoogleAccount(" ");

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsNonTrustedDomain_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.LoginWithGoogleAccount(" ");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.LoginWithGoogleAccount(" ");

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithGoogleAccount(" ");

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(" ");

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreRelativeUrl_AssertActionWasNotCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreRelativeUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUriForReturnUrl()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreNonTrustedRelativeUrl_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToHAccount()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreNonTrustedRelativeUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        string returnUrl = _fixture.CreateRelativeEndpointString();
	        IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreNonTrustedRelativeUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateRelativeEndpointString();
            IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        string returnUrl = _fixture.CreateRelativeEndpointString();
	        IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(returnUrl);

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithReturnUrl()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, returnUrl) == 0)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreNonTrustedAbsoluteUrl_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreNonTrustedAbsoluteUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateEndpointString();
	        IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateEndpointString();
            IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            sut.LoginWithGoogleAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
	        IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            IActionResult result = sut.LoginWithGoogleAccount(returnUrl);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(returnUrl);

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        private Controller CreateSut(bool isTrustedDomain = true, string absolutePath = null)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);

            _urlHelperMock.Setup(_fixture, absolutePath: absolutePath);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _trustedDomainResolverMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object)
            {
                Url = _urlHelperMock.Object
            };
        }
    }
}