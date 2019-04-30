using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Resolvers.PrincipalResolver
{
    [TestFixture]
    public class GetCurrentPrincipalTests
    {
        #region Private variables

        Mock<IHttpContextAccessor> _httpContextAccessorMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        [Test]
        [Category("UnitTest")]
        public void GetCurrentPrincipal_WhenCalled_AssertHttpContextWasCalledOnHttpContextAccessor()
        {
            IPrincipalResolver sut = CreateSut();

            sut.GetCurrentPrincipal();

            _httpContextAccessorMock.Verify(m => m.HttpContext, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetCurrentPrincipal_WhenCalled_ReturnsPrincipalFromHttpContext()
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            IPrincipalResolver sut = CreateSut(claimsPrincipal);

            IPrincipal result = sut.GetCurrentPrincipal();

            Assert.That(result, Is.EqualTo(claimsPrincipal));
        }

        private IPrincipalResolver CreateSut(ClaimsPrincipal claimsPrincipal = null)
        {
            HttpContext httpContext = new DefaultHttpContext();
            httpContext.User = claimsPrincipal ?? new ClaimsPrincipal();

            _httpContextAccessorMock.Setup(m => m.HttpContext)
                .Returns(httpContext);

            return new WebApi.Helpers.Resolvers.PrincipalResolver(_httpContextAccessorMock.Object);
        }
    }
}