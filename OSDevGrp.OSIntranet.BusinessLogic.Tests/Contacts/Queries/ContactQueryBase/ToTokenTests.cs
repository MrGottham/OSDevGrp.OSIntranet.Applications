using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Queries.ContactQueryBase
{
	[TestFixture]
    public class ToTokenTests
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
        public void ToToken_WhenCalled_ReturnsNotNull()
        {
            IContactQuery sut = CreateSut();

            IRefreshableToken result = sut.ToToken();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereTokeTypeIsNotNull()
        {
	        IContactQuery sut = CreateSut();

	        IRefreshableToken result = sut.ToToken();

	        Assert.That(result.TokenType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereTokeTypeIsEqualToTokenTypeFromQuery()
        {
            string tokenType = _fixture.Create<string>();
            IContactQuery sut = CreateSut(tokenType);

            IRefreshableToken result = sut.ToToken();

            Assert.That(result.TokenType, Is.EqualTo(tokenType));
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
        {
	        IContactQuery sut = CreateSut();

	        IRefreshableToken result = sut.ToToken();

	        Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsEqualToAccessTokenFromQuery()
        {
	        string accessToken = _fixture.Create<string>();
	        IContactQuery sut = CreateSut(accessToken: accessToken);

	        IRefreshableToken result = sut.ToToken();

	        Assert.That(result.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
        {
	        IContactQuery sut = CreateSut();

	        IRefreshableToken result = sut.ToToken();

	        Assert.That(result.RefreshToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsEqualToRefreshTokenFromQuery()
        {
	        string refreshToken = _fixture.Create<string>();
	        IContactQuery sut = CreateSut(refreshToken: refreshToken);

	        IRefreshableToken result = sut.ToToken();

	        Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereExpiresIsEqualToExpiresFromQuery()
        {
	        DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
	        IContactQuery sut = CreateSut(expires: expires);

	        IRefreshableToken result = sut.ToToken();

	        Assert.That(result.Expires, Is.EqualTo(expires));
        }

        private IContactQuery CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.TokenType, tokenType ?? _fixture.Create<string>())
                .With(m => m.AccessToken, accessToken ?? _fixture.Create<string>())
                .With(m => m.RefreshToken, refreshToken ?? _fixture.Create<string>())
                .With(m => m.Expires, expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime())
                .Create();
        }

        private class Sut : BusinessLogic.Contacts.Queries.ContactQueryBase
        {
        }
    }
}