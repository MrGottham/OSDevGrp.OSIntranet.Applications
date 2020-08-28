using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class DeleteContactTests
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
        public void DeleteContact_WhenExternalIdentifierIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContact(null));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContact_WhenExternalIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContact(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void DeleteContact_WhenExternalIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.DeleteContact(" "));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpace_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.DeleteContact(_fixture.Create<string>());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndNoTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            await sut.DeleteContact(_fixture.Create<string>());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IDeleteContactCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.DeleteContact(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasCalledOnCommandBus()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 300));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            string externalIdentifier = _fixture.Create<string>();
            await sut.DeleteContact(externalIdentifier);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IDeleteContactCommand>(command =>
                    string.CompareOrdinal(command.TokenType, tokenType) == 0 &&
                    string.CompareOrdinal(command.AccessToken, accessToken) == 0 &&
                    string.CompareOrdinal(command.RefreshToken, refreshToken) == 0 &&
                    command.Expires == expires &&
                    string.CompareOrdinal(command.ExternalIdentifier, externalIdentifier) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.DeleteContact(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteContact(_fixture.Create<string>());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereActionNameIsEqualToContacts()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteContact(_fixture.Create<string>());

            Assert.That(result.ActionName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task DeleteContact_WhenExternalIdentifierIsNotEmptyNullOrWhiteSpaceAndTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesIsNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.DeleteContact(_fixture.Create<string>());

            Assert.That(result.RouteValues, Is.Null);
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IDeleteContactCommand>()))
                .Returns(Task.CompletedTask);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}