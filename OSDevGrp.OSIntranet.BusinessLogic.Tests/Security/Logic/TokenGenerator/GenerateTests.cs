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
        public void Generate_WhenClaimsIdentityIsNull_ThrowsArgumentNullException()
        {
	        ITokenGenerator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Generate(null, TimeSpan.FromMinutes(_random.Next(5, 60))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claimsIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertValueWasCalledOnTokenGeneratorOptions()
        {
	        ITokenGenerator sut = CreateSut();

            sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

            _tokenGeneratorOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertGetUtcNowWasCalledOnTimeProvider()
        {
            ITokenGenerator sut = CreateSut();

            sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

            _timeProviderMock.Verify(m => m.GetUtcNow(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsNotNull()
        {
            ITokenGenerator sut = CreateSut();

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

	        Assert.That(result.TokenType, Is.Not.Null);
        }

		[Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

	        Assert.That(result.TokenType, Is.Not.Empty);
        }

		[Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToBearer()
        {
            ITokenGenerator sut = CreateSut();

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

            Assert.That(result.TokenType, Is.EqualTo("Bearer"));
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

	        Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

	        Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsJwtToken()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

	        Assert.That(_jwtTokenRegex.IsMatch(result.AccessToken), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsValidJwtToken()
        {
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions();
            ITokenGenerator sut = CreateSut(tokenGeneratorOptions);

            IToken result = sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(result.AccessToken, tokenGeneratorOptions.ToTokenValidationParameters(), out _);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereExpiresIsEqualCalculatedExpireTimeBasedOnUtcNowAndExpiresIn()
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;
	        ITokenGenerator sut = CreateSut(utcNow: utcNow);

            TimeSpan expiresIn = TimeSpan.FromMinutes(_random.Next(5, 60));
            IToken result = sut.Generate(CreateClaimsIdentity(), expiresIn);

	        Assert.That(result.Expires, Is.EqualTo(utcNow.Add(expiresIn).UtcDateTime));
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalledMultipleTimes_ExpectNoExceptionToBeThrown()
        {
            ITokenGenerator sut = CreateSut();

            try
            {
                sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));
                sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));
                sut.Generate(CreateClaimsIdentity(), TimeSpan.FromMinutes(_random.Next(5, 60)));
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