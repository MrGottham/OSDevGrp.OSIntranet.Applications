using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class UserInfoTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<TimeProvider> _timeProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _timeProviderMock = new Mock<TimeProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusWithGetUserInfoAsTokenQuery()
        {
            Controller sut = CreateSut();

            await sut.UserInfo();

            _queryBusMock.Verify(m => m.QueryAsync<IGetUserInfoAsTokenQuery, IToken>(It.IsNotNull<IGetUserInfoAsTokenQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfo_WhenNoTokenForUserInfoWasReturnedFromQueryBus_ThrowsIntranetBusinessException()
        {
            Controller sut = CreateSut(hasToken: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(sut.UserInfo);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfo_WhenNoTokenForUserInfoWasReturnedFromQueryBus_ThrowsIntranetBusinessExceptionWhereErrorCodeIsEqualToErrorCodeForCannotRetrieveJwtBearerTokenForAuthenticatedUser()
        {
            Controller sut = CreateSut(hasToken: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(sut.UserInfo);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser));
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfo_WhenNoTokenForUserInfoWasReturnedFromQueryBus_ThrowsIntranetBusinessExceptionWhereMessageIsNotNull()
        {
            Controller sut = CreateSut(hasToken: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(sut.UserInfo);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfo_WhenNoTokenForUserInfoWasReturnedFromQueryBus_ThrowsIntranetBusinessExceptionWhereMessageIsNotEmpty()
        {
            Controller sut = CreateSut(hasToken: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(sut.UserInfo);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfo_WhenNoTokenForUserInfoWasReturnedFromQueryBus_ThrowsIntranetBusinessExceptionWhereMessageIsEqualToMessageForCannotRetrieveJwtBearerTokenForAuthenticatedUser()
        {
            Controller sut = CreateSut(hasToken: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(sut.UserInfo);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo(ErrorDescriptionResolver.Resolve(ErrorCode.CannotRetrieveJwtBearerTokenForAuthenticatedUser)));
        }

        [Test]
        [Category("UnitTest")]
        public void UserInfo_WhenNoTokenForUserInfoWasReturnedFromQueryBus_ThrowsIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            Controller sut = CreateSut(hasToken: false);

            IntranetBusinessException result = Assert.ThrowsAsync<IntranetBusinessException>(sut.UserInfo);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_AssertAccessTokenWasCalledTwiceOnTokenReturnedByQueryBus()
        {
            Mock<IToken> tokenMock = _fixture.BuildTokenMock();
            Controller sut = CreateSut(token: tokenMock.Object);

            await sut.UserInfo();

            tokenMock.Verify(m => m.AccessToken, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UserInfo();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UserInfo();

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsNotNull()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) await sut.UserInfo();

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsString()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) await sut.UserInfo();

            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsNonEmptyString()
        {
            Controller sut = CreateSut();

            OkObjectResult result = (OkObjectResult) await sut.UserInfo();

            Assert.That((string) result.Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UserInfo_WhenTokenForUserInfoWasReturnedFromQueryBus_ReturnsOkObjectResultWhereValueIsNonEmptyStringEqualToAccessTokenOnToken()
        {
            string accessToken = _fixture.Create<string>();
            IToken token = _fixture.BuildTokenMock(accessToken: accessToken).Object;
            Controller sut = CreateSut(token: token);

            OkObjectResult result = (OkObjectResult) await sut.UserInfo();

            Assert.That((string) result.Value, Is.EqualTo(accessToken));
        }

        private Controller CreateSut(bool hasToken = true, IToken token = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetUserInfoAsTokenQuery, IToken>(It.IsAny<IGetUserInfoAsTokenQuery>()))
                .Returns(Task.FromResult(hasToken ? token ?? _fixture.BuildTokenMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _dataProtectionProviderMock.Object, _timeProviderMock.Object);
        }
    }
}