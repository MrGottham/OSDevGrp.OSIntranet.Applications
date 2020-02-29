using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperFactory
{
    [TestFixture]
    public class GetTokenAsyncTests
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
        public void GetTokenAsync_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetTokenAsync<IRefreshableToken>(_fixture.Create<TokenType>(), null));

            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetTokenAsync_WhenCalledWithUnsupportedTokenType_ThrowsNotSupportedException()
        {
            ITokenHelperFactory sut = CreateSut();

            TokenType tokenType = _fixture.Create<TokenType>();
            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(async () => await sut.GetTokenAsync<IRefreshableToken>(tokenType, CreateHttpContext()));

            Assert.That(result.Message, Is.EqualTo($"The token type '{tokenType}' is not supported within the method 'GetTokenAsync'."));
        }

        [Test]
        [Category("UnitTest")]
        public void GetTokenAsync_WhenCalledWithSupportedTokenTypeButUnsupportedGenericToken_ThrowsNotSupportedException()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            ITokenHelper<IToken> tokenHelper = BuildTokenHelperMock<IToken>(tokenType).Object;
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelper});

            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(async () => await sut.GetTokenAsync<IRefreshableToken>(tokenType, CreateHttpContext()));

            Assert.That(result.Message, Is.EqualTo($"The helper logic for the token type '{tokenType}' does not support the generic type 'IRefreshableToken' within the method 'GetTokenAsync'."));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetTokenAsync_WhenCalledWithSupportedTokenType_AssertGetTokenAsyncWasCalledOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            Mock<ITokenHelper<IRefreshableToken>> tokenHelperMock = BuildTokenHelperMock<IRefreshableToken>(tokenType);
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelperMock.Object});

            HttpContext httpContext = CreateHttpContext();
            await sut.GetTokenAsync<IRefreshableToken>(tokenType, httpContext);

            tokenHelperMock.Verify(m => m.GetTokenAsync(It.Is<HttpContext>(value => value == httpContext)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetTokenAsync_WhenCalledWithSupportedTokenType_ReturnTokenFromGetTokenAsyncOnTokenHelperForTokenType()
        {
            TokenType tokenType = _fixture.Create<TokenType>();
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock().Object;
            ITokenHelper<IRefreshableToken> tokenHelper = BuildTokenHelperMock(tokenType, refreshableToken).Object;
            ITokenHelperFactory sut = CreateSut(new[] {tokenHelper});

            HttpContext httpContext = CreateHttpContext();
            IRefreshableToken result = await sut.GetTokenAsync<IRefreshableToken>(tokenType, httpContext);

            Assert.That(result, Is.EqualTo(refreshableToken));
        }

        private ITokenHelperFactory CreateSut(IEnumerable<ITokenHelper> tokenHelperCollection = null)
        {
            return new Mvc.Helpers.Security.TokenHelperFactory(tokenHelperCollection ?? new ITokenHelper[0]);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }

        private Mock<ITokenHelper<T>> BuildTokenHelperMock<T>(TokenType tokenType = TokenType.MicrosoftGraphToken, T token = null) where T : class, IToken
        {
            Mock<ITokenHelper<T>> tokenHelperMock = new Mock<ITokenHelper<T>>();
            tokenHelperMock.Setup(m => m.TokenType)
                .Returns(tokenType);
            tokenHelperMock.Setup(m => m.GetTokenAsync(It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => token));
            return tokenHelperMock;
        }
    }
}