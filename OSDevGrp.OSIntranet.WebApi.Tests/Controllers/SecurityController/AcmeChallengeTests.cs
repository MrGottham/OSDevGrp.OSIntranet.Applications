using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using System.Text;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
	[TestFixture]
    public class AcmeChallengeTests
    {
		#region Private variables

		private Mock<ICommandBus> _commandBusMock;
        private Mock<IAcmeChallengeResolver> _acmeChallengeResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
	        _commandBusMock = new Mock<ICommandBus>();
            _acmeChallengeResolverMock = new Mock<IAcmeChallengeResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(null));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToChallengeToken()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToChallengeToken()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(" "));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToChallengeToken()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.AcmeChallenge(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("challengeToken"));
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
        public void AcmeChallenge_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ThrowsIntranetBusinessException()
        {
            Controller sut = CreateSut(false);

            Assert.Throws<IntranetBusinessException>(() => sut.AcmeChallenge(_fixture.Create<string>()));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveAcmeChallengeForToken()
        {
            Controller sut = CreateSut(false);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.AcmeChallenge(_fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveAcmeChallengeForToken));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolver_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
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

            return new Controller(_commandBusMock.Object, _acmeChallengeResolverMock.Object);
        }
    }
}