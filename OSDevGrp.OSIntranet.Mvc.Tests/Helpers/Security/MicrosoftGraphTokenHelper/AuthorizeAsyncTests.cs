using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.MicrosoftGraphTokenHelper
{
    [TestFixture]
    public class AuthorizeAsyncTests
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
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrl_AssertQueryAsyncWasCalledOnQueryBus()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>(It.Is<IGetAuthorizeUriForMicrosoftGraphQuery>(value => value.RedirectUri != null && value.RedirectUri.AbsoluteUri.EndsWith("/Account/MicrosoftGraphCallback".ToLower()))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResultWithUrlEqualToAbsoluteUriFromAuthorizeUri()
        {
            Uri authorizeUri = new Uri("http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}");
            ITokenHelper sut = CreateSut(authorizeUri: authorizeUri);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = $"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            RedirectResult result = (RedirectResult) await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result.Url, Is.EqualTo(authorizeUri.AbsoluteUri));
        }

        private ITokenHelper CreateSut(Uri authorizeUri = null)
        {
            _trustedDomainHelperMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(true);
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);
            _dataProtectorMock.Setup(m => m.Protect(It.IsAny<byte[]>()))
                .Returns(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray());
            _queryBusMock.Setup(m => m.QueryAsync<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>(It.IsAny<IGetAuthorizeUriForMicrosoftGraphQuery>()))
                .Returns(Task.Run(() => authorizeUri ?? new Uri("http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}")));

            return new Mvc.Helpers.Security.MicrosoftGraphTokenHelper(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainHelperMock.Object, _dataProtectionProviderMock.Object);
        }

        private HttpContext CreateHttpContext()
        {
            DefaultHttpContext defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = HttpMethods.Get;
            defaultHttpContext.Request.Scheme = "http";
            defaultHttpContext.Request.Host = new HostString("localhost");
            defaultHttpContext.Request.PathBase = new PathString($"/{_fixture.Create<string>()}");
            return defaultHttpContext;
        }
    }
}