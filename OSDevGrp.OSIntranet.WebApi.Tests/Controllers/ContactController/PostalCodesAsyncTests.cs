using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;
using Controller=OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    [TestFixture]
    public class PostalCodesAsyncTests
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
        public async Task PostalCodesAsync_WhenCalled_QueriesForPostalCodesUsingTheRequestedCountryCode()
        {
            Controller sut = CreateSut();

            await sut.PostalCodesAsync("DK");

            _queryBusMock.Verify(mock => mock.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(
                It.Is<IGetPostalCodeCollectionQuery>(query => query.CountryCode == "DK")), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodesAsync_WhenCalled_ReturnsOkObjectResult()
        {
            Controller sut = CreateSut();

            ActionResult<IEnumerable<PostalCodeModel>> result = await sut.PostalCodesAsync("DK");

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodesAsync_WhenCalled_MapsPostalCodesAndCountry()
        {
            ICountry country = _fixture.BuildCountryMock(code: "DK", name: "Denmark", universalName: "Danmark", phonePrefix: "+45").Object;
            IList<IPostalCode> postalCodes =
            [
                _fixture.BuildPostalCodeMock(country: country, code: "1000", city: "Copenhagen", state: "Capital Region").Object
            ];
            Controller sut = CreateSut(postalCodes);

            OkObjectResult result = (OkObjectResult) (await sut.PostalCodesAsync("DK")).Result;
            PostalCodeModel model = ((IEnumerable<PostalCodeModel>) result.Value).Single();

            Assert.That(model.Country.Code, Is.EqualTo("DK"));
            Assert.That(model.Code, Is.EqualTo("1000"));
            Assert.That(model.City, Is.EqualTo("Copenhagen"));
            Assert.That(model.State, Is.EqualTo("Capital Region"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task PostalCodesAsync_WhenCalled_OrdersPostalCodesByCode()
        {
            IList<IPostalCode> postalCodes =
            [
                _fixture.BuildPostalCodeMock(code: "2000").Object,
                _fixture.BuildPostalCodeMock(code: "1000").Object
            ];
            Controller sut = CreateSut(postalCodes);

            OkObjectResult result = (OkObjectResult) (await sut.PostalCodesAsync("DK")).Result;

            Assert.That(((IEnumerable<PostalCodeModel>) result.Value).Select(model => model.Code), Is.EqualTo(new[] { "1000", "2000" }));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("ABCDE", false)]
        [TestCase("dk", false)]
        [TestCase("DK", true)]
        [Category("UnitTest")]
        public void PostalCodesAsync_WhenCalled_ValidatesCountryCodeLikeCountryCodeModel(string countryCode, bool expectedValid)
        {
            ParameterInfo parameter = typeof(Controller).GetMethod(nameof(Controller.PostalCodesAsync)).GetParameters().Single();
            IEnumerable<ValidationAttribute> validationAttributes = parameter.GetCustomAttributes(typeof(ValidationAttribute), false).Cast<ValidationAttribute>();
            List<ValidationResult> validationResults = [];

            bool result = Validator.TryValidateValue(countryCode, new ValidationContext(new object()), validationResults, validationAttributes);

            Assert.That(result, Is.EqualTo(expectedValid));
        }

        private Controller CreateSut(IEnumerable<IPostalCode> postalCodes = null)
        {
            _queryBusMock.Setup(mock => mock.QueryAsync<IGetPostalCodeCollectionQuery, IEnumerable<IPostalCode>>(It.IsAny<IGetPostalCodeCollectionQuery>()))
                .Returns(Task.FromResult(postalCodes ?? Enumerable.Empty<IPostalCode>()));

            return new Controller(_queryBusMock.Object, ConverterFactoryCreator.Create());
        }
    }
}