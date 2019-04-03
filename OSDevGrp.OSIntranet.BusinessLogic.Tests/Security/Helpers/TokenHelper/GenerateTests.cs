using System;
using System.Security.Claims;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Helpers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Helpers.TokenHelper
{
    [TestFixture]
    public class GenerateTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<IConfiguration> _configurationMock;
        private Fixture _fixture;

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

            Assert.That(result.ParamName, Is.EqualTo("clientSecretIdentity"));
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertSecurityJwtKeyWasCalledOnConfiguration()
        {
            ITokenHelper sut = CreateSut();

            sut.Generate(CreateClientSecretIdentityMock().Object);

            _configurationMock.Verify(m => m[It.Is<string>(value => string.Compare(value, "Security:JWT:Key", StringComparison.Ordinal) == 0)], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_AssertToClaimsIdentityWasCalledOnClientSecretIdentity()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            sut.Generate(clientSecretIdentityMock.Object);

            clientSecretIdentityMock.Verify(m => m.ToClaimsIdentity(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsToken()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            IToken result = sut.Generate(clientSecretIdentityMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWithValue()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            string result = sut.Generate(clientSecretIdentityMock.Object).Value;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Generate_WhenCalled_ReturnsTokenWithExpireTime()
        {
            ITokenHelper sut = CreateSut();

            Mock<IClientSecretIdentity> clientSecretIdentityMock = CreateClientSecretIdentityMock();
            DateTime result = sut.Generate(clientSecretIdentityMock.Object).Expires;

            Assert.That(result, Is.EqualTo(DateTime.UtcNow.AddHours(1)).Within(1).Seconds);
        }

        private ITokenHelper CreateSut()
        {
            _configurationMock.Setup(m => m[It.Is<string>(value => string.Compare(value, "Security:JWT:Key", StringComparison.Ordinal) == 0)])
                .Returns(_fixture.Create<string>());

            return new BusinessLogic.Security.Helpers.TokenHelper(_configurationMock.Object);
        }

        private Mock<IClientSecretIdentity> CreateClientSecretIdentityMock()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = new Mock<IClientSecretIdentity>();
            clientSecretIdentityMock.Setup(m => m.ToClaimsIdentity())
                .Returns(new ClaimsIdentity());
            return clientSecretIdentityMock;
        }
    }
}
