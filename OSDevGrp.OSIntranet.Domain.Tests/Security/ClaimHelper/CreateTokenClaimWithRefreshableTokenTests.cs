using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClaimHelper
{
	[TestFixture]
	public class CreateTokenClaimWithRefreshableTokenTests
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
		public void CreateTokenClaim_WhenClaimTypeIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.CreateTokenClaim(null, _fixture.BuildRefreshableTokenMock().Object, value => value));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("claimType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenClaimTypeIsEmpty_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.CreateTokenClaim(string.Empty, _fixture.BuildRefreshableTokenMock().Object, value => value));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("claimType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenClaimTypeIsWhiteSpace_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.CreateTokenClaim(" ", _fixture.BuildRefreshableTokenMock().Object, value => value));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("claimType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenTokenIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), null, value => value));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshableToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenProtectorIsNull_ThrowsArgumentNullException()
		{
			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("protector"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_AssertToBase64StringWasCalledOnRefreshableToken()
		{
			Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();

			Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), refreshableTokenMock.Object, value => value);

			refreshableTokenMock.Verify(m => m.ToBase64String());
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_AssertProtectorWasCalled()
		{
			bool protectorWasCalled = false;
			Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value =>
			{
				protectorWasCalled = true;

				return value;
			});

			Assert.That(protectorWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_AssertProtectorWasCalledWithBase64StringFromRefreshableToken()
		{
			string toBase64String = _fixture.Create<string>();
			IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(toBase64String: toBase64String).Object;

			string protectorWasCalledWith = null;
			Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), refreshableToken, value =>
			{
				protectorWasCalledWith = value;

				return value;
			});

			Assert.That(protectorWasCalledWith, Is.EqualTo(toBase64String));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsNotNull()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereTypeIsNotNull()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Type, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereTypeIsNotEmpty()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Type, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereTypeIsEqualToClaimType()
		{
			string claimType = _fixture.Create<string>();

			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(claimType, _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Type, Is.EqualTo(claimType));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereValueIsNotNull()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Value, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereValueIsNotEmpty()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Value, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereValueIsEqualToResultOfProtector()
		{
			string protectorResult = _fixture.Create<string>();

			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, _ => protectorResult);

			Assert.That(result.Value, Is.EqualTo(protectorResult));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereValueTypeIsNotNull()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.ValueType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereValueTypeIsNotEmpty()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.ValueType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereValueTypeIsEqualToRefreshableToken()
		{
			string claimType = _fixture.Create<string>();

			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(claimType, _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.ValueType, Is.EqualTo(typeof(IRefreshableToken).FullName));
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereIssuerIsNotNull()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Issuer, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void CreateTokenClaim_WhenCalled_ReturnsClaimWhereIssuerIsNotEmpty()
		{
			Claim result = Domain.Security.ClaimHelper.CreateTokenClaim(_fixture.Create<string>(), _fixture.BuildRefreshableTokenMock().Object, value => value);

			Assert.That(result.Issuer, Is.Not.Empty);
		}

	}
}