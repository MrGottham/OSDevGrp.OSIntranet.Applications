using System;
using System.Collections.Generic;
using System.Linq;
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
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class LoadContactTests
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
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoadContact_WhenExternalIdentifierIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.LoadContact(null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void LoadContact_WhenExternalIdentifierIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.LoadContact(string.Empty, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void LoadContact_WhenExternalIdentifierIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.LoadContact(" ", _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("externalIdentifier"));
        }

        [Test]
        [Category("UnitTest")]
        public void LoadContact_WhenCountryCodeIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.LoadContact(_fixture.Create<string>(), null));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void LoadContact_WhenCountryCodeIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.LoadContact(_fixture.Create<string>(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void LoadContact_WhenCountryCodeIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.LoadContact(_fixture.Create<string>(), " "));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCalledWithExternalIdentifierAndCountryCode_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithEmptyQueryForContactGroupCollection()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsAny<EmptyQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithEmptyQueryForPaymentTermCollection()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsAny<EmptyQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithEmptyQueryForCountryCollection()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithGetCountryQuery()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBusWithGetContactQuery()
        {
            Controller sut = CreateSut(false);

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactQuery, IContact>(It.IsAny<IGetContactQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForContactGroupCollection()
        {
            Controller sut = CreateSut();

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsNotNull<EmptyQuery>()), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForPaymentTermCollection()
        {
            Controller sut = CreateSut();

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForCountryCollection()
        {
            Controller sut = CreateSut();

            await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBusWithGetCountryQuery()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            await sut.LoadContact(_fixture.Create<string>(), countryCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.Is<IGetCountryQuery>(value => value != null && string.CompareOrdinal(value.CountryCode, countryCode.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncNotCalledOnQueryBusWithGetContactQuery()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            string externalIdentifier = _fixture.Create<string>();
            await sut.LoadContact(externalIdentifier, _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactQuery, IContact>(It.Is<IGetContactQuery>(value => value != null && string.CompareOrdinal(value.TokenType, tokenType) == 0 && string.CompareOrdinal(value.AccessToken, accessToken) == 0 && string.CompareOrdinal(value.RefreshToken, refreshToken) == 0 && value.Expires == expires && string.CompareOrdinal(value.ExternalIdentifier, externalIdentifier) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoCountryWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasCountry: false);

            IActionResult result = await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenNoContactWasReturnedFromQueryBus_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(hasContact: false);

            IActionResult result = await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereViewNameIsEqualToContactPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_ContactPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<ContactViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWithoutAddressWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereAddressIsNotNul()
        {
            IContact contact = _fixture.BuildContactMock(hasAddress: false).Object;
            Controller sut = CreateSut(contact: contact);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.Address, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWithoutAddressWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereAddressIsAddressViewModel()
        {
            IContact contact = _fixture.BuildContactMock(hasAddress: false).Object;
            Controller sut = CreateSut(contact: contact);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.Address, Is.TypeOf<AddressViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWithAddressWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereAddressIsNotNul()
        {
            IContact contact = _fixture.BuildContactMock().Object;
            Controller sut = CreateSut(contact: contact);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.Address, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWithAddressWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereAddressIsAddressViewModel()
        {
            IContact contact = _fixture.BuildContactMock().Object;
            Controller sut = CreateSut(contact: contact);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.Address, Is.TypeOf<AddressViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereEditModeIsEqualToNone()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.EditMode, Is.EqualTo(EditMode.None));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereCountryIsEqualToCountryReturnedFromQueryBus()
        {
            string countryCode = _fixture.Create<string>();
            ICountry country = _fixture.BuildCountryMock(countryCode).Object;
            Controller sut = CreateSut(country: country);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.Country.Code, Is.EqualTo(countryCode));
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereCountriesIsEqualToCountryCollectionReturnedFromQueryBus()
        {
            IEnumerable<ICountry> countryCollection = _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(countryCollection: countryCollection);

            PartialViewResult result = (PartialViewResult)await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.Countries, Is.Not.Null);
            Assert.That(contactViewModel.Countries.Count, Is.EqualTo(countryCollection.Count()));
            Assert.That(contactViewModel.Countries.All(countryViewModel => countryCollection.Any(country => string.CompareOrdinal(country.Code, countryViewModel.Code) == 0)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWhereContactGroupsIsEqualToContactGroupCollectionReturnedFromQueryBus()
        {
            IEnumerable<IContactGroup> contactGroupCollection = _fixture.CreateMany<IContactGroup>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(contactGroupCollection: contactGroupCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.ContactGroups, Is.Not.Null);
            Assert.That(contactViewModel.ContactGroups.Count, Is.EqualTo(contactGroupCollection.Count()));
            Assert.That(contactViewModel.ContactGroups.All(contactGroupViewModel => contactGroupCollection.Any(contactGroup => contactGroup.Number == contactGroupViewModel.Number)), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task LoadContact_WhenCountryAndContactWasReturnedFromQueryBus_ReturnsPartialViewResultWhereModelIsContactViewModelWherePaymentTermsIsEqualToPaymentTermCollectionReturnedFromQueryBus()
        {
            IEnumerable<IPaymentTerm> paymentTermCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(paymentTermCollection: paymentTermCollection);

            PartialViewResult result = (PartialViewResult) await sut.LoadContact(_fixture.Create<string>(), _fixture.Create<string>());

            ContactViewModel contactViewModel = (ContactViewModel) result.Model;

            Assert.That(contactViewModel.PaymentTerms, Is.Not.Null);
            Assert.That(contactViewModel.PaymentTerms.Count, Is.EqualTo(paymentTermCollection.Count()));
            Assert.That(contactViewModel.PaymentTerms.All(paymentTermViewModel => paymentTermCollection.Any(paymentTerm => paymentTerm.Number == paymentTermViewModel.Number)), Is.True);
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, IEnumerable<IContactGroup> contactGroupCollection = null, IEnumerable<IPaymentTerm> paymentTermCollection = null, IEnumerable<ICountry> countryCollection = null, bool hasCountry = true, ICountry country = null, bool hasContact = true, IContact contact = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.Run(() => hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IContactGroup>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(contactGroupCollection ?? _fixture.CreateMany<IContactGroup>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(paymentTermCollection ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(countryCollection ?? _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()))
                .Returns(Task.FromResult(hasCountry ? country ?? _fixture.BuildCountryMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<IGetContactQuery, IContact>(It.IsAny<IGetContactQuery>()))
                .Returns(Task.FromResult(hasContact ? contact ?? _fixture.BuildContactMock().Object : null));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}