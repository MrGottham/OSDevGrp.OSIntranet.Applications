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
    public class StoreGoogleTokenTests
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
        public void StoreGoogleToken_WhenTokenHelperFactoryIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await Mvc.Helpers.Security.TokenHelperFactoryExtensions.StoreGoogleToken(null, CreateHttpContext(), _fixture.BuildTokenMock().Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("tokenHelperFactory"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreGoogleToken_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreGoogleToken(null, _fixture.BuildTokenMock().Object));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreGoogleToken_WhenGoogleTokenIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreGoogleToken(CreateHttpContext(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("googleToken"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreGoogleToken_Called_AssertToBase64StringWasNotCalledOnGoogleToken()
        {
            ITokenHelperFactory sut = CreateSut();

            Mock<IToken> googleTokenMock = _fixture.BuildTokenMock();
            await sut.StoreGoogleToken(CreateHttpContext(), googleTokenMock.Object);

            googleTokenMock.Verify(m => m.ToBase64String(), Times.Never);

        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreGoogleToken_Called_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            ITokenHelperFactory sut = CreateSut();

            await sut.StoreGoogleToken(CreateHttpContext(), _fixture.BuildTokenMock().Object);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
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