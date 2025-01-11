using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperFactoryExtensions
{
    [TestFixture]
    public class StoreMicrosoftTokenTests
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
        public void StoreMicrosoftToken_WhenTokenHelperFactoryIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await Mvc.Helpers.Security.TokenHelperFactoryExtensions.StoreMicrosoftToken(null, CreateHttpContext(), _fixture.BuildRefreshableTokenMock().Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("tokenHelperFactory"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreMicrosoftToken_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreMicrosoftToken(null, _fixture.BuildRefreshableTokenMock().Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreMicrosoftToken_WhenMicrosoftTokenIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreMicrosoftToken(CreateHttpContext(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("microsoftToken"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreMicrosoftToken_Called_AssertToBase64StringWasCalledOnMicrosoftToken()
        {
            ITokenHelperFactory sut = CreateSut();

            Mock<IRefreshableToken> microsoftTokenMock = _fixture.BuildRefreshableTokenMock();
            await sut.StoreMicrosoftToken(CreateHttpContext(), microsoftTokenMock.Object);

            microsoftTokenMock.Verify(m => m.ToBase64String(), Times.Once);

        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreMicrosoftToken_Called_AssertStoreTokenAsyncWasCalledOnTokenHelperFactoryWithTokenTypeEqualToMicrosoftGraphToken()
        {
            ITokenHelperFactory sut = CreateSut();

            await sut.StoreMicrosoftToken(CreateHttpContext(), _fixture.BuildRefreshableTokenMock().Object);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreMicrosoftToken_Called_AssertStoreTokenAsyncWasCalledOnTokenHelperFactoryWithHttpContextEqualToHttpContextFromArguments()
        {
            ITokenHelperFactory sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            await sut.StoreMicrosoftToken(httpContext, _fixture.BuildRefreshableTokenMock().Object);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.IsAny<TokenType>(),
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreMicrosoftToken_Called_AssertStoreTokenAsyncWasCalledOnTokenHelperFactoryWithBase64StringForMicrosoftTokenGivenInArguments()
        {
            ITokenHelperFactory sut = CreateSut();

            string base64String = _fixture.Create<string>();
            IRefreshableToken microsoftToken = _fixture.BuildRefreshableTokenMock(toBase64String: base64String).Object;
            await sut.StoreMicrosoftToken(CreateHttpContext(), microsoftToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.Compare(value, base64String) == 0)),
                Times.Once);
        }

        private ITokenHelperFactory CreateSut()
        {
            _tokenHelperFactoryMock.Setup(m => m.StoreTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            return _tokenHelperFactoryMock.Object;
        }

        private static HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }
    }
}