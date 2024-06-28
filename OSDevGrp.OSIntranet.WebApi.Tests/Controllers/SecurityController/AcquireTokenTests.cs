using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcquireTokenTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
	        _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(null));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNull_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToGrantType()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("grantType"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(string.Empty));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsEmpty_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToGrantType()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("grantType"));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(" "));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationExceptionWhereErrorCodeIsEqualToValueCannotBeNullOrWhiteSpace()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNullOrWhiteSpace));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingTypeIsTypeOfString()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingType, Is.EqualTo(typeof(string)));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsWhiteSpace_ThrowsIntranetValidationExceptionWhereValidatingFieldIsEqualToGrantType()
        {
            Controller sut = CreateSut();

            IntranetValidationException result = Assert.ThrowsAsync<IntranetValidationException>(async () => await sut.AcquireToken(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValidatingField, Is.EqualTo("grantType"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenGrantTypeIsNotNullEmptyOrWhiteSpace_AssertPublishAsyncWasCalledOnCommandBus()
        {
            Controller sut = CreateSut();

            await sut.AcquireToken(GetLegalGrantType());

            _commandBusMock.Verify(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsNotNull<IGenerateTokenCommand>()));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNotEqualToClientCredentials_ThrowsIntranetBusinessException()
        {
            Controller sut = CreateSut();

            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.AcquireToken(_fixture.Create<string>()));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenGrantTypeIsNotEqualToClientCredentials_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveJwtBearerTokenForAuthenticatedUser()
        {
            Controller sut = CreateSut();

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.AcquireToken(_fixture.Create<string>()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenTokenWasNotGenerated_ThrowsIntranetBusinessException()
        {
            Controller sut = CreateSut(false);

            Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.AcquireToken(GetLegalGrantType()));
        }

        [Test]
        [Category("UnitTest")]
        public void AcquireToken_WhenTokenWasNotGenerated_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToCannotRetrieveJwtBearerTokenForAuthenticatedUser()
        {
            Controller sut = CreateSut(false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(async () => await sut.AcquireToken(GetLegalGrantType()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ActionResult<AccessTokenModel> result = await sut.AcquireToken(GetLegalGrantType());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<AccessTokenModel> result = await sut.AcquireToken(GetLegalGrantType());

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) (await sut.AcquireToken(GetLegalGrantType())).Result;

            Assert.That(result!.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsOkObjectResultWhereValueIsAccessTokenModel()
        {
	        Controller sut = CreateSut();

	        OkObjectResult result = (OkObjectResult) (await sut.AcquireToken(GetLegalGrantType())).Result;

	        Assert.That(result!.Value, Is.TypeOf<AccessTokenModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsOkObjectResultWithAccessTokenModelWhereTokenTypeIsEqualToTokenTypeFromToken()
        {
            string tokenType = _fixture.Create<string>();
            IToken token = _fixture.BuildTokenMock(tokenType).Object;
            Controller sut = CreateSut(token: token);

            AccessTokenModel result = (AccessTokenModel) ((OkObjectResult) (await sut.AcquireToken(GetLegalGrantType())).Result)!.Value;

            Assert.That(result!.TokenType, Is.EqualTo(tokenType));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsOkObjectResultWithAccessTokenModelWhereAccessTokenIsEqualToAccessTokenFromToken()
        {
            string accessToken = _fixture.Create<string>();
            IToken token = _fixture.BuildTokenMock(accessToken: accessToken).Object;
            Controller sut = CreateSut(token: token);

            AccessTokenModel result = (AccessTokenModel) ((OkObjectResult) (await sut.AcquireToken(GetLegalGrantType())).Result)!.Value;

            Assert.That(result!.AccessToken, Is.EqualTo(accessToken));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AcquireToken_WhenTokenWasGenerated_ReturnsOkObjectResultWithAccessTokenModelWhereExpiresIsEqualToExpiresFromToken()
        {
            DateTimeOffset expires = DateTime.Today.AddMinutes(5);
            IToken token = _fixture.BuildTokenMock(expires: expires.DateTime).Object;
            Controller sut = CreateSut(token: token);

            AccessTokenModel result = (AccessTokenModel) ((OkObjectResult) (await sut.AcquireToken(GetLegalGrantType())).Result)!.Value;

            Assert.That(result!.Expires, Is.EqualTo(expires));
        }

        private Controller CreateSut(bool hasToken = true, IToken token = null)
        {
            _commandBusMock.Setup(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()))
                .Returns(Task.FromResult(hasToken ? token ?? _fixture.BuildTokenMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _dataProtectionProviderMock.Object);
        }

        private string GetLegalGrantType() => "client_credentials";
    }
}