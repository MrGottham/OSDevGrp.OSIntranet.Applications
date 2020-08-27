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
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class CreateContactWithModelTests
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
        public void CreateContact_WhenContactViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateContact((ContactViewModel) null));

            Assert.That(result.ParamName, Is.EqualTo("contactViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.CreateContact(CreateContactViewModel());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasNotCalledOnCommandBus() 
        {
            Controller sut = CreateSut(false);

            await sut.CreateContact(CreateContactViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateContactCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateContact(CreateContactViewModel());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            await sut.CreateContact(CreateContactViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<ICreateContactCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            IActionResult result = await sut.CreateContact(CreateContactViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContact(CreateContactViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereActionNameIsEqualToContacts()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContact(CreateContactViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesIsNull()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContact(CreateContactViewModel());

            Assert.That(result.RouteValues, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasCalledOnCommandBus()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 300));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            await sut.CreateContact(CreateContactViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateContactCommand>(command =>
                    string.CompareOrdinal(command.TokenType, tokenType) == 0 &&
                    string.CompareOrdinal(command.AccessToken, accessToken) == 0 &&
                    string.CompareOrdinal(command.RefreshToken, refreshToken) == 0 &&
                    command.Expires == expires)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateContact(CreateContactViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContact(CreateContactViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereActionNameIsEqualToContacts()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContact(CreateContactViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesIsNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateContact(CreateContactViewModel());

            Assert.That(result.RouteValues, Is.Null);
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, bool modelIsValid = true)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateContactCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private ContactViewModel CreateContactViewModel()
        {
            CompanyViewModel companyViewModel = _fixture.Build<CompanyViewModel>()
                .With(m => m.PrimaryPhone, _fixture.Create<string>())
                .With(m => m.SecondaryPhone, _fixture.Create<string>())
                .Create();

            return _fixture.Build<ContactViewModel>()
                .With(m => m.ContactType, _random.Next(100) > 50 ? ContactType.Company : ContactType.Person)
                .With(m => m.PrimaryPhone, _fixture.Create<string>())
                .With(m => m.SecondaryPhone, _fixture.Create<string>())
                .With(m => m.HomePhone, _fixture.Create<string>())
                .With(m => m.MobilePhone, _fixture.Create<string>())
                .With(m => m.Company, companyViewModel)
                .Create();
        }
    }
}