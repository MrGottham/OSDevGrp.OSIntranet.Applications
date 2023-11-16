using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class ResolveAccessTokenTests
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
		public void ResolveAccessToken_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.ResolveAccessToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveAccessToken_WhenItemsDoesNotContainKeyForAccessToken_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(false);

			string result = sut.ResolveAccessToken();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveAccessToken_WhenItemsContainsKeyForAccessTokenWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			string result = sut.ResolveAccessToken();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveAccessToken_WhenItemsContainsKeyForAccessTokenWithAnyValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut();

			string result = sut.ResolveAccessToken();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveAccessToken_WhenItemsContainsKeyForAccessTokenWithAnyValue_ReturnsAccessToken()
		{
			string accessToken = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(true, true, accessToken);

			string result = sut.ResolveAccessToken();

			Assert.That(result, Is.EqualTo(accessToken));
		}

		private IDictionary<string, string> CreateSut(bool hasAccessTokenKey = true, bool hasAccessTokenValue = true, string accessToken = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasAccessTokenKey)
			{
				items.Add(AuthenticationSessionKeys.AccessTokenKey, hasAccessTokenValue ? accessToken ?? _fixture.Create<string>() : null);
			}

			return items;
		}
	}
}