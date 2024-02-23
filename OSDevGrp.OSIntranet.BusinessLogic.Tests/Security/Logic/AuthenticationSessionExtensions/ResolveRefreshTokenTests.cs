using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class ResolveRefreshTokenTests
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
		public void ResolveRefreshToken_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.ResolveRefreshToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveRefreshToken_WhenItemsDoesNotContainKeyForRefreshToken_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(false);

			string result = sut.ResolveRefreshToken();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveRefreshToken_WhenItemsContainsKeyForRefreshTokenWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			string result = sut.ResolveRefreshToken();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveRefreshToken_WhenItemsContainsKeyForRefreshTokenWithAnyValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut();

			string result = sut.ResolveRefreshToken();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveRefreshToken_WhenItemsContainsKeyForRefreshTokenWithAnyValue_ReturnsRefreshToken()
		{
			string refreshToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(true, true, refreshToken);

			string result = sut.ResolveRefreshToken();

			Assert.That(result, Is.EqualTo(refreshToken));
		}

		private IDictionary<string, string> CreateSut(bool hasRefreshTokenKey = true, bool hasRefreshTokenValue = true, string refreshToken = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasRefreshTokenKey)
			{
				items.Add(AuthenticationSessionKeys.RefreshTokenKey, hasRefreshTokenValue ? refreshToken ?? _fixture.Create<string>() : null);
			}

			return items;
		}
	}
}