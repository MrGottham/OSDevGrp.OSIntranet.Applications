using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Attributes;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.Filters.AcquireTokenActionFilter
{
    [TestFixture]
    public class OnActionExecutingTests
    {
        #region Private variables

        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenContextIsNull_ThrowsArgumentNullException()
        {
            IActionFilter sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.OnActionExecuting(null));

            Assert.That(result.ParamName, Is.EqualTo("context"));
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionNotDecoratedWithAcquireTokenAttribute_AssertGetTokenAsyncWasNotCalledOnTokenHelperFactoryForToken()
        {
            IActionFilter sut = CreateSut();

            ActionDescriptor actionDescriptor = CreateActionDescriptor(false);
            ActionExecutingContext context = CreateActionExecutingContext(actionDescriptor);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionNotDecoratedWithAcquireTokenAttribute_AssertGetTokenAsyncWasNotCalledOnTokenHelperFactoryForRefreshableToken()
        {
            IActionFilter sut = CreateSut();

            ActionDescriptor actionDescriptor = CreateActionDescriptor(false);
            ActionExecutingContext context = CreateActionExecutingContext(actionDescriptor);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionNotDecoratedWithAcquireTokenAttribute_AssertHasExpiredWasNotCalledOnRefreshableToken()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            IActionFilter sut = CreateSut(refreshableToken: refreshableTokenMock.Object);

            ActionDescriptor actionDescriptor = CreateActionDescriptor(false);
            ActionExecutingContext context = CreateActionExecutingContext(actionDescriptor);
            sut.OnActionExecuting(context);

            refreshableTokenMock.Verify(m => m.HasExpired, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionNotDecoratedWithAcquireTokenAttribute_AssertRefreshTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            IActionFilter sut = CreateSut();

            ActionDescriptor actionDescriptor = CreateActionDescriptor(false);
            ActionExecutingContext context = CreateActionExecutingContext(actionDescriptor);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.RefreshTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionNotDecoratedWithAcquireTokenAttribute_AssertAuthorizeAsyncWasNotCalledOnTokenHelperFactory()
        {
            IActionFilter sut = CreateSut();

            ActionDescriptor actionDescriptor = CreateActionDescriptor(false);
            ActionExecutingContext context = CreateActionExecutingContext(actionDescriptor);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.AuthorizeAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionNotDecoratedWithAcquireTokenAttribute_ReturnsWithContextWhereResultIsNull()
        {
            IActionFilter sut = CreateSut();

            ActionDescriptor actionDescriptor = CreateActionDescriptor(false);
            ActionExecutingContext context = CreateActionExecutingContext(actionDescriptor);
            sut.OnActionExecuting(context);

            Assert.That(context.Result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttribute_AssertGetTokenAsyncWasCalledOnTokenHelperFactoryForToken()
        {
            IActionFilter sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            ActionExecutingContext context = CreateActionExecutingContext(httpContext: httpContext);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value == httpContext)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttribute_AssertGetTokenAsyncWasCalledOnTokenHelperFactoryForRefreshableToken()
        {
            IActionFilter sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            ActionExecutingContext context = CreateActionExecutingContext(httpContext: httpContext);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value == httpContext)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndRefreshableTokenWasNotPresent_AssertHasExpiredWasNotCalledOnRefreshableToken()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            IActionFilter sut = CreateSut(hasRefreshableToken: false, refreshableToken: refreshableTokenMock.Object);

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            refreshableTokenMock.Verify(m => m.HasExpired, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndRefreshableTokenWasNotPresent_AssertRefreshTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            IActionFilter sut = CreateSut(hasRefreshableToken: false);

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.RefreshTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndRefreshableTokenWasNotPresent_AssertAuthorizeAsyncWasCalledOnTokenHelperFactory()
        {
            IActionFilter sut = CreateSut(hasRefreshableToken: false);

            string pathBase = $"/{_fixture.Create<string>()}";
            string path = $"/{_fixture.Create<string>()}";
            HttpContext httpContext = CreateHttpContext(pathBase, path);
            ActionExecutingContext context = CreateActionExecutingContext(httpContext: httpContext);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.AuthorizeAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value == httpContext),
                    It.Is<string>(value => string.CompareOrdinal(value, $"http://localhost{pathBase}{path}") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndRefreshableTokenWasNotPresent_ReturnsWithContextWhereResultIsActionResultFromAuthorizeAsync()
        {
            IActionResult authorizeActionResult = new Mock<IActionResult>().Object;
            IActionFilter sut = CreateSut(hasRefreshableToken: false, authorizeActionResult: authorizeActionResult);

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            Assert.That(context.Result, Is.EqualTo(authorizeActionResult));
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndRefreshableTokenWasPresent_AssertHasExpiredWasCalledOnRefreshableToken()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            IActionFilter sut = CreateSut(refreshableToken: refreshableTokenMock.Object);

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            refreshableTokenMock.Verify(m => m.HasExpired, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndExpiredRefreshableTokenWasPresent_AssertRefreshTokenAsyncWasCalledOnTokenHelperFactory()
        {
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(hasExpired: true).Object;
            IActionFilter sut = CreateSut(refreshableToken: refreshableToken);

            string pathBase = $"/{_fixture.Create<string>()}";
            string path = $"/{_fixture.Create<string>()}";
            HttpContext httpContext = CreateHttpContext(pathBase, path);
            ActionExecutingContext context = CreateActionExecutingContext(httpContext: httpContext);
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.RefreshTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value == httpContext),
                    It.Is<string>(value => string.CompareOrdinal(value, $"http://localhost{pathBase}{path}") == 0)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndExpiredRefreshableTokenWasPresent_AssertAuthorizeAsyncWasNotCalledOnTokenHelperFactory()
        {
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(hasExpired: true).Object;
            IActionFilter sut = CreateSut(refreshableToken: refreshableToken);

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.AuthorizeAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndExpiredRefreshableTokenWasPresent_ReturnsWithContextWhereResultIsActionResultFromRefreshTokenAsync()
        {
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(hasExpired: true).Object;
            IActionResult refreshTokenActionResult = new Mock<IActionResult>().Object;
            IActionFilter sut = CreateSut(refreshableToken: refreshableToken, refreshTokenActionResult: refreshTokenActionResult);

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            Assert.That(context.Result, Is.EqualTo(refreshTokenActionResult));
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndNonExpiredRefreshableTokenWasPresent_AssertRefreshTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            IActionFilter sut = CreateSut();

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.RefreshTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndNonExpiredRefreshableTokenWasPresent_AssertAuthorizeAsyncWasNotCalledOnTokenHelperFactory()
        {
            IActionFilter sut = CreateSut();

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            _tokenHelperFactoryMock.Verify(m => m.AuthorizeAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuting_WhenActionDecoratedWithAcquireTokenAttributeAndNonExpiredRefreshableTokenWasPresent_ReturnsWithContextWhereResultIsNull()
        {
            IActionFilter sut = CreateSut();

            ActionExecutingContext context = CreateActionExecutingContext();
            sut.OnActionExecuting(context);

            Assert.That(context.Result, Is.Null);
        }

        private IActionFilter CreateSut(bool hasToken = true, IToken token = null, bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, IActionResult refreshTokenActionResult = null, IActionResult authorizeActionResult = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasToken ? token ?? _fixture.BuildTokenMock().Object : null));
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));
            _tokenHelperFactoryMock.Setup(m => m.RefreshTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.Run(() => refreshTokenActionResult ?? new Mock<IActionResult>().Object));
            _tokenHelperFactoryMock.Setup(m => m.AuthorizeAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.Run(() => authorizeActionResult ?? new Mock<IActionResult>().Object));

            return new Mvc.Helpers.Security.Filters.AcquireTokenActionFilter(_tokenHelperFactoryMock.Object);
        }

        private ActionExecutingContext CreateActionExecutingContext(ActionDescriptor actionDescriptor = null, HttpContext httpContext = null)
        {
            actionDescriptor ??= CreateActionDescriptor();
            httpContext ??= CreateHttpContext();

            return new ActionExecutingContext(CreateActionContext(httpContext, actionDescriptor), new List<IFilterMetadata>(), new Dictionary<string, object>(), null)
            {
                ActionDescriptor = actionDescriptor
            };
        }

        private ActionContext CreateActionContext(HttpContext httpContext, ActionDescriptor actionDescriptor)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNull(actionDescriptor, nameof(actionDescriptor));

            return new ActionContext(httpContext, new RouteData(), actionDescriptor);
        }

        private ActionDescriptor CreateActionDescriptor(bool methodInfoHasAcquireTokenAttributeDecoration = true)
        {
            return new ControllerActionDescriptor
            {
                MethodInfo = methodInfoHasAcquireTokenAttributeDecoration ? GetMethodInfoWithAcquireTokenAttributeDecoration() : GetMethodInfoWithoutAcquireTokenAttributeDecoration()
            };
        }

        [AcquireToken(TokenType.MicrosoftGraphToken)]
        private MethodInfo GetMethodInfoWithAcquireTokenAttributeDecoration()
        {
            return (MethodInfo) MethodBase.GetCurrentMethod();
        }

        private MethodInfo GetMethodInfoWithoutAcquireTokenAttributeDecoration()
        {
            return (MethodInfo) MethodBase.GetCurrentMethod();
        }

        private HttpContext CreateHttpContext(string pathBase = null, string path = null)
        {
            DefaultHttpContext defaultHttpContext = new DefaultHttpContext();
            defaultHttpContext.Request.Scheme = "http";
            defaultHttpContext.Request.Host = new HostString("localhost");
            defaultHttpContext.Request.PathBase = new PathString(pathBase ?? $"/{_fixture.Create<string>()}");
            defaultHttpContext.Request.Path = new PathString(path ?? $"/{_fixture.Create<string>()}");
            return defaultHttpContext;
        }
    }
}