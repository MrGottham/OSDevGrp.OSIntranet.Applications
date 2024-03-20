using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AcmeChallengeCommandHandler
{
    [TestFixture]
    internal class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;
        private Mock<IAcmeChallengeResolver> _acmeChallengeResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
            _acmeChallengeResolverMock = new Mock<IAcmeChallengeResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommand_AssertValidateWasCalledOnAcmeChallengeCommand()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut();

            Mock<IAcmeChallengeCommand> acmeChallengeCommandMock = CreateAcmeChallengeCommandMock();
            await sut.ExecuteAsync(acmeChallengeCommandMock.Object);

            acmeChallengeCommandMock.Verify(m => m.Validate(It.Is<IValidator>(value => value != null && value == _validatorMock.Object)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommand_AssertChallengeTokenWasCalledOnAcmeChallengeCommand()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut();

            Mock<IAcmeChallengeCommand> acmeChallengeCommandMock = CreateAcmeChallengeCommandMock();
            await sut.ExecuteAsync(acmeChallengeCommandMock.Object);

            acmeChallengeCommandMock.Verify(m => m.ChallengeToken, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCommand_AssertGetConstructedKeyAuthorizationWasCalledOnAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut();

            string challengeToken = _fixture.Create<string>();
            IAcmeChallengeCommand acmeChallengeCommand = CreateAcmeChallengeCommand(challengeToken);
            await sut.ExecuteAsync(acmeChallengeCommand);

            _acmeChallengeResolverMock.Verify(m => m.GetConstructedKeyAuthorization(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, challengeToken) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand_ThrowsIntranetBusinessException()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut(hasConstructedKeyAuthorization: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(CreateAcmeChallengeCommand()));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveAcmeChallengeForToken()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut(hasConstructedKeyAuthorization: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(CreateAcmeChallengeCommand()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveAcmeChallengeForToken));
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenNoConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNotNull()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut(hasConstructedKeyAuthorization: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.ExecuteAsync(CreateAcmeChallengeCommand()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand_ReturnsNotNull()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut();

            byte[] result = await sut.ExecuteAsync(CreateAcmeChallengeCommand());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand_ReturnsNonEmptyByteArray()
        {
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut();

            byte[] result = await sut.ExecuteAsync(CreateAcmeChallengeCommand());

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenConstructedKeyAuthorizationWasReturnedFromAcmeChallengeResolverWithChallengeTokenFromAcmeChallengeCommand_ReturnsUtf8ByteArrayForConstructedKeyAuthorization()
        {
            string constructedKeyAuthorization = _fixture.Create<string>();
            ICommandHandler<IAcmeChallengeCommand, byte[]> sut = CreateSut(constructedKeyAuthorization: constructedKeyAuthorization);

            byte[] result = await sut.ExecuteAsync(CreateAcmeChallengeCommand());

            Assert.That(result, Is.EqualTo(Encoding.UTF8.GetBytes(constructedKeyAuthorization)));
        }

        private ICommandHandler<IAcmeChallengeCommand, byte[]> CreateSut(bool hasConstructedKeyAuthorization = true, string constructedKeyAuthorization = null)
        {
            _acmeChallengeResolverMock.Setup(m => m.GetConstructedKeyAuthorization(It.IsAny<string>()))
                .Returns(hasConstructedKeyAuthorization ? constructedKeyAuthorization ?? _fixture.Create<string>() : null);

            return new BusinessLogic.Security.CommandHandlers.AcmeChallengeCommandHandler(_validatorMock.Object, _acmeChallengeResolverMock.Object);
        }

        private IAcmeChallengeCommand CreateAcmeChallengeCommand(string challengeToken = null)
        {
            return CreateAcmeChallengeCommandMock(challengeToken).Object;
        }

        private Mock<IAcmeChallengeCommand> CreateAcmeChallengeCommandMock(string challengeToken = null)
        {
            Mock<IAcmeChallengeCommand> acmeChallengeCommandMock = new Mock<IAcmeChallengeCommand>();
            acmeChallengeCommandMock.Setup(m => m.ChallengeToken)
                .Returns(challengeToken ?? _fixture.Create<string>());
            return acmeChallengeCommandMock;
        }
    }
}