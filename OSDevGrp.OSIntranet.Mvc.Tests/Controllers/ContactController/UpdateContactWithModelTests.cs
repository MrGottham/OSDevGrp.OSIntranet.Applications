using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
    public class UpdateContactWithModelTests
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
        public void UpdateContact_WhenContactViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContact(null));

            Assert.That(result.ParamName, Is.EqualTo("contactViewModel"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.UpdateContact(CreateContactViewModel());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(false);

            await sut.UpdateContact(CreateContactViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateContactCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasNotCalledOnCommandBus()
        {
            Controller sut = CreateSut(modelIsValid: false);

            await sut.UpdateContact(CreateContactViewModel());

            _commandBusMock.Verify(m => m.PublishAsync(It.IsAny<IUpdateContactCommand>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(modelIsValid: false);

            IActionResult result = await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereActionNameIsEqualToContacts()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesDoesNotContainFilter()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            RouteValueDictionary routeValues = result.RouteValues;

            Assert.That(routeValues.ContainsKey("Filter"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesContainsExternalIdentifier()
        {
            Controller sut = CreateSut(modelIsValid: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            RouteValueDictionary routeValues = result.RouteValues;

            Assert.That(routeValues.ContainsKey("ExternalIdentifier"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsInvalidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesContainsExternalIdentifierWithValueFromContactViewModel()
        {
            Controller sut = CreateSut(modelIsValid: false);

            string externalIdentifier = _fixture.Create<string>();
            ContactViewModel contactViewModel = CreateContactViewModel(externalIdentifier);
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(contactViewModel);

            RouteValueDictionary routeValues = result.RouteValues;

            Assert.That(routeValues["ExternalIdentifier"], Is.EqualTo(externalIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_AssertPublishAsyncWasCalledOnCommandBus()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 300));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            string externalIdentifier = _fixture.Create<string>();
            ContactViewModel contactViewModel = CreateContactViewModel(externalIdentifier);
            await sut.UpdateContact(contactViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactCommand>(command =>
                    string.CompareOrdinal(command.TokenType, tokenType) == 0 &&
                    string.CompareOrdinal(command.AccessToken, accessToken) == 0 &&
                    string.CompareOrdinal(command.RefreshToken, refreshToken) == 0 &&
                    command.Expires == expires &&
                    string.CompareOrdinal(command.ExternalIdentifier, externalIdentifier) == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereActionNameIsEqualToContacts()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result.ActionName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesDoesNotContainFilter()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            RouteValueDictionary routeValues = result.RouteValues;

            Assert.That(routeValues.ContainsKey("Filter"), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesContainsExternalIdentifier()
        {
            Controller sut = CreateSut();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(CreateContactViewModel());

            RouteValueDictionary routeValues = result.RouteValues;

            Assert.That(routeValues.ContainsKey("ExternalIdentifier"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContact_WhenContactViewModelIsValidTokenWasReturnedFromTokenHelperFactory_ReturnsRedirectToActionResultWhereRouteValuesContainsExternalIdentifierWithValueFromContactViewModel()
        {
            Controller sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            ContactViewModel contactViewModel = CreateContactViewModel(externalIdentifier);
            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContact(contactViewModel);

            RouteValueDictionary routeValues = result.RouteValues;

            Assert.That(routeValues["ExternalIdentifier"], Is.EqualTo(externalIdentifier));
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, bool modelIsValid = true)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateContactCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());
            }
            return controller;
        }

        private ContactViewModel CreateContactViewModel(string externalIdentifier = null)
        {
            CompanyViewModel companyViewModel = _fixture.Build<CompanyViewModel>()
                .With(m => m.PrimaryPhone, _fixture.Create<string>())
                .With(m => m.SecondaryPhone, _fixture.Create<string>())
                .Create();

            return _fixture.Build<ContactViewModel>()
                .With(m => m.ExternalIdentifier, externalIdentifier ?? _fixture.Create<string>())
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