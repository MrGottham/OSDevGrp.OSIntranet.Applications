using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.HomeController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.HomeController
{
    [TestFixture]
    public class AcmeChallengeTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNull_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(null);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsEmpty_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(string.Empty);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(string.Empty);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_AssertPublishAsyncWasCalledOnCommandBusWithAcmeChallengeCommand()
        {
            Controller sut = CreateSut();

            string challengeToken = _fixture.Create<string>();
            await sut.AcmeChallenge(challengeToken);

            _commandBusMock.Verify(m => m.PublishAsync<IAcmeChallengeCommand, byte[]>(It.Is<IAcmeChallengeCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ChallengeToken) == false && string.CompareOrdinal(value.ChallengeToken, challengeToken) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsFileContentResultWhereFileContentsIsNotEmpty()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsFileContentResultWhereFileContentsIsEqualToConstructedKeyAuthorizationFromAcmeChallengeCommand()
        {
            byte[] constructedKeyAuthorization = _fixture.CreateMany<byte>(_random.Next(32, 64)).ToArray();
            Controller sut = CreateSut(constructedKeyAuthorization: constructedKeyAuthorization);

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.EqualTo(constructedKeyAuthorization));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsFileContentResultWhereContentTypeIsEqualToApplicationOctetStream()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.ContentType, Is.EqualTo("application/octet-stream"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeCommand_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenFromAcmeChallengeCommandThrowsIntranetBusinessException_ReturnsNotNull()
        {
            Controller sut = CreateSut(exception: _fixture.Create<IntranetBusinessException>());

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenFromAcmeChallengeCommandThrowsIntranetBusinessException_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(exception: _fixture.Create<IntranetBusinessException>());

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenFromAcmeChallengeCommandThrowsException_ThrowsException()
        {
            Controller sut = CreateSut(exception: _fixture.Create<Exception>());

            Exception result = Assert.ThrowsAsync<Exception>(async () => await sut.AcmeChallenge(_fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenFromAcmeChallengeCommandThrowsException_ThrowsSameException()
        {
            Exception exception = _fixture.Create<Exception>();
            Controller sut = CreateSut(exception: exception);

            Exception result = Assert.ThrowsAsync<Exception>(async () => await sut.AcmeChallenge(_fixture.Create<string>()));

            Assert.That(result, Is.SameAs(exception));
        }

        private Controller CreateSut(bool hasConstructedKeyAuthorization = true, byte[] constructedKeyAuthorization = null, Exception exception = null)
        {
            if (exception == null)
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAcmeChallengeCommand, byte[]>(It.IsAny<IAcmeChallengeCommand>()))
                    .Returns(Task.FromResult(hasConstructedKeyAuthorization ? constructedKeyAuthorization ?? _fixture.CreateMany<byte>(_random.Next(32, 64)).ToArray() : null));
            }
            else
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAcmeChallengeCommand, byte[]>(It.IsAny<IAcmeChallengeCommand>()))
                    .Throws(exception);
            }

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}