using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperFactory
{
    [TestFixture]
    public class HandleLogoutAsyncTests
    {
        [Test]
        [Category("UnitTest")]
        public void HandleLogoutAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.HandleLogoutAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task HandleLogoutAsync_WhenCalled_AssertHandleLogoutAsyncWasCalledOnEachTokenHelper()
        {
            IEnumerable<Mock<ITokenHelper>> tokenHelperMockCollection = new List<Mock<ITokenHelper>>
            {
                BuildTokenHelperMock()
            };
            ITokenHelperFactory sut = CreateSut(tokenHelperMockCollection.Select(m => m.Object).ToArray());

            HttpContext httpContext = CreateHttpContext();
            await sut.HandleLogoutAsync(httpContext);

            foreach (Mock<ITokenHelper> tokenHelperMock in tokenHelperMockCollection)
            {
                tokenHelperMock.Verify(m => m.HandleLogoutAsync(It.Is<HttpContext>(value => value == httpContext)), Times.Once);
            }
        }

        private ITokenHelperFactory CreateSut(IEnumerable<ITokenHelper> tokenHelperCollection = null)
        {
            return new Mvc.Helpers.Security.TokenHelperFactory(tokenHelperCollection ?? new ITokenHelper[0]);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }

        private Mock<ITokenHelper> BuildTokenHelperMock(TokenType tokenType = TokenType.MicrosoftGraphToken)
        {
            Mock<ITokenHelper> tokenHelperMock = new Mock<ITokenHelper>();
            tokenHelperMock.Setup(m => m.TokenType)
                .Returns(tokenType);
            tokenHelperMock.Setup(m => m.HandleLogoutAsync(It.IsAny<HttpContext>()))
                .Returns(Task.CompletedTask);
            return tokenHelperMock;
        }
    }
}