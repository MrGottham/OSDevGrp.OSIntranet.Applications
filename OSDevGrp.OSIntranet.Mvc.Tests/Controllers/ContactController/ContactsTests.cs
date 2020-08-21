using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactsTests
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
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));

            _random = new Random();
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_AssertGetCountryCodeWasCalledOnClaimResolver()
        {
            Controller sut = CreateSut();

            await sut.Contacts();

            _claimResolverMock.Verify(m => m.GetCountryCode(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_AssertQueryAsyncWasCalledOnQueryBusToGetCountryCollection()
        {
            Controller sut = CreateSut();

            await sut.Contacts();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.Is<EmptyQuery>(emptyQuery => emptyQuery != null)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Contacts();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToContacts()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts();

            Assert.That(result.ViewName, Is.EqualTo("Contacts"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_ReturnsViewResultWhereModelIsContactOptionsViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts();

            Assert.That(result.Model, Is.TypeOf<ContactOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenFilterIsNull_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenFilterIsEmpty_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts(string.Empty);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenFilterIsWhiteSpace_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts(" ");

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenFilterIsNotNullEmptyOrWhiteSpace_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereFilterIsEqualToFilterFromArgument()
        {
            Controller sut = CreateSut();

            string filter = _fixture.Create<string>();
            ViewResult result = (ViewResult) await sut.Contacts(filter);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Filter, Is.EqualTo(filter));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenExternalIdentifierIsNull_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereExternalIdentifierIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.ExternalIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenExternalIdentifierIsEmpty_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereExternalIdentifierIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts(externalIdentifier: string.Empty);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.ExternalIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenExternalIdentifierIsWhiteSpace_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereExternalIdentifierIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Contacts(externalIdentifier: " ");

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.ExternalIdentifier, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenExternalIdentifierIsNotNullEmptyOrWhiteSpace_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereExternalIdentifierIsEqualToFilterFromArgument()
        {
            Controller sut = CreateSut();

            string externalIdentifier = _fixture.Create<string>();
            ViewResult result = (ViewResult) await sut.Contacts(externalIdentifier: externalIdentifier);

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.ExternalIdentifier, Is.EqualTo(externalIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereDefaultCountryCodeIsEqualToCountryCodeFromClaimResolver()
        {
            string countryCode = _fixture.Create<string>();
            Controller sut = CreateSut(countryCode: countryCode);

            ViewResult result = (ViewResult) await sut.Contacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.DefaultCountryCode, Is.EqualTo(countryCode));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Contacts_WhenCalled_ReturnsViewResultWhereModelIsContactOptionsViewModelWhereCountriesContainsViewModelForCountries()
        {
            IEnumerable<ICountry> countryCollection = _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(countryCollection: countryCollection);

            ViewResult result = (ViewResult) await sut.Contacts();

            ContactOptionsViewModel contactOptionsViewModel = (ContactOptionsViewModel) result.Model;

            Assert.That(contactOptionsViewModel.Countries.All(countryViewModel => countryCollection.SingleOrDefault(country => string.CompareOrdinal(countryViewModel.Code, country.Code) == 0) != null), Is.True);
        }

        private Controller CreateSut(bool hasCountryCode = true, string countryCode = null, IEnumerable<ICountry> countryCollection = null)
        {
            _claimResolverMock.Setup(m => m.GetCountryCode())
                .Returns(hasCountryCode ? countryCode ?? _fixture.Create<string>() : null);
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => countryCollection ?? _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}