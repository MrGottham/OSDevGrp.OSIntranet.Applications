using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.AcmeChallengeResolver
{
    [TestFixture]
    public class GetWellKnownChallengeTokenTests
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
        public void GetWellKnownChallengeToken_WhenCalled_AssertGetterWasCalledOnConfigurationWithSecurityAcmeChallengeWellKnownChallengeToken()
        {
            IAcmeChallengeResolver sut = CreateSut();

            sut.GetWellKnownChallengeToken();

            _configurationMock.Verify(m => m[SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenNullWasReturnedFromConfiguration_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(false);

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenEmptyWasReturnedFromConfiguration_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: string.Empty);

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenWhiteSpaceWasReturnedFromConfiguration_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: " ");

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenNonNullEmptyOrWhiteSpaceWasReturnedFromConfiguration_ReturnsValueFromConfiguration()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.EqualTo(wellKnownChallengeToken));
        }

        private IAcmeChallengeResolver CreateSut(bool hasWellKnownChallengeToken = true, string wellKnownChallengeToken = null)
        {
            _configurationMock.Setup(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken) == 0)])
                .Returns(hasWellKnownChallengeToken ? wellKnownChallengeToken ?? _fixture.Create<string>() : null);

            return new Core.Resolvers.AcmeChallengeResolver(_configurationMock.Object);
        }
    }
}