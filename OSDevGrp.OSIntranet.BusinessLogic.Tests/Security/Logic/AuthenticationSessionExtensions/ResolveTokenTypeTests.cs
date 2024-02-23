using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.AuthenticationSessionExtensions
{
	[TestFixture]
	public class ResolveTokenTypeTests
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
		public void ResolveTokenType_WhenItemsIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => BusinessLogic.Security.Logic.AuthenticationSessionExtensions.ResolveTokenType(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("items"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveTokenType_WhenItemsDoesNotContainKeyForTokenType_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(false);

			string result = sut.ResolveTokenType();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveTokenType_WhenItemsContainsKeyForTokenTypeWithoutAnyValue_ReturnsNull()
		{
			IDictionary<string, string> sut = CreateSut(true, false);

			string result = sut.ResolveTokenType();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveTokenType_WhenItemsContainsKeyForTokenTypeWithAnyValue_ReturnsNotNull()
		{
			IDictionary<string, string> sut = CreateSut();

			string result = sut.ResolveTokenType();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ResolveTokenType_WhenItemsContainsKeyForTokenTypeWithAnyValue_ReturnsTokenType()
		{
			string tokenType = _fixture.Create<string>();
			IDictionary<string, string> sut = CreateSut(true, true, tokenType);

			string result = sut.ResolveTokenType();

			Assert.That(result, Is.EqualTo(tokenType));
		}

		private IDictionary<string, string> CreateSut(bool hasTokenTypeKey = true, bool hasTokenTypeValue = true, string tokenType = null)
		{
			IDictionary<string, string> items = new ConcurrentDictionary<string, string>();

			if (hasTokenTypeKey)
			{
				items.Add(AuthenticationSessionKeys.TokenTypeKey, hasTokenTypeValue ? tokenType ?? _fixture.Create<string>() : null);
			}

			return items;
		}
	}
}