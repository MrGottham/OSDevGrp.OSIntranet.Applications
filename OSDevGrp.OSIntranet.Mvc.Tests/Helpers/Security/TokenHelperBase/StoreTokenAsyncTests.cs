using System;
using System.Linq;
using System.Text;
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
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperBase
{
    [TestFixture]
    public class StoreTokenAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
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
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenBase64TokenIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(CreateHttpContext(), null));

            Assert.That(result.ParamName, Is.EqualTo("base64Token"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenBase64TokenIsEmpty_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(CreateHttpContext(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("base64Token"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenBase64TokenIsWhiteSpace_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(CreateHttpContext(), " "));

            Assert.That(result.ParamName, Is.EqualTo("base64Token"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenCalled_AssertGenerateTokenAsyncWasCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            Assert.That(((Sut)sut).GenerateTokenAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenNoTokenWasGenerated_AssertCreateProtectorWasNotCalledOnDataProtectionProvider()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenNoTokenWasGenerated_AssertToByteArrayWasNotCalledOnToken()
        {
            Mock<IRefreshableToken> tokenMock = _fixture.BuildRefreshableTokenMock();
            ITokenHelper sut = CreateSut(false, tokenMock.Object);

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            tokenMock.Verify(m => m.ToByteArray(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenNoTokenWasGenerated_AssertProtectWasNotCalledOnDataProtector()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            _dataProtectorMock.Verify(m => m.Protect(It.IsAny<byte[]>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenNoTokenWasGenerated_AssertTokenCookieNameWasNotCalledOnSut()
        {
            ITokenHelper sut = CreateSut(false);

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            Assert.That(((Sut) sut).TokenCookieNameWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenTokenWasGenerated_AssertCreateProtectorWasCalledOnDataProtectionProviderWithTokenProtection()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.Is<string>(value => string.CompareOrdinal("TokenProtection", value) == 0)), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenTokenWasGenerated_AssertToByteArrayWasCalledOnToken()
        {
            Mock<IRefreshableToken> tokenMock = _fixture.BuildRefreshableTokenMock();
            ITokenHelper sut = CreateSut(token: tokenMock.Object);

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            tokenMock.Verify(m => m.ToByteArray(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenTokenWasGenerated_AssertProtectWasCalledOnDataProtectorWithTokenByteArray()
        {
            byte[] tokenByteArray = _fixture.CreateMany<byte>(_random.Next(512, 1024)).ToArray();
            IRefreshableToken token = _fixture.BuildRefreshableTokenMock(tokenByteArray: tokenByteArray).Object;
            ITokenHelper sut = CreateSut(token: token);

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            _dataProtectorMock.Verify(m => m.Protect(It.Is<byte[]>(value => value != null && string.CompareOrdinal(Encoding.UTF8.GetString(value), Encoding.UTF8.GetString(tokenByteArray)) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenTokenWasGenerated_AssertTokenCookieNameWasCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(httpContext, base64Token);

            Assert.That(((Sut)sut).TokenCookieNameWasCalled, Is.True);
        }

        private ITokenHelper CreateSut(bool hasToken = true, IRefreshableToken token = null)
        {
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);
            _dataProtectorMock.Setup(m => m.Protect(It.IsAny<byte[]>()))
                .Returns(_fixture.CreateMany<byte>(_random.Next(512, 1024)).ToArray);

            return new Sut(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainHelperMock.Object, _dataProtectionProviderMock.Object, hasToken, token, _fixture);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }

        private class Sut : TokenHelperBase<IRefreshableToken>
        {
            #region Private variables

            private readonly IRefreshableToken _token;
            private readonly Fixture _fixture;

            #endregion

            #region Constructor

            public Sut(IQueryBus queryBus, ICommandBus commandBus, ITrustedDomainHelper trustedDomainHelper, IDataProtectionProvider dataProtectionProvider, bool hasToken, IRefreshableToken token, Fixture fixture)
                : base(queryBus, commandBus, trustedDomainHelper, dataProtectionProvider)
            {
                NullGuard.NotNull(fixture, nameof(fixture));

                _token = hasToken ? token ?? fixture.BuildRefreshableTokenMock().Object : null;
                _fixture = fixture;
            }

            #endregion

            #region Properties

            public override TokenType TokenType => TokenType.MicrosoftGraphToken;

            public bool TokenCookieNameWasCalled { get; private set; }

            public bool GenerateTokenAsyncWasCalled { get; private set; }

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
                NullGuard.NotNull(httpContext, nameof(httpContext))
                    .NotNullOrWhiteSpace(base64Token, nameof(base64Token));

                return Task.Run(() =>
                {
                    GenerateTokenAsyncWasCalled = true;

                    return _token;
                });
            }

            #endregion
        }
    }
}