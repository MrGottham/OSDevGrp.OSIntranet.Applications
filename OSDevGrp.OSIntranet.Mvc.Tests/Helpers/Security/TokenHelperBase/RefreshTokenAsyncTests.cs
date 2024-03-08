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
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperBase
{
    [TestFixture]
    public class RefreshTokenAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Fixture _fixture;

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
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(null, _fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenReturnUrlIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(CreateHttpContext(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenReturnUrlIsEmpty_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(CreateHttpContext(), string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenReturnUrlIsWhiteSpace_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(CreateHttpContext(), " "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsNonAbsoluteUrl_AssertIsTrustedDomainWasNotCalledOnTrustedDomainResolver()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsNonAbsoluteUrl_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.Create<string>();
            IActionResult result = await sut.RefreshTokenAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsAbsoluteUrl_AssertIsTrustedDomainWasCalledOnTrustedDomainResolverWithUriForReturnUrl()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            _trustedDomainResolverMock.Verify(m => m.IsTrustedDomain(It.Is<Uri>(value => value != null && string.CompareOrdinal(value.AbsoluteUri, returnUrl) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsNonTrustedAbsoluteUrl_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = await sut.RefreshTokenAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsTrustedAbsoluteUrl_AssertTokenCookieNameWasCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            Assert.That(((Sut) sut).TokenCookieNameWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsTrustedAbsoluteUrlButNoCookieForTokenWasFound_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsTrustedAbsoluteUrlButNoCookieForTokenWasFound_AssertUnprotectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            _dataProtectorMock.Verify(m => m.Unprotect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsTrustedAbsoluteUrlButNoCookieForTokenWasFound_AssertDoRefreshTokenAsyncWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            Assert.That(((Sut) sut).DoRefreshTokenAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsTrustedAbsoluteUrlButNoCookieForTokenWasFound_AssertProtectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.RefreshTokenAsync(httpContext, returnUrl);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenReturnUrlIsTrustedAbsoluteUrlButNoCookieForTokenWasFound_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = await sut.RefreshTokenAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        private ITokenHelper CreateSut(bool isTrustedDomain = true)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(isTrustedDomain);
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);

            return new Sut(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainResolverMock.Object, _dataProtectionProviderMock.Object, _fixture);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }

        private class Sut : TokenHelperBase<IRefreshableToken>
        {
            #region Private variables

            private readonly Fixture _fixture;

            #endregion

            #region Constructor

            public Sut(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainResolver trustedDomainResolver, IDataProtectionProvider dataProtectionProvider, Fixture fixture)
                : base(queryBus, commandBus, trustedDomainResolver, dataProtectionProvider)
            {
                NullGuard.NotNull(fixture, nameof(fixture));

                _fixture = fixture;
            }

            #endregion

            #region Properties

            public override TokenType TokenType => TokenType.MicrosoftGraphToken;

            public bool TokenCookieNameWasCalled { get; private set; }

            public bool DoRefreshTokenAsyncWasCalled { get; private set; }

            protected override string TokenCookieName
            {
                get
                {
                    TokenCookieNameWasCalled = true;
                    return _fixture.Create<string>();
                }
            }

            #endregion

            #region Methods

            protected override Task<Uri> GenerateAuthorizeUriAsync(HttpContext httpContext, Guid stateIdentifier)
            {
                throw new NotSupportedException();
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
                NullGuard.NotNull(httpContext, nameof(httpContext))
                    .NotNull(expiredToken, nameof(expiredToken));

                return Task.Run(() =>
                {
                    DoRefreshTokenAsyncWasCalled = true;

                    return _fixture.BuildRefreshableTokenMock().Object;
                });
            }

            protected override Task<IRefreshableToken> GenerateTokenAsync(HttpContext httpContext, string base64Token)
            {
                throw new NotSupportedException();
            }

            #endregion
        }
    }
}