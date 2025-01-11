using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.TokenHelperFactoryExtensions
{
    internal class StoreExternalTokensInSecurityTokenTests
    {
        #region Private variables

        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void StoreExternalTokensInSecurityToken_WhenTokenHelperFactoryIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await Mvc.Helpers.Security.TokenHelperFactoryExtensions.StoreExternalTokensInSecurityToken(null, CreateHttpContext(), CreateSecurityToken()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("tokenHelperFactory"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreExternalTokensInSecurityToken_WhenHttpContextIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreExternalTokensInSecurityToken(null, CreateSecurityToken()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("httpContext"));
        }

        [Test]
        [Category("UnitTest")]
        public void StoreExternalTokensInSecurityToken_WhenSecurityTokenIsNull_ThrowsArgumentNullException()
        {
            ITokenHelperFactory sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("securityToken"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenDoesNotHaveClaims_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            ITokenHelperFactory sut = CreateSut();

            JwtSecurityToken securityToken = CreateSecurityToken(hasClaims: false);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasEmptyClaims_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            ITokenHelperFactory sut = CreateSut();

            JwtSecurityToken securityToken = CreateSecurityToken(claims: Array.Empty<Claim>());
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsNotContainingAnyExternalTokenClaims_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactory()
        {
            ITokenHelperFactory sut = CreateSut();

            IEnumerable<Claim> claims = CreateClaims(hasMicrosoftTokenClaim: false, hasGoogleTokenClaim: false);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.IsAny<TokenType>(),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsContainingMicrosoftTokenClaimWithoutValue_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithTokenTypeEqualToMicrosoftGraphToken()
        {
            ITokenHelperFactory sut = CreateSut();

            Claim microsoftTokenClaim = CreateMicrosoftTokenClaim(hasValue: false);
            IEnumerable<Claim> claims = CreateClaims(hasMicrosoftTokenClaim: true, microsoftTokenClaim: microsoftTokenClaim);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsContainingMicrosoftTokenClaimWithValue_AssertStoreTokenAsyncWasCalledOnTokenHelperFactoryWithTokenTypeEqualToMicrosoftGraphToken()
        {
            ITokenHelperFactory sut = CreateSut();

            Claim microsoftTokenClaim = CreateMicrosoftTokenClaim(hasValue: true);
            IEnumerable<Claim> claims = CreateClaims(hasMicrosoftTokenClaim: true, microsoftTokenClaim: microsoftTokenClaim);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsContainingMicrosoftTokenClaimWithValue_AssertStoreTokenAsyncWasCalledOnTokenHelperFactoryWithTokenTypeEqualToMicrosoftGraphTokenAndHttpContextEqualToHttpContextFromArguments()
        {
            ITokenHelperFactory sut = CreateSut();

            HttpContext httpContext = CreateHttpContext();
            Claim microsoftTokenClaim = CreateMicrosoftTokenClaim(hasValue: true);
            IEnumerable<Claim> claims = CreateClaims(hasMicrosoftTokenClaim: true, microsoftTokenClaim: microsoftTokenClaim);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(httpContext, securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.Is<HttpContext>(value => value != null && value == httpContext),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsContainingMicrosoftTokenClaimWithValue_AssertStoreTokenAsyncWasCalledOnTokenHelperFactoryWithTokenTypeEqualToMicrosoftGraphTokenAndWithBase64TokenEqualToBase64StringForMicrosoftToken()
        {
            ITokenHelperFactory sut = CreateSut();

            string microsoftTokenAsBase64String = CreateMicrosoftTokenAsBase64String();
            Claim microsoftTokenClaim = CreateMicrosoftTokenClaim(hasValue: true, valueAsBase64String: microsoftTokenAsBase64String);
            IEnumerable<Claim> claims = CreateClaims(hasMicrosoftTokenClaim: true, microsoftTokenClaim: microsoftTokenClaim);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.Is<string>(value => string.IsNullOrWhiteSpace(microsoftTokenAsBase64String) == false && string.Compare(value, microsoftTokenAsBase64String) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsContainingGoogleTokenClaimWithoutValue_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithTokenTypeNotEqualToMicrosoftGraphToken()
        {
            ITokenHelperFactory sut = CreateSut();

            Claim googleTokenClaim = CreateGoogleTokenClaim(hasValue: false);
            IEnumerable<Claim> claims = CreateClaims(hasGoogleTokenClaim: true, googleTokenClaim: googleTokenClaim);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value != TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreExternalTokensInSecurityToken_WhenSecurityTokenHasNonEmptyClaimsContainingGoogleTokenClaimWithValue_AssertStoreTokenAsyncWasNotCalledOnTokenHelperFactoryWithTokenTypeNotEqualToMicrosoftGraphToken()
        {
            ITokenHelperFactory sut = CreateSut();

            string googleTokenAsBase64String = CreateGoogleTokenAsBase64String();
            Claim googleTokenClaim = CreateGoogleTokenClaim(hasValue: true, valueAsBase64String: googleTokenAsBase64String);
            IEnumerable<Claim> claims = CreateClaims(hasGoogleTokenClaim: true, googleTokenClaim: googleTokenClaim);
            JwtSecurityToken securityToken = CreateSecurityToken(claims: claims);
            await sut.StoreExternalTokensInSecurityToken(CreateHttpContext(), securityToken);

            _tokenHelperFactoryMock.Verify(m => m.StoreTokenAsync(
                    It.Is<TokenType>(value => value != TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        private ITokenHelperFactory CreateSut()
        {
            _tokenHelperFactoryMock.Setup(m => m.StoreTokenAsync(It.IsAny<TokenType>(), It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            return _tokenHelperFactoryMock.Object;
        }

        private JwtSecurityToken CreateSecurityToken(bool hasClaims = true, IEnumerable<Claim> claims = null)
        {
            return hasClaims == false
                ? new JwtSecurityToken(_fixture.Create<string>(), _fixture.Create<string>())
                : new JwtSecurityToken(_fixture.Create<string>(), _fixture.Create<string>(), claims ?? CreateClaims());
        }

        private IEnumerable<Claim> CreateClaims(bool hasMicrosoftTokenClaim = true, Claim microsoftTokenClaim = null, bool hasGoogleTokenClaim = true, Claim googleTokenClaim = null)
        {
            IList<Claim> claims = new List<Claim>(_fixture.CreateClaims(_random));
            if (hasMicrosoftTokenClaim)
            {
                claims.Add(microsoftTokenClaim ?? CreateMicrosoftTokenClaim());
            }
            if (hasGoogleTokenClaim)
            {
                claims.Add(googleTokenClaim ?? CreateGoogleTokenClaim());
            }
            return claims;
        }

        private Claim CreateMicrosoftTokenClaim(bool hasValue = true, string valueAsBase64String = null)
        {
            return _fixture.CreateClaim(ClaimHelper.MicrosoftTokenClaimType, hasValue: hasValue, value: valueAsBase64String ?? CreateMicrosoftTokenAsBase64String());
        }

        private Claim CreateGoogleTokenClaim(bool hasValue = true, string valueAsBase64String = null)
        {
            return _fixture.CreateClaim(ClaimHelper.GoogleTokenClaimType, hasValue: hasValue, value: valueAsBase64String ?? CreateGoogleTokenAsBase64String());
        }

        private string CreateMicrosoftTokenAsBase64String(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
        {
            return RefreshableTokenFactory.Create()
                .WithTokenType(tokenType ?? _fixture.Create<string>())
                .WithAccessToken(accessToken ?? _fixture.Create<string>())
                .WithRefreshToken(refreshToken ?? _fixture.Create<string>())
                .WithExpires(expires ?? DateTime.UtcNow.AddMinutes(_random.Next(5, 60)))
                .Build()
                .ToBase64String();
        }

        private string CreateGoogleTokenAsBase64String(string tokenType = null, string accessToken = null, DateTime? expires = null)
        {
            return TokenFactory.Create()
                .WithTokenType(tokenType ?? _fixture.Create<string>())
                .WithAccessToken(accessToken ?? _fixture.Create<string>())
                .WithExpires(expires ?? DateTime.UtcNow.AddMinutes(_random.Next(5, 60)))
                .Build()
                .ToBase64String();
        }

        private static HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }
    }
}