using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ExternalTokenClaimCreator
{
	[TestFixture]
	public class CanBuildWithDictionaryTests : ExternalTokenClaimCreatorBase
	{
		#region Private variables

		private Mock<IExternalTokenCreator> _externalTokenCreatorMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_externalTokenCreatorMock = new Mock<IExternalTokenCreator>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsIsNull_ThrowsArgumentNullException()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.CanBuild((IDictionary<string, string>) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("authenticationSessionItems"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsDoesNotContainKeyForExternalTokenClaimType_AssertCanBuildWasNotCalledOnExternalTokenCreator()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: false);
			sut.CanBuild(authenticationSessionItems);

			_externalTokenCreatorMock.Verify(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsContainsKeyForExternalTokenClaimTypeWithValueForMicrosoftTokenClaimType_AssertCanBuildWasCalledOnExternalTokenCreator()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: ClaimHelper.MicrosoftTokenClaimType);
			sut.CanBuild(authenticationSessionItems);

			_externalTokenCreatorMock.Verify(m => m.CanBuild(It.Is<IDictionary<string, string>>(value => value != null && value == authenticationSessionItems)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsContainsKeyForExternalTokenClaimTypeWithValueForGoogleTokenClaimType_AssertCanBuildWasCalledOnExternalTokenCreator()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: ClaimHelper.GoogleTokenClaimType);
			sut.CanBuild(authenticationSessionItems);

			_externalTokenCreatorMock.Verify(m => m.CanBuild(It.Is<IDictionary<string, string>>(value => value != null && value == authenticationSessionItems)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsContainsKeyForExternalTokenClaimTypeWithUnknownValue_AssertCanBuildWasNotCalledOnExternalTokenCreator()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: _fixture.Create<string>());
			sut.CanBuild(authenticationSessionItems);

			_externalTokenCreatorMock.Verify(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsDoesNotContainKeyForExternalTokenClaimType_ReturnsFalse()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: false);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void CanBuild_WhenAuthenticationSessionItemsContainsKeyForExternalTokenClaimTypeWithValueForMicrosoftTokenClaimType_ReturnsResultFromCanBuildOnExternalTokenCreator(bool canBuild)
		{
			IExternalTokenClaimCreator sut = CreateSut(canBuild);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: ClaimHelper.MicrosoftTokenClaimType);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.EqualTo(canBuild));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(true)]
		[TestCase(false)]
		public void CanBuild_WhenAuthenticationSessionItemsContainsKeyForExternalTokenClaimTypeWithValueForGoogleTokenClaimType_ReturnsResultFromCanBuildOnExternalTokenCreator(bool canBuild)
		{
			IExternalTokenClaimCreator sut = CreateSut(canBuild);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: ClaimHelper.GoogleTokenClaimType);
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.EqualTo(canBuild));
		}

		[Test]
		[Category("UnitTest")]
		public void CanBuild_WhenAuthenticationSessionItemsContainsKeyForExternalTokenClaimTypeWithUnknownValue_ReturnsFalse()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: _fixture.Create<string>());
			bool result = sut.CanBuild(authenticationSessionItems);

			Assert.That(result, Is.False);
		}

		private IExternalTokenClaimCreator CreateSut(bool? canBuild = null)
		{
			_externalTokenCreatorMock.Setup(m => m.CanBuild(It.IsAny<IDictionary<string, string>>()))
				.Returns(canBuild ?? _random.Next(100) > 50);

			return new BusinessLogic.Security.Logic.ExternalTokenClaimCreator(_externalTokenCreatorMock.Object);
		}
	}
}