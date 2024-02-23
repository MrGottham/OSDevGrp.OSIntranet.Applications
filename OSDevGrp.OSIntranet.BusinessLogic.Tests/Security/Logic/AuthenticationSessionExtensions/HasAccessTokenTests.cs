using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class HasAccessTokenTests
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
		public void HasAccessToken_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.HasAccessToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void HasAccessToken_WhenItemsDoesNotContainKeyForAccessToken_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(false);

			bool result = sut.HasAccessToken();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasAccessToken_WhenItemsContainsKeyForAccessTokenWithoutAnyValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			bool result = sut.HasAccessToken();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasAccessToken_WhenItemsContainsKeyForAccessTokenWithAnyValue_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut();

			bool result = sut.HasAccessToken();

			Assert.That(result, Is.True);
		}

		private IDictionary<string, string> CreateSut(bool hasAccessTokenKey = true, bool hasAccessTokenValue = true)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasAccessTokenKey)
			{
				items.Add(AuthenticationSessionKeys.AccessTokenKey, hasAccessTokenValue ? _fixture.Create<string>() : null);
			}

			return items;
		}
	}
}