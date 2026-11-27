using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    [TestFixture]
    public class CountriesAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public async Task CountriesAsync_WhenCalled_QueriesForCountries()
        {
            Controller sut = CreateSut();

            await sut.CountriesAsync();

            _queryBusMock.Verify(mock => mock.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CountriesAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<CountryCodeModel>> result = await sut.CountriesAsync();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CountriesAsync_WhenCalled_MapsCountries()
        {
            IList<ICountry> countries =
            [
                _fixture.BuildCountryMock(code: "DK", name: "Denmark", universalName: "Danmark", phonePrefix: "+45").Object
            ];
            Controller sut = CreateSut(countries);

            OkObjectResult result = (OkObjectResult) (await sut.CountriesAsync()).Result;
            CountryCodeModel model = ((IEnumerable<CountryCodeModel>) result.Value).Single();

            Assert.That(model.Code, Is.EqualTo("DK"));
            Assert.That(model.Name, Is.EqualTo("Denmark"));
            Assert.That(model.UniversalName, Is.EqualTo("Danmark"));
            Assert.That(model.PhonePrefix, Is.EqualTo("+45"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CountriesAsync_WhenCalled_OrdersCountriesByCode()
        {
            IList<ICountry> countries =
            [
                _fixture.BuildCountryMock(code: "US").Object,
                _fixture.BuildCountryMock(code: "DK").Object
            ];
            Controller sut = CreateSut(countries);

            OkObjectResult result = (OkObjectResult) (await sut.CountriesAsync()).Result;

            Assert.That(((IEnumerable<CountryCodeModel>) result.Value).Select(model => model.Code), Is.EqualTo(new[] { "DK", "US" }));
        }

        private Controller CreateSut(IEnumerable<ICountry> countries = null)
        {
            _queryBusMock.Setup(mock => mock.QueryAsync<EmptyQuery, IEnumerable<ICountry>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(countries ?? Enumerable.Empty<ICountry>()));

            return new Controller(_queryBusMock.Object, ConverterFactoryCreator.Create());
        }
    }
}