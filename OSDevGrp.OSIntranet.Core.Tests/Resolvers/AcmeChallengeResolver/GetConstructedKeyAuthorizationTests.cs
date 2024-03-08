using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Options;
using System;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.AcmeChallengeResolver
{
    [TestFixture]
    public class GetConstructedKeyAuthorizationTests
    {
        #region Private variables

        private Mock<IOptions<AcmeChallengeOptions>> _acmeChallengeOptionsMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _acmeChallengeOptionsMock = new Mock<IOptions<AcmeChallengeOptions>>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsNull_ThrowsArgumentNullException()
        {
            IAcmeChallengeResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetConstructedKeyAuthorization(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsEmpty_ThrowsArgumentNullException()
        {
            IAcmeChallengeResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetConstructedKeyAuthorization(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAcmeChallengeResolver sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetConstructedKeyAuthorization(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_AssertValueWasCalledOnOptionsForAcmeChallengeOptions()
        {
            IAcmeChallengeResolver sut = CreateSut();

            sut.GetConstructedKeyAuthorization(_fixture.Create<string>());

            _acmeChallengeOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenNoWellKnownChallengeTokenWasReturnedFromOptionsForAcmeChallengeOptions_AssertValueWasCalledOnlyOnceOnOptionsForAcmeChallengeOptions()
        {
            IAcmeChallengeResolver sut = CreateSut(false);

            sut.GetConstructedKeyAuthorization(_fixture.Create<string>());

            _acmeChallengeOptionsMock.Verify(m => m.Value, Times.Once);
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
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenDoesNotMatchChallengeToken_AssertValueWasCalledOnlyOnceOnOptionsForAcmeChallengeOptions()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            sut.GetConstructedKeyAuthorization(wellKnownChallengeToken + _fixture.Create<string>());

            _acmeChallengeOptionsMock.Verify(m => m.Value, Times.Once);
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
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeToken_AssertValueWasCalledTwiceOnOptionsForAcmeChallengeOptions()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            _acmeChallengeOptionsMock.Verify(m => m.Value, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenButNullWasReturnedFromConstructedKeyAuthorizationOnAcmeChallengeOptions_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, hasConstructedKeyAuthorization: false);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenButEmptyWasReturnedFromConstructedKeyAuthorizationOnAcmeChallengeOptions_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, constructedKeyAuthorization: string.Empty);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenButWhiteSpaceWasReturnedFromConstructedKeyAuthorizationOnAcmeChallengeOptions_ReturnsNull()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, constructedKeyAuthorization: " ");

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetConstructedKeyAuthorization_WhenWellKnownChallengeTokenMatchesChallengeTokenAndNonNullEmptyOrWhiteSpaceWasReturnedFromConstructedKeyAuthorizationOnAcmeChallengeOptions_ReturnsValueFromConstructedKeyAuthorizationOnAcmeChallengeOptions()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            string constructedKeyAuthorization = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken, constructedKeyAuthorization: constructedKeyAuthorization);

            string result = sut.GetConstructedKeyAuthorization(wellKnownChallengeToken);

            Assert.That(result, Is.EqualTo(constructedKeyAuthorization));
        }

        private IAcmeChallengeResolver CreateSut(bool hasWellKnownChallengeToken = true, string wellKnownChallengeToken = null, bool hasConstructedKeyAuthorization = true, string constructedKeyAuthorization = null)
        {
            _acmeChallengeOptionsMock.Setup(m => m.Value)
                .Returns(CreateAcmeChallengeOptions(hasWellKnownChallengeToken, wellKnownChallengeToken, hasConstructedKeyAuthorization, constructedKeyAuthorization));

            return new Core.Resolvers.AcmeChallengeResolver(_acmeChallengeOptionsMock.Object);
        }

        private AcmeChallengeOptions CreateAcmeChallengeOptions(bool hasWellKnownChallengeToken = true, string wellKnownChallengeToken = null, bool hasConstructedKeyAuthorization = true, string constructedKeyAuthorization = null)
        {
            return new AcmeChallengeOptions
            {
                WellKnownChallengeToken = hasWellKnownChallengeToken
                    ? wellKnownChallengeToken ?? _fixture.Create<string>()
                    : null,
                ConstructedKeyAuthorization = hasConstructedKeyAuthorization
                    ? constructedKeyAuthorization ?? _fixture.Create<string>()
                    : null
            };
        }
    }
}