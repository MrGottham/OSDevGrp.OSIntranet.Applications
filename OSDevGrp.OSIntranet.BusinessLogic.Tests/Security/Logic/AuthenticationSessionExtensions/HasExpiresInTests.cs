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
	public class HasExpiresInTests
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
		public void HasExpiresIn_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.HasExpiresIn(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresIn_WhenItemsDoesNotContainKeyForExpiresIn_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(false);

			bool result = sut.HasExpiresIn();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresIn_WhenItemsContainsKeyForExpiresInWithoutAnyValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			bool result = sut.HasExpiresIn();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresIn_WhenItemsContainsKeyForExpiresInWithDoubleValue_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut(true, true, Convert.ToString((double) _random.Next(60, 3600), CultureInfo.InvariantCulture));

			bool result = sut.HasExpiresIn();

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExpiresIn_WhenItemsContainsKeyForExpiresInWithNonDoubleValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, true, _fixture.Create<string>());

			bool result = sut.HasExpiresIn();

			Assert.That(result, Is.False);
		}

		private IDictionary<string, string> CreateSut(bool hasExpiresInKey = true, bool hasExpiresInValue = true, string expiresIn = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasExpiresInKey)
			{
				items.Add(AuthenticationSessionKeys.ExpiresInKey, hasExpiresInValue ? expiresIn ?? Convert.ToString((double)_random.Next(60, 3600), CultureInfo.InvariantCulture) : null);
			}

			return items;
		}
	}
}