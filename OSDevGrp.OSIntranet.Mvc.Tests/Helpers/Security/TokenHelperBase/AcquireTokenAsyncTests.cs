using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperBase
{
    [TestFixture]
    public class AcquireTokenAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _commandBusMock = new Mock<ICommandBus>();
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AcquireTokenAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenCalled_AssertGetStateIdentifierAsyncWasCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            Assert.That(((Sut) sut).GetStateIdentifierAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_AssertUnprotectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _dataProtectorMock.Verify(m => m.Unprotect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_AssertIsTrustedDomainWasNotCalledOnTrustedDomainHelper()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _trustedDomainHelperMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_AssertDoAcquireTokenAsyncWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            Assert.That(((Sut) sut).DoAcquireTokenAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_AssertProtectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_AssertTokenCookieNameWasCalledWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            Assert.That(((Sut) sut).TokenCookieNameWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenNoStateIdentifierWasReturnsFromGetStateIdentifierAsync_ReturnsBadRequestResult()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            IActionResult result = await sut.AcquireTokenAsync(httpContext);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_AssertUnprotectWasNotCalledOnDataProtector()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _dataProtectorMock.Verify(m => m.Unprotect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_AssertIsTrustedDomainWasNotCalledOnTrustedDomainHelper()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _trustedDomainHelperMock.Verify(m => m.IsTrustedDomain(It.IsAny<Uri>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_AssertDoAcquireTokenAsyncWasNotCalledOnSut()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            Assert.That(((Sut) sut).DoAcquireTokenAsyncWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_AssertProtectWasNotCalledOnDataProtector()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_AssertTokenCookieNameWasCalledWasNotCalledOnSut()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(httpContext);

            Assert.That(((Sut) sut).TokenCookieNameWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenStateIdentifierWasReturnsFromGetStateIdentifierAsyncButNoCookieForStateIdentifierWasFound_ReturnsBadRequestResult()
        {
            Guid stateIdentifier = Guid.NewGuid();
            ITokenHelper sut = CreateSut(stateIdentifier: stateIdentifier);

            HttpContext httpContext = CreateHttpContext();
            IActionResult result = await sut.AcquireTokenAsync(httpContext);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        private ITokenHelper CreateSut(bool hasStateIdentifier = true, Guid? stateIdentifier = null)
        {
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);

            return new Sut(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainHelperMock.Object, _dataProtectionProviderMock.Object, hasStateIdentifier, stateIdentifier, _fixture);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }

        private class Sut : TokenHelperBase<IRefreshableToken>
        {
            #region Private variables

            private readonly Guid? _stateIdentifier;
            private readonly Fixture _fixture;

            #endregion

            #region Constructor

            public Sut(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, IDataProtectionProvider dataProtectionProvider, bool hasStateIdentifier, Guid? stateIdentifier, Fixture fixture)
                : base(queryBus, commandBus, trustedDomainHelper, dataProtectionProvider)
            {
                NullGuard.NotNull(fixture, nameof(fixture));

                _stateIdentifier = hasStateIdentifier ? stateIdentifier ?? Guid.NewGuid() : (Guid?) null;
                _fixture = fixture;
            }

            #endregion

            #region Properties

            public override TokenType TokenType => TokenType.MicrosoftGraphToken;

            public bool GetStateIdentifierAsyncWasCalled { get; private set; }

            public bool DoAcquireTokenAsyncWasCalled { get; private set; }

            public bool TokenCookieNameWasCalled { get; private set; }

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
                NullGuard.NotNull(httpContext, nameof(httpContext))
                    .NotNull(arguments, nameof(arguments));

                return Task.Run(() =>
                {
                    GetStateIdentifierAsyncWasCalled = true;

                    return _stateIdentifier;
                });
            }

            protected override Task<IRefreshableToken> DoAcquireTokenAsync(HttpContext httpContext, params object[] arguments)
            {
                NullGuard.NotNull(httpContext, nameof(httpContext))
                    .NotNull(arguments, nameof(arguments));

                return Task.Run(() =>
                {
                    DoAcquireTokenAsyncWasCalled = true;

                    return _fixture.BuildRefreshableTokenMock().Object;
                });
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