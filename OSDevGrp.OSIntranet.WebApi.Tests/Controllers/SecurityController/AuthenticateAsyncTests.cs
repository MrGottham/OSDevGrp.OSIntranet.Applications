using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Security;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AuthenticateAsyncTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<ISecurityContextReader> _securityContextReaderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _securityContextReaderMock = new Mock<ISecurityContextReader>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalled_AssertGetBasicAuthenticationHeaderWasCalledOnSecurityContextReader()
        {
            Controller sut = CreateSut();

            await sut.AuthenticateAsync();

            _securityContextReaderMock.Verify(m => m.GetBasicAuthenticationHeader(It.IsAny<HttpRequest>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndDoesNotHaveBasicAuthenticationHeader_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            await sut.AuthenticateAsync();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndDoesNotHaveBasicAuthenticationHeader_AssertTokenWasNotCalledOnClientSecretIdentity()
        {
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            Controller sut = CreateSut(false, clientSecretIdentity: clientSecretIdentityMock.Object);

            await sut.AuthenticateAsync();

            clientSecretIdentityMock.VerifyGet(m => m.Token, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndDoesNotHaveBasicAuthenticationHeader_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut(false);

            ActionResult<AccessTokenModel> result = await sut.AuthenticateAsync();

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndDoesNotHaveBasicAuthenticationHeader_AssertBadRequestObjectResultContainsMessage()
        {
            Controller sut = CreateSut(false);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.AuthenticateAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value, Is.Not.Empty);
            Assert.That(result.Value, Is.EqualTo("An Authorization Header is missing in the submitted request."));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithInvalidParameter_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            string invalidParameter = _fixture.Create<string>();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(invalidParameter);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader);

            await sut.AuthenticateAsync();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(It.IsAny<IAuthenticateClientSecretCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithInvalidParameter_AssertTokenWasNotCalledOnClientSecretIdentity()
        {
            string invalidParameter = _fixture.Create<string>();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(invalidParameter);
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, clientSecretIdentity: clientSecretIdentityMock.Object);

            await sut.AuthenticateAsync();

            clientSecretIdentityMock.VerifyGet(m => m.Token, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithInvalidParameter_ReturnsUnauthorizedResult()
        {
            string invalidParameter = _fixture.Create<string>();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(invalidParameter);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader);

            ActionResult<AccessTokenModel> result = await sut.AuthenticateAsync();

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameter_AssertPublishAsyncWasCalledOnCommandBus()
        {
            string clientId = CreateMd5Hash();
            string clientSecret = CreateMd5Hash();
            string validParameter = CreateValidBasicAuthenticationHeaderParameter(clientId, clientSecret);
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader);

            await sut.AuthenticateAsync();

            _commandBusMock.Verify(m => m.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(It.Is<IAuthenticateClientSecretCommand>(command => string.Compare(command.ClientId, clientId, StringComparison.Ordinal) == 0 && string.Compare(command.ClientSecret, clientSecret, StringComparison.Ordinal) == 0 && command.Claims != null && command.Claims.Any() == false)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCannotBeAuthenticated_AssertTokenWasNotCalledOnClientSecretIdentity()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, hasClientSecretIdentity: false, clientSecretIdentity: clientSecretIdentityMock.Object);

            await sut.AuthenticateAsync();

            clientSecretIdentityMock.VerifyGet(m => m.Token, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCannotBeAuthenticated_ReturnsUnauthorizedResult()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, hasClientSecretIdentity: false);

            ActionResult<AccessTokenModel> result = await sut.AuthenticateAsync();

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCanBeAuthenticated_AssertTokenWasCalledOnClientSecretIdentity()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, clientSecretIdentity: clientSecretIdentityMock.Object);

            await sut.AuthenticateAsync();

            clientSecretIdentityMock.VerifyGet(m => m.Token, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCanBeAuthenticated_AssertAccessTokenWasCalledOnToken()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Mock<IToken> tokenMock = _fixture.BuildTokenMock();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(token: tokenMock.Object);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, clientSecretIdentity: clientSecretIdentityMock.Object);

            await sut.AuthenticateAsync();

            tokenMock.VerifyGet(m => m.AccessToken, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCanBeAuthenticated_AssertExpiresWasCalledOnToken()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Mock<IToken> tokenMock = _fixture.BuildTokenMock();
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(token: tokenMock.Object);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, clientSecretIdentity: clientSecretIdentityMock.Object);

            await sut.AuthenticateAsync();

            tokenMock.VerifyGet(m => m.Expires, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCanBeAuthenticated_ReturnsOkObjectResult()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock();
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, clientSecretIdentity: clientSecretIdentityMock.Object);

            ActionResult<AccessTokenModel> result = await sut.AuthenticateAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCanBeAuthenticated_AssertOkObjectResultContainsAccessTokenModel()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            string tokenType = _fixture.Create<string>();
            string tokenValue = _fixture.Create<string>();
            DateTime tokenExpires = DateTime.UtcNow.AddMinutes(_random.Next(30, 60));
            IToken tokenMock = _fixture.BuildTokenMock(tokenType, tokenValue, tokenExpires).Object;
            Mock<IClientSecretIdentity> clientSecretIdentityMock = _fixture.BuildClientSecretIdentityMock(token: tokenMock);
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, clientSecretIdentity: clientSecretIdentityMock.Object);

            OkObjectResult result = (OkObjectResult) (await sut.AuthenticateAsync()).Result;

            Assert.That(result.Value, Is.Not.Null);

            AccessTokenModel accessTokenModel = (AccessTokenModel) result.Value;
            Assert.That(accessTokenModel.TokenType, Is.Not.Null);
            Assert.That(accessTokenModel.TokenType, Is.Not.Empty);
            Assert.That(accessTokenModel.TokenType, Is.EqualTo(tokenType));
            Assert.That(accessTokenModel.AccessToken, Is.Not.Null);
            Assert.That(accessTokenModel.AccessToken, Is.Not.Empty);
            Assert.That(accessTokenModel.AccessToken, Is.EqualTo(tokenValue));
            Assert.That(accessTokenModel.Expires, Is.EqualTo(tokenExpires));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCauseIntranetException_ReturnsBadRequestObjectResult()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, exception: intranetRepositoryException);

            ActionResult<AccessTokenModel> result = await sut.AuthenticateAsync();

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AuthenticateAsync_WhenCalledAndHasBasicAuthenticationHeaderWithValidParameterWhichCauseIntranetException_AssertBadRequestObjectResultContainsErrorModel()
        {
            string validParameter = CreateValidBasicAuthenticationHeaderParameter();
            AuthenticationHeaderValue basicAuthenticationHeader = CreateBasicAuthenticationHeader(validParameter);
            IntranetRepositoryException intranetRepositoryException = new IntranetRepositoryException(_fixture.Create<ErrorCode>(), _fixture.Create<string>());
            Controller sut = CreateSut(basicAuthenticationHeader: basicAuthenticationHeader, exception: intranetRepositoryException);

            BadRequestObjectResult result = (BadRequestObjectResult) (await sut.AuthenticateAsync()).Result;

            Assert.That(result.Value, Is.TypeOf<ErrorModel>());
        }

        private Controller CreateSut(bool hasBasicAuthenticationHeader = true, AuthenticationHeaderValue basicAuthenticationHeader = null, bool hasClientSecretIdentity = true, IClientSecretIdentity clientSecretIdentity = null, Exception exception = null)
        {
            _securityContextReaderMock.Setup(m => m.GetBasicAuthenticationHeader(It.IsAny<HttpRequest>()))
                .Returns(hasBasicAuthenticationHeader ? basicAuthenticationHeader ?? CreateBasicAuthenticationHeader() : null);

            if (exception != null)
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(It.IsAny<IAuthenticateClientSecretCommand>()))
                    .Throws(exception);
            }
            else
            {
                _commandBusMock.Setup(m => m.PublishAsync<IAuthenticateClientSecretCommand, IClientSecretIdentity>(It.IsAny<IAuthenticateClientSecretCommand>()))
                    .Returns(() => Task.Run(() => hasClientSecretIdentity ? clientSecretIdentity ?? _fixture.BuildClientSecretIdentityMock().Object : null));
            }

            return new Controller(_commandBusMock.Object, _securityContextReaderMock.Object);
        }

        private AuthenticationHeaderValue CreateBasicAuthenticationHeader(string parameter = null)
        {
            return new AuthenticationHeaderValue(AuthenticationSchemes.Basic.ToString(), Convert.ToBase64String(Encoding.UTF8.GetBytes(parameter ?? CreateValidBasicAuthenticationHeaderParameter())));
        }

        private string CreateValidBasicAuthenticationHeaderParameter(string clientId = null, string clientSecret = null)
        {
            return $"{clientId ?? CreateMd5Hash()}:{clientSecret ?? CreateMd5Hash()}";
        }

        private string CreateMd5Hash()
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(_fixture.Create<string>()));

                StringBuilder resultBuilder = new StringBuilder();
                foreach (byte b in data)
                {
                    resultBuilder.Append(b.ToString("x2"));
                }

                return resultBuilder.ToString();
            }
       }
    }
}
