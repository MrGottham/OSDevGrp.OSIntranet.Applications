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
        private Fixture _fixture;
        private readonly Regex _jwtTokenRegex = new(RegexTestHelper.JwtTokenPattern, RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenGeneratorOptionsMock = new Mock<IOptions<TokenGeneratorOptions>>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenClaimsIdentityIsNull_ThrowsArgumentNullException()
        {
	        ITokenGenerator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Generate(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("claimsIdentity"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertValueWasCalledOnTokenGeneratorOptions()
        {
	        ITokenGenerator sut = CreateSut();

            sut.Generate(CreateClaimsIdentity());

            _tokenGeneratorOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsNotNull()
        {
            ITokenGenerator sut = CreateSut();

            IToken result = sut.Generate(CreateClaimsIdentity());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(result.TokenType, Is.Not.Null);
        }

		[Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(result.TokenType, Is.Not.Empty);
        }

		[Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToBearer()
        {
            ITokenGenerator sut = CreateSut();

            IToken result = sut.Generate(CreateClaimsIdentity());

            Assert.That(result.TokenType, Is.EqualTo("Bearer"));
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsJwtToken()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(_jwtTokenRegex.IsMatch(result.AccessToken), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsValidJwtToken()
        {
            TokenGeneratorOptions tokenGeneratorOptions = CreateTokenGeneratorOptions();
            ITokenGenerator sut = CreateSut(tokenGeneratorOptions);

            IToken result = sut.Generate(CreateClaimsIdentity());

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(result.AccessToken, tokenGeneratorOptions.ToTokenValidationParameters(), out _);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereExpiresIsCalculateExpireTime()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(result.Expires, Is.EqualTo(DateTime.UtcNow.AddHours(1)).Within(1).Seconds);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalledMultipleTimes_ExpectNoExceptionToBeThrown()
        {
            ITokenGenerator sut = CreateSut();

            try
            {
                sut.Generate(CreateClaimsIdentity());
                sut.Generate(CreateClaimsIdentity());
                sut.Generate(CreateClaimsIdentity());
            }
            catch (Exception ex)
            {
                Assert.Fail($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private ITokenGenerator CreateSut(TokenGeneratorOptions tokenGeneratorOptions = null)
        {
	        _tokenGeneratorOptionsMock.Setup(m => m.Value)
		        .Returns(tokenGeneratorOptions ?? CreateTokenGeneratorOptions());

            return new BusinessLogic.Security.Logic.TokenGenerator(_tokenGeneratorOptionsMock.Object);
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