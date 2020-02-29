using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperFactory
{
    [TestFixture]
    public class RefreshTokenAsyncTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(_fixture.Create<TokenType>(), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenReturnUrlIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(_fixture.Create<TokenType>(), CreateHttpContext(), null));

            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenReturnUrlIsEmpty_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(_fixture.Create<TokenType>(), CreateHttpContext(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenReturnUrlIsWhiteSpace_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.RefreshTokenAsync(_fixture.Create<TokenType>(), CreateHttpContext(), " "));

            Assert.That(result.ParamName, Is.EqualTo("returnUrl"));
        }

        [Test]
        [Category("UnitTest")]
        public void RefreshTokenAsync_WhenCalledWithUnsupportedTokenType_ThrowsNotSupportedException()
        {
            ITokenHelperFactory sut = CreateSut();

            TokenType tokenType = _fixture.Create<TokenType>();
            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(async () => await sut.RefreshTokenAsync(tokenType, CreateHttpContext(), _fixture.Create<string>()));

            Assert.That(result.Message, Is.EqualTo($"The token type '{tokenType}' is not supported within the method 'RefreshTokenAsync'."));
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenCalledWithSupportedTokenType_AssertRefreshTokenAsyncWasCalledOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            Mock<ITokenHelper> tokenHelperMock = BuildTokenHelperMock(tokenType);
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelperMock.Object});

            HttpContext httpContext = CreateHttpContext();
            string resultUrl = _fixture.Create<string>();
            await sut.RefreshTokenAsync(tokenType, httpContext, resultUrl);

            tokenHelperMock.Verify(m => m.RefreshTokenAsync(
                    It.Is<HttpContext>(value => value == httpContext),
                    It.Is<string>(value => string.CompareOrdinal(value, resultUrl) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task RefreshTokenAsync_WhenCalledWithSupportedTokenType_ReturnsActionResultFromRefreshTokenAsyncOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            IActionResult actionResult = new Mock<IActionResult>().Object;
            ITokenHelper tokenHelper = BuildTokenHelperMock(tokenType, actionResult).Object;
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelper});

            HttpContext httpContext = CreateHttpContext();
            string resultUrl = _fixture.Create<string>();
            IActionResult result = await sut.RefreshTokenAsync(tokenType, httpContext, resultUrl);

            Assert.That(result, Is.EqualTo(actionResult));
        }

        private ITokenHelperFactory CreateSut(IEnumerable<ITokenHelper> tokenHelperCollection = null)
        {
            return new Mvc.Helpers.Security.TokenHelperFactory(tokenHelperCollection ?? new ITokenHelper[0]);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }

        private Mock<ITokenHelper> BuildTokenHelperMock(TokenType tokenType = TokenType.MicrosoftGraphToken, IActionResult actionResult = null)
        {
            Mock<ITokenHelper> tokenHelperMock = new Mock<ITokenHelper>();
            tokenHelperMock.Setup(m => m.TokenType)
                .Returns(tokenType);
            tokenHelperMock.Setup(m => m.RefreshTokenAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.Run(() => actionResult ?? new Mock<IActionResult>().Object));
            return tokenHelperMock;
        }
    }
}