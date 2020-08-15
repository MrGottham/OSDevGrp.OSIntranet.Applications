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
    public class StartAddingAssociatedCompanyTests
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
        public void StartAddingAssociatedCompany_WhenCountryCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartAddingAssociatedCompany(null));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartAddingAssociatedCompany_WhenCountryCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartAddingAssociatedCompany(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartAddingAssociatedCompany_WhenCountryCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.StartAddingAssociatedCompany(" "));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenCalledWithCountryCode_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereViewNameIsEqualToAddingAssociatedCompanyPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_AddingAssociatedCompanyPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<ContactOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereDefaultCountryCodeIsEqualToCountryCode()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.StartAddingAssociatedCompany(countryCode);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.DefaultCountryCode, Is.EqualTo(countryCode));
        }

        [Test]
        [Category("UnitTest")]
        public async Task StartAddingAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsContactOptionsViewModelWhereCountriesIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.StartAddingAssociatedCompany(_fixture.Create<string>());

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