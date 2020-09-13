using System;
using System.Text;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.HomeController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.HomeController
{
    [TestFixture]
    public class AcmeChallengeTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IAcmeChallengeResolver> _acmeChallengeResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _acmeChallengeResolverMock = new Mock<IAcmeChallengeResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AcmeChallenge(null));

            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AcmeChallenge(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.AcmeChallenge(" "));

            Assert.That(result.ParamName, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_AssertGetConstructedKeyAuthorizationWasCalledOnAcmeChallengeResolver()
        {
            Controller sut = CreateSut();

            string challengeToken = _fixture.Create<string>();
            sut.AcmeChallenge(challengeToken);

            _acmeChallengeResolverMock.Verify(m => m.GetConstructedKeyAuthorization(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, challengeToken) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsFileContentResultWhereFileContentsIsNotEmpty()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsFileContentResultWhereFileContentsIsEqualToConstructedKeyAuthorization()
        {
            string constructedKeyAuthorization = _fixture.Create<string>();
            Controller sut = CreateSut(constructedKeyAuthorization: constructedKeyAuthorization);

            FileContentResult result = (FileContentResult) sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(Encoding.UTF8.GetString(result.FileContents), Is.EqualTo(constructedKeyAuthorization));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsFileContentResultWhereContentTypeIsEqualToApplicationOctetStream()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.ContentType, Is.EqualTo("application/octet-stream"));
        }

        private Controller CreateSut(bool hasConstructedKeyAuthorization = true, string constructedKeyAuthorization = null)
        {
            _acmeChallengeResolverMock.Setup(m => m.GetConstructedKeyAuthorization(It.IsAny<string>()))
                .Returns(hasConstructedKeyAuthorization ? constructedKeyAuthorization ?? _fixture.Create<string>() : null);

            return new Controller(_queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object, _acmeChallengeResolverMock.Object);
        }
    }
}