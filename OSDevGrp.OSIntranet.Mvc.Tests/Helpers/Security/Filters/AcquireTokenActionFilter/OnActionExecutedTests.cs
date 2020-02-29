using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.Filters.AcquireTokenActionFilter
{
    [TestFixture]
    public class OnActionExecutedTests
    {
        #region Private variables

        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuted_WhenContextIsNull_ThrowsArgumentNullException()
        {
            IActionFilter sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.OnActionExecuted(null));

            Assert.That(result.ParamName, Is.EqualTo("context"));
        }

        [Test]
        [Category("UnitTest")]
        public void OnActionExecuted_WhenCalled_ReturnsWithContextWhereResultIsNull()
        {
            IActionFilter sut = CreateSut();

            ActionExecutedContext context = CreateActionExecutedContext();
            sut.OnActionExecuted(context);

            Assert.That(context.Result, Is.Null);
        }

        private IActionFilter CreateSut()
        {
            return new Mvc.Helpers.Security.Filters.AcquireTokenActionFilter(_tokenHelperFactoryMock.Object);
        }

        private ActionExecutedContext CreateActionExecutedContext()
        {
            return new ActionExecutedContext(CreateActionContext(CreateHttpContext(), CreateActionDescriptor()), new List<IFilterMetadata>(), null);
        }

        private ActionContext CreateActionContext(HttpContext httpContext, ActionDescriptor actionDescriptor)
        {
            NullGuard.NotNull(httpContext, nameof(httpContext))
                .NotNull(actionDescriptor, nameof(actionDescriptor));

            return new ActionContext(httpContext, new RouteData(), actionDescriptor);
        }

        private ActionDescriptor CreateActionDescriptor()
        {
            return new ControllerActionDescriptor();
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }
    }
}