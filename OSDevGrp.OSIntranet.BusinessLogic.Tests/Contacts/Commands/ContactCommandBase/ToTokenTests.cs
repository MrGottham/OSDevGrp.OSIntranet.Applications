using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.ContactCommandBase
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
			IContactCommand sut = CreateSut();

			IRefreshableToken result = sut.ToToken();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereTokeTypeIsNotNull()
		{
			IContactCommand sut = CreateSut();

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereTokeTypeIsEqualToTokenTypeFromCommand()
		{
			string tokenType = _fixture.Create<string>();
			IContactCommand sut = CreateSut(tokenType);

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
		{
			IContactCommand sut = CreateSut();

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsEqualToAccessTokenFromCommand()
		{
			string accessToken = _fixture.Create<string>();
			IContactCommand sut = CreateSut(accessToken: accessToken);

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
		{
			IContactCommand sut = CreateSut();

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsEqualToRefreshTokenFromCommand()
		{
			string refreshToken = _fixture.Create<string>();
			IContactCommand sut = CreateSut(refreshToken: refreshToken);

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToToken_WhenCalled_ReturnsRefreshableTokenWhereExpiresIsEqualToExpiresFromCommand()
		{
			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IContactCommand sut = CreateSut(expires: expires);

			IRefreshableToken result = sut.ToToken();

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private IContactCommand CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return _fixture.Build<Sut>()
	            .With(m => m.TokenType, tokenType ?? _fixture.Create<string>())
	            .With(m => m.AccessToken, accessToken ?? _fixture.Create<string>())
	            .With(m => m.RefreshToken, refreshToken ?? _fixture.Create<string>())
	            .With(m => m.Expires, expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime())
	            .Create();
        }

		private class Sut : BusinessLogic.Contacts.Commands.ContactCommandBase
        {
        }
    }
}