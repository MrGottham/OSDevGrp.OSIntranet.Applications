﻿using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.MicrosoftGraphTokenHelper
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
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrl_AssertQueryAsyncWasCalledOnQueryBus()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.CreateEndpointString();
            await sut.AuthorizeAsync(httpContext, returnUrl);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>(It.Is<IGetAuthorizeUriForMicrosoftGraphQuery>(value => value.RedirectUri != null && value.RedirectUri.AbsoluteUri.EndsWith("/Account/MicrosoftGraphCallback".ToLower()))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResult()
        {
            ITokenHelper sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.CreateEndpointString();
            IActionResult result = await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthorizeAsync_WhenReturnUrlIsTrustedAbsoluteUrl_ReturnsRedirectResultWithUrlEqualToAbsoluteUriFromAuthorizeUri()
        {
            Uri authorizeUri = _fixture.CreateEndpoint();
            ITokenHelper sut = CreateSut(authorizeUri: authorizeUri);

            HttpContext httpContext = CreateHttpContext();
            string returnUrl = _fixture.CreateEndpointString();
            RedirectResult result = (RedirectResult) await sut.AuthorizeAsync(httpContext, returnUrl);

            Assert.That(result.Url, Is.EqualTo(authorizeUri.AbsoluteUri));
        }

        private ITokenHelper CreateSut(Uri authorizeUri = null)
        {
            _trustedDomainResolverMock.Setup(m => m.IsTrustedDomain(It.IsAny<Uri>()))
                .Returns(true);
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);
            _dataProtectorMock.Setup(m => m.Protect(It.IsAny<byte[]>()))
                .Returns(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray());
            _queryBusMock.Setup(m => m.QueryAsync<IGetAuthorizeUriForMicrosoftGraphQuery, Uri>(It.IsAny<IGetAuthorizeUriForMicrosoftGraphQuery>()))
                .Returns(Task.Run(() => authorizeUri ?? _fixture.CreateEndpoint()));

            return new Mvc.Helpers.Security.MicrosoftGraphTokenHelper(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainResolverMock.Object, _dataProtectionProviderMock.Object);
        }

        private HttpContext CreateHttpContext()
        {
            Uri endpoint = _fixture.CreateEndpoint();

            DefaultHttpContext defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Method = HttpMethods.Get;
            defaultHttpContext.Request.Scheme = endpoint.Scheme;
            defaultHttpContext.Request.Host = new HostString(endpoint.Host);
            defaultHttpContext.Request.PathBase = new PathString($"/{_fixture.Create<string>()}");
            return defaultHttpContext;
        }
    }
}