using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
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
        public void Login_WhenReturnUrlIsNull_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            sut.Login();

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.Login();

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsNotNul()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.Login();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login();

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login();

            Assert.That(result.Model, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            sut.Login(string.Empty);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.Login(string.Empty);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.Login(string.Empty);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(string.Empty);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(string.Empty);

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(string.Empty);

            Assert.That(result.Model, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            Controller sut = CreateSut();

            sut.Login(" ");

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.Login(" ");

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.Login(" ");

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(" ");

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(" ");

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(" ");

            Assert.That(result.Model, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUrl()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString();
            sut.Login(absoluteUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, absoluteUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUrl_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString();
            sut.Login(absoluteUrl);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        string absoluteUrl = _fixture.CreateEndpointString();
	        IActionResult result = sut.Login(absoluteUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            string absoluteUrl = _fixture.CreateEndpointString();
            IActionResult result = sut.Login(absoluteUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        string absoluteUrl = _fixture.CreateEndpointString();
	        IActionResult result = sut.Login(absoluteUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString();
            IActionResult result = sut.Login(absoluteUrl);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString();
            ViewResult result = (ViewResult) sut.Login(absoluteUrl);

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsViewResultWhereModelIsUri()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString();
            ViewResult result = (ViewResult) sut.Login(absoluteUrl);

            Assert.That(result.Model, Is.TypeOf<Uri>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsViewResultWhereModelIsUriWithAbsoluteUriEqualToInput()
        {
            Controller sut = CreateSut();

            string absoluteUrl = _fixture.CreateEndpointString();
            Uri result = (Uri) ((ViewResult) sut.Login(absoluteUrl)).Model;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.AbsoluteUri, Is.EqualTo(absoluteUrl));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUrlNotStartingWithTildeAndSlash_AssertContentWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            if (relativeUrl.StartsWith('/'))
            {
                relativeUrl = relativeUrl.Substring(2);
            }
            sut.Login(relativeUrl);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.Compare(value, $"~/{relativeUrl}", StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUrlStartingWithSlash_AssertContentWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            if (relativeUrl.StartsWith('/') == false)
            {
                relativeUrl = $"/{relativeUrl}";
            }
            sut.Login(relativeUrl);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.Compare(value, $"~{relativeUrl}", StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUrlStartingWithTildeAndSlash_AssertContentWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            if (relativeUrl.StartsWith("~") == false && relativeUrl.StartsWith('/'))
            {
                relativeUrl = $"~{relativeUrl}";
            }
            else if (relativeUrl.StartsWith("~") == false)
            {
                relativeUrl = $"~/{relativeUrl}";
            }
            sut.Login(relativeUrl);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.Compare(value, $"{relativeUrl}", StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithAbsoluteUriForRelativeUrl()
        {
            string absolutePath = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            sut.Login(relativeUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && value.AbsoluteUri.EndsWith(absolutePath))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNonTrustedRelativeUrl_ReturnsNotNull()
        {
	        Controller sut = CreateSut(false);

	        string relativeUrl = _fixture.CreateRelativeEndpointString();
	        IActionResult result = sut.Login(relativeUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNonTrustedRelativeUrl_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            IActionResult result = sut.Login(relativeUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedRelativeUrl_ReturnsNotNul()
        {
	        Controller sut = CreateSut();

	        string relativeUrl = _fixture.CreateRelativeEndpointString();
	        IActionResult result = sut.Login(relativeUrl);

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedRelativeUrl_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            IActionResult result = sut.Login(relativeUrl);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedRelativeUrl_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            ViewResult result = (ViewResult) sut.Login(relativeUrl);

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedRelativeUrl_ReturnsViewResultWhereModelIsUri()
        {
            Controller sut = CreateSut();

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            ViewResult result = (ViewResult) sut.Login(relativeUrl);

            Assert.That(result.Model, Is.TypeOf<Uri>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsTrustedRelativeUrl_ReturnsViewResultWhereModelIsUriWithAbsoluteUriForInput()
        {
            string absolutePath = _fixture.CreateEndpointString();
            Controller sut = CreateSut(absolutePath: absolutePath);

            string relativeUrl = _fixture.CreateRelativeEndpointString();
            Uri result = (Uri) ((ViewResult) sut.Login(relativeUrl)).Model;

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.AbsoluteUri.EndsWith(absolutePath), Is.True);
            // ReSharper restore PossibleNullReferenceException
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