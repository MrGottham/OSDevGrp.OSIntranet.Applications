using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using CommandHandler=OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers.AuthenticateUserCommandHandler;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.AuthenticateUserCommandHandler
{
    [TestFixture]
    public class ExecuteAsyncTests : BusinessLogicTestBase
    {
        #region Private variables

        private Mock<ISecurityRepository> _securityRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _securityRepositoryMock = new Mock<ISecurityRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            Assert.That(result.ParamName, Is.EqualTo("command"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertGetUserIdentityAsyncWasCalledOnSecurityRepository()
        {
            CommandHandler sut = CreateSut();

            string externalUserIdentifier = _fixture.Create<string>();
            IAuthenticateUserCommand command = CreateCommand(externalUserIdentifier);
            await sut.ExecuteAsync(command);

            _securityRepositoryMock.Verify(m => m.GetUserIdentityAsync(It.Is<string>(value => string.Compare(value, externalUserIdentifier, StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenExternalUserIdentifierIsUnknown_AssertAddClaimsWasNotCalledOnUserIdentity()
        {
            Mock<IUserIdentity> userIdentityMock = CreateUserIdentityMock();
            CommandHandler sut = CreateSut(false, userIdentityMock.Object);

            IAuthenticateUserCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            userIdentityMock.Verify(m=> m.AddClaims(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenExternalUserIdentifierIsUnknown_AssertClearSensitiveDataWasNotCalledOnUserIdentity()
        {
            Mock<IUserIdentity> userIdentityMock = CreateUserIdentityMock();
            CommandHandler sut = CreateSut(false, userIdentityMock.Object);

            IAuthenticateUserCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            userIdentityMock.Verify(m=> m.ClearSensitiveData(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenExternalUserIdentifierIsUnknown_ReturnsNull()
        {
            CommandHandler sut = CreateSut(false);

            IAuthenticateUserCommand command = CreateCommand();
            IUserIdentity result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenExternalUserIdentifierIsKnown_AssertAddClaimsWasCalledOnUserIdentity()
        {
            Mock<IUserIdentity> userIdentityMock = CreateUserIdentityMock();
            CommandHandler sut = CreateSut(userIdentity: userIdentityMock.Object);

            IEnumerable<Claim> claims = new List<Claim>(0);
            IAuthenticateUserCommand command = CreateCommand(claims: claims);
            await sut.ExecuteAsync(command);

            userIdentityMock.Verify(m => m.AddClaims(It.Is<IEnumerable<Claim>>(value => Equals(value, claims))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenExternalUserIdentifierIsKnown_AssertClearSensitiveDataWasCalledOnUserIdentity()
        {
            Mock<IUserIdentity> userIdentityMock = CreateUserIdentityMock();
            CommandHandler sut = CreateSut(userIdentity: userIdentityMock.Object);

            IAuthenticateUserCommand command = CreateCommand();
            await sut.ExecuteAsync(command);

            userIdentityMock.Verify(m => m.ClearSensitiveData(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenExternalUserIdentifierIsKnown_ReturnsUserIdentity()
        {
            IUserIdentity userIdentity = CreateUserIdentityMock().Object;
            CommandHandler sut = CreateSut(userIdentity: userIdentity);

            IAuthenticateUserCommand command = CreateCommand();
            IUserIdentity result = await sut.ExecuteAsync(command);

            Assert.That(result, Is.EqualTo(userIdentity));
        }

        private CommandHandler CreateSut(bool hasUserIdentityForExternalUserIdentifier = true, IUserIdentity userIdentity = null)
        {
            _securityRepositoryMock.Setup(m => m.GetUserIdentityAsync(It.IsAny<string>()))
                .Returns(Task.Run(() => hasUserIdentityForExternalUserIdentifier ? userIdentity ?? CreateUserIdentityMock().Object : null));

            return new CommandHandler(_securityRepositoryMock.Object);
        }

        private IAuthenticateUserCommand CreateCommand(string externalUserIdentifier = null, IEnumerable<Claim> claims = null)
        {
            return new AuthenticateUserCommand(externalUserIdentifier ?? _fixture.Create<string>(), claims ?? new List<Claim>(0));
        }

        private Mock<IUserIdentity> CreateUserIdentityMock()
        {
            return new Mock<IUserIdentity>();
        }
    }
}
