using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class HasExpiresAtTests
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
		public void HasExpiresAt_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.HasExpiresAt(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresAt_WhenItemsDoesNotContainKeyForExpiresAt_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(false);

			bool result = sut.HasExpiresAt();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresAt_WhenItemsContainsKeyForExpiresAtWithoutAnyValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			bool result = sut.HasExpiresAt();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresAt_WhenItemsContainsKeyForExpiresAtWithDateTimeValueInRcf1123Format_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut(true, true, DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture));

			bool result = sut.HasExpiresAt();

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresAt_WhenItemsContainsKeyForExpiresAtWithNonDateTimeValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, true, _fixture.Create<string>());

			bool result = sut.HasExpiresAt();

			Assert.That(result, Is.False);
		}

		private IDictionary<string, string> CreateSut(bool hasExpiresAtKey = true, bool hasExpiresAtValue = true, string expiresAt = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasExpiresAtKey)
			{
				items.Add(AuthenticationSessionKeys.ExpiresAtKey, hasExpiresAtValue ? expiresAt ?? DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture) : null);
			}

			return items;
		}
	}
}