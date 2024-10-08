using AutoFixture;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
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
    public class LoginWithMicrosoftAccountTests
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
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNull_AssertActionWasCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            sut.LoginWithMicrosoftAccount();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value  => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNull_AssertIsTrustedDomainWasCalledOnTrustedDomainResolver()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.LoginWithMicrosoftAccount();

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsNonTrustedDomain_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            sut.LoginWithMicrosoftAccount();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsNonTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        IActionResult result = sut.LoginWithMicrosoftAccount();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsNonTrustedDomain_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.LoginWithMicrosoftAccount();

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            sut.LoginWithMicrosoftAccount();

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.LoginWithMicrosoftAccount();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount();

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNullAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount();

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmpty_AssertActionWasCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            sut.LoginWithMicrosoftAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmpty_AssertIsTrustedDomainWasCalledOnTrustedDomainResolver()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.LoginWithMicrosoftAccount(string.Empty);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsNonTrustedDomain_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            sut.LoginWithMicrosoftAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsNonTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        IActionResult result = sut.LoginWithMicrosoftAccount(string.Empty);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsNonTrustedDomain_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.LoginWithMicrosoftAccount(string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            sut.LoginWithMicrosoftAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.LoginWithMicrosoftAccount(string.Empty);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount(string.Empty);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmptyAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount(string.Empty);

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpace_AssertActionWasCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            sut.LoginWithMicrosoftAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpace_AssertIsTrustedDomainWasCalledOnTrustedDomainResolver()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            sut.LoginWithMicrosoftAccount(" ");

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsNonTrustedDomain_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            sut.LoginWithMicrosoftAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsNonTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        IActionResult result = sut.LoginWithMicrosoftAccount(" ");

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsNonTrustedDomain_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.LoginWithMicrosoftAccount(" ");

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            sut.LoginWithMicrosoftAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.LoginWithMicrosoftAccount(" ");

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount(" ");

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpaceAndCalculatedReturnUriIsTrustedDomain_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount(" ");

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreRelativeUrl_AssertActionWasNotCalledOnUrlHelperWithActionEqualToIndexAndControllerEqualToHome()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithMicrosoftAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "Index") == 0 && string.CompareOrdinal(value.Controller, "Home") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreRelativeUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUriForReturnUrl()
        {
            string absolutePath = _fixture.CreateRelativeEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithMicrosoftAccount(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreNonTrustedRelativeUrl_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            string returnUrl = $"/{_fixture.Create<string>()}";
            sut.LoginWithMicrosoftAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreNonTrustedRelativeUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        string returnUrl = _fixture.CreateRelativeEndpointString();
	        IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreNonTrustedRelativeUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateRelativeEndpointString();
            IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            sut.LoginWithMicrosoftAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        string returnUrl = _fixture.CreateRelativeEndpointString();
	        IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedRelativeUrl_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateRelativeEndpointString();
            ChallengeResult result = (ChallengeResult)sut.LoginWithMicrosoftAccount(returnUrl);

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithReturnUrl()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            sut.LoginWithMicrosoftAccount(returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, returnUrl) == 0)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreNonTrustedAbsoluteUrl_AssertActionWasNotCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateEndpointString();
            sut.LoginWithMicrosoftAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreNonTrustedAbsoluteUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateEndpointString();
	        IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            string returnUrl = _fixture.CreateEndpointString();
            IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_AssertActionWasCalledOnUrlHelperWithActionEqualToLoginCallbackAndControllerEqualToAccount()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            sut.LoginWithMicrosoftAccount(returnUrl);

            _urlHelperMock.Verify(m => m.Action(It.Is<UrlActionContext>(value => value != null && string.CompareOrdinal(value.Action, "LoginCallback") == 0 && string.CompareOrdinal(value.Controller, "Account") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
	        IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

	        Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValueWhichAreTrustedAbsoluteUrl_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            string returnUrl = _fixture.CreateEndpointString();
            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount(returnUrl);

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
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