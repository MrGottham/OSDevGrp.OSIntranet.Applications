using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
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
    public class PostalCodesTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();

            _fixture = new Fixture();
            _fixture.Customize<ICountry>(builder => builder.FromFactory(() => _fixture.BuildCountryMock().Object));
            _fixture.Customize<IPostalCode>(builder => builder.FromFactory(() => _fixture.BuildPostalCodeMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithoutCountryCode_AssertQueryAsyncWasCalledOnQueryBus()
        {
            Controller sut = CreateSut();

            await sut.PostalCodes();

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithoutCountryCode_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.PostalCodes();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithoutCountryCode_ReturnsViewResultWhereViewNameIsEqualToPostalCodes()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.PostalCodes();

            Assert.That(result.ViewName, Is.EqualTo("PostalCodes"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithoutCountryCode_ReturnsViewResultWhereModelIsCollectionOfCountryViewModel()
        {
            IEnumerable<ICountry> countryMockCollection = _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList();
            Controller sut = CreateSut(countryMockCollection);

            ViewResult result = (ViewResult) await sut.PostalCodes();

            Assert.That(result.Model, Is.TypeOf<List<CountryViewModel>>());

            List<CountryViewModel> countryViewModelCollection = (List<CountryViewModel>) result.Model;

            Assert.That(countryViewModelCollection, Is.Not.Null);
            Assert.That(countryViewModelCollection, Is.Not.Empty);
            Assert.That(countryViewModelCollection.Count, Is.EqualTo(countryMockCollection.Count()));
        }

        [Test]
        [Category("UnitTest")]
        public void PostalCodes_WhenCalledWithCountryCodeEqualToNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PostalCodes(null));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void PostalCodes_WhenCalledWithCountryCodeEqualToEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PostalCodes(string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public void PostalCodes_WhenCalledWithCountryCodeEqualToWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PostalCodes(" "));

            Assert.That(result.ParamName, Is.EqualTo("countryCode"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithCountryCodeNotEqualToNullEmptyOrWhiteSpace_AssertQueryAsyncWasCalledOnQueryBusWithGetCountryQuery()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            await sut.PostalCodes(countryCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.Is<IGetCountryQuery>(query => query != null && string.CompareOrdinal(query.CountryCode, countryCode.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithUnknownCountryCode_AssertQueryAsyncWasNotCalledOnQueryBusWithGetPostalCodeCollectionQuery()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            await sut.PostalCodes(_fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(It.IsAny<IGetPostalCodeCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            IActionResult result = await sut.PostalCodes(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResultWhereControllerNameIsEqualToContact()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.PostalCodes(_fixture.Create<string>());

            Assert.That(result.ControllerName, Is.EqualTo("Contact"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResultWhereActionNameIsEqualToPostalCodes()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.PostalCodes(_fixture.Create<string>());

            Assert.That(result.ActionName, Is.EqualTo("PostalCodes"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithUnknownCountryCode_ReturnsRedirectToActionResultWhereRouteValuesIsEqualToNull()
        {
            Controller sut = CreateSut(knownCountryCode: false);

            RedirectToActionResult result = (RedirectToActionResult) await sut.PostalCodes(_fixture.Create<string>());

            Assert.That(result.RouteValues, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithKnownCountryCode_AssertQueryAsyncWasCalledOnQueryBusWithGetPostalCodeCollectionQuery()
        {
            Controller sut = CreateSut();

            string countryCode = _fixture.Create<string>();
            await sut.PostalCodes(countryCode);

            _queryBusMock.Verify(m => m.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(It.Is<IGetPostalCodeCollectionQuery>(query => query != null && string.CompareOrdinal(query.CountryCode, countryCode.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithKnownCountryCode_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.PostalCodes(_fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithKnownCountryCode_ReturnsPartialViewResultWhereViewNameIsEqualToPostalCodeTablePartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.PostalCodes(_fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_PostalCodeTablePartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodes_WhenCalledWithKnownCountryCode_ReturnsPartialViewResultModelIsCollectionOfPostalCodeViewModel()
        {
            IEnumerable<IPostalCode> postalCodeMockCollection = _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList();
            Controller sut = CreateSut(postalCodeCollection: postalCodeMockCollection);

            string countryCode = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) await sut.PostalCodes(countryCode);

            Assert.That(result.ViewData, Is.Not.Null);
            Assert.That(result.ViewData.ContainsKey("CountryCode"), Is.True);
            Assert.That(result.ViewData["CountryCode"], Is.EqualTo(countryCode));

            Assert.That(result.Model, Is.TypeOf<List<PostalCodeViewModel>>());

            List<PostalCodeViewModel> postalCodeViewModelCollection = (List<PostalCodeViewModel>) result.Model;

            Assert.That(postalCodeViewModelCollection, Is.Not.Null);
            Assert.That(postalCodeViewModelCollection, Is.Not.Empty);
            Assert.That(postalCodeViewModelCollection.Count, Is.EqualTo(postalCodeMockCollection.Count()));
        }

        private Controller CreateSut(IEnumerable<ICountry> countryCollection = null, bool knownCountryCode = true, ICountry country = null, IEnumerable<IPostalCode> postalCodeCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.Run(() => countryCollection ?? _fixture.CreateMany<ICountry>(_random.Next(5, 10)).ToList()));
            _queryBusMock.Setup(m => m.QueryAsync<IGetCountryQuery, ICountry>(It.IsAny<IGetCountryQuery>()))
                .Returns(Task.Run(() => knownCountryCode ? country ?? _fixture.BuildCountryMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(It.IsAny<IGetPostalCodeCollectionQuery>()))
                .Returns(Task.Run(() => postalCodeCollection ?? _fixture.CreateMany<IPostalCode>(_random.Next(10, 25)).ToList()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _tokenHelperFactoryMock.Object);
        }
    }
}