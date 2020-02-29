using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AcquireTokenAsyncTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AcquireTokenAsync(_fixture.Create<TokenType>(), null));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireTokenAsync_WhenCalledWithUnsupportedTokenType_ThrowsNotSupportedException()
        {
            ITokenHelperFactory sut = CreateSut();

            TokenType tokenType = _fixture.Create<TokenType>();
            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(async () => await sut.AcquireTokenAsync(tokenType, CreateHttpContext()));

            Assert.That(result.Message, Is.EqualTo($"The token type '{tokenType}' is not supported within the method 'AcquireTokenAsync'."));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenCalledWithSupportedTokenTypeWithoutArguments_AssertAcquireTokenAsyncWasCalledOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            Mock<ITokenHelper> tokenHelperMock = BuildTokenHelperMock(tokenType);
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelperMock.Object});

            HttpContext httpContext = CreateHttpContext();
            await sut.AcquireTokenAsync(tokenType, httpContext);

            tokenHelperMock.Verify(m => m.AcquireTokenAsync(
                    It.Is<HttpContext>(value => value == httpContext),
                    It.Is<object[]>(value => value != null && value.Length == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenCalledWithSupportedTokenTypeWithoutArguments_ReturnsActionResultFromAcquireTokenAsyncOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            IActionResult actionResult = new Mock<IActionResult>().Object;
            ITokenHelper tokenHelper = BuildTokenHelperMock(tokenType, actionResult).Object;
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelper});

            HttpContext httpContext = CreateHttpContext();
            IActionResult result = await sut.AcquireTokenAsync(tokenType, httpContext);

            Assert.That(result, Is.EqualTo(actionResult));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenCalledWithSupportedTokenTypeWithArguments_AssertAcquireTokenAsyncWasCalledOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            Mock<ITokenHelper> tokenHelperMock = BuildTokenHelperMock(tokenType);
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelperMock.Object});

            HttpContext httpContext = CreateHttpContext();
            object[] arguments = _fixture.CreateMany<object>(_random.Next(5, 10)).ToArray();
            await sut.AcquireTokenAsync(tokenType, httpContext, arguments);

            tokenHelperMock.Verify(m => m.AcquireTokenAsync(
                    It.Is<HttpContext>(value => value == httpContext),
                    It.Is<object[]>(value => value == arguments)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireTokenAsync_WhenCalledWithSupportedTokenTypeWithArguments_ReturnsActionResultFromAcquireTokenAsyncOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            IActionResult actionResult = new Mock<IActionResult>().Object;
            ITokenHelper tokenHelper = BuildTokenHelperMock(tokenType, actionResult).Object;
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelper});

            HttpContext httpContext = CreateHttpContext();
            object[] arguments = _fixture.CreateMany<object>(_random.Next(5, 10)).ToArray();
            IActionResult result = await sut.AcquireTokenAsync(tokenType, httpContext, arguments);

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
            tokenHelperMock.Setup(m => m.AcquireTokenAsync(It.IsAny<HttpContext>(), It.IsAny<object[]>()))
                .Returns(Task.Run(() => actionResult ?? new Mock<IActionResult>().Object));
            return tokenHelperMock;
        }
    }
}