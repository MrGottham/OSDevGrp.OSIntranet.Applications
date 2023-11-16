using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ExternalTokenCreator
{
	[TestFixture]
	public class CanBuildTests : ExternalTokenCreatorTestBase
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
		public void CanBuild_WhenAuthenticationSessionItemsIsNull_ThrowsArgumentNullException()
		{
			IExternalTokenCreator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.CanBuild(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("authenticationSessionItems"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true, true, true)]
		[TestCase(true, true, true, false)]
		[TestCase(true, true, false, true)]
		[TestCase(true, true, false, false)]
		[TestCase(true, false, true, true)]
		[TestCase(true, false, true, false)]
		[TestCase(true, false, false, true)]
		[TestCase(true, false, false, false)]
		[TestCase(false, true, true, true)]
		[TestCase(false, true, true, false)]
		[TestCase(false, true, false, true)]
		[TestCase(false, true, false, false)]
		[TestCase(false, false, true, true)]
		[TestCase(false, false, true, false)]
		[TestCase(false, false, false, true)]
		[TestCase(false, false, false, false)]
		public void CanBuild_WhenAuthenticationSessionItemsDoesNotContainKeyForTokenType_ReturnsFalse(bool hasAccessToken, bool hasRefreshToken, bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: false, hasAccessToken: hasAccessToken, hasRefreshToken: hasRefreshToken, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true, true, true)]
		[TestCase(true, true, true, false)]
		[TestCase(true, true, false, true)]
		[TestCase(true, true, false, false)]
		[TestCase(true, false, true, true)]
		[TestCase(true, false, true, false)]
		[TestCase(true, false, false, true)]
		[TestCase(true, false, false, false)]
		[TestCase(false, true, true, true)]
		[TestCase(false, true, true, false)]
		[TestCase(false, true, false, true)]
		[TestCase(false, true, false, false)]
		[TestCase(false, false, true, true)]
		[TestCase(false, false, true, false)]
		[TestCase(false, false, false, true)]
		[TestCase(false, false, false, false)]
		public void CanBuild_WhenAuthenticationSessionItemsDoesNotContainKeyForAccessToken_ReturnsFalse(bool hasTokenType, bool hasRefreshToken, bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: hasTokenType, hasAccessToken: false, hasRefreshToken: hasRefreshToken, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true, true)]
		[TestCase(true, true, false)]
		[TestCase(true, false, true)]
		[TestCase(true, false, false)]
		[TestCase(false, true, true)]
		[TestCase(false, true, false)]
		[TestCase(false, false, true)]
		[TestCase(false, false, false)]
		public void CanBuild_WhenAuthenticationSessionItemsDoesNotContainKeyForExpiresAtAndNoKeyForExpiresIn_ReturnsFalse(bool hasTokenType, bool hasAccessToken, bool hasRefreshToken)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: hasTokenType, hasAccessToken: hasAccessToken, hasRefreshToken: hasRefreshToken, hasExpiresAt: false, hasExpiresIn: false);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true, true, true)]
		[TestCase(true, true, false)]
		[TestCase(true, false, true)]
		[TestCase(false, true, true)]
		[TestCase(false, true, false)]
		[TestCase(false, false, true)]
		public void CanBuild_WhenAuthenticationSessionItemsContainsNecessaryKeysToBuildExternalToken_ReturnsTrue(bool hasRefreshToken, bool hasExpiresAt, bool hasExpiresIn)
		{
			IExternalTokenCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, _random, hasTokenType: true, hasAccessToken: true, hasRefreshToken: hasRefreshToken, hasExpiresAt: hasExpiresAt, hasExpiresIn: hasExpiresIn);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.True);
		}
	}
}