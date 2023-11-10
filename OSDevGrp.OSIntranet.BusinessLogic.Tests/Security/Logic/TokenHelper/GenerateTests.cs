using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.TokenHelper
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
        public void Generate_WhenClientSecretIdentityIsNull_ThrowsArgumentNullException()
        {
            ITokenHelper sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Generate(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("clientSecretIdentity"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertSecurityJwtKeyWasCalledOnConfiguration()
        {
            ITokenHelper sut = CreateSut();

            sut.Generate(_fixture.BuildClientSecretIdentityMock().Object);

            _configurationMock.Verify(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.JwtKey) == 0)], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertToClaimsIdentityWasCalledOnClientSecretIdentity()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            sut.Generate(clientSecretIdentityMock.Object);

            clientSecretIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsNotNull()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            IToken result = sut.Generate(clientSecretIdentityMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
        {
	        ITokenHelper sut = CreateSut();

	        Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
	        IToken result = sut.Generate(clientSecretIdentityMock.Object);

	        Assert.That(result.TokenType, Is.Not.Null);
        }

		[Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
        {
	        ITokenHelper sut = CreateSut();

	        Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
	        IToken result = sut.Generate(clientSecretIdentityMock.Object);

	        Assert.That(result.TokenType, Is.Not.Empty);
        }

		[Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToBearer()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            IToken result = sut.Generate(clientSecretIdentityMock.Object);

            Assert.That(result.TokenType, Is.EqualTo("Bearer"));
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
        {
	        ITokenHelper sut = CreateSut();

	        Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
	        IToken result = sut.Generate(clientSecretIdentityMock.Object);

	        Assert.That(result.AccessToken, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
        {
	        ITokenHelper sut = CreateSut();

	        Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
	        IToken result = sut.Generate(clientSecretIdentityMock.Object);

	        Assert.That(result.AccessToken, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereAccessTokenIsJwtToken()
        {
	        ITokenHelper sut = CreateSut();

	        Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
	        IToken result = sut.Generate(clientSecretIdentityMock.Object);

	        Assert.That(_jwtTokenRegex.IsMatch(result.AccessToken), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWhereExpiresIsCalculateExpireTime()
        {
	        ITokenHelper sut = CreateSut();

	        Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
	        IToken result = sut.Generate(clientSecretIdentityMock.Object);

	        Assert.That(result.Expires, Is.EqualTo(DateTime.UtcNow.AddHours(1)).Within(1).Seconds);
        }

        private ITokenHelper CreateSut()
        {
	        _configurationMock.Setup(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.JwtKey) == 0)])
		        .Returns(_fixture.Create<string>());

            return new BusinessLogic.Security.Logic.TokenHelper(_configurationMock.Object);
        }
    }
}