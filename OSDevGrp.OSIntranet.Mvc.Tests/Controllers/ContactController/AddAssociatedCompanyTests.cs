using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class AddAssociatedCompanyTests
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
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));
        }

        [Test]
        [Category("UnitTest")]
        public void AddAssociatedCompany_WhenCountryCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AddAssociatedCompany(null));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddAssociatedCompany_WhenCountryCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AddAssociatedCompany(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void AddAssociatedCompany_WhenCountryCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.AddAssociatedCompany(" "));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCalledWithCountryCode_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.AddAssociatedCompany(_fixture.Create<string>());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithGetCountryQuery()
        {
            Controller sut = CreateSut(false);

            await sut.AddAssociatedCompany(_fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBusWithGetCountryQuery()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            await sut.AddAssociatedCompany(countryCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.Is<IGetCountryQuery>(value => value != null && string.CompareOrdinal(value.CountryCode, countryCode.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenNoCountryWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasCountry: false);

            IActionResult result = await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsEqualToEditCompanyPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_EditCompanyPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<CompanyViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereCompanyNameIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.CompanyName, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereAddressIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.Address, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereAddressIsAddressViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.Address, Is.TypeOf<AddressViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWhichAreDefaultForPrincipalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereCountryInAddressIsEqualToNull()
        {
            ICountry country = _fixture.BuildCountryMock(defaultForPrincipal: true).Object;
            Controller sut = CreateSut(country: country);

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.Address.Country, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWhichAreNotDefaultForPrincipalWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereCountryInAddressIsEqualToUniversalNameFromCountry()
        {
            string universalName = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(universalName: universalName, defaultForPrincipal: false).Object;
            Controller sut = CreateSut(country: country);

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.Address.Country, Is.EqualTo(universalName));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWherePrimaryPhoneIsEqualToPhonePrefixFromCountry()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(phonePrefix: phonePrefix).Object;
            Controller sut = CreateSut(country: country);

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.PrimaryPhone, Is.EqualTo(phonePrefix));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereSecondaryPhoneIsEqualToPhonePrefixFromCountry()
        {
            string phonePrefix = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(phonePrefix: phonePrefix).Object;
            Controller sut = CreateSut(country: country);

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.SecondaryPhone, Is.EqualTo(phonePrefix));
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsCompanyViewModelWhereHomePageIsNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            CompanyViewModel companyViewModel = (CompanyViewModel) result.Model;

            Assert.That(companyViewModel.HomePage, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewDataIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.ViewData, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereTemplateInfoWithinViewDataIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.ViewData.TemplateInfo, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task AddAssociatedCompany_WhenCountryWasReturnedFromQueryBus_ReturnsPartialViewResultWhereHtmlFieldPrefixInTemplateInfoWithinViewDataIsEqualToCompany()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.AddAssociatedCompany(_fixture.Create<string>());

            Assert.That(result.ViewData.TemplateInfo.HtmlFieldPrefix, Is.EqualTo("Company"));
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, bool hasCountry = true, ICountry country = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _queryBusMock.Setup(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()))
                .Returns(Task.FromResult(hasCountry ? country ?? _fixture.BuildCountryMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}