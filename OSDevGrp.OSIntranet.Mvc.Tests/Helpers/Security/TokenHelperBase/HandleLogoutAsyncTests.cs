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
    public class HandleLogoutAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _commandBusMock = new Mock<ICommandBus>();
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void HandleLogoutAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.HandleLogoutAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task HandleLogoutAsync_WhenCalled_AssertTokenCookieNameWasCalledOnSut()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            await sut.HandleLogoutAsync(httpContext);

            Assert.That(((Sut) sut).TokenCookieNameWasCalled, Is.True);
        }

        private ITokenHelper CreateSut()
        {
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