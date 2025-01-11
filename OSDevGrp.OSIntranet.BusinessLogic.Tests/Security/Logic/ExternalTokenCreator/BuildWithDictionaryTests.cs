using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ExternalTokenCreator
{
	[TestFixture]
	public class BuildWithDictionaryTests : ExternalTokenCreatorTestBase
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
		public void Build_WhenAuthenticationSessionItemsIsNull_ThrowsArgumentNullException()
		{
			IExternalTokenCreator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Build((IDictionary<string, string>) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("authenticationSessionItems"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, false)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsNotNull(bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, false)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsToken(bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result, Is.AssignableTo<IToken>());
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, false)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsNotRefreshableToken(bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result, Is.Not.AssignableTo<IRefreshableToken>());
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: true, hasExpiresIn: true);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsTokenWithTokenTypeFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, tokenType: tokenType, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: true, hasExpiresIn: true);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: true, hasExpiresIn: true);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshToken_ReturnsTokenWithAccessTokenFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, accessToken: accessToken, hasRefreshToken: false, hasExpiresAt: true, hasExpiresIn: true);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshTokenButKeyForExpiresAt_ReturnsTokenWithExpiresBasedOnExpiresAtFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			DateTime expiresAt = DateTime.UtcNow.AddSeconds(_random.Next(60, 3600));
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: true, expiresAt: expiresAt, hasExpiresIn: true);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result.Expires, Is.EqualTo(expiresAt).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsDoesNotContainKeyForRefreshTokenAndNoKeyForExpiresAt_ReturnsTokenWithExpiresBasedOnExpiresInFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: false, hasExpiresAt: false, hasExpiresIn: true, expiresIn: expiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result.Expires, Is.EqualTo(DateTime.UtcNow.Add(expiresIn)).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, false)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsNotNull(bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, false)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsToken(bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result, Is.AssignableTo<IToken>());
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, false)]
		[TestCase(true, true)]
		[TestCase(false, true)]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableToken(bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			IToken result = sut.Build(authenticationSessionItems);

			Assert.That(result, Is.AssignableTo<IRefreshableToken>());
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableTokenWhereTokenTypeIsNotNull()
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: true, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableTokenWithTokenTypeFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, tokenType: tokenType, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: true, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: true, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableTokenWithAccessTokenFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, accessToken: accessToken, hasRefreshToken: true, hasExpiresAt: true, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: true, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshToken_ReturnsRefreshableTokenWithRefreshTokenFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, refreshToken: refreshToken, hasExpiresAt: true, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshTokenButKeyForExpiresAt_ReturnsRefreshableTokenWithExpiresBasedOnExpiresAtFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			DateTime expiresAt = DateTime.UtcNow.AddSeconds(_random.Next(60, 3600));
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: true, expiresAt: expiresAt, hasExpiresIn: true);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.Expires, Is.EqualTo(expiresAt).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionContainsKeyForRefreshTokenAndNoKeyForExpiresAt_ReturnsRefreshableTokenWithExpiresBasedOnExpiresInFromAuthenticationSessionItems()
		{
			IExternalTokenCreator sut = CreateSut();

			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: true, hasExpiresAt: false, hasExpiresIn: true, expiresIn: expiresIn);
			IRefreshableToken result = (IRefreshableToken) sut.Build(authenticationSessionItems);

			Assert.That(result.Expires, Is.EqualTo(DateTime.UtcNow.Add(expiresIn)).Within(1).Seconds);
		}
	}
}