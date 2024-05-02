using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System;
using System.Linq;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcmeChallengeTests
    {
		#region Private variables

		private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
	        _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(null));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToChallengeToken()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToChallengeToken()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("challengeToken"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(" "));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcmeChallenge_WhenChallengeTokenIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToChallengeToken()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcmeChallenge(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("challengeToken"));
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
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_ReturnsFileContentResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<FileContentResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_ReturnsFileContentResultWhereFileContentsIsNotNull()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_ReturnsFileContentResultWhereFileContentsIsNotEmpty()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_ReturnsFileContentResultWhereFileContentsIsEqualToConstructedKeyAuthorizationFromAcmeChallengeCommand()
        {
            byte[] constructedKeyAuthorization = _fixture.CreateMany<byte>(_random.Next(32, 64)).ToArray();
            Controller sut = CreateSut(constructedKeyAuthorization: constructedKeyAuthorization);

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.FileContents, Is.EqualTo(constructedKeyAuthorization));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcmeChallenge_WhenChallengeTokenIsNotNullEmptyOrWhiteSpace_ReturnsFileContentResultWhereContentTypeIsEqualToApplicationOctetStream()
        {
            Controller sut = CreateSut();

            FileContentResult result = (FileContentResult) await sut.AcmeChallenge(_fixture.Create<string>());

            Assert.That(result.ContentType, Is.EqualTo("application/octet-stream"));
        }

        private Controller CreateSut(byte[] constructedKeyAuthorization = null)
        {
            _commandBusMock.Setup(m => m.PublishAsync<IAcmeChallengeCommand, byte[]>(It.IsAny<IAcmeChallengeCommand>()))
                .Returns(Task.FromResult(constructedKeyAuthorization ?? _fixture.CreateMany<byte>(_random.Next(32, 64)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
    }
}