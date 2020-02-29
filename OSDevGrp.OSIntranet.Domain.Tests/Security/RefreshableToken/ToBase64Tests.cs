using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableToken
{
    [TestFixture]
    public class ToBase64Tests
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
        public void ToBase64_WhenCalled_AssertRefreshableTokenIsSerialized()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = _fixture.Create<DateTime>().ToUniversalTime();
            IToken sut = CreateSut(tokenType, accessToken, refreshToken, expires);

            string base64String = sut.ToBase64();

            IRefreshableToken result = Domain.Security.Token.Create<Domain.Security.RefreshableToken>(base64String);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TokenType, Is.EqualTo(tokenType));
            Assert.That(result.AccessToken, Is.EqualTo(accessToken));
            Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
            Assert.That(result.Expires, Is.EqualTo(expires));
        }

        [Test]
        [Category("UnitTest")]
        public void ToBase64_WhenCalled_ReturnsBase64String()
        {
            IToken sut = CreateSut();

            string result = sut.ToBase64();

            Assert.That(result.IsBase64String(), Is.True);
        }

        private IRefreshableToken CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? _fixture.Create<DateTime>().ToUniversalTime());
        }
    }
}