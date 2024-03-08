using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Options;

namespace OSDevGrp.OSIntranet.Core.Tests.Resolvers.AcmeChallengeResolver
{
    [TestFixture]
    public class GetWellKnownChallengeTokenTests
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
        public void GetWellKnownChallengeToken_WhenCalled_AssertValueWasCalledOnOptionsForAcmeChallengeOptions()
        {
            IAcmeChallengeResolver sut = CreateSut();

            sut.GetWellKnownChallengeToken();

            _acmeChallengeOptionsMock.Verify(m => m.Value, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenNullWasReturnedFromWellKnownChallengeTokenOnAcmeChallengeOptions_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(false);

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenEmptyWasReturnedFromWellKnownChallengeTokenOnAcmeChallengeOptions_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: string.Empty);

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenWhiteSpaceWasReturnedFromWellKnownChallengeTokenOnAcmeChallengeOptions_ReturnsNull()
        {
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: " ");

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetWellKnownChallengeToken_WhenNonNullEmptyOrWhiteSpaceWasReturnedFromWellKnownChallengeTokenOnAcmeChallengeOptions_ReturnsValueFromWellKnownChallengeTokenOnAcmeChallengeOptions()
        {
            string wellKnownChallengeToken = _fixture.Create<string>();
            IAcmeChallengeResolver sut = CreateSut(wellKnownChallengeToken: wellKnownChallengeToken);

            string result = sut.GetWellKnownChallengeToken();

            Assert.That(result, Is.EqualTo(wellKnownChallengeToken));
        }

        private IAcmeChallengeResolver CreateSut(bool hasWellKnownChallengeToken = true, string wellKnownChallengeToken = null)
        {
            _acmeChallengeOptionsMock.Setup(m => m.Value)
                .Returns(CreateAcmeChallengeOptions(hasWellKnownChallengeToken, wellKnownChallengeToken));

            return new Core.Resolvers.AcmeChallengeResolver(_acmeChallengeOptionsMock.Object);
        }

        private AcmeChallengeOptions CreateAcmeChallengeOptions(bool hasWellKnownChallengeToken = true, string wellKnownChallengeToken = null)
        {
            return new AcmeChallengeOptions
            {
                WellKnownChallengeToken = hasWellKnownChallengeToken
                    ? wellKnownChallengeToken ?? _fixture.Create<string>()
                    : null,
                ConstructedKeyAuthorization = _fixture.Create<string>()
            };
        }
    }
}