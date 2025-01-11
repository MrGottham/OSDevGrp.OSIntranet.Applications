using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Options;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.TokenGenerator
{
    [TestFixture]
    public class SigningAlgorithmTests
    {
        #region Private variables

        private Mock<IOptions<TokenGeneratorOptions>> _tokenGeneratorOptionsMock;
        private Mock<TimeProvider> _timeProviderMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _tokenGeneratorOptionsMock = new Mock<IOptions<TokenGeneratorOptions>>();
            _timeProviderMock = new Mock<TimeProvider>();
        }

        [Test]
        [Category("UnitTest")]
        public void SigningAlgorithm_WhenCalled_ReturnsNotNull()
        {
            ITokenGenerator sut = CreateSut();

            string result = sut.SigningAlgorithm;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void SigningAlgorithm_WhenCalled_ReturnsNonEmptyString()
        {
            ITokenGenerator sut = CreateSut();

            string result = sut.SigningAlgorithm;

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void SigningAlgorithm_WhenCalled_ReturnsNonEmptyStringEqualToRsa256()
        {
            ITokenGenerator sut = CreateSut();

            string result = sut.SigningAlgorithm;

            Assert.That(result, Is.EqualTo(SecurityAlgorithms.RsaSha256));
        }

        private ITokenGenerator CreateSut()
        {
            return new BusinessLogic.Security.Logic.TokenGenerator(_tokenGeneratorOptionsMock.Object, _timeProviderMock.Object);
        }
    }
}