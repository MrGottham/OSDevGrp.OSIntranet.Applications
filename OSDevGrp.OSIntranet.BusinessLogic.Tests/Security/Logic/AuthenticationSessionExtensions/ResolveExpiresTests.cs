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
	public class ResolveExpiresTests
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
		public void ResolveExpires_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.ResolveExpires(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithoutAnyValueAndHasNoKeyForExpiresIn_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: false, hasExpiresInKey: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithoutAnyValueAndHasKeyForExpiresInWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: false, hasExpiresInKey: true, hasExpiresInValue: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithoutAnyValueAndHasKeyForExpiresInWithNonDoubleValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: false, hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: _fixture.Create<string>());

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithoutAnyValueAndHasKeyForExpiresInWithDoubleValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: false, hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString((double) _random.Next(60, 3600), CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithoutAnyValueAndHasKeyForExpiresInWithDoubleValue_ReturnsExpiresBasedOnExpiresIn()
		{
			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: false, hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString(expiresIn.TotalSeconds, CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(DateTime.UtcNow.Add(expiresIn)).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithNonDateTimeValueAndHasNoKeyForExpiresIn_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: _fixture.Create<string>(), hasExpiresInKey: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithNonDateTimeValueAndHasKeyForExpiresInWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: _fixture.Create<string>(), hasExpiresInKey: true, hasExpiresInValue: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithNonDateTimeValueAndHasKeyForExpiresInWithNonDoubleValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: _fixture.Create<string>(), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: _fixture.Create<string>());

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithNonDateTimeValueAndHasKeyForExpiresInWithDoubleValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: _fixture.Create<string>(), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString((double)_random.Next(60, 3600), CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithNonDateTimeValueAndHasKeyForExpiresInWithDoubleValue_ReturnsExpiresBasedOnExpiresIn()
		{
			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: _fixture.Create<string>(), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString(expiresIn.TotalSeconds, CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(DateTime.UtcNow.Add(expiresIn)).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasNoKeyForExpiresIn_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasNoKeyForExpiresIn_ReturnsExpiresBasedOnExpiresAt()
		{
			DateTime expiresAt = DateTime.UtcNow.AddSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: expiresAt.ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(expiresAt).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasKeyForExpiresInWithoutAnyValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: true, hasExpiresInValue: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasKeyForExpiresInWithoutAnyValue_ReturnsExpiresBasedOnExpiresAt()
		{
			DateTime expiresAt = DateTime.UtcNow.AddSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: expiresAt.ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: true, hasExpiresInValue: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(expiresAt).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasKeyForExpiresInWithNonDoubleValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: _fixture.Create<string>());

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasKeyForExpiresInWithNonDoubleValue_ReturnsExpiresBasedOnExpiresAt()
		{
			DateTime expiresAt = DateTime.UtcNow.AddSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: expiresAt.ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: _fixture.Create<string>());

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(expiresAt).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasKeyForExpiresInWithDoubleValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString((double)_random.Next(60, 3600), CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenContainsKeyForExpiresAtWithDateTimeValueAndHasKeyForExpiresInWithDoubleValue_ReturnsExpiresBasedOnExpiresAt()
		{
			DateTime expiresAt = DateTime.UtcNow.AddSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: true, hasExpiresAtValue: true, expiresAt: expiresAt.ToString("R", CultureInfo.InvariantCulture), hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString((double)_random.Next(60, 3600), CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(expiresAt).Within(1).Seconds);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenItemsDoesNotContainKeyForExpiresAtAndHasNoKeyForExpiresIn_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: false, hasExpiresInKey: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenItemsDoesNotContainKeyForExpiresAtButHasKeyForExpiresInWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: false, hasExpiresInKey: true, hasExpiresInValue: false);

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenItemsDoesNotContainKeyForExpiresAtButHasKeyForExpiresInWithNonDoubleValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: false, hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: _fixture.Create<string>());

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenItemsDoesNotContainKeyForExpiresAtButHasKeyForExpiresInWithWithDoubleValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: false, hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString((double) _random.Next(60, 3600), CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExpires_WhenItemsDoesNotContainKeyForExpiresAtButHasKeyForExpiresInWithWithDoubleValue_ReturnsExpiresBasedOnExpiresIn()
		{
			TimeSpan expiresIn = TimeSpan.FromSeconds(_random.Next(60, 3600));
			IDictionary<string, string> sut = CreateSut(hasExpiresAtKey: false, hasExpiresInKey: true, hasExpiresInValue: true, expiresIn: Convert.ToString(expiresIn.TotalSeconds, CultureInfo.InvariantCulture));

			DateTime? result = sut.ResolveExpires();

			Assert.That(result, Is.EqualTo(DateTime.UtcNow.Add(expiresIn)).Within(1).Seconds);
		}

		private IDictionary<string, string> CreateSut(bool hasExpiresAtKey = true, bool hasExpiresAtValue = true, string expiresAt = null, bool hasExpiresInKey = true, bool hasExpiresInValue = true, string expiresIn = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasExpiresAtKey)
			{
				items.Add(AuthenticationSessionKeys.ExpiresAtKey, hasExpiresAtValue ? expiresAt ?? DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)).ToString("R", CultureInfo.InvariantCulture) : null);
			}

			if (hasExpiresInKey)
			{
				items.Add(AuthenticationSessionKeys.ExpiresInKey, hasExpiresInValue ? expiresIn ?? Convert.ToString((double) _random.Next(60, 3600), CultureInfo.InvariantCulture) : null);
			}

			return items;
		}
	}
}