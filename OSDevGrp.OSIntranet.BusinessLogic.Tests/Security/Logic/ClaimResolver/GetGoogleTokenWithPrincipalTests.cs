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
	public class GetGoogleTokenWithPrincipalTests
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
		public void GetGoogleToken_WhenPrincipalIsNull_ThrowsArgumentNullException()
		{
			IClaimResolver sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetGoogleToken(null, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("principal"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenUnprotectIsNull_ThrowsArgumentNullException()
		{
			IClaimResolver sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetGoogleToken(CreateClaimsPrincipal(), null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("unprotect"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCalled_AssertGetCurrentPrincipalWasNotCalledOnPrincipalResolver()
		{
			IClaimResolver sut = CreateSut();

			sut.GetGoogleToken(CreateClaimsPrincipal(), value => value);

			_principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalDoesNotHaveGoogleTokenClaim_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(withGoogleTokenClaim: false));

			bool unprotectWasCalled = false;
			sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueIsEmpty_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, string.Empty, typeof(IRefreshableToken).FullName)});

			bool unprotectWasCalled = false;
			sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueIsWhiteSpace_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, " ", typeof(IRefreshableToken).FullName)});

			bool unprotectWasCalled = false;
			sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsNull_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), null)});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsEmpty_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), string.Empty)});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsWhiteSpace_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), " ")});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsUnsupported_AssertUnprotectWasNotCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), _fixture.Create<string>())});

			bool unprotectWasCalled = false;
			Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal,value =>
			{
				unprotectWasCalled = true;
				return value;
			}));

			Assert.That(unprotectWasCalled, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupported_AssertUnprotectWasCalled()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal();

			bool unprotectWasCalled = false;
			sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalled = true;
				return value;
			});

			Assert.That(unprotectWasCalled, Is.True);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupported_AssertUnprotectWasCalledWithClaimValue()
		{
			IClaimResolver sut = CreateSut();

			IToken googleToken = CreateGoogleToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(googleToken: googleToken));

			string unprotectWasCalledWithValue = null;
			sut.GetGoogleToken(principal, value =>
			{
				unprotectWasCalledWithValue = value;
				return value;
			});

			Assert.That(unprotectWasCalledWithValue, Is.EqualTo(googleToken.ToBase64String()));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalDoesNotHaveGoogleTokenClaim_ReturnsNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(withGoogleTokenClaim: false));

			IToken result = sut.GetGoogleToken(principal, value => value);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueIsEmpty_ReturnsNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, string.Empty, typeof(IRefreshableToken).FullName)});

			IToken result = sut.GetGoogleToken(principal,value => value);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueIsWhiteSpace_ReturnsNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal(new[] {new Claim(ClaimHelper.GoogleTokenClaimType, " ", typeof(IRefreshableToken).FullName)});

			IToken result = sut.GetGoogleToken(principal, value => value);

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsNull_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), null);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsEmpty_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), string.Empty);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsWhiteSpace_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), " ");
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsUnsupported_ThrowsNotSupportedException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.GoogleTokenClaimType, CreateGoogleToken().ToBase64String(), _fixture.Create<string>());
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.GetGoogleToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Message, Is.EqualTo($"Unsupported token type: {invalidClaim.ValueType}"));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupported_ReturnsNotNull()
		{
			IClaimResolver sut = CreateSut();

			IPrincipal principal = CreateClaimsPrincipal();

			IToken result = sut.GetGoogleToken(principal, value => value);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupported_ReturnsTokenWithMatchingTokenType()
		{
			IClaimResolver sut = CreateSut();

			IToken googleToken = CreateGoogleToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(googleToken: googleToken));

			IToken result = sut.GetGoogleToken(principal, value => value);

			Assert.That(result.TokenType, Is.EqualTo(googleToken.TokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupported_ReturnsTokenWithMatchingAccessToken()
		{
			IClaimResolver sut = CreateSut();

			IToken googleToken = CreateGoogleToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(googleToken: googleToken));

			IToken result = sut.GetGoogleToken(principal, value => value);

			Assert.That(result.AccessToken, Is.EqualTo(googleToken.AccessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupported_ReturnsTokenWithMatchingExpires()
		{
			IClaimResolver sut = CreateSut();

			IToken googleToken = CreateGoogleToken();
			IPrincipal principal = CreateClaimsPrincipal(CreateClaimCollection(googleToken: googleToken));

			IToken result = sut.GetGoogleToken(principal, value => value);

			Assert.That(result.Expires, Is.EqualTo(googleToken.Expires));
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupportedButClaimValueNotBase64_ThrowsFormatException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.GoogleTokenClaimType, _fixture.Create<string>(), typeof(IToken).FullName);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			FormatException result = Assert.Throws<FormatException>(() => sut.GetGoogleToken(principal, value => value));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetGoogleToken_WhenCurrentPrincipalHasGoogleTokenClaimWhereValueTypeIsSupportedButClaimValueNotDeserializable_ThrowsJsonException()
		{
			IClaimResolver sut = CreateSut();

			Claim invalidClaim = new Claim(ClaimHelper.GoogleTokenClaimType, Convert.ToBase64String(_fixture.CreateMany<byte>(_random.Next(256, 512)).ToArray()), typeof(IToken).FullName);
			IPrincipal principal = CreateClaimsPrincipal(new[] {invalidClaim});

			JsonException result = Assert.Throws<JsonException>(() => sut.GetGoogleToken(principal, value => value));

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

		private IEnumerable<Claim> CreateClaimCollection(bool withGoogleTokenClaim = true, IToken googleToken = null)
		{
			if (withGoogleTokenClaim)
			{
				return new[]
				{
					new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
					new Claim(_fixture.Create<string>(), _fixture.Create<string>()),
					ClaimHelper.CreateTokenClaim(ClaimHelper.GoogleTokenClaimType, googleToken ?? CreateGoogleToken(), value => value),
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

		private IToken CreateGoogleToken()
		{
			return TokenFactory.Create()
				.WithTokenType(_fixture.Create<string>())
				.WithAccessToken(_fixture.Create<string>())
				.WithExpires(DateTime.UtcNow.AddSeconds(_random.Next(60, 3600)))
				.Build();
		}
	}
}