using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperFactory
{
    [TestFixture]
    public class StoreTokenAsyncTests
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
        public void StoreTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(_fixture.Create<TokenType>(), null,  _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenBase64TokenIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(_fixture.Create<TokenType>(), CreateHttpContext(), null));

            Assert.That(result.ParamName, Is.EqualTo("base64Token"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenBase64TokenIsEmpty_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(_fixture.Create<TokenType>(), CreateHttpContext(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("base64Token"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenBase64TokenIsWhiteSpace_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreTokenAsync(_fixture.Create<TokenType>(), CreateHttpContext(), " "));

            Assert.That(result.ParamName, Is.EqualTo("base64Token"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreTokenAsync_WhenCalledWithUnsupportedTokenType_ThrowsNotSupportedException()
        {
            ITokenHelperFactory sut = CreateSut();

            TokenType tokenType = _fixture.Create<TokenType>();
            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(async () => await sut.StoreTokenAsync(tokenType, CreateHttpContext(), _fixture.Create<string>()));

            Assert.That(result.Message, Is.EqualTo($"The token type '{tokenType}' is not supported within the method 'StoreTokenAsync'."));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenCalledWithSupportedTokenType_AssertStoreTokenAsyncWasCalledOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            Mock<ITokenHelper> tokenHelperMock = BuildTokenHelperMock(tokenType);
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelperMock.Object});

            HttpContext httpContext = CreateHttpContext();
            string base64Token = _fixture.Create<string>();
            await sut.StoreTokenAsync(tokenType, httpContext, base64Token);

            tokenHelperMock.Verify(m => m.StoreTokenAsync(
                    It.Is<HttpContext>(value => value == httpContext),
                    It.Is<string>(value => string.CompareOrdinal(value, base64Token) == 0)),
                Times.Once);
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
            tokenHelperMock.Setup(m => m.StoreTokenAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return tokenHelperMock;
        }
    }
}