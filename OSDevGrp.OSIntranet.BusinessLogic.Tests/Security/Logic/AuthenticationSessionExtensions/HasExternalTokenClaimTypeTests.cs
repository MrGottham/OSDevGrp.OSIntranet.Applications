using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class HasExternalTokenClaimTypeTests
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
		public void HasExternalTokenClaimType_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.HasExternalTokenClaimType(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void HasExternalTokenClaimType_WhenItemsDoesNotContainKeyForExternalClaimType_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(false);

			bool result = sut.HasExternalTokenClaimType();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithoutAnyValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			bool result = sut.HasExternalTokenClaimType();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithValueForMicrosoftTokenClaimType_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut(true, true, ClaimHelper.MicrosoftTokenClaimType);

			bool result = sut.HasExternalTokenClaimType();

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithValueForGoogleTokenClaimType_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut(true, true, ClaimHelper.GoogleTokenClaimType);

			bool result = sut.HasExternalTokenClaimType();

			Assert.That(result, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void HasExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithUnknownValueForExternalClaimType_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, true, _fixture.Create<string>());

			bool result = sut.HasExternalTokenClaimType();

			Assert.That(result, Is.False);
		}

		private IDictionary<string, string> CreateSut(bool hasExternalTokenClaimTypeKey = true, bool hasExternalTokenClaimTypeValue = true, string externalTokenClaimType = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasExternalTokenClaimTypeKey)
			{
				items.Add(AuthenticationSessionKeys.ExternalTokenClaimTypeKey, hasExternalTokenClaimTypeValue ? externalTokenClaimType ?? ClaimHelper.MicrosoftTokenClaimType : null);
			}

			return items;
		}
	}
}