using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class HasRefreshTokenTests
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
		public void HasRefreshToken_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.HasRefreshToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void HasRefreshToken_WhenItemsDoesNotContainKeyForRefreshToken_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(false);

			bool result = sut.HasRefreshToken();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasRefreshToken_WhenItemsContainsKeyForRefreshTokenWithoutAnyValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			bool result = sut.HasRefreshToken();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasRefreshToken_WhenItemsContainsKeyForRefreshTokenWithAnyValue_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut();

			bool result = sut.HasRefreshToken();

			Assert.That(result, Is.True);
		}

		private IDictionary<string, string> CreateSut(bool hasRefreshTokenKey = true, bool hasRefreshTokenValue = true)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasRefreshTokenKey)
			{
				items.Add(AuthenticationSessionKeys.RefreshTokenKey, hasRefreshTokenValue ? _fixture.Create<string>() : null);
			}

			return items;
		}
	}
}