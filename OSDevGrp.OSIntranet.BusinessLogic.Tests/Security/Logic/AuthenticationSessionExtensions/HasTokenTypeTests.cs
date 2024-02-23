using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class HasTokenTypeTests
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
		public void HasTokenType_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.HasTokenType(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void HasTokenType_WhenItemsDoesNotContainKeyForTokenType_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(false);

			bool result = sut.HasTokenType();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasTokenType_WhenItemsContainsKeyForTokenTypeWithoutAnyValue_ReturnsFalse()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			bool result = sut.HasTokenType();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void HasTokenType_WhenItemsContainsKeyForTokenTypeWithAnyValue_ReturnsTrue()
		{
			IDictionary<string, string> sut = CreateSut();

			bool result = sut.HasTokenType();

			Assert.That(result, Is.True);
		}

		private IDictionary<string, string> CreateSut(bool hasTokenTypeKey = true, bool hasTokenTypeValue = true)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasTokenTypeKey)
			{
				items.Add(AuthenticationSessionKeys.TokenTypeKey, hasTokenTypeValue ? _fixture.Create<string>() : null);
			}

			return items;
		}
	}
}