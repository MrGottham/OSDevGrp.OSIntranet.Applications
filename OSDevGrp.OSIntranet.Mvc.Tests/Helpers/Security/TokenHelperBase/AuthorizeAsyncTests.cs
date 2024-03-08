using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperBase
{
    [TestFixture]
    public class AuthorizeAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _commandBusMock = new Mock<ICommandBus>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void AuthorizeAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AuthorizeAsync(null, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void AuthorizeAsync_WhenReturnUrlIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AuthorizeAsync(CreateHttpContext(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void AuthorizeAsync_WhenReturnUrlIsEmpty_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AuthorizeAsync(CreateHttpContext(), string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void AuthorizeAsync_WhenReturnUrlIsWhiteSpace_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AuthorizeAsync(CreateHttpContext(), " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonAbsoluteUrl_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonAbsoluteUrl_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonAbsoluteUrl_AssertProtectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonAbsoluteUrl_AssertGenerateAuthorizeUriAsyncWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(((Sut) sut).GenerateAuthorizeUriAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            IActionResult result = await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithUriForReturnUrl()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, returnUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertProtectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonTrustedAbsoluteUrl_AssertGenerateAuthorizeUriAsyncWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(((Sut) sut).GenerateAuthorizeUriAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsNotGet_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext(HttpMethods.Post);
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsNotGet_AssertProtectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext(HttpMethods.Post);
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsNotGet_AssertGenerateAuthorizeUriAsyncWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext(HttpMethods.Post);
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(((Sut)sut).GenerateAuthorizeUriAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsNotGet_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext(HttpMethods.Post);
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsGet_AssertCreateProtectorWasCalledOnDataProtectionProviderWithStateProtection()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.Is<string>(value => string.CompareOrdinal(value, "StateProtection") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsGet_AssertProtectWasCalledOnDataProtectorWithByteArrayForReturnUrl()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _dataProtectorMock.Verify(m => m.Protect(It.Is<byte[]>(value => value != null && string.CompareOrdinal(Encoding.UTF8.GetString(value), returnUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsGet_AssertGenerateAuthorizeUriAsyncWasCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(((Sut) sut).GenerateAuthorizeUriAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsGet_ReturnsRedirectResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrlAndHttpRequestOnHttpContextIsGet_ReturnsRedirectResultWithUrlEqualToAbsoluteUriFromAuthorizeUri()
        {
            Uri authorizeUri = new Uri("http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}");
            ITokenHelper sut = CreateSut(authorizeUri: authorizeUri);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            RedirectResult result = (RedirectResult) await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result.Url, Is.EqualTo(authorizeUri.AbsoluteUri));
        }

        private ITokenHelper CreateSut(bool isTrustedDomain = true, Uri authorizeUri = null)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);
            _dataProtectorMock.Setup(m => m.Protect(It.IsAny<byte[]>()))
                .Returns(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray());

            return new Sut(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainResolverMock.Object, _dataProtectionProviderMock.Object, authorizeUri);
        }

        private HttpContext CreateHttpContext(string httpMethod = null)
        {
            HttpContext defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = httpMethod ?? HttpMethods.Get;
            return defaultHttpContext;
        }

        private class Sut : TokenHelperBase<IRefreshableToken>
        {
            #region Private variables

            private readonly Uri _authorizeUri;

            #endregion

            #region Constructor

            public Sut(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainResolver trustedDomainResolver, IDataProtectionProvider dataProtectionProvider, Uri authorizeUri) 
                : base(queryBus, commandBus, trustedDomainResolver, dataProtectionProvider)
            {
                _authorizeUri = authorizeUri ?? new Uri("http://localhost");
            }

            #endregion

            #region Properties

            public override TokenType TokenType => TokenType.MicrosoftGraphToken;

            public bool GenerateAuthorizeUriAsyncWasCalled { get; private set; }

            protected override string TokenCookieName => throw new NotSupportedException();

            #endregion

            #region Methods

            protected override Task<Uri> GenerateAuthorizeUriAsync(HttpContext httpContext, Guid stateIdentifier)
            {
                NullGuard.NotNull(httpContext, nameof(httpContext));

                return Task.Run(() =>
                {
                    GenerateAuthorizeUriAsyncWasCalled = true;

                    return _authorizeUri;
                });
            }

            protected override Task<Guid?> GetStateIdentifierAsync(HttpContext httpContext, params object[] arguments)
            {
                throw new NotSupportedException();
            }

            protected override Task<IRefreshableToken> DoAcquireTokenAsync(HttpContext httpContext, params object[] arguments)
            {
                throw new NotSupportedException();
            }

            protected override Task<IRefreshableToken> DoRefreshTokenAsync(HttpContext httpContext, IRefreshableToken expiredToken)
            {
                throw new NotSupportedException();
            }

            protected override Task<IRefreshableToken> GenerateTokenAsync(HttpContext httpContext, string base64Token)
            {
                throw new NotSupportedException();
            }

            #endregion
        }
    }
}