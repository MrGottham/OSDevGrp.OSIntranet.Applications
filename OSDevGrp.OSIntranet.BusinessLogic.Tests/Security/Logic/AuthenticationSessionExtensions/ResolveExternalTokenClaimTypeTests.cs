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
	public class ResolveExternalTokenClaimTypeTests
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
		public void ResolveExternalTokenClaimType_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.ResolveExternalTokenClaimType(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsDoesNotContainKeyForExternalClaimType_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(false);

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithValueForMicrosoftTokenClaimType_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(true, true, ClaimHelper.MicrosoftTokenClaimType);

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithValueForMicrosoftTokenClaimType_ReturnsMicrosoftTokenClaimType()
		{
			IDictionary<string, string> sut = CreateSut(true, true, ClaimHelper.MicrosoftTokenClaimType);

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.EqualTo(ClaimHelper.MicrosoftTokenClaimType));
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithValueForGoogleTokenClaimType_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut(true, true, ClaimHelper.GoogleTokenClaimType);

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithValueForGoogleTokenClaimType_ReturnsGoogleTokenClaimType()
		{
			IDictionary<string, string> sut = CreateSut(true, true, ClaimHelper.GoogleTokenClaimType);

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.EqualTo(ClaimHelper.GoogleTokenClaimType));
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveExternalTokenClaimType_WhenItemsContainsKeyForExternalClaimTypeWithUnknownValueForExternalClaimType_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(true, true, _fixture.Create<string>());

			string result = sut.ResolveExternalTokenClaimType();

			Assert.That(result, Is.Null);
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