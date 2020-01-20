using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.ContactCommandBase
{
    [TestFixture]
    public class ToTokenTests
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
        public void ToToken_WhenCalled_ReturnsRefreshableToken()
        {
            IContactCommand sut = CreateSut();

            IRefreshableToken result = sut.ToToken();

            Assert.That(result, Is.TypeOf<Domain.Security.RefreshableToken>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWithTokenTypeFromCommand()
        {
            string tokenType = _fixture.Create<string>();
            IContactCommand sut = CreateSut(tokenType);

            string result = sut.ToToken().TokenType;

            Assert.That(result, Is.EqualTo(tokenType));
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWithAccessTokenFromCommand()
        {
            string accessToken = _fixture.Create<string>();
            IContactCommand sut = CreateSut(accessToken: accessToken);

            string result = sut.ToToken().AccessToken;

            Assert.That(result, Is.EqualTo(accessToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWithRefreshTokenFromCommand()
        {
            string refreshToken = _fixture.Create<string>();
            IContactCommand sut = CreateSut(refreshToken: refreshToken);

            string result = sut.ToToken().RefreshToken;

            Assert.That(result, Is.EqualTo(refreshToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWithExpiresFromCommand()
        {
            DateTime expires = _fixture.Create<DateTime>();
            IContactCommand sut = CreateSut(expires: expires);

            DateTime result = sut.ToToken().Expires;

            Assert.That(result, Is.EqualTo(expires));
        }

        private IContactCommand CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.TokenType, tokenType ?? _fixture.Create<string>())
                .With(m => m.AccessToken, accessToken ?? _fixture.Create<string>())
                .With(m => m.RefreshToken, refreshToken ?? _fixture.Create<string>())
                .With(m => m.Expires, expires ?? _fixture.Create<DateTime>())
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Commands.ContactCommandBase
        {
        }
    }
}
