using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers;
using System;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AuthorizeTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(null, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(null, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(null, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("unsupported_response_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "response_type"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(string.Empty, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(string.Empty, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(string.Empty, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("unsupported_response_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "response_type"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(" ", GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(" ", GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(" ", GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("unsupported_response_type", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "response_type"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsNotEqualToCode_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(_fixture.Create<string>(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsNotEqualToCode_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(_fixture.Create<string>(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenResponseTypeIsNotEqualToCode_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(_fixture.Create<string>(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("unsupported_response_type", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), null, GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), null, GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), null, GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), string.Empty, GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), string.Empty, GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), string.Empty, GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), " ", GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), " ", GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenClientIdIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), " ", GetLegalRedirectUri(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "client_id"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), null, GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), null, GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), null, GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), string.Empty, GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), string.Empty, GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), string.Empty, GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), " ", GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), " ", GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), " ", GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "redirect_uri"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsNonUri_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), _fixture.Create<string>(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsNonUri_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), _fixture.Create<string>(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsNonUri_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), _fixture.Create<string>(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsRelativeUri_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            Uri relativeUri = _fixture.CreateRelativeEndpoint();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), relativeUri.ToString(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsRelativeUri_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            Uri relativeUri = _fixture.CreateRelativeEndpoint();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), relativeUri.ToString(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenRedirectUriIsRelativeUri_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            Uri relativeUri = _fixture.CreateRelativeEndpoint();
            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), relativeUri.ToString(), GetLegalScope(), state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_request", ErrorDescriptionResolver.Resolve(ErrorCode.UnableToAuthorizeUser), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsNull_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), null, GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsNull_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), null, GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsNull_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), null, state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_scope", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "scope"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsEmpty_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), string.Empty, GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsEmpty_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), string.Empty, GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsEmpty_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), string.Empty, state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_scope", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "scope"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), " ", GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsWhiteSpace_ReturnsBadRequestObjectResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), " ", GetLegalState());

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenScopeIsWhiteSpace_ReturnsBadRequestObjectResultWithExpectedErrorResponseModel()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), " ", state);

            ((BadRequestObjectResult) result).AssertExpectedErrorResponseModel("invalid_scope", ErrorDescriptionResolver.Resolve(ErrorCode.ValueCannotBeNullOrWhiteSpace, "scope"), null, state);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_AssertCreateProtectorWasCalledOnDataProtectionProviderWithAuthorizationStateProtection()
        {
            Controller sut = CreateSut();

            await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            _dataProtectionProviderMock.Verify(m => m.CreateProtector(It.Is<string>(value => string.IsNullOrWhiteSpace(value) == false && string.CompareOrdinal(value, "AuthorizationStateProtection") == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommand()
        {
            Controller sut = CreateSut();

            await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.IsNotNull<IPrepareAuthorizationCodeFlowCommand>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommandWithResponseType()
        {
            Controller sut = CreateSut();

            string responseType = GetLegalResponseType();
            await sut.Authorize(responseType, GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.Is<IPrepareAuthorizationCodeFlowCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ResponseType) == false && string.CompareOrdinal(value.ResponseType, responseType) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommandWithClientId()
        {
            Controller sut = CreateSut();

            string clientId = GetLegalClientId();
            await sut.Authorize(GetLegalResponseType(), clientId, GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.Is<IPrepareAuthorizationCodeFlowCommand>(value => value != null && string.IsNullOrWhiteSpace(value.ClientId) == false && string.CompareOrdinal(value.ClientId, clientId) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommandWithRedirectUri()
        {
            Controller sut = CreateSut();

            string redirectUri = GetLegalRedirectUri();
            await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), redirectUri, GetLegalScope(), GetLegalState());

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.Is<IPrepareAuthorizationCodeFlowCommand>(value => value != null && value.RedirectUri != null && string.CompareOrdinal(value.RedirectUri.AbsoluteUri, redirectUri) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommandWithScopes()
        {
            Controller sut = CreateSut();

            string scopes = GetLegalScope();
            await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), scopes, GetLegalState());

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.Is<IPrepareAuthorizationCodeFlowCommand>(value => value != null && value.Scopes != null && string.CompareOrdinal(string.Join(' ', value.Scopes), scopes) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalledWithState_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommandWithState()
        {
            Controller sut = CreateSut();

            string state = GetLegalState();
            await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), state);

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.Is<IPrepareAuthorizationCodeFlowCommand>(value => value != null && string.IsNullOrWhiteSpace(value.State) == false && string.CompareOrdinal(value.State, state) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalledWithoutState_AssertPublishAsyncWasCalledOnCommandBusWithPrepareAuthorizationCodeFlowCommandWithoutState()
        {
            Controller sut = CreateSut();

            await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), null);

            _commandBusMock.Verify(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.Is<IPrepareAuthorizationCodeFlowCommand>(value => value != null && value.State == null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWherePageNameIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.PageName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWherePageNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.PageName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWherePageNameIsEqualToSecurityLogin()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.PageName, Is.EqualTo("/Security/Login"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWhereRouteValuesIsNotEmpty()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.RouteValues, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWhereRouteValuesContainsAuthorizationState()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.RouteValues!.ContainsKey("authorizationState"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWhereRouteValuesContainsNonNullValueForAuthorizationStateFromCommandBus()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.RouteValues!["authorizationState"], Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWhereRouteValuesContainsNonEmptyValueForAuthorizationStateFromCommandBus()
        {
            Controller sut = CreateSut();

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.RouteValues!["authorizationState"], Is.Not.Empty);
        }

        [Test] [Category("UnitTest")]
        public async Task Authorize_WhenCalled_ReturnsRedirectToPageResultWhereRouteValuesContainsValueFromCommandBusForAuthorizationStateFromCommandBus()
        {
            string authorizationState = _fixture.Create<string>();
            Controller sut = CreateSut(authorizationState: authorizationState);

            RedirectToPageResult result = (RedirectToPageResult) await sut.Authorize(GetLegalResponseType(), GetLegalClientId(), GetLegalRedirectUri(), GetLegalScope(), GetLegalState());

            Assert.That(result.RouteValues!["authorizationState"], Is.EqualTo(authorizationState));
        }

        private Controller CreateSut(string authorizationState = null)
        {
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);

            _commandBusMock.Setup(m => m.PublishAsync<IPrepareAuthorizationCodeFlowCommand, string>(It.IsAny<IPrepareAuthorizationCodeFlowCommand>()))
                .Returns(Task.FromResult(authorizationState ?? _fixture.Create<string>()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _dataProtectionProviderMock.Object);
        }

        private string GetLegalClientId() => _fixture.Create<string>();

        private string GetLegalRedirectUri() => _fixture.CreateEndpointString();

        private string GetLegalScope() => string.Join(' ', _fixture.CreateMany<string>(_random.Next(1, 5)));

        private string GetLegalState() => _fixture.Create<string>();

        private static string GetLegalResponseType() => "code";
    }
}