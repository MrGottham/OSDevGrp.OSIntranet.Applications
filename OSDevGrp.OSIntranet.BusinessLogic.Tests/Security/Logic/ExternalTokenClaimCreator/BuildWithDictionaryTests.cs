using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ExternalTokenClaimCreator
{
	[TestFixture]
	public class BuildWithDictionaryTests : ExternalTokenClaimCreatorBase
	{
		#region Private variables

		private Mock<IExternalTokenCreator> _externalTokenCreatorMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_externalTokenCreatorMock = new Mock<IExternalTokenCreator>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenAuthenticationSessionItemsIsNull_ThrowsArgumentNullException()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Build((IDictionary<string, string>) null, value => value));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("authenticationSessionItems"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void Build_WhenProtectorIsNull_ThrowsArgumentNullException()
		{
			IExternalTokenClaimCreator sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Build(CreateAuthenticationSessionItems(_fixture), null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("protector"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenCalled_AssertBuildWasCalledOnExternalTokenCreator(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			sut.Build(authenticationSessionItems, value => value);

			_externalTokenCreatorMock.Verify(m => m.Build(It.Is<IDictionary<string, string>>(value => value != null && value == authenticationSessionItems)), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenTokenWasBuildByExternalTokenCreator_AssertToBase64StringWasCalledOnTokenFromExternalTokenCreator(string externalTokenClaimType)
		{
			Mock<IToken> tokenMock = _fixture.BuildTokenMock();
			IExternalTokenClaimCreator sut = CreateSut(tokenMock.Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			sut.Build(authenticationSessionItems, value => value);

			tokenMock.Verify(m => m.ToBase64String());
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenTokenWasBuildByExternalTokenCreator_AssertProtectorWasCalled(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut(_fixture.BuildTokenMock().Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			bool protectorWasCalled = false;
			sut.Build(authenticationSessionItems, value =>
			{
				protectorWasCalled = true;

				return value;
			});

			Assert.That(protectorWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenTokenWasBuildByExternalTokenCreator_AssertProtectorWasCalledWithBase64StringBuildToken(string externalTokenClaimType)
		{
			string toBase64String = _fixture.Create<string>();
			IToken token = _fixture.BuildTokenMock(toBase64String: toBase64String).Object;
			IExternalTokenClaimCreator sut = CreateSut(token);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			string protectorWasCalledWith = null;
			sut.Build(authenticationSessionItems, value =>
			{
				protectorWasCalledWith = value;

				return value;
			});

			Assert.That(protectorWasCalledWith, Is.EqualTo(toBase64String));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenRefreshableTokenWasBuildByExternalTokenCreator_AssertToBase64StringWasCalledOnRefreshableTokenFromExternalTokenCreator(string externalTokenClaimType)
		{
			Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
			IExternalTokenClaimCreator sut = CreateSut(refreshableTokenMock.Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			sut.Build(authenticationSessionItems, value => value);

			refreshableTokenMock.Verify(m => m.ToBase64String());
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenRefreshableTokenWasBuildByExternalTokenCreator_AssertProtectorWasCalled(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut(_fixture.BuildRefreshableTokenMock().Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			bool protectorWasCalled = false;
			sut.Build(authenticationSessionItems, value =>
			{
				protectorWasCalled = true;

				return value;
			});

			Assert.That(protectorWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenRefreshableTokenWasBuildByExternalTokenCreator_AssertProtectorWasCalledWithBase64StringBuildRefreshableToken(string externalTokenClaimType)
		{
			string toBase64String = _fixture.Create<string>();
			IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(toBase64String: toBase64String).Object;
			IExternalTokenClaimCreator sut = CreateSut(refreshableToken);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			string protectorWasCalledWith = null;
			sut.Build(authenticationSessionItems, value =>
			{
				protectorWasCalledWith = value;

				return value;
			});

			Assert.That(protectorWasCalledWith, Is.EqualTo(toBase64String));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenCalled_ReturnsNotNull(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenCalled_ReturnsClaimTypeIsNotNull(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.Type, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenCalled_ReturnsClaimTypeIsEqualToExternalTokenClaimTypeFromAuthenticationSessionItems(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.Type, Is.EqualTo(externalTokenClaimType));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenCalled_ReturnsValueIsNotNull(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.Value, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenCalled_ReturnsValueIsEqualToResultOfProtector(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut();

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			string resultOfProtector = _fixture.Create<string>();
			Claim result = sut.Build(authenticationSessionItems, _ => resultOfProtector);

			Assert.That(result.Value, Is.EqualTo(resultOfProtector));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenTokenWasBuildByExternalTokenCreator_ReturnsValueTypeIsNotNull(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut(_fixture.BuildTokenMock().Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.ValueType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenTokenWasBuildByExternalTokenCreator_ReturnsValueTypeIsEqualToToken(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut(_fixture.BuildTokenMock().Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.ValueType, Is.EqualTo(typeof(IToken).FullName));
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenRefreshableTokenWasBuildByExternalTokenCreator_ReturnsValueTypeIsNotNull(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut(_fixture.BuildRefreshableTokenMock().Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);
			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.ValueType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		[TestCase(ClaimHelper.MicrosoftTokenClaimType)]
		[TestCase(ClaimHelper.GoogleTokenClaimType)]
		public void Build_WhenRefreshableTokenWasBuildByExternalTokenCreator_ReturnsValueTypeIsEqualToRefreshableToken(string externalTokenClaimType)
		{
			IExternalTokenClaimCreator sut = CreateSut(_fixture.BuildRefreshableTokenMock().Object);

			IDictionary<string, string> authenticationSessionItems = CreateAuthenticationSessionItems(_fixture, hasExternalTokenClaimType: true, externalTokenClaimType: externalTokenClaimType);

			Claim result = sut.Build(authenticationSessionItems, value => value);

			Assert.That(result.ValueType, Is.EqualTo(typeof(IRefreshableToken).FullName));
		}

		private IExternalTokenClaimCreator CreateSut(IToken token = null)
		{
			_externalTokenCreatorMock.Setup(m => m.Build(It.IsAny<IDictionary<string, string>>()))
				.Returns(token ?? _fixture.BuildTokenMock().Object);

			return new BusinessLogic.Security.Logic.ExternalTokenClaimCreator(_externalTokenCreatorMock.Object);
		}
	}
}