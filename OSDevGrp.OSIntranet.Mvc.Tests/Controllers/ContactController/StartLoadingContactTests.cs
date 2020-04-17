using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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
    public class StartLoadingContactTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingContact_WhenExternalIdentifierIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartLoadingContact(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingContact_WhenExternalIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartLoadingContact(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingContact_WhenExternalIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartLoadingContact(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingContact_WhenCountryCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartLoadingContact(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingContact_WhenCountryCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartLoadingContact(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingContact_WhenCountryCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartLoadingContact(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenCalledWithExternalIdentifierAndCountryCode_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereViewNameIsEqualToLoadingContactPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_LoadingContactPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactIdentificationViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<ContactIdentificationViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactIdentificationViewModelWhereExternalIdentifierIsEqualToInput()
        {
            Controller sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContact(externalIdentifier, _fixture.Create<string>());

            ContactIdentificationViewModel contactIdentificationViewModel = (ContactIdentificationViewModel) result.Model;

            Assert.That(contactIdentificationViewModel.ExternalIdentifier, Is.EqualTo(externalIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactIdentificationViewModelWhereDisplayNameIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactIdentificationViewModel contactIdentificationViewModel = (ContactIdentificationViewModel) result.Model;

            Assert.That(contactIdentificationViewModel.DisplayName, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactIdentificationViewModelWhereContactTypeIsUnknown()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactIdentificationViewModel contactIdentificationViewModel = (ContactIdentificationViewModel) result.Model;

            Assert.That(contactIdentificationViewModel.ContactType, Is.EqualTo(ContactType.Unknown));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContact_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultViewDataContainsCountryCode()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContact(_fixture.Create<string>(), countryCode);

            Assert.That(result.ViewData.ContainsKey("CountryCode"), Is.True);
            Assert.That(result.ViewData["CountryCode"], Is.EqualTo(countryCode));
        }

        private Controller CreateSut(bool hasRefreshableToken = true)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasRefreshableToken ? _fixture.BuildRefreshableTokenMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}