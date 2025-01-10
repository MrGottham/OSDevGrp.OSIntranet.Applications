using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.SecurityKeyBuilder;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.TokenGenerator
{
    [TestFixture]
    public class GenerateTests : SecurityKeyBuilderTestBase
    {
        #region Private variables

        private Mock<IOptions<TokenGeneratorOptions>> _tokenGeneratorOptionsMock;
        private Mock<TimeProvider> _timeProviderMock;
        private Fixture _fixture;
        private Random _random;
        private readonly Regex _jwtTokenRegex = new(RegexTestHelper.JwtTokenPattern, RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenGeneratorOptionsMock = new Mock<IOptions<TokenGeneratorOptions>>();
            _timeProviderMock = new Mock<TimeProvider>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenClaimsIdentityIsNull_ThrowsArgumentNullException(bool withAudience)
        {
	        ITokenGenerator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Generate(null, TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimsIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_AssertValueWasCalledOnTokenGeneratorOptions(bool withAudience)
        {
            ITokenGenerator sut = CreateSut();

            sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

            _tokenGeneratorOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_AssertGetUtcNowWasCalledOnTimeProvider(bool withAudience)
        {
            ITokenGenerator sut = CreateSut();

            sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

            _timeProviderMock.Verify(m => m.GetUtcNow(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsNotNull(bool withAudience)
        {
            ITokenGenerator sut = CreateSut();

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull(bool withAudience)
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

	        Assert.That(result.TokenType, Is.Not.Null);
        }

		[Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty(bool withAudience)
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

	        Assert.That(result.TokenType, Is.Not.Empty);
        }

		[Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToBearer(bool withAudience)
        {
            ITokenGenerator sut = CreateSut();

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

            Assert.That(result.TokenType, Is.EqualTo("Bearer"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull(bool withAudience)
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

	        Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty(bool withAudience)
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

	        Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsJwtToken(bool withAudience)
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);

	        Assert.That(_jwtTokenRegex.IsMatch(result.AccessToken), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenAudienceIsNull_ReturnsTokenWhereAccessTokenIsValidJwtToken()
        {
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions();
            ITokenGenerator sut = CreateSut(tokenGeneratorOptions);

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(result.AccessToken, tokenGeneratorOptions.ToTokenValidationParameters(), out _);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenAudienceIsEmpty_ReturnsTokenWhereAccessTokenIsValidJwtToken()
        {
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions();
            ITokenGenerator sut = CreateSut(tokenGeneratorOptions);

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), string.Empty);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(result.AccessToken, tokenGeneratorOptions.ToTokenValidationParameters(), out _);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenAudienceIsWhiteSpace_ReturnsTokenWhereAccessTokenIsValidJwtToken()
        {
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions();
            ITokenGenerator sut = CreateSut(tokenGeneratorOptions);

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), " ");

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(result.AccessToken, tokenGeneratorOptions.ToTokenValidationParameters(), out _);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenAudienceIsNotNullEmptyOrWhiteSpace_ReturnsTokenWhereAccessTokenIsValidJwtToken()
        {
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions();
            ITokenGenerator sut = CreateSut(tokenGeneratorOptions);

            string audience = _fixture.Create<string>();
            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), audience);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(result.AccessToken, tokenGeneratorOptions.ToTokenValidationParameters(audience), out _);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalled_ReturnsTokenWhereExpiresIsEqualCalculatedExpireTimeBasedOnUtcNowAndExpiresIn(bool withAudience)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
	        ITokenGenerator sut = CreateSut(utcNow: utcNow);

            TimeSpan expiresIn = TimeSpan.FromMinutes(_random.Next(5, 60));
            IToken result = sut.Generate(CreateClaimsIdentity(), expiresIn, withAudience ? _fixture.Create<string>() : null);

	        Assert.That(result.Expires, Is.EqualTo(utcNow.Add(expiresIn).UtcDateTime));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Generate_WhenCalledMultipleTimes_ExpectNoExceptionToBeThrown(bool withAudience)
        {
            ITokenGenerator sut = CreateSut();

            try
            {
                sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);
                sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);
                sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)), withAudience ? _fixture.Create<string>() : null);
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private ITokenGenerator CreateSut(TokenGeneratorOptions tokenGeneratorOptions = null, DateTimeOffset? utcNow = null)
        {
	        _tokenGeneratorOptionsMock.Setup(m => m.Value)
		        .Returns(tokenGeneratorOptions ?? CreateTokenGeneratorOptions());

            _timeProviderMock.Setup(m => m.GetUtcNow())
                .Returns(utcNow ?? DateTimeOffset.UtcNow);

            return new BusinessLogic.Security.Logic.TokenGenerator(_tokenGeneratorOptionsMock.Object, _timeProviderMock.Object);
        }

        private TokenGeneratorOptions CreateTokenGeneratorOptions()
        {
            return new TokenGeneratorOptions
            {
                Key = CreateJsonWebKey(),
                Issuer = _fixture.CreateEndpointString(withoutPathAndQuery: true),
                Audience = _fixture.CreateEndpointString(withoutPathAndQuery: true)
            };
        }

        private ClaimsIdentity CreateClaimsIdentity()
        {
	        Claim[] claims =
            [
                ClaimHelper.CreateNameIdentifierClaim(_fixture.Create<string>()),
		        ClaimHelper.CreateNameClaim(_fixture.Create<string>()),
		        ClaimHelper.CreateEmailClaim($"{_fixture.Create<string>()}@{_fixture.Create<string>()}.{_fixture.Create<string>()}")
            ];

	        return new ClaimsIdentity(claims, _fixture.Create<string>());
        }
    }
}