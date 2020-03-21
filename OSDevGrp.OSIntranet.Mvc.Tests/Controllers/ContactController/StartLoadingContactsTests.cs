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
    public class StartLoadingContactsTests
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
        public async Task StartLoadingContacts_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.StartLoadingContacts();

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.StartLoadingContacts();

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.StartLoadingContacts();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereViewNameIsEqualToLoadingContactsPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts();

            Assert.That(result.ViewName, Is.EqualTo("_LoadingContactsPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts();

            Assert.That(result.Model, Is.TypeOf<ContactOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNull_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsEmpty_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts(string.Empty);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsWhiteSpace_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts(" ");

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactoryAndFilterIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereFilterIsEqualToFilterFromArgument()
        {
            Controller sut = CreateSut();

            string filter = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts(filter);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.EqualTo(filter));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereDefaultCountryCodeIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.DefaultCountryCode, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartLoadingContacts_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereDefaultCountriesIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartLoadingContacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Countries, Is.Null);
        }

        private Controller CreateSut(bool hasRefreshableToken = true)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasRefreshableToken ? _fixture.BuildRefreshableTokenMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}