using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimResolver
{
	[TestFixture]
	public class GetMicrosoftTokenWithPrincipalTests
	{
		#region Private variables

		private Mock<IPrincipalResolver> _principalResolverMock;
		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_principalResolverMock = new Mock<IPrincipalResolver>();
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenPrincipalIsNull_ThrowsArgumentNullException()
		{
			IClaimResolver sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetMicrosoftToken(null, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("principal"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenUnprotectIsNull_ThrowsArgumentNullException()
		{
			IClaimResolver sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetMicrosoftToken(CreateClaimsPrincipal(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("unprotect"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCalled_AssertGetCurrentPrincipalWasNotCalledOnPrincipalResolver()
		{
			IClaimResolver sut = CreateSut();

			sut.GetMicrosoftToken(CreateClaimsPrincipal(), value => value);

			_principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalDoesNotHaveMicrosoftTokenClaim_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(withMicrosoftTokenClaim: false));

			bool unprotectWasCalled = false;
			sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueIsEmpty_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, string.Empty, typeof(IRefreshableToken).FullName)});

			bool unprotectWasCalled = false;
			sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueIsWhiteSpace_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, " ", typeof(IRefreshableToken).FullName)});

			bool unprotectWasCalled = false;
			sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsNull_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), null)});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsEmpty_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), string.Empty)});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsWhiteSpace_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), " ")});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsUnsupported_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), _fixture.Create<string>())});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_AssertUnprotectWasCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal();

			bool unprotectWasCalled = false;
			sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_AssertUnprotectWasCalledWithClaimValue()
		{
			IClaimResolver sut = CreateSut();

			IRefreshableToken microsoftToken = CreateMicrosoftToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(microsoftToken: microsoftToken));

			string unprotectWasCalledWithValue = null;
			sut.GetMicrosoftToken(principal, value =>
			{
				unprotectWasCalledWithValue = value;
				return value;
			});

			Assert.That(unprotectWasCalledWithValue, Is.EqualTo(microsoftToken.ToBase64String()));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalDoesNotHaveMicrosoftTokenClaim_ReturnsNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(withMicrosoftTokenClaim: false));

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueIsEmpty_ReturnsNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, string.Empty, typeof(IRefreshableToken).FullName)});

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueIsWhiteSpace_ReturnsNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.MicrosoftTokenClaimType, " ", typeof(IRefreshableToken).FullName)});

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsNull_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), null);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsEmpty_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), string.Empty);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsWhiteSpace_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), " ");
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal,value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsUnsupported_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.MicrosoftTokenClaimType, CreateMicrosoftToken().ToBase64String(), _fixture.Create<string>());
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetMicrosoftToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_ReturnsNotNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection());

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_ReturnsRefreshableTokenWithMatchingTokenType()
		{
			IClaimResolver sut = CreateSut();

			IRefreshableToken microsoftToken = CreateMicrosoftToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(microsoftToken: microsoftToken));

			IRefreshableToken result = sut.GetMicrosoftToken(principal,value => value);

			Assert.That(result.TokenType, Is.EqualTo(microsoftToken.TokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_ReturnsRefreshableTokenWithMatchingAccessToken()
		{
			IClaimResolver sut = CreateSut();

			IRefreshableToken microsoftToken = CreateMicrosoftToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(microsoftToken: microsoftToken));

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result.AccessToken, Is.EqualTo(microsoftToken.AccessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_ReturnsRefreshableTokenWithMatchingRefreshToken()
		{
			IClaimResolver sut = CreateSut();

			IRefreshableToken microsoftToken = CreateMicrosoftToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(microsoftToken: microsoftToken));

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result.RefreshToken, Is.EqualTo(microsoftToken.RefreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupported_ReturnsRefreshableTokenWithMatchingExpires()
		{
			IClaimResolver sut = CreateSut();

			IRefreshableToken microsoftToken = CreateMicrosoftToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(microsoftToken: microsoftToken));

			IRefreshableToken result = sut.GetMicrosoftToken(principal, value => value);

			Assert.That(result.Expires, Is.EqualTo(microsoftToken.Expires));
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupportedButClaimValueNotBase64_ThrowsFormatException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.MicrosoftTokenClaimType, _fixture.Create<string>(), typeof(IRefreshableToken).FullName);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			FormatException result = Assert.Throws<FormatException>(() => sut.GetMicrosoftToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetMicrosoftToken_WhenCurrentPrincipalHasMicrosoftTokenClaimWhereValueTypeIsSupportedButClaimValueNotDeserializable_ThrowsJsonException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.MicrosoftTokenClaimType, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray()), typeof(IRefreshableToken).FullName);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			JsonException result = Assert.Throws<JsonException>(() => sut.GetMicrosoftToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
		}

		private IClaimResolver CreateSut()
		{
			return new BusinessLogic.Security.Logic.ClaimResolver(_principalResolverMock.Object);
		}

		private IPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claims = null)
		{
			return new ClaimsPrincipal(new ClaimsIdentity(claims ?? CreateClaimCollection()));
		}

		private IEnumerable<Claim> CreateClaimCollection(bool withMicrosoftTokenClaim = true, IRefreshableToken microsoftToken = null)
		{
			if (withMicrosoftTokenClaim)
			{
				return new[]
				{
					new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
					new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
					ClaimHelper.CreateTokenClaim(ClaimHelper.MicrosoftTokenClaimType, microsoftToken ?? CreateMicrosoftToken(), value => value),
					new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
					new Claim(_fixture.Create<string>(), _fixture.Create<string>())
				};
			}

			return new[]
			{
				new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
				new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
				new Claim(_fixture.Create<string>(), _fixture.Create<string>())
			};
		}

		private IRefreshableToken CreateMicrosoftToken()
		{
			return RefreshableTokenFactory.Create()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithRefreshToken(_fixture.Create<string>())
				.WithExpires(DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)))
				.Build();
		}
	}
}