using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperBase
{
    [TestFixture]
    public class GetTokenAsyncTests
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
        public void GetTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper<IRefreshableToken> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetTokenAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetTokenAsync_WhenCalled_AssertTokenCookieNameWasCalledOnSut()
        {
            ITokenHelper<IRefreshableToken> sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            await sut.GetTokenAsync(httpContext);

            Assert.That(((Sut) sut).TokenCookieNameWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetTokenAsync_WhenNoCookieForTokenWasFound_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper<IRefreshableToken> sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            await sut.GetTokenAsync(httpContext);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetTokenAsync_WhenNoCookieForTokenWasFound_AssertUnprotectWasNotCalledOnDataProtector()
        {
            ITokenHelper<IRefreshableToken> sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            await sut.GetTokenAsync(httpContext);

            _dataProtectorMock.Verify(m => m.Unprotect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetTokenAsync_WhenNoCookieForTokenWasFound_ReturnsNull()
        {
            ITokenHelper<IRefreshableToken> sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            IRefreshableToken result = await sut.GetTokenAsync(httpContext);

            Assert.That(result, Is.Null);
        }

        private ITokenHelper<IRefreshableToken> CreateSut()
        {
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);

            return new Sut(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainHelperMock.Object, _dataProtectionProviderMock.Object, _fixture);
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

            public Sut(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, IDataProtectionProvider dataProtectionProvider, Fixture fixture)
                : base(queryBus, commandBus, trustedDomainHelper, dataProtectionProvider)
            {
                NullGuard.NotNull(fixture, nameof(fixture));

                _fixture = fixture;
            }

            #endregion

            #region Properties

            public override TokenType TokenType => TokenType.MicrosoftGraphToken;

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