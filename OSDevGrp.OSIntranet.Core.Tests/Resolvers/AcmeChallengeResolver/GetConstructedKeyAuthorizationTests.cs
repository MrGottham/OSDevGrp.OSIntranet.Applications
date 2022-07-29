using System;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.AcmeChallengeResolver
{
    [TestFixture]
    public class GetConstructedKeyAuthorizationTests
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
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsNull_ThrowsArgumentNullException()
        {
            IAcmeChallengeResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetConstructedKeyAuthorization(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsEmpty_ThrowsArgumentNullException()
        {
            IAcmeChallengeResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetConstructedKeyAuthorization(string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAcmeChallengeResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetConstructedKeyAuthorization(" "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_AssertGetterWasCalledOnConfigurationWithSecurityAcmeChallengeWellKnownChallengeToken()
        {
            IAcmeChallengeResolver sut = CreateSut();

            sut.GetConstructedKeyAuthorization(_fixture.Create<string>());

            _configurationMock.Verify(m => m[SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenNoWellKnownChallengeTokenWasReturnedFromConfiguration_AssertGetterWasNotCalledOnConfigurationWithSecurityAcmeChallengeConstructedKeyAuthorization()
        {
            IAcmeChallengeResolver sut = CreateSut(false);

            sut.GetConstructedKeyAuthorization(_fixture.Create<string>());

            _configurationMock.Verify(m => m[SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization], Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenNoWellKnownChallengeTokenWasReturnedFromConfiguration_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(false);

            string result = sut.GetConstructedKeyAuthorization(_fixture.Create<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenDoesNotMatchChallengeToken_AssertGetterNotCalledOnConfigurationWithSecurityAcmeChallengeConstructedKeyAuthorization()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            sut.GetConstructedKeyAuthorization(wellKnownChallengeToken + _fixture.Create<string>());

            _configurationMock.Verify(m => m[SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization], Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenDoesNotMatchChallengeToken_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken + _fixture.Create<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeToken_AssertGetterCalledOnConfigurationWithSecurityAcmeChallengeConstructedKeyAuthorization()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            _configurationMock.Verify(m => m[SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization], Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenButNullWasReturnedFromConfigurationForConstructedKeyAuthorization_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, hasConstructedKeyAuthorization: false);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenButEmptyWasReturnedFromConfigurationForConstructedKeyAuthorization_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, constructedKeyAuthorization: string.Empty);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenButWhiteSpaceWasReturnedFromConfigurationForConstructedKeyAuthorization_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, constructedKeyAuthorization: " ");

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenAndNonNullEmptyOrWhiteSpaceWasReturnedFromConfigurationForConstructedKeyAuthorization_ReturnsValueFromConfiguration()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            string constructedKeyAuthorization = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, constructedKeyAuthorization: constructedKeyAuthorization);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.EqualTo(constructedKeyAuthorization));
        }

        private IAcmeChallengeResolver CreateSut(bool hasWellKnownChallengeToken = true, string wellKnownChallengeToken = null, bool hasConstructedKeyAuthorization = true, string constructedKeyAuthorization = null)
        {
            _configurationMock.Setup(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.AcmeChallengeWellKnownChallengeToken) == 0)])
                .Returns(hasWellKnownChallengeToken ? wellKnownChallengeToken ?? _fixture.Create<string>() : null);
            _configurationMock.Setup(m => m[It.Is<string>(value => string.CompareOrdinal(value, SecurityConfigurationKeys.AcmeChallengeConstructedKeyAuthorization) == 0)])
                .Returns(hasConstructedKeyAuthorization ? constructedKeyAuthorization ?? _fixture.Create<string>() : null);

            return new Core.Resolvers.AcmeChallengeResolver(_configurationMock.Object);
        }
    }
}