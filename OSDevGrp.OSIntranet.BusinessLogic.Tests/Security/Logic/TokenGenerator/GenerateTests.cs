using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.TokenGenerator
{
	[TestFixture]
    public class GenerateTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IConfiguration> _configurationMock;
        private Fixture _fixture;
        private readonly Regex _jwtTokenRegex = new(RegexTestHelper.JwtTokenPattern, RegexOptions.Compiled);

        #endregion

        [SetUp]
        public void SetUp()
        {
            _configurationMock = new Mock<IConfiguration>();
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
        public void Generate_WhenCalled_AssertSecurityJwtKeyWasCalledOnConfiguration()
        {
	        ITokenGenerator sut = CreateSut();

            sut.Generate(CreateClaimsIdentity());

            _configurationMock.Verify(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.JwtKey) == 0)], Times.Once);
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
        public void Generate_WhenCalled_ReturnsTokenWhereExpiresIsCalculateExpireTime()
        {
	        ITokenGenerator sut = CreateSut();

	        IToken result = sut.Generate(CreateClaimsIdentity());

	        Assert.That(result.Expires, Is.EqualTo(DateTime.UtcNow.AddHours(1)).Within(1).Seconds);
        }

        private ITokenGenerator CreateSut()
        {
	        _configurationMock.Setup(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.JwtKey) == 0)])
		        .Returns(_fixture.Create<string>());

            return new BusinessLogic.Security.Logic.TokenGenerator(_configurationMock.Object);
        }

        private ClaimsIdentity CreateClaimsIdentity()
        {
	        Claim[] claims =
	        {
		        ClaimHelper.CreateNameIdentifierClaim(_fixture.Create<string>()),
		        ClaimHelper.CreateNameClaim(_fixture.Create<string>()),
		        ClaimHelper.CreateEmailClaim($"{_fixture.Create<string>()}@{_fixture.Create<string>()}.{_fixture.Create<string>()}")
			};

	        return new ClaimsIdentity(claims, _fixture.Create<string>());
        }
    }
}